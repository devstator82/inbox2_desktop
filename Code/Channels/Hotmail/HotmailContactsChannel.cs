using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Xml;
using System.Xml.Linq;
using Inbox2.Channels.Hotmail.REST;
using Inbox2.Platform.Channels.ServiceModel;
using Inbox2.Platform.Channels.Entities;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Channels.Hotmail
{
    class HotmailContactsChannel : IClientContactsChannel
    {
        protected const string contactsFeedUrl = "https://livecontacts.services.live.com/users/@L@{0}/REST/LiveContacts/Contacts";
        private const string appId = "000000004C00C9B8";
        private const string secretKey = "3QYoxaTDOAi1gYzMqg3lls4bvhz48ip8";

        private static string authHeader;
        private static string lid;

        static void GetAuthHeader()
        {
            //TODO: replace this with scraping and return the right response values for the 4 variables
            //i.e.: call method ContactsAuthentication.AuthenticateContactsSharing(username, password)
            string responseConsentToken = "eact%3Duw5Ym5wpNL9Jo51f9N38Div7B9HmBTkwDof75T22MRvbToDeXSEYvZzWRuRMjHkw%252B8kbWcbKJ4ZwWdGcKm%252FeCJYCPIKTHMSCzx4gZscI9w21NKV%252BqjKFkQ6t9b6%252FccObokDfN6XCU1MBFddNoenuIAyo2o44luRpkDcokM3TgfSdtj%252Bg0hZUR8V8IIIvLmelc1KTjcI%252F3GJ2pBQXkxur0O4YKGu8cFF0y8gNVuXJvpL%252BcFq5%252Fdq8wMzTT6SnGbZgHyPqJv6UQZsYpC9EM%252FQmSlcxinLZf%252BV%252BxB25qNHE2GD05SFiUYKwEf%252FSS5a9RMziv8jThJTReCeFaBLbJClvGIr7D3zHh051REFWl97FlvH2oGcNMze5K9R%252Fxb9pi0%252BFcUCA6hTveVuyih0zOZY5TFyhvltaU2aemq3BBFOa%252FHRbTMu5kGAoW%252FUJiNf4FODvuZCF%252BrH%252BMMEV5V5b3WEsTPlytEG9HhzAjWpMV6i784pqXQjB7gF6Iuy2T8JKujk2wN%252FuFIS6F4Z%252BGMUbRYBxgmuLZMALShiscC6gC4JQuW7fr2zcTqBEmChmJAi9D9T97AVBWL5cyzrjxpkqm8Dr7z8KYqGcoWQ8VVy1JMcxW3vMGMJZPZBQUz1AOYut2xqd7kt1sssDfI7Owv4M7uU55K16IImxPR4LAmapujGLtTjWwfSoKYZBl2%252B41%252BuzPcvyUw%252BQqDu2aOCe1YuZ%252FaJry3tcIk43R0V9LFQ9HFEto8g8R7PeUlx9469jFhNPGPqzFy7Qlc%252BYGwf4nRwbrizAx5HKx9QuIcl%252FByK227CX0u7e7UkUzMQ%252FfTc13TpfvFk%252FBo5aXeoxjAKwrnyFeeDUKHiEsHwjwu2EW7kGfTdAOodUACDsoQDnrrrs%252BOX80Q4BzfVhZYy3F8XDAmdczqqzaGimQLK9UETvr%252F03GzCZdJResKRJNAeaaZ6Ahdy9ebJ64qvSvFq8jBOV1%252FY1rq8clX%252F5GZxnXzqT8gN%252F%252B9%252F8ktllYZi4QLmxaT2ChY2qnE8Fxx7GomY0k%252B5sDWhdqD18Ddu9HSFSaAFq4DflnTWis6jCLJ6WpqaeBGwJyPEGm0IYlVbQ0k7X5WnKYjKrC3kl68%252BqU6lL0T1nRoa8mZNXdeXZ72W%252FLQp21IrOH6NZhnomPBID2myyJ09QtuUkXlSaWbT5A0oRcE9ywPY4EvplbggyLxpke9jL%252BtIUQtNXdGN%252B%252FwjQhREw8dfMRHoRl6VN%252BmGLONeVvoR6Lvi%252BXJZhBz36XwwfhXtGb7kKIud8E9as%252FMC6YZcUNBn%252F28Zj9YdcRmM2iAAs9DBvw5tTmgAfPYU%253D";
            string responseResponseCode = "RequestApproved";
            string responseAction = "delauth";
            string responseAppctx = "";

            NameValueCollection nvc = new NameValueCollection(4);
            WindowsLiveLogin wll = new WindowsLiveLogin(appId, secretKey);
            nvc.Add("ConsentToken", responseConsentToken);
            nvc.Add("ResponseCode", responseResponseCode);
            nvc.Add("action", responseAction);
            nvc.Add("appctx", responseAppctx);

            WindowsLiveLogin.ConsentToken ct = wll.ProcessConsent(nvc);
            authHeader = String.Format("DelegatedToken dt=\"{0}\"", ct.DelegationToken);
            lid = ct.LocationID;
        }

        public IEnumerable<ChannelContact> GetContacts()
        {
            EnsureAuthentication();
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "HTTPS";
            uriBuilder.Path = "/users/@L@" + lid + "/REST/LiveContacts";
            uriBuilder.Host = "livecontacts.services.live.com";
            uriBuilder.Port = 443;
            string uriPath = uriBuilder.Uri.AbsoluteUri;
          
            ILiveContacts channel = BuildContactsChannel(uriPath);
            using (new OperationContextScope(channel as IContextChannel))
            {
                // Auth header is required
                WebOperationContext.Current.OutgoingRequest.Headers.Set("Authorization", authHeader);
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Accept-Encoding", "deflate");
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Pragma", "No-Cache");
                WebOperationContext.Current.OutgoingRequest.UserAgent = "Windows Live Data Interactive SDK";
                WebOperationContext.Current.OutgoingRequest.Method = "GET";
                WebOperationContext.Current.OutgoingRequest.ContentType = "application/xml; charset=utf-8";
                   
                // Since the response is chucked we have to read it in as a Stream
                Stream responseStream = channel.Contacts();
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                XElement element;

                using (XmlTextReader reader = new XmlTextReader(responseStream))
                    element = XElement.Load(reader);

                foreach (var entry in element.Elements("Contact"))
                {
                    if(entry.Element("Emails") != null)
                    {
                        yield return ParseContact(entry);
                    }
                }        
            }
        }

    	public IClientContactsChannel Clone()
    	{
			return new HotmailContactsChannel
				{
					Hostname = Hostname,
					Port = Port,
					IsSecured = IsSecured,
					MaxConcurrentConnections = MaxConcurrentConnections,
					CredentialsProvider = CredentialsProvider
				};
    	}

    	protected ILiveContacts BuildContactsChannel(string epAddress)
        {
            EndpointAddress address = new EndpointAddress(epAddress);

			var binding = RawMapper.GetCustomBinding(new WebHttpBinding(WebHttpSecurityMode.Transport)); 
            ChannelFactory<ILiveContacts> cf = new ChannelFactory<ILiveContacts>(binding, address);
            cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

            var channel = cf.CreateChannel();

            return channel;
        }

        //TODO: right implementation
        protected ChannelContact ParseContact(XElement element)
        {
            ChannelContact contact = new ChannelContact();

            contact.Person.Name = element.Element("Profiles").Element("Personal").Element("DisplayName").Value;
            contact.Profile.ChannelProfileKey = element.Element("ID").Value;

            // Todo could be different kind of addressses; home, work etc
            contact.Profile.SourceAddress = new SourceAddress(
                element.Element("Emails").Element("Email").Element("Address").Value,
                element.Element("Profiles").Element("Personal").Element("DisplayName").Value);

            return contact;
        }

        protected void EnsureAuthentication()
        {
            if (String.IsNullOrEmpty(authHeader))
                GetAuthHeader();
        }

        #region Properties

        public string Hostname
        {
            get { return "https://livecontacts.services.live.com/"; }
            set { }
        }
        public int Port
        {
            get { return 443; }
            set { }
        }

        public bool IsSecured
        {
            get { return true; }
            set { }
        }

        public bool IsEnabled { get; set; }
        public int MaxConcurrentConnections { get; set; }
        public IChannelCredentialsProvider CredentialsProvider { get; set; }

        public string Protocol
        {
            get { return "HotmailContactsAPI"; }
        }

        #endregion

    	public void Dispose()
    	{
    		
    	}
    }
}
