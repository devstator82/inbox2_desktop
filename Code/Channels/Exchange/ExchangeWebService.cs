using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace ExchangeServicesWsdlClient
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ExchangeServiceBinding", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttendeeConflictData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseSubscriptionRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseGroupByType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RecurrenceRangeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RecurrencePatternBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChangeDescriptionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BasePagingType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BasePermissionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseEmailAddressType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseFolderIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseRequestType))]
    public partial class ExchangeServiceBinding : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private ExchangeImpersonationType exchangeImpersonationField;

        private SerializedSecurityContextType serializedSecurityContextField;

        private language mailboxCultureField;

        private RequestServerVersion requestServerVersionValueField;

        private ServerVersionInfo serverVersionInfoValueField;

        private System.Threading.SendOrPostCallback ResolveNamesOperationCompleted;

        private System.Threading.SendOrPostCallback ExpandDLOperationCompleted;

        private System.Threading.SendOrPostCallback FindFolderOperationCompleted;

        private System.Threading.SendOrPostCallback FindItemOperationCompleted;

        private System.Threading.SendOrPostCallback GetFolderOperationCompleted;

        private System.Threading.SendOrPostCallback ConvertIdOperationCompleted;

        private System.Threading.SendOrPostCallback CreateFolderOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteFolderOperationCompleted;

        private System.Threading.SendOrPostCallback UpdateFolderOperationCompleted;

        private System.Threading.SendOrPostCallback MoveFolderOperationCompleted;

        private System.Threading.SendOrPostCallback CopyFolderOperationCompleted;

        private System.Threading.SendOrPostCallback SubscribeOperationCompleted;

        private System.Threading.SendOrPostCallback UnsubscribeOperationCompleted;

        private System.Threading.SendOrPostCallback GetEventsOperationCompleted;

        private System.Threading.SendOrPostCallback SyncFolderHierarchyOperationCompleted;

        private System.Threading.SendOrPostCallback SyncFolderItemsOperationCompleted;

        private System.Threading.SendOrPostCallback CreateManagedFolderOperationCompleted;

        private System.Threading.SendOrPostCallback GetItemOperationCompleted;

        private System.Threading.SendOrPostCallback CreateItemOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteItemOperationCompleted;

        private System.Threading.SendOrPostCallback UpdateItemOperationCompleted;

        private System.Threading.SendOrPostCallback SendItemOperationCompleted;

        private System.Threading.SendOrPostCallback MoveItemOperationCompleted;

        private System.Threading.SendOrPostCallback CopyItemOperationCompleted;

        private System.Threading.SendOrPostCallback CreateAttachmentOperationCompleted;

        private System.Threading.SendOrPostCallback DeleteAttachmentOperationCompleted;

        private System.Threading.SendOrPostCallback GetAttachmentOperationCompleted;

        private System.Threading.SendOrPostCallback GetDelegateOperationCompleted;

        private System.Threading.SendOrPostCallback AddDelegateOperationCompleted;

        private System.Threading.SendOrPostCallback RemoveDelegateOperationCompleted;

        private System.Threading.SendOrPostCallback UpdateDelegateOperationCompleted;

        private AvailabilityProxyRequestType proxyRequestTypeHeaderField;

        private System.Threading.SendOrPostCallback GetUserAvailabilityOperationCompleted;

        private System.Threading.SendOrPostCallback GetUserOofSettingsOperationCompleted;

        private System.Threading.SendOrPostCallback SetUserOofSettingsOperationCompleted;

        /// <remarks/>
        public ExchangeServiceBinding()
        {            
        }

        public ExchangeImpersonationType ExchangeImpersonation
        {
            get
            {
                return this.exchangeImpersonationField;
            }
            set
            {
                this.exchangeImpersonationField = value;
            }
        }

        public SerializedSecurityContextType SerializedSecurityContext
        {
            get
            {
                return this.serializedSecurityContextField;
            }
            set
            {
                this.serializedSecurityContextField = value;
            }
        }

        public language MailboxCulture
        {
            get
            {
                return this.mailboxCultureField;
            }
            set
            {
                this.mailboxCultureField = value;
            }
        }

        public RequestServerVersion RequestServerVersionValue
        {
            get
            {
                return this.requestServerVersionValueField;
            }
            set
            {
                this.requestServerVersionValueField = value;
            }
        }

        public ServerVersionInfo ServerVersionInfoValue
        {
            get
            {
                return this.serverVersionInfoValueField;
            }
            set
            {
                this.serverVersionInfoValueField = value;
            }
        }

        public AvailabilityProxyRequestType ProxyRequestTypeHeader
        {
            get
            {
                return this.proxyRequestTypeHeaderField;
            }
            set
            {
                this.proxyRequestTypeHeaderField = value;
            }
        }

        /// <remarks/>
        public event ResolveNamesCompletedEventHandler ResolveNamesCompleted;

        /// <remarks/>
        public event ExpandDLCompletedEventHandler ExpandDLCompleted;

        /// <remarks/>
        public event FindFolderCompletedEventHandler FindFolderCompleted;

        /// <remarks/>
        public event FindItemCompletedEventHandler FindItemCompleted;

        /// <remarks/>
        public event GetFolderCompletedEventHandler GetFolderCompleted;

        /// <remarks/>
        public event ConvertIdCompletedEventHandler ConvertIdCompleted;

        /// <remarks/>
        public event CreateFolderCompletedEventHandler CreateFolderCompleted;

        /// <remarks/>
        public event DeleteFolderCompletedEventHandler DeleteFolderCompleted;

        /// <remarks/>
        public event UpdateFolderCompletedEventHandler UpdateFolderCompleted;

        /// <remarks/>
        public event MoveFolderCompletedEventHandler MoveFolderCompleted;

        /// <remarks/>
        public event CopyFolderCompletedEventHandler CopyFolderCompleted;

        /// <remarks/>
        public event SubscribeCompletedEventHandler SubscribeCompleted;

        /// <remarks/>
        public event UnsubscribeCompletedEventHandler UnsubscribeCompleted;

        /// <remarks/>
        public event GetEventsCompletedEventHandler GetEventsCompleted;

        /// <remarks/>
        public event SyncFolderHierarchyCompletedEventHandler SyncFolderHierarchyCompleted;

        /// <remarks/>
        public event SyncFolderItemsCompletedEventHandler SyncFolderItemsCompleted;

        /// <remarks/>
        public event CreateManagedFolderCompletedEventHandler CreateManagedFolderCompleted;

        /// <remarks/>
        public event GetItemCompletedEventHandler GetItemCompleted;

        /// <remarks/>
        public event CreateItemCompletedEventHandler CreateItemCompleted;

        /// <remarks/>
        public event DeleteItemCompletedEventHandler DeleteItemCompleted;

        /// <remarks/>
        public event UpdateItemCompletedEventHandler UpdateItemCompleted;

        /// <remarks/>
        public event SendItemCompletedEventHandler SendItemCompleted;

        /// <remarks/>
        public event MoveItemCompletedEventHandler MoveItemCompleted;

        /// <remarks/>
        public event CopyItemCompletedEventHandler CopyItemCompleted;

        /// <remarks/>
        public event CreateAttachmentCompletedEventHandler CreateAttachmentCompleted;

        /// <remarks/>
        public event DeleteAttachmentCompletedEventHandler DeleteAttachmentCompleted;

        /// <remarks/>
        public event GetAttachmentCompletedEventHandler GetAttachmentCompleted;

        /// <remarks/>
        public event GetDelegateCompletedEventHandler GetDelegateCompleted;

        /// <remarks/>
        public event AddDelegateCompletedEventHandler AddDelegateCompleted;

        /// <remarks/>
        public event RemoveDelegateCompletedEventHandler RemoveDelegateCompleted;

        /// <remarks/>
        public event UpdateDelegateCompletedEventHandler UpdateDelegateCompleted;

        /// <remarks/>
        public event GetUserAvailabilityCompletedEventHandler GetUserAvailabilityCompleted;

        /// <remarks/>
        public event GetUserOofSettingsCompletedEventHandler GetUserOofSettingsCompleted;

        /// <remarks/>
        public event SetUserOofSettingsCompletedEventHandler SetUserOofSettingsCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/ResolveNames", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ResolveNamesResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public ResolveNamesResponseType ResolveNames([System.Xml.Serialization.XmlElementAttribute("ResolveNames", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] ResolveNamesType ResolveNames1)
        {
            object[] results = this.Invoke("ResolveNames", new object[] {
                    ResolveNames1});
            return ((ResolveNamesResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginResolveNames(ResolveNamesType ResolveNames1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ResolveNames", new object[] {
                    ResolveNames1}, callback, asyncState);
        }

        /// <remarks/>
        public ResolveNamesResponseType EndResolveNames(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ResolveNamesResponseType)(results[0]));
        }

        /// <remarks/>
        public void ResolveNamesAsync(ResolveNamesType ResolveNames1)
        {
            this.ResolveNamesAsync(ResolveNames1, null);
        }

        /// <remarks/>
        public void ResolveNamesAsync(ResolveNamesType ResolveNames1, object userState)
        {
            if ((this.ResolveNamesOperationCompleted == null))
            {
                this.ResolveNamesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnResolveNamesOperationCompleted);
            }
            this.InvokeAsync("ResolveNames", new object[] {
                    ResolveNames1}, this.ResolveNamesOperationCompleted, userState);
        }

        private void OnResolveNamesOperationCompleted(object arg)
        {
            if ((this.ResolveNamesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ResolveNamesCompleted(this, new ResolveNamesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/ExpandDL", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ExpandDLResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public ExpandDLResponseType ExpandDL([System.Xml.Serialization.XmlElementAttribute("ExpandDL", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] ExpandDLType ExpandDL1)
        {
            object[] results = this.Invoke("ExpandDL", new object[] {
                    ExpandDL1});
            return ((ExpandDLResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginExpandDL(ExpandDLType ExpandDL1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ExpandDL", new object[] {
                    ExpandDL1}, callback, asyncState);
        }

        /// <remarks/>
        public ExpandDLResponseType EndExpandDL(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ExpandDLResponseType)(results[0]));
        }

        /// <remarks/>
        public void ExpandDLAsync(ExpandDLType ExpandDL1)
        {
            this.ExpandDLAsync(ExpandDL1, null);
        }

        /// <remarks/>
        public void ExpandDLAsync(ExpandDLType ExpandDL1, object userState)
        {
            if ((this.ExpandDLOperationCompleted == null))
            {
                this.ExpandDLOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExpandDLOperationCompleted);
            }
            this.InvokeAsync("ExpandDL", new object[] {
                    ExpandDL1}, this.ExpandDLOperationCompleted, userState);
        }

        private void OnExpandDLOperationCompleted(object arg)
        {
            if ((this.ExpandDLCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExpandDLCompleted(this, new ExpandDLCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/FindFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("FindFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public FindFolderResponseType FindFolder([System.Xml.Serialization.XmlElementAttribute("FindFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] FindFolderType FindFolder1)
        {
            object[] results = this.Invoke("FindFolder", new object[] {
                    FindFolder1});
            return ((FindFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginFindFolder(FindFolderType FindFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("FindFolder", new object[] {
                    FindFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public FindFolderResponseType EndFindFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((FindFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void FindFolderAsync(FindFolderType FindFolder1)
        {
            this.FindFolderAsync(FindFolder1, null);
        }

        /// <remarks/>
        public void FindFolderAsync(FindFolderType FindFolder1, object userState)
        {
            if ((this.FindFolderOperationCompleted == null))
            {
                this.FindFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFindFolderOperationCompleted);
            }
            this.InvokeAsync("FindFolder", new object[] {
                    FindFolder1}, this.FindFolderOperationCompleted, userState);
        }

        private void OnFindFolderOperationCompleted(object arg)
        {
            if ((this.FindFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FindFolderCompleted(this, new FindFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/FindItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("FindItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public FindItemResponseType FindItem([System.Xml.Serialization.XmlElementAttribute("FindItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] FindItemType FindItem1)
        {
            object[] results = this.Invoke("FindItem", new object[] {
                    FindItem1});
            return ((FindItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginFindItem(FindItemType FindItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("FindItem", new object[] {
                    FindItem1}, callback, asyncState);
        }

        /// <remarks/>
        public FindItemResponseType EndFindItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((FindItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void FindItemAsync(FindItemType FindItem1)
        {
            this.FindItemAsync(FindItem1, null);
        }

        /// <remarks/>
        public void FindItemAsync(FindItemType FindItem1, object userState)
        {
            if ((this.FindItemOperationCompleted == null))
            {
                this.FindItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFindItemOperationCompleted);
            }
            this.InvokeAsync("FindItem", new object[] {
                    FindItem1}, this.FindItemOperationCompleted, userState);
        }

        private void OnFindItemOperationCompleted(object arg)
        {
            if ((this.FindItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FindItemCompleted(this, new FindItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetFolderResponseType GetFolder([System.Xml.Serialization.XmlElementAttribute("GetFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetFolderType GetFolder1)
        {
            object[] results = this.Invoke("GetFolder", new object[] {
                    GetFolder1});
            return ((GetFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetFolder(GetFolderType GetFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetFolder", new object[] {
                    GetFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public GetFolderResponseType EndGetFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void GetFolderAsync(GetFolderType GetFolder1)
        {
            this.GetFolderAsync(GetFolder1, null);
        }

        /// <remarks/>
        public void GetFolderAsync(GetFolderType GetFolder1, object userState)
        {
            if ((this.GetFolderOperationCompleted == null))
            {
                this.GetFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetFolderOperationCompleted);
            }
            this.InvokeAsync("GetFolder", new object[] {
                    GetFolder1}, this.GetFolderOperationCompleted, userState);
        }

        private void OnGetFolderOperationCompleted(object arg)
        {
            if ((this.GetFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetFolderCompleted(this, new GetFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/ConvertId", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ConvertIdResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public ConvertIdResponseType ConvertId([System.Xml.Serialization.XmlElementAttribute("ConvertId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] ConvertIdType ConvertId1)
        {
            object[] results = this.Invoke("ConvertId", new object[] {
                    ConvertId1});
            return ((ConvertIdResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginConvertId(ConvertIdType ConvertId1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ConvertId", new object[] {
                    ConvertId1}, callback, asyncState);
        }

        /// <remarks/>
        public ConvertIdResponseType EndConvertId(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((ConvertIdResponseType)(results[0]));
        }

        /// <remarks/>
        public void ConvertIdAsync(ConvertIdType ConvertId1)
        {
            this.ConvertIdAsync(ConvertId1, null);
        }

        /// <remarks/>
        public void ConvertIdAsync(ConvertIdType ConvertId1, object userState)
        {
            if ((this.ConvertIdOperationCompleted == null))
            {
                this.ConvertIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConvertIdOperationCompleted);
            }
            this.InvokeAsync("ConvertId", new object[] {
                    ConvertId1}, this.ConvertIdOperationCompleted, userState);
        }

        private void OnConvertIdOperationCompleted(object arg)
        {
            if ((this.ConvertIdCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConvertIdCompleted(this, new ConvertIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CreateFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CreateFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CreateFolderResponseType CreateFolder([System.Xml.Serialization.XmlElementAttribute("CreateFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CreateFolderType CreateFolder1)
        {
            object[] results = this.Invoke("CreateFolder", new object[] {
                    CreateFolder1});
            return ((CreateFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCreateFolder(CreateFolderType CreateFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CreateFolder", new object[] {
                    CreateFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public CreateFolderResponseType EndCreateFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CreateFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void CreateFolderAsync(CreateFolderType CreateFolder1)
        {
            this.CreateFolderAsync(CreateFolder1, null);
        }

        /// <remarks/>
        public void CreateFolderAsync(CreateFolderType CreateFolder1, object userState)
        {
            if ((this.CreateFolderOperationCompleted == null))
            {
                this.CreateFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateFolderOperationCompleted);
            }
            this.InvokeAsync("CreateFolder", new object[] {
                    CreateFolder1}, this.CreateFolderOperationCompleted, userState);
        }

        private void OnCreateFolderOperationCompleted(object arg)
        {
            if ((this.CreateFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateFolderCompleted(this, new CreateFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/DeleteFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public DeleteFolderResponseType DeleteFolder([System.Xml.Serialization.XmlElementAttribute("DeleteFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] DeleteFolderType DeleteFolder1)
        {
            object[] results = this.Invoke("DeleteFolder", new object[] {
                    DeleteFolder1});
            return ((DeleteFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteFolder(DeleteFolderType DeleteFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteFolder", new object[] {
                    DeleteFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteFolderResponseType EndDeleteFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void DeleteFolderAsync(DeleteFolderType DeleteFolder1)
        {
            this.DeleteFolderAsync(DeleteFolder1, null);
        }

        /// <remarks/>
        public void DeleteFolderAsync(DeleteFolderType DeleteFolder1, object userState)
        {
            if ((this.DeleteFolderOperationCompleted == null))
            {
                this.DeleteFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteFolderOperationCompleted);
            }
            this.InvokeAsync("DeleteFolder", new object[] {
                    DeleteFolder1}, this.DeleteFolderOperationCompleted, userState);
        }

        private void OnDeleteFolderOperationCompleted(object arg)
        {
            if ((this.DeleteFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteFolderCompleted(this, new DeleteFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/UpdateFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UpdateFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public UpdateFolderResponseType UpdateFolder([System.Xml.Serialization.XmlElementAttribute("UpdateFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] UpdateFolderType UpdateFolder1)
        {
            object[] results = this.Invoke("UpdateFolder", new object[] {
                    UpdateFolder1});
            return ((UpdateFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUpdateFolder(UpdateFolderType UpdateFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UpdateFolder", new object[] {
                    UpdateFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public UpdateFolderResponseType EndUpdateFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((UpdateFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void UpdateFolderAsync(UpdateFolderType UpdateFolder1)
        {
            this.UpdateFolderAsync(UpdateFolder1, null);
        }

        /// <remarks/>
        public void UpdateFolderAsync(UpdateFolderType UpdateFolder1, object userState)
        {
            if ((this.UpdateFolderOperationCompleted == null))
            {
                this.UpdateFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateFolderOperationCompleted);
            }
            this.InvokeAsync("UpdateFolder", new object[] {
                    UpdateFolder1}, this.UpdateFolderOperationCompleted, userState);
        }

        private void OnUpdateFolderOperationCompleted(object arg)
        {
            if ((this.UpdateFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateFolderCompleted(this, new UpdateFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/MoveFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("MoveFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public MoveFolderResponseType MoveFolder([System.Xml.Serialization.XmlElementAttribute("MoveFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] MoveFolderType MoveFolder1)
        {
            object[] results = this.Invoke("MoveFolder", new object[] {
                    MoveFolder1});
            return ((MoveFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginMoveFolder(MoveFolderType MoveFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("MoveFolder", new object[] {
                    MoveFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public MoveFolderResponseType EndMoveFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((MoveFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void MoveFolderAsync(MoveFolderType MoveFolder1)
        {
            this.MoveFolderAsync(MoveFolder1, null);
        }

        /// <remarks/>
        public void MoveFolderAsync(MoveFolderType MoveFolder1, object userState)
        {
            if ((this.MoveFolderOperationCompleted == null))
            {
                this.MoveFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMoveFolderOperationCompleted);
            }
            this.InvokeAsync("MoveFolder", new object[] {
                    MoveFolder1}, this.MoveFolderOperationCompleted, userState);
        }

        private void OnMoveFolderOperationCompleted(object arg)
        {
            if ((this.MoveFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MoveFolderCompleted(this, new MoveFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CopyFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CopyFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CopyFolderResponseType CopyFolder([System.Xml.Serialization.XmlElementAttribute("CopyFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CopyFolderType CopyFolder1)
        {
            object[] results = this.Invoke("CopyFolder", new object[] {
                    CopyFolder1});
            return ((CopyFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCopyFolder(CopyFolderType CopyFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CopyFolder", new object[] {
                    CopyFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public CopyFolderResponseType EndCopyFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CopyFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void CopyFolderAsync(CopyFolderType CopyFolder1)
        {
            this.CopyFolderAsync(CopyFolder1, null);
        }

        /// <remarks/>
        public void CopyFolderAsync(CopyFolderType CopyFolder1, object userState)
        {
            if ((this.CopyFolderOperationCompleted == null))
            {
                this.CopyFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCopyFolderOperationCompleted);
            }
            this.InvokeAsync("CopyFolder", new object[] {
                    CopyFolder1}, this.CopyFolderOperationCompleted, userState);
        }

        private void OnCopyFolderOperationCompleted(object arg)
        {
            if ((this.CopyFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CopyFolderCompleted(this, new CopyFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/Subscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SubscribeResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public SubscribeResponseType Subscribe([System.Xml.Serialization.XmlElementAttribute("Subscribe", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] SubscribeType Subscribe1)
        {
            object[] results = this.Invoke("Subscribe", new object[] {
                    Subscribe1});
            return ((SubscribeResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSubscribe(SubscribeType Subscribe1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Subscribe", new object[] {
                    Subscribe1}, callback, asyncState);
        }

        /// <remarks/>
        public SubscribeResponseType EndSubscribe(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SubscribeResponseType)(results[0]));
        }

        /// <remarks/>
        public void SubscribeAsync(SubscribeType Subscribe1)
        {
            this.SubscribeAsync(Subscribe1, null);
        }

        /// <remarks/>
        public void SubscribeAsync(SubscribeType Subscribe1, object userState)
        {
            if ((this.SubscribeOperationCompleted == null))
            {
                this.SubscribeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSubscribeOperationCompleted);
            }
            this.InvokeAsync("Subscribe", new object[] {
                    Subscribe1}, this.SubscribeOperationCompleted, userState);
        }

        private void OnSubscribeOperationCompleted(object arg)
        {
            if ((this.SubscribeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SubscribeCompleted(this, new SubscribeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/Unsubscribe", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public UnsubscribeResponseType Unsubscribe([System.Xml.Serialization.XmlElementAttribute("Unsubscribe", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] UnsubscribeType Unsubscribe1)
        {
            object[] results = this.Invoke("Unsubscribe", new object[] {
                    Unsubscribe1});
            return ((UnsubscribeResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUnsubscribe(UnsubscribeType Unsubscribe1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Unsubscribe", new object[] {
                    Unsubscribe1}, callback, asyncState);
        }

        /// <remarks/>
        public UnsubscribeResponseType EndUnsubscribe(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((UnsubscribeResponseType)(results[0]));
        }

        /// <remarks/>
        public void UnsubscribeAsync(UnsubscribeType Unsubscribe1)
        {
            this.UnsubscribeAsync(Unsubscribe1, null);
        }

        /// <remarks/>
        public void UnsubscribeAsync(UnsubscribeType Unsubscribe1, object userState)
        {
            if ((this.UnsubscribeOperationCompleted == null))
            {
                this.UnsubscribeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUnsubscribeOperationCompleted);
            }
            this.InvokeAsync("Unsubscribe", new object[] {
                    Unsubscribe1}, this.UnsubscribeOperationCompleted, userState);
        }

        private void OnUnsubscribeOperationCompleted(object arg)
        {
            if ((this.UnsubscribeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UnsubscribeCompleted(this, new UnsubscribeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetEvents", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetEventsResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetEventsResponseType GetEvents([System.Xml.Serialization.XmlElementAttribute("GetEvents", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetEventsType GetEvents1)
        {
            object[] results = this.Invoke("GetEvents", new object[] {
                    GetEvents1});
            return ((GetEventsResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetEvents(GetEventsType GetEvents1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetEvents", new object[] {
                    GetEvents1}, callback, asyncState);
        }

        /// <remarks/>
        public GetEventsResponseType EndGetEvents(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetEventsResponseType)(results[0]));
        }

        /// <remarks/>
        public void GetEventsAsync(GetEventsType GetEvents1)
        {
            this.GetEventsAsync(GetEvents1, null);
        }

        /// <remarks/>
        public void GetEventsAsync(GetEventsType GetEvents1, object userState)
        {
            if ((this.GetEventsOperationCompleted == null))
            {
                this.GetEventsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEventsOperationCompleted);
            }
            this.InvokeAsync("GetEvents", new object[] {
                    GetEvents1}, this.GetEventsOperationCompleted, userState);
        }

        private void OnGetEventsOperationCompleted(object arg)
        {
            if ((this.GetEventsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetEventsCompleted(this, new GetEventsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/SyncFolderHierarchy", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SyncFolderHierarchyResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public SyncFolderHierarchyResponseType SyncFolderHierarchy([System.Xml.Serialization.XmlElementAttribute("SyncFolderHierarchy", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] SyncFolderHierarchyType SyncFolderHierarchy1)
        {
            object[] results = this.Invoke("SyncFolderHierarchy", new object[] {
                    SyncFolderHierarchy1});
            return ((SyncFolderHierarchyResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSyncFolderHierarchy(SyncFolderHierarchyType SyncFolderHierarchy1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SyncFolderHierarchy", new object[] {
                    SyncFolderHierarchy1}, callback, asyncState);
        }

        /// <remarks/>
        public SyncFolderHierarchyResponseType EndSyncFolderHierarchy(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SyncFolderHierarchyResponseType)(results[0]));
        }

        /// <remarks/>
        public void SyncFolderHierarchyAsync(SyncFolderHierarchyType SyncFolderHierarchy1)
        {
            this.SyncFolderHierarchyAsync(SyncFolderHierarchy1, null);
        }

        /// <remarks/>
        public void SyncFolderHierarchyAsync(SyncFolderHierarchyType SyncFolderHierarchy1, object userState)
        {
            if ((this.SyncFolderHierarchyOperationCompleted == null))
            {
                this.SyncFolderHierarchyOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSyncFolderHierarchyOperationCompleted);
            }
            this.InvokeAsync("SyncFolderHierarchy", new object[] {
                    SyncFolderHierarchy1}, this.SyncFolderHierarchyOperationCompleted, userState);
        }

        private void OnSyncFolderHierarchyOperationCompleted(object arg)
        {
            if ((this.SyncFolderHierarchyCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SyncFolderHierarchyCompleted(this, new SyncFolderHierarchyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/SyncFolderItems", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SyncFolderItemsResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public SyncFolderItemsResponseType SyncFolderItems([System.Xml.Serialization.XmlElementAttribute("SyncFolderItems", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] SyncFolderItemsType SyncFolderItems1)
        {
            object[] results = this.Invoke("SyncFolderItems", new object[] {
                    SyncFolderItems1});
            return ((SyncFolderItemsResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSyncFolderItems(SyncFolderItemsType SyncFolderItems1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SyncFolderItems", new object[] {
                    SyncFolderItems1}, callback, asyncState);
        }

        /// <remarks/>
        public SyncFolderItemsResponseType EndSyncFolderItems(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SyncFolderItemsResponseType)(results[0]));
        }

        /// <remarks/>
        public void SyncFolderItemsAsync(SyncFolderItemsType SyncFolderItems1)
        {
            this.SyncFolderItemsAsync(SyncFolderItems1, null);
        }

        /// <remarks/>
        public void SyncFolderItemsAsync(SyncFolderItemsType SyncFolderItems1, object userState)
        {
            if ((this.SyncFolderItemsOperationCompleted == null))
            {
                this.SyncFolderItemsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSyncFolderItemsOperationCompleted);
            }
            this.InvokeAsync("SyncFolderItems", new object[] {
                    SyncFolderItems1}, this.SyncFolderItemsOperationCompleted, userState);
        }

        private void OnSyncFolderItemsOperationCompleted(object arg)
        {
            if ((this.SyncFolderItemsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SyncFolderItemsCompleted(this, new SyncFolderItemsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CreateManagedFolder", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CreateManagedFolderResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CreateManagedFolderResponseType CreateManagedFolder([System.Xml.Serialization.XmlElementAttribute("CreateManagedFolder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CreateManagedFolderRequestType CreateManagedFolder1)
        {
            object[] results = this.Invoke("CreateManagedFolder", new object[] {
                    CreateManagedFolder1});
            return ((CreateManagedFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCreateManagedFolder(CreateManagedFolderRequestType CreateManagedFolder1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CreateManagedFolder", new object[] {
                    CreateManagedFolder1}, callback, asyncState);
        }

        /// <remarks/>
        public CreateManagedFolderResponseType EndCreateManagedFolder(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CreateManagedFolderResponseType)(results[0]));
        }

        /// <remarks/>
        public void CreateManagedFolderAsync(CreateManagedFolderRequestType CreateManagedFolder1)
        {
            this.CreateManagedFolderAsync(CreateManagedFolder1, null);
        }

        /// <remarks/>
        public void CreateManagedFolderAsync(CreateManagedFolderRequestType CreateManagedFolder1, object userState)
        {
            if ((this.CreateManagedFolderOperationCompleted == null))
            {
                this.CreateManagedFolderOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateManagedFolderOperationCompleted);
            }
            this.InvokeAsync("CreateManagedFolder", new object[] {
                    CreateManagedFolder1}, this.CreateManagedFolderOperationCompleted, userState);
        }

        private void OnCreateManagedFolderOperationCompleted(object arg)
        {
            if ((this.CreateManagedFolderCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateManagedFolderCompleted(this, new CreateManagedFolderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetItemResponseType GetItem([System.Xml.Serialization.XmlElementAttribute("GetItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetItemType GetItem1)
        {
            object[] results = this.Invoke("GetItem", new object[] {
                    GetItem1});
            return ((GetItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetItem(GetItemType GetItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetItem", new object[] {
                    GetItem1}, callback, asyncState);
        }

        /// <remarks/>
        public GetItemResponseType EndGetItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void GetItemAsync(GetItemType GetItem1)
        {
            this.GetItemAsync(GetItem1, null);
        }

        /// <remarks/>
        public void GetItemAsync(GetItemType GetItem1, object userState)
        {
            if ((this.GetItemOperationCompleted == null))
            {
                this.GetItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetItemOperationCompleted);
            }
            this.InvokeAsync("GetItem", new object[] {
                    GetItem1}, this.GetItemOperationCompleted, userState);
        }

        private void OnGetItemOperationCompleted(object arg)
        {
            if ((this.GetItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetItemCompleted(this, new GetItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CreateItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CreateItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CreateItemResponseType CreateItem([System.Xml.Serialization.XmlElementAttribute("CreateItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CreateItemType CreateItem1)
        {
            object[] results = this.Invoke("CreateItem", new object[] {
                    CreateItem1});
            return ((CreateItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCreateItem(CreateItemType CreateItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CreateItem", new object[] {
                    CreateItem1}, callback, asyncState);
        }

        /// <remarks/>
        public CreateItemResponseType EndCreateItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CreateItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void CreateItemAsync(CreateItemType CreateItem1)
        {
            this.CreateItemAsync(CreateItem1, null);
        }

        /// <remarks/>
        public void CreateItemAsync(CreateItemType CreateItem1, object userState)
        {
            if ((this.CreateItemOperationCompleted == null))
            {
                this.CreateItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateItemOperationCompleted);
            }
            this.InvokeAsync("CreateItem", new object[] {
                    CreateItem1}, this.CreateItemOperationCompleted, userState);
        }

        private void OnCreateItemOperationCompleted(object arg)
        {
            if ((this.CreateItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateItemCompleted(this, new CreateItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/DeleteItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public DeleteItemResponseType DeleteItem([System.Xml.Serialization.XmlElementAttribute("DeleteItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] DeleteItemType DeleteItem1)
        {
            object[] results = this.Invoke("DeleteItem", new object[] {
                    DeleteItem1});
            return ((DeleteItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteItem(DeleteItemType DeleteItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteItem", new object[] {
                    DeleteItem1}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteItemResponseType EndDeleteItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void DeleteItemAsync(DeleteItemType DeleteItem1)
        {
            this.DeleteItemAsync(DeleteItem1, null);
        }

        /// <remarks/>
        public void DeleteItemAsync(DeleteItemType DeleteItem1, object userState)
        {
            if ((this.DeleteItemOperationCompleted == null))
            {
                this.DeleteItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteItemOperationCompleted);
            }
            this.InvokeAsync("DeleteItem", new object[] {
                    DeleteItem1}, this.DeleteItemOperationCompleted, userState);
        }

        private void OnDeleteItemOperationCompleted(object arg)
        {
            if ((this.DeleteItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteItemCompleted(this, new DeleteItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/UpdateItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UpdateItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public UpdateItemResponseType UpdateItem([System.Xml.Serialization.XmlElementAttribute("UpdateItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] UpdateItemType UpdateItem1)
        {
            object[] results = this.Invoke("UpdateItem", new object[] {
                    UpdateItem1});
            return ((UpdateItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUpdateItem(UpdateItemType UpdateItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UpdateItem", new object[] {
                    UpdateItem1}, callback, asyncState);
        }

        /// <remarks/>
        public UpdateItemResponseType EndUpdateItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((UpdateItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void UpdateItemAsync(UpdateItemType UpdateItem1)
        {
            this.UpdateItemAsync(UpdateItem1, null);
        }

        /// <remarks/>
        public void UpdateItemAsync(UpdateItemType UpdateItem1, object userState)
        {
            if ((this.UpdateItemOperationCompleted == null))
            {
                this.UpdateItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateItemOperationCompleted);
            }
            this.InvokeAsync("UpdateItem", new object[] {
                    UpdateItem1}, this.UpdateItemOperationCompleted, userState);
        }

        private void OnUpdateItemOperationCompleted(object arg)
        {
            if ((this.UpdateItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateItemCompleted(this, new UpdateItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/SendItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SendItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public SendItemResponseType SendItem([System.Xml.Serialization.XmlElementAttribute("SendItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] SendItemType SendItem1)
        {
            object[] results = this.Invoke("SendItem", new object[] {
                    SendItem1});
            return ((SendItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSendItem(SendItemType SendItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SendItem", new object[] {
                    SendItem1}, callback, asyncState);
        }

        /// <remarks/>
        public SendItemResponseType EndSendItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SendItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void SendItemAsync(SendItemType SendItem1)
        {
            this.SendItemAsync(SendItem1, null);
        }

        /// <remarks/>
        public void SendItemAsync(SendItemType SendItem1, object userState)
        {
            if ((this.SendItemOperationCompleted == null))
            {
                this.SendItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendItemOperationCompleted);
            }
            this.InvokeAsync("SendItem", new object[] {
                    SendItem1}, this.SendItemOperationCompleted, userState);
        }

        private void OnSendItemOperationCompleted(object arg)
        {
            if ((this.SendItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendItemCompleted(this, new SendItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/MoveItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("MoveItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public MoveItemResponseType MoveItem([System.Xml.Serialization.XmlElementAttribute("MoveItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] MoveItemType MoveItem1)
        {
            object[] results = this.Invoke("MoveItem", new object[] {
                    MoveItem1});
            return ((MoveItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginMoveItem(MoveItemType MoveItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("MoveItem", new object[] {
                    MoveItem1}, callback, asyncState);
        }

        /// <remarks/>
        public MoveItemResponseType EndMoveItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((MoveItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void MoveItemAsync(MoveItemType MoveItem1)
        {
            this.MoveItemAsync(MoveItem1, null);
        }

        /// <remarks/>
        public void MoveItemAsync(MoveItemType MoveItem1, object userState)
        {
            if ((this.MoveItemOperationCompleted == null))
            {
                this.MoveItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMoveItemOperationCompleted);
            }
            this.InvokeAsync("MoveItem", new object[] {
                    MoveItem1}, this.MoveItemOperationCompleted, userState);
        }

        private void OnMoveItemOperationCompleted(object arg)
        {
            if ((this.MoveItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MoveItemCompleted(this, new MoveItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CopyItem", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CopyItemResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CopyItemResponseType CopyItem([System.Xml.Serialization.XmlElementAttribute("CopyItem", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CopyItemType CopyItem1)
        {
            object[] results = this.Invoke("CopyItem", new object[] {
                    CopyItem1});
            return ((CopyItemResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCopyItem(CopyItemType CopyItem1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CopyItem", new object[] {
                    CopyItem1}, callback, asyncState);
        }

        /// <remarks/>
        public CopyItemResponseType EndCopyItem(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CopyItemResponseType)(results[0]));
        }

        /// <remarks/>
        public void CopyItemAsync(CopyItemType CopyItem1)
        {
            this.CopyItemAsync(CopyItem1, null);
        }

        /// <remarks/>
        public void CopyItemAsync(CopyItemType CopyItem1, object userState)
        {
            if ((this.CopyItemOperationCompleted == null))
            {
                this.CopyItemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCopyItemOperationCompleted);
            }
            this.InvokeAsync("CopyItem", new object[] {
                    CopyItem1}, this.CopyItemOperationCompleted, userState);
        }

        private void OnCopyItemOperationCompleted(object arg)
        {
            if ((this.CopyItemCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CopyItemCompleted(this, new CopyItemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/CreateAttachment", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("CreateAttachmentResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public CreateAttachmentResponseType CreateAttachment([System.Xml.Serialization.XmlElementAttribute("CreateAttachment", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] CreateAttachmentType CreateAttachment1)
        {
            object[] results = this.Invoke("CreateAttachment", new object[] {
                    CreateAttachment1});
            return ((CreateAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCreateAttachment(CreateAttachmentType CreateAttachment1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CreateAttachment", new object[] {
                    CreateAttachment1}, callback, asyncState);
        }

        /// <remarks/>
        public CreateAttachmentResponseType EndCreateAttachment(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((CreateAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public void CreateAttachmentAsync(CreateAttachmentType CreateAttachment1)
        {
            this.CreateAttachmentAsync(CreateAttachment1, null);
        }

        /// <remarks/>
        public void CreateAttachmentAsync(CreateAttachmentType CreateAttachment1, object userState)
        {
            if ((this.CreateAttachmentOperationCompleted == null))
            {
                this.CreateAttachmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateAttachmentOperationCompleted);
            }
            this.InvokeAsync("CreateAttachment", new object[] {
                    CreateAttachment1}, this.CreateAttachmentOperationCompleted, userState);
        }

        private void OnCreateAttachmentOperationCompleted(object arg)
        {
            if ((this.CreateAttachmentCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateAttachmentCompleted(this, new CreateAttachmentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/DeleteAttachment", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("DeleteAttachmentResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public DeleteAttachmentResponseType DeleteAttachment([System.Xml.Serialization.XmlElementAttribute("DeleteAttachment", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] DeleteAttachmentType DeleteAttachment1)
        {
            object[] results = this.Invoke("DeleteAttachment", new object[] {
                    DeleteAttachment1});
            return ((DeleteAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginDeleteAttachment(DeleteAttachmentType DeleteAttachment1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("DeleteAttachment", new object[] {
                    DeleteAttachment1}, callback, asyncState);
        }

        /// <remarks/>
        public DeleteAttachmentResponseType EndDeleteAttachment(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((DeleteAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public void DeleteAttachmentAsync(DeleteAttachmentType DeleteAttachment1)
        {
            this.DeleteAttachmentAsync(DeleteAttachment1, null);
        }

        /// <remarks/>
        public void DeleteAttachmentAsync(DeleteAttachmentType DeleteAttachment1, object userState)
        {
            if ((this.DeleteAttachmentOperationCompleted == null))
            {
                this.DeleteAttachmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteAttachmentOperationCompleted);
            }
            this.InvokeAsync("DeleteAttachment", new object[] {
                    DeleteAttachment1}, this.DeleteAttachmentOperationCompleted, userState);
        }

        private void OnDeleteAttachmentOperationCompleted(object arg)
        {
            if ((this.DeleteAttachmentCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteAttachmentCompleted(this, new DeleteAttachmentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetAttachment", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetAttachmentResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetAttachmentResponseType GetAttachment([System.Xml.Serialization.XmlElementAttribute("GetAttachment", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetAttachmentType GetAttachment1)
        {
            object[] results = this.Invoke("GetAttachment", new object[] {
                    GetAttachment1});
            return ((GetAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetAttachment(GetAttachmentType GetAttachment1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetAttachment", new object[] {
                    GetAttachment1}, callback, asyncState);
        }

        /// <remarks/>
        public GetAttachmentResponseType EndGetAttachment(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetAttachmentResponseType)(results[0]));
        }

        /// <remarks/>
        public void GetAttachmentAsync(GetAttachmentType GetAttachment1)
        {
            this.GetAttachmentAsync(GetAttachment1, null);
        }

        /// <remarks/>
        public void GetAttachmentAsync(GetAttachmentType GetAttachment1, object userState)
        {
            if ((this.GetAttachmentOperationCompleted == null))
            {
                this.GetAttachmentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetAttachmentOperationCompleted);
            }
            this.InvokeAsync("GetAttachment", new object[] {
                    GetAttachment1}, this.GetAttachmentOperationCompleted, userState);
        }

        private void OnGetAttachmentOperationCompleted(object arg)
        {
            if ((this.GetAttachmentCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetAttachmentCompleted(this, new GetAttachmentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetDelegate", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetDelegateResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetDelegateResponseMessageType GetDelegate([System.Xml.Serialization.XmlElementAttribute("GetDelegate", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetDelegateType GetDelegate1)
        {
            object[] results = this.Invoke("GetDelegate", new object[] {
                    GetDelegate1});
            return ((GetDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetDelegate(GetDelegateType GetDelegate1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetDelegate", new object[] {
                    GetDelegate1}, callback, asyncState);
        }

        /// <remarks/>
        public GetDelegateResponseMessageType EndGetDelegate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public void GetDelegateAsync(GetDelegateType GetDelegate1)
        {
            this.GetDelegateAsync(GetDelegate1, null);
        }

        /// <remarks/>
        public void GetDelegateAsync(GetDelegateType GetDelegate1, object userState)
        {
            if ((this.GetDelegateOperationCompleted == null))
            {
                this.GetDelegateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetDelegateOperationCompleted);
            }
            this.InvokeAsync("GetDelegate", new object[] {
                    GetDelegate1}, this.GetDelegateOperationCompleted, userState);
        }

        private void OnGetDelegateOperationCompleted(object arg)
        {
            if ((this.GetDelegateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetDelegateCompleted(this, new GetDelegateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/AddDelegate", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("AddDelegateResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public AddDelegateResponseMessageType AddDelegate([System.Xml.Serialization.XmlElementAttribute("AddDelegate", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] AddDelegateType AddDelegate1)
        {
            object[] results = this.Invoke("AddDelegate", new object[] {
                    AddDelegate1});
            return ((AddDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddDelegate(AddDelegateType AddDelegate1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddDelegate", new object[] {
                    AddDelegate1}, callback, asyncState);
        }

        /// <remarks/>
        public AddDelegateResponseMessageType EndAddDelegate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((AddDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public void AddDelegateAsync(AddDelegateType AddDelegate1)
        {
            this.AddDelegateAsync(AddDelegate1, null);
        }

        /// <remarks/>
        public void AddDelegateAsync(AddDelegateType AddDelegate1, object userState)
        {
            if ((this.AddDelegateOperationCompleted == null))
            {
                this.AddDelegateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddDelegateOperationCompleted);
            }
            this.InvokeAsync("AddDelegate", new object[] {
                    AddDelegate1}, this.AddDelegateOperationCompleted, userState);
        }

        private void OnAddDelegateOperationCompleted(object arg)
        {
            if ((this.AddDelegateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddDelegateCompleted(this, new AddDelegateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/RemoveDelegate", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("RemoveDelegateResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public RemoveDelegateResponseMessageType RemoveDelegate([System.Xml.Serialization.XmlElementAttribute("RemoveDelegate", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] RemoveDelegateType RemoveDelegate1)
        {
            object[] results = this.Invoke("RemoveDelegate", new object[] {
                    RemoveDelegate1});
            return ((RemoveDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRemoveDelegate(RemoveDelegateType RemoveDelegate1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("RemoveDelegate", new object[] {
                    RemoveDelegate1}, callback, asyncState);
        }

        /// <remarks/>
        public RemoveDelegateResponseMessageType EndRemoveDelegate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((RemoveDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public void RemoveDelegateAsync(RemoveDelegateType RemoveDelegate1)
        {
            this.RemoveDelegateAsync(RemoveDelegate1, null);
        }

        /// <remarks/>
        public void RemoveDelegateAsync(RemoveDelegateType RemoveDelegate1, object userState)
        {
            if ((this.RemoveDelegateOperationCompleted == null))
            {
                this.RemoveDelegateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRemoveDelegateOperationCompleted);
            }
            this.InvokeAsync("RemoveDelegate", new object[] {
                    RemoveDelegate1}, this.RemoveDelegateOperationCompleted, userState);
        }

        private void OnRemoveDelegateOperationCompleted(object arg)
        {
            if ((this.RemoveDelegateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RemoveDelegateCompleted(this, new RemoveDelegateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("MailboxCulture")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ExchangeImpersonation")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("RequestServerVersionValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/UpdateDelegate", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("UpdateDelegateResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public UpdateDelegateResponseMessageType UpdateDelegate([System.Xml.Serialization.XmlElementAttribute("UpdateDelegate", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] UpdateDelegateType UpdateDelegate1)
        {
            object[] results = this.Invoke("UpdateDelegate", new object[] {
                    UpdateDelegate1});
            return ((UpdateDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUpdateDelegate(UpdateDelegateType UpdateDelegate1, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UpdateDelegate", new object[] {
                    UpdateDelegate1}, callback, asyncState);
        }

        /// <remarks/>
        public UpdateDelegateResponseMessageType EndUpdateDelegate(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((UpdateDelegateResponseMessageType)(results[0]));
        }

        /// <remarks/>
        public void UpdateDelegateAsync(UpdateDelegateType UpdateDelegate1)
        {
            this.UpdateDelegateAsync(UpdateDelegate1, null);
        }

        /// <remarks/>
        public void UpdateDelegateAsync(UpdateDelegateType UpdateDelegate1, object userState)
        {
            if ((this.UpdateDelegateOperationCompleted == null))
            {
                this.UpdateDelegateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUpdateDelegateOperationCompleted);
            }
            this.InvokeAsync("UpdateDelegate", new object[] {
                    UpdateDelegate1}, this.UpdateDelegateOperationCompleted, userState);
        }

        private void OnUpdateDelegateOperationCompleted(object arg)
        {
            if ((this.UpdateDelegateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.UpdateDelegateCompleted(this, new UpdateDelegateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapHeaderAttribute("SerializedSecurityContext")]
        [System.Web.Services.Protocols.SoapHeaderAttribute("ProxyRequestTypeHeader")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetUserAvailability", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetUserAvailabilityResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetUserAvailabilityResponseType GetUserAvailability([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetUserAvailabilityRequestType GetUserAvailabilityRequest)
        {
            object[] results = this.Invoke("GetUserAvailability", new object[] {
                    GetUserAvailabilityRequest});
            return ((GetUserAvailabilityResponseType)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetUserAvailability(GetUserAvailabilityRequestType GetUserAvailabilityRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetUserAvailability", new object[] {
                    GetUserAvailabilityRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetUserAvailabilityResponseType EndGetUserAvailability(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetUserAvailabilityResponseType)(results[0]));
        }

        /// <remarks/>
        public void GetUserAvailabilityAsync(GetUserAvailabilityRequestType GetUserAvailabilityRequest)
        {
            this.GetUserAvailabilityAsync(GetUserAvailabilityRequest, null);
        }

        /// <remarks/>
        public void GetUserAvailabilityAsync(GetUserAvailabilityRequestType GetUserAvailabilityRequest, object userState)
        {
            if ((this.GetUserAvailabilityOperationCompleted == null))
            {
                this.GetUserAvailabilityOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserAvailabilityOperationCompleted);
            }
            this.InvokeAsync("GetUserAvailability", new object[] {
                    GetUserAvailabilityRequest}, this.GetUserAvailabilityOperationCompleted, userState);
        }

        private void OnGetUserAvailabilityOperationCompleted(object arg)
        {
            if ((this.GetUserAvailabilityCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserAvailabilityCompleted(this, new GetUserAvailabilityCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/GetUserOofSettings", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("GetUserOofSettingsResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public GetUserOofSettingsResponse GetUserOofSettings([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] GetUserOofSettingsRequest GetUserOofSettingsRequest)
        {
            object[] results = this.Invoke("GetUserOofSettings", new object[] {
                    GetUserOofSettingsRequest});
            return ((GetUserOofSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetUserOofSettings(GetUserOofSettingsRequest GetUserOofSettingsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetUserOofSettings", new object[] {
                    GetUserOofSettingsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public GetUserOofSettingsResponse EndGetUserOofSettings(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((GetUserOofSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public void GetUserOofSettingsAsync(GetUserOofSettingsRequest GetUserOofSettingsRequest)
        {
            this.GetUserOofSettingsAsync(GetUserOofSettingsRequest, null);
        }

        /// <remarks/>
        public void GetUserOofSettingsAsync(GetUserOofSettingsRequest GetUserOofSettingsRequest, object userState)
        {
            if ((this.GetUserOofSettingsOperationCompleted == null))
            {
                this.GetUserOofSettingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUserOofSettingsOperationCompleted);
            }
            this.InvokeAsync("GetUserOofSettings", new object[] {
                    GetUserOofSettingsRequest}, this.GetUserOofSettingsOperationCompleted, userState);
        }

        private void OnGetUserOofSettingsOperationCompleted(object arg)
        {
            if ((this.GetUserOofSettingsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUserOofSettingsCompleted(this, new GetUserOofSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServerVersionInfoValue", Direction = System.Web.Services.Protocols.SoapHeaderDirection.Out)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/exchange/services/2006/messages/SetUserOofSettings", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("SetUserOofSettingsResponse", Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
        public SetUserOofSettingsResponse SetUserOofSettings([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")] SetUserOofSettingsRequest SetUserOofSettingsRequest)
        {
            object[] results = this.Invoke("SetUserOofSettings", new object[] {
                    SetUserOofSettingsRequest});
            return ((SetUserOofSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSetUserOofSettings(SetUserOofSettingsRequest SetUserOofSettingsRequest, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SetUserOofSettings", new object[] {
                    SetUserOofSettingsRequest}, callback, asyncState);
        }

        /// <remarks/>
        public SetUserOofSettingsResponse EndSetUserOofSettings(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((SetUserOofSettingsResponse)(results[0]));
        }

        /// <remarks/>
        public void SetUserOofSettingsAsync(SetUserOofSettingsRequest SetUserOofSettingsRequest)
        {
            this.SetUserOofSettingsAsync(SetUserOofSettingsRequest, null);
        }

        /// <remarks/>
        public void SetUserOofSettingsAsync(SetUserOofSettingsRequest SetUserOofSettingsRequest, object userState)
        {
            if ((this.SetUserOofSettingsOperationCompleted == null))
            {
                this.SetUserOofSettingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetUserOofSettingsOperationCompleted);
            }
            this.InvokeAsync("SetUserOofSettings", new object[] {
                    SetUserOofSettingsRequest}, this.SetUserOofSettingsOperationCompleted, userState);
        }

        private void OnSetUserOofSettingsOperationCompleted(object arg)
        {
            if ((this.SetUserOofSettingsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetUserOofSettingsCompleted(this, new SetUserOofSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class ServerVersionInfo : System.Web.Services.Protocols.SoapHeader
    {

        private int majorVersionField;

        private bool majorVersionFieldSpecified;

        private int minorVersionField;

        private bool minorVersionFieldSpecified;

        private int majorBuildNumberField;

        private bool majorBuildNumberFieldSpecified;

        private int minorBuildNumberField;

        private bool minorBuildNumberFieldSpecified;

        private string versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MajorVersion
        {
            get
            {
                return this.majorVersionField;
            }
            set
            {
                this.majorVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MajorVersionSpecified
        {
            get
            {
                return this.majorVersionFieldSpecified;
            }
            set
            {
                this.majorVersionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MinorVersion
        {
            get
            {
                return this.minorVersionField;
            }
            set
            {
                this.minorVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinorVersionSpecified
        {
            get
            {
                return this.minorVersionFieldSpecified;
            }
            set
            {
                this.minorVersionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MajorBuildNumber
        {
            get
            {
                return this.majorBuildNumberField;
            }
            set
            {
                this.majorBuildNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MajorBuildNumberSpecified
        {
            get
            {
                return this.majorBuildNumberFieldSpecified;
            }
            set
            {
                this.majorBuildNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MinorBuildNumber
        {
            get
            {
                return this.minorBuildNumberField;
            }
            set
            {
                this.minorBuildNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinorBuildNumberSpecified
        {
            get
            {
                return this.minorBuildNumberFieldSpecified;
            }
            set
            {
                this.minorBuildNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SetUserOofSettingsResponse
    {

        private ResponseMessageType responseMessageField;

        /// <remarks/>
        public ResponseMessageType ResponseMessage
        {
            get
            {
                return this.responseMessageField;
            }
            set
            {
                this.responseMessageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DelegateUserResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConvertIdResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderItemsResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderHierarchyResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SendNotificationResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetEventsResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SubscribeResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExpandDLResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolveNamesResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindItemResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteAttachmentResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentInfoResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemInfoResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateItemResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindFolderResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FolderInfoResponseMessageType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ResponseMessageType
    {

        private string messageTextField;

        private ResponseCodeType responseCodeField;

        private bool responseCodeFieldSpecified;

        private int descriptiveLinkKeyField;

        private bool descriptiveLinkKeyFieldSpecified;

        private ResponseMessageTypeMessageXml messageXmlField;

        private ResponseClassType responseClassField;

        /// <remarks/>
        public string MessageText
        {
            get
            {
                return this.messageTextField;
            }
            set
            {
                this.messageTextField = value;
            }
        }

        /// <remarks/>
        public ResponseCodeType ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ResponseCodeSpecified
        {
            get
            {
                return this.responseCodeFieldSpecified;
            }
            set
            {
                this.responseCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int DescriptiveLinkKey
        {
            get
            {
                return this.descriptiveLinkKeyField;
            }
            set
            {
                this.descriptiveLinkKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DescriptiveLinkKeySpecified
        {
            get
            {
                return this.descriptiveLinkKeyFieldSpecified;
            }
            set
            {
                this.descriptiveLinkKeyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ResponseMessageTypeMessageXml MessageXml
        {
            get
            {
                return this.messageXmlField;
            }
            set
            {
                this.messageXmlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ResponseClassType ResponseClass
        {
            get
            {
                return this.responseClassField;
            }
            set
            {
                this.responseClassField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public enum ResponseCodeType
    {

        /// <remarks/>
        NoError,

        /// <remarks/>
        ErrorAccessDenied,

        /// <remarks/>
        ErrorAccountDisabled,

        /// <remarks/>
        ErrorAddDelegatesFailed,

        /// <remarks/>
        ErrorAddressSpaceNotFound,

        /// <remarks/>
        ErrorADOperation,

        /// <remarks/>
        ErrorADSessionFilter,

        /// <remarks/>
        ErrorADUnavailable,

        /// <remarks/>
        ErrorAutoDiscoverFailed,

        /// <remarks/>
        ErrorAffectedTaskOccurrencesRequired,

        /// <remarks/>
        ErrorAttachmentSizeLimitExceeded,

        /// <remarks/>
        ErrorAvailabilityConfigNotFound,

        /// <remarks/>
        ErrorBatchProcessingStopped,

        /// <remarks/>
        ErrorCalendarCannotMoveOrCopyOccurrence,

        /// <remarks/>
        ErrorCalendarCannotUpdateDeletedItem,

        /// <remarks/>
        ErrorCalendarCannotUseIdForOccurrenceId,

        /// <remarks/>
        ErrorCalendarCannotUseIdForRecurringMasterId,

        /// <remarks/>
        ErrorCalendarDurationIsTooLong,

        /// <remarks/>
        ErrorCalendarEndDateIsEarlierThanStartDate,

        /// <remarks/>
        ErrorCalendarFolderIsInvalidForCalendarView,

        /// <remarks/>
        ErrorCalendarInvalidAttributeValue,

        /// <remarks/>
        ErrorCalendarInvalidDayForTimeChangePattern,

        /// <remarks/>
        ErrorCalendarInvalidDayForWeeklyRecurrence,

        /// <remarks/>
        ErrorCalendarInvalidPropertyState,

        /// <remarks/>
        ErrorCalendarInvalidPropertyValue,

        /// <remarks/>
        ErrorCalendarInvalidRecurrence,

        /// <remarks/>
        ErrorCalendarInvalidTimeZone,

        /// <remarks/>
        ErrorCalendarIsDelegatedForAccept,

        /// <remarks/>
        ErrorCalendarIsDelegatedForDecline,

        /// <remarks/>
        ErrorCalendarIsDelegatedForRemove,

        /// <remarks/>
        ErrorCalendarIsDelegatedForTentative,

        /// <remarks/>
        ErrorCalendarIsNotOrganizer,

        /// <remarks/>
        ErrorCalendarIsOrganizerForAccept,

        /// <remarks/>
        ErrorCalendarIsOrganizerForDecline,

        /// <remarks/>
        ErrorCalendarIsOrganizerForRemove,

        /// <remarks/>
        ErrorCalendarIsOrganizerForTentative,

        /// <remarks/>
        ErrorCalendarOccurrenceIndexIsOutOfRecurrenceRange,

        /// <remarks/>
        ErrorCalendarOccurrenceIsDeletedFromRecurrence,

        /// <remarks/>
        ErrorCalendarOutOfRange,

        /// <remarks/>
        ErrorCalendarMeetingRequestIsOutOfDate,

        /// <remarks/>
        ErrorCalendarViewRangeTooBig,

        /// <remarks/>
        ErrorCannotCreateCalendarItemInNonCalendarFolder,

        /// <remarks/>
        ErrorCannotCreateContactInNonContactFolder,

        /// <remarks/>
        ErrorCannotCreatePostItemInNonMailFolder,

        /// <remarks/>
        ErrorCannotCreateTaskInNonTaskFolder,

        /// <remarks/>
        ErrorCannotDeleteObject,

        /// <remarks/>
        ErrorCannotOpenFileAttachment,

        /// <remarks/>
        ErrorCannotDeleteTaskOccurrence,

        /// <remarks/>
        ErrorCannotSetCalendarPermissionOnNonCalendarFolder,

        /// <remarks/>
        ErrorCannotSetNonCalendarPermissionOnCalendarFolder,

        /// <remarks/>
        ErrorCannotSetPermissionUnknownEntries,

        /// <remarks/>
        ErrorCannotUseFolderIdForItemId,

        /// <remarks/>
        ErrorCannotUseItemIdForFolderId,

        /// <remarks/>
        ErrorChangeKeyRequired,

        /// <remarks/>
        ErrorChangeKeyRequiredForWriteOperations,

        /// <remarks/>
        ErrorConnectionFailed,

        /// <remarks/>
        ErrorContentConversionFailed,

        /// <remarks/>
        ErrorCorruptData,

        /// <remarks/>
        ErrorCreateItemAccessDenied,

        /// <remarks/>
        ErrorCreateManagedFolderPartialCompletion,

        /// <remarks/>
        ErrorCreateSubfolderAccessDenied,

        /// <remarks/>
        ErrorCrossMailboxMoveCopy,

        /// <remarks/>
        ErrorDataSizeLimitExceeded,

        /// <remarks/>
        ErrorDataSourceOperation,

        /// <remarks/>
        ErrorDelegateAlreadyExists,

        /// <remarks/>
        ErrorDelegateCannotAddOwner,

        /// <remarks/>
        ErrorDelegateMissingConfiguration,

        /// <remarks/>
        ErrorDelegateNoUser,

        /// <remarks/>
        ErrorDelegateValidationFailed,

        /// <remarks/>
        ErrorDeleteDistinguishedFolder,

        /// <remarks/>
        ErrorDeleteItemsFailed,

        /// <remarks/>
        ErrorDistinguishedUserNotSupported,

        /// <remarks/>
        ErrorDuplicateInputFolderNames,

        /// <remarks/>
        ErrorDuplicateUserIdsSpecified,

        /// <remarks/>
        ErrorEmailAddressMismatch,

        /// <remarks/>
        ErrorEventNotFound,

        /// <remarks/>
        ErrorExpiredSubscription,

        /// <remarks/>
        ErrorFolderCorrupt,

        /// <remarks/>
        ErrorFolderNotFound,

        /// <remarks/>
        ErrorFolderPropertRequestFailed,

        /// <remarks/>
        ErrorFolderSave,

        /// <remarks/>
        ErrorFolderSaveFailed,

        /// <remarks/>
        ErrorFolderSavePropertyError,

        /// <remarks/>
        ErrorFolderExists,

        /// <remarks/>
        ErrorFreeBusyGenerationFailed,

        /// <remarks/>
        ErrorGetServerSecurityDescriptorFailed,

        /// <remarks/>
        ErrorImpersonateUserDenied,

        /// <remarks/>
        ErrorImpersonationDenied,

        /// <remarks/>
        ErrorImpersonationFailed,

        /// <remarks/>
        ErrorIncorrectSchemaVersion,

        /// <remarks/>
        ErrorIncorrectUpdatePropertyCount,

        /// <remarks/>
        ErrorIndividualMailboxLimitReached,

        /// <remarks/>
        ErrorInsufficientResources,

        /// <remarks/>
        ErrorInternalServerError,

        /// <remarks/>
        ErrorInternalServerTransientError,

        /// <remarks/>
        ErrorInvalidAccessLevel,

        /// <remarks/>
        ErrorInvalidAttachmentId,

        /// <remarks/>
        ErrorInvalidAttachmentSubfilter,

        /// <remarks/>
        ErrorInvalidAttachmentSubfilterTextFilter,

        /// <remarks/>
        ErrorInvalidAuthorizationContext,

        /// <remarks/>
        ErrorInvalidChangeKey,

        /// <remarks/>
        ErrorInvalidClientSecurityContext,

        /// <remarks/>
        ErrorInvalidCompleteDate,

        /// <remarks/>
        ErrorInvalidCrossForestCredentials,

        /// <remarks/>
        ErrorInvalidDelegatePermission,

        /// <remarks/>
        ErrorInvalidDelegateUserId,

        /// <remarks/>
        ErrorInvalidExcludesRestriction,

        /// <remarks/>
        ErrorInvalidExpressionTypeForSubFilter,

        /// <remarks/>
        ErrorInvalidExtendedProperty,

        /// <remarks/>
        ErrorInvalidExtendedPropertyValue,

        /// <remarks/>
        ErrorInvalidFolderId,

        /// <remarks/>
        ErrorInvalidFolderTypeForOperation,

        /// <remarks/>
        ErrorInvalidFractionalPagingParameters,

        /// <remarks/>
        ErrorInvalidFreeBusyViewType,

        /// <remarks/>
        ErrorInvalidId,

        /// <remarks/>
        ErrorInvalidIdEmpty,

        /// <remarks/>
        ErrorInvalidIdMalformed,

        /// <remarks/>
        ErrorInvalidIdMalformedEwsLegacyIdFormat,

        /// <remarks/>
        ErrorInvalidIdMonikerTooLong,

        /// <remarks/>
        ErrorInvalidIdNotAnItemAttachmentId,

        /// <remarks/>
        ErrorInvalidIdReturnedByResolveNames,

        /// <remarks/>
        ErrorInvalidIdStoreObjectIdTooLong,

        /// <remarks/>
        ErrorInvalidIdTooManyAttachmentLevels,

        /// <remarks/>
        ErrorInvalidIdXml,

        /// <remarks/>
        ErrorInvalidIndexedPagingParameters,

        /// <remarks/>
        ErrorInvalidInternetHeaderChildNodes,

        /// <remarks/>
        ErrorInvalidItemForOperationCreateItemAttachment,

        /// <remarks/>
        ErrorInvalidItemForOperationCreateItem,

        /// <remarks/>
        ErrorInvalidItemForOperationAcceptItem,

        /// <remarks/>
        ErrorInvalidItemForOperationDeclineItem,

        /// <remarks/>
        ErrorInvalidItemForOperationCancelItem,

        /// <remarks/>
        ErrorInvalidItemForOperationExpandDL,

        /// <remarks/>
        ErrorInvalidItemForOperationRemoveItem,

        /// <remarks/>
        ErrorInvalidItemForOperationSendItem,

        /// <remarks/>
        ErrorInvalidItemForOperationTentative,

        /// <remarks/>
        ErrorInvalidManagedFolderProperty,

        /// <remarks/>
        ErrorInvalidManagedFolderQuota,

        /// <remarks/>
        ErrorInvalidManagedFolderSize,

        /// <remarks/>
        ErrorInvalidMergedFreeBusyInterval,

        /// <remarks/>
        ErrorInvalidNameForNameResolution,

        /// <remarks/>
        ErrorInvalidOperation,

        /// <remarks/>
        ErrorInvalidNetworkServiceContext,

        /// <remarks/>
        ErrorInvalidOofParameter,

        /// <remarks/>
        ErrorInvalidPagingMaxRows,

        /// <remarks/>
        ErrorInvalidParentFolder,

        /// <remarks/>
        ErrorInvalidPercentCompleteValue,

        /// <remarks/>
        ErrorInvalidPermissionSettings,

        /// <remarks/>
        ErrorInvalidUserInfo,

        /// <remarks/>
        ErrorInvalidPropertyAppend,

        /// <remarks/>
        ErrorInvalidPropertyDelete,

        /// <remarks/>
        ErrorInvalidPropertyForExists,

        /// <remarks/>
        ErrorInvalidPropertyForOperation,

        /// <remarks/>
        ErrorInvalidPropertyRequest,

        /// <remarks/>
        ErrorInvalidPropertySet,

        /// <remarks/>
        ErrorInvalidPropertyUpdateSentMessage,

        /// <remarks/>
        ErrorInvalidProxySecurityContext,

        /// <remarks/>
        ErrorInvalidPullSubscriptionId,

        /// <remarks/>
        ErrorInvalidPushSubscriptionUrl,

        /// <remarks/>
        ErrorInvalidRecipients,

        /// <remarks/>
        ErrorInvalidRecipientSubfilter,

        /// <remarks/>
        ErrorInvalidRecipientSubfilterComparison,

        /// <remarks/>
        ErrorInvalidRecipientSubfilterOrder,

        /// <remarks/>
        ErrorInvalidRecipientSubfilterTextFilter,

        /// <remarks/>
        ErrorInvalidReferenceItem,

        /// <remarks/>
        ErrorInvalidRequest,

        /// <remarks/>
        ErrorInvalidRestriction,

        /// <remarks/>
        ErrorInvalidRoutingType,

        /// <remarks/>
        ErrorInvalidScheduledOofDuration,

        /// <remarks/>
        ErrorInvalidSecurityDescriptor,

        /// <remarks/>
        ErrorInvalidSendItemSaveSettings,

        /// <remarks/>
        ErrorInvalidSerializedAccessToken,

        /// <remarks/>
        ErrorInvalidServerVersion,

        /// <remarks/>
        ErrorInvalidSid,

        /// <remarks/>
        ErrorInvalidSmtpAddress,

        /// <remarks/>
        ErrorInvalidSubfilterType,

        /// <remarks/>
        ErrorInvalidSubfilterTypeNotAttendeeType,

        /// <remarks/>
        ErrorInvalidSubfilterTypeNotRecipientType,

        /// <remarks/>
        ErrorInvalidSubscription,

        /// <remarks/>
        ErrorInvalidSubscriptionRequest,

        /// <remarks/>
        ErrorInvalidSyncStateData,

        /// <remarks/>
        ErrorInvalidTimeInterval,

        /// <remarks/>
        ErrorInvalidUserOofSettings,

        /// <remarks/>
        ErrorInvalidUserPrincipalName,

        /// <remarks/>
        ErrorInvalidUserSid,

        /// <remarks/>
        ErrorInvalidUserSidMissingUPN,

        /// <remarks/>
        ErrorInvalidValueForProperty,

        /// <remarks/>
        ErrorInvalidWatermark,

        /// <remarks/>
        ErrorIrresolvableConflict,

        /// <remarks/>
        ErrorItemCorrupt,

        /// <remarks/>
        ErrorItemNotFound,

        /// <remarks/>
        ErrorItemPropertyRequestFailed,

        /// <remarks/>
        ErrorItemSave,

        /// <remarks/>
        ErrorItemSavePropertyError,

        /// <remarks/>
        ErrorLegacyMailboxFreeBusyViewTypeNotMerged,

        /// <remarks/>
        ErrorLocalServerObjectNotFound,

        /// <remarks/>
        ErrorLogonAsNetworkServiceFailed,

        /// <remarks/>
        ErrorMailboxConfiguration,

        /// <remarks/>
        ErrorMailboxDataArrayEmpty,

        /// <remarks/>
        ErrorMailboxDataArrayTooBig,

        /// <remarks/>
        ErrorMailboxLogonFailed,

        /// <remarks/>
        ErrorMailboxMoveInProgress,

        /// <remarks/>
        ErrorMailboxStoreUnavailable,

        /// <remarks/>
        ErrorMailRecipientNotFound,

        /// <remarks/>
        ErrorManagedFolderAlreadyExists,

        /// <remarks/>
        ErrorManagedFolderNotFound,

        /// <remarks/>
        ErrorManagedFoldersRootFailure,

        /// <remarks/>
        ErrorMeetingSuggestionGenerationFailed,

        /// <remarks/>
        ErrorMessageDispositionRequired,

        /// <remarks/>
        ErrorMessageSizeExceeded,

        /// <remarks/>
        ErrorMimeContentConversionFailed,

        /// <remarks/>
        ErrorMimeContentInvalid,

        /// <remarks/>
        ErrorMimeContentInvalidBase64String,

        /// <remarks/>
        ErrorMissingArgument,

        /// <remarks/>
        ErrorMissingEmailAddress,

        /// <remarks/>
        ErrorMissingEmailAddressForManagedFolder,

        /// <remarks/>
        ErrorMissingInformationEmailAddress,

        /// <remarks/>
        ErrorMissingInformationReferenceItemId,

        /// <remarks/>
        ErrorMissingItemForCreateItemAttachment,

        /// <remarks/>
        ErrorMissingManagedFolderId,

        /// <remarks/>
        ErrorMissingRecipients,

        /// <remarks/>
        ErrorMissingUserIdInformation,

        /// <remarks/>
        ErrorMoreThanOneAccessModeSpecified,

        /// <remarks/>
        ErrorMoveCopyFailed,

        /// <remarks/>
        ErrorMoveDistinguishedFolder,

        /// <remarks/>
        ErrorNameResolutionMultipleResults,

        /// <remarks/>
        ErrorNameResolutionNoMailbox,

        /// <remarks/>
        ErrorNameResolutionNoResults,

        /// <remarks/>
        ErrorNoCalendar,

        /// <remarks/>
        ErrorNoDestinationCASDueToKerberosRequirements,

        /// <remarks/>
        ErrorNoDestinationCASDueToSSLRequirements,

        /// <remarks/>
        ErrorNoDestinationCASDueToVersionMismatch,

        /// <remarks/>
        ErrorNoFolderClassOverride,

        /// <remarks/>
        ErrorNoFreeBusyAccess,

        /// <remarks/>
        ErrorNonExistentMailbox,

        /// <remarks/>
        ErrorNonPrimarySmtpAddress,

        /// <remarks/>
        ErrorNoPropertyTagForCustomProperties,

        /// <remarks/>
        ErrorNoPublicFolderReplicaAvailable,

        /// <remarks/>
        ErrorNoRespondingCASInDestinationSite,

        /// <remarks/>
        ErrorNotDelegate,

        /// <remarks/>
        ErrorNotEnoughMemory,

        /// <remarks/>
        ErrorObjectTypeChanged,

        /// <remarks/>
        ErrorOccurrenceCrossingBoundary,

        /// <remarks/>
        ErrorOccurrenceTimeSpanTooBig,

        /// <remarks/>
        ErrorOperationNotAllowedWithPublicFolderRoot,

        /// <remarks/>
        ErrorParentFolderIdRequired,

        /// <remarks/>
        ErrorParentFolderNotFound,

        /// <remarks/>
        ErrorPasswordChangeRequired,

        /// <remarks/>
        ErrorPasswordExpired,

        /// <remarks/>
        ErrorPropertyUpdate,

        /// <remarks/>
        ErrorPropertyValidationFailure,

        /// <remarks/>
        ErrorProxiedSubscriptionCallFailure,

        /// <remarks/>
        ErrorProxyCallFailed,

        /// <remarks/>
        ErrorProxyGroupSidLimitExceeded,

        /// <remarks/>
        ErrorProxyRequestNotAllowed,

        /// <remarks/>
        ErrorProxyRequestProcessingFailed,

        /// <remarks/>
        ErrorProxyTokenExpired,

        /// <remarks/>
        ErrorPublicFolderRequestProcessingFailed,

        /// <remarks/>
        ErrorPublicFolderServerNotFound,

        /// <remarks/>
        ErrorQueryFilterTooLong,

        /// <remarks/>
        ErrorQuotaExceeded,

        /// <remarks/>
        ErrorReadEventsFailed,

        /// <remarks/>
        ErrorReadReceiptNotPending,

        /// <remarks/>
        ErrorRecurrenceEndDateTooBig,

        /// <remarks/>
        ErrorRecurrenceHasNoOccurrence,

        /// <remarks/>
        ErrorRemoveDelegatesFailed,

        /// <remarks/>
        ErrorRequestAborted,

        /// <remarks/>
        ErrorRequestStreamTooBig,

        /// <remarks/>
        ErrorRequiredPropertyMissing,

        /// <remarks/>
        ErrorResolveNamesInvalidFolderType,

        /// <remarks/>
        ErrorResolveNamesOnlyOneContactsFolderAllowed,

        /// <remarks/>
        ErrorResponseSchemaValidation,

        /// <remarks/>
        ErrorRestrictionTooLong,

        /// <remarks/>
        ErrorRestrictionTooComplex,

        /// <remarks/>
        ErrorResultSetTooBig,

        /// <remarks/>
        ErrorInvalidExchangeImpersonationHeaderData,

        /// <remarks/>
        ErrorSavedItemFolderNotFound,

        /// <remarks/>
        ErrorSchemaValidation,

        /// <remarks/>
        ErrorSearchFolderNotInitialized,

        /// <remarks/>
        ErrorSendAsDenied,

        /// <remarks/>
        ErrorSendMeetingCancellationsRequired,

        /// <remarks/>
        ErrorSendMeetingInvitationsOrCancellationsRequired,

        /// <remarks/>
        ErrorSendMeetingInvitationsRequired,

        /// <remarks/>
        ErrorSentMeetingRequestUpdate,

        /// <remarks/>
        ErrorSentTaskRequestUpdate,

        /// <remarks/>
        ErrorServerBusy,

        /// <remarks/>
        ErrorServiceDiscoveryFailed,

        /// <remarks/>
        ErrorStaleObject,

        /// <remarks/>
        ErrorSubscriptionAccessDenied,

        /// <remarks/>
        ErrorSubscriptionDelegateAccessNotSupported,

        /// <remarks/>
        ErrorSubscriptionNotFound,

        /// <remarks/>
        ErrorSyncFolderNotFound,

        /// <remarks/>
        ErrorTimeIntervalTooBig,

        /// <remarks/>
        ErrorTimeoutExpired,

        /// <remarks/>
        ErrorToFolderNotFound,

        /// <remarks/>
        ErrorTokenSerializationDenied,

        /// <remarks/>
        ErrorUpdatePropertyMismatch,

        /// <remarks/>
        ErrorUnableToGetUserOofSettings,

        /// <remarks/>
        ErrorUnsupportedSubFilter,

        /// <remarks/>
        ErrorUnsupportedCulture,

        /// <remarks/>
        ErrorUnsupportedMapiPropertyType,

        /// <remarks/>
        ErrorUnsupportedMimeConversion,

        /// <remarks/>
        ErrorUnsupportedPathForQuery,

        /// <remarks/>
        ErrorUnsupportedPathForSortGroup,

        /// <remarks/>
        ErrorUnsupportedPropertyDefinition,

        /// <remarks/>
        ErrorUnsupportedQueryFilter,

        /// <remarks/>
        ErrorUnsupportedRecurrence,

        /// <remarks/>
        ErrorUnsupportedTypeForConversion,

        /// <remarks/>
        ErrorUpdateDelegatesFailed,

        /// <remarks/>
        ErrorVoiceMailNotImplemented,

        /// <remarks/>
        ErrorVirusDetected,

        /// <remarks/>
        ErrorVirusMessageDeleted,

        /// <remarks/>
        ErrorWebRequestInInvalidState,

        /// <remarks/>
        ErrorWin32InteropError,

        /// <remarks/>
        ErrorWorkingHoursSaveFailed,

        /// <remarks/>
        ErrorWorkingHoursXmlMalformed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ResponseMessageTypeMessageXml
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ResponseClassType
    {

        /// <remarks/>
        Success,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Error,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddDelegateResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetDelegateResponseMessageType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public abstract partial class BaseDelegateResponseMessageType : ResponseMessageType
    {

        private DelegateUserResponseMessageType[] responseMessagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public DelegateUserResponseMessageType[] ResponseMessages
        {
            get
            {
                return this.responseMessagesField;
            }
            set
            {
                this.responseMessagesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DelegateUserResponseMessageType : ResponseMessageType
    {

        private DelegateUserType delegateUserField;

        /// <remarks/>
        public DelegateUserType DelegateUser
        {
            get
            {
                return this.delegateUserField;
            }
            set
            {
                this.delegateUserField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DelegateUserType
    {

        private UserIdType userIdField;

        private DelegatePermissionsType delegatePermissionsField;

        private bool receiveCopiesOfMeetingMessagesField;

        private bool receiveCopiesOfMeetingMessagesFieldSpecified;

        private bool viewPrivateItemsField;

        private bool viewPrivateItemsFieldSpecified;

        /// <remarks/>
        public UserIdType UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        public DelegatePermissionsType DelegatePermissions
        {
            get
            {
                return this.delegatePermissionsField;
            }
            set
            {
                this.delegatePermissionsField = value;
            }
        }

        /// <remarks/>
        public bool ReceiveCopiesOfMeetingMessages
        {
            get
            {
                return this.receiveCopiesOfMeetingMessagesField;
            }
            set
            {
                this.receiveCopiesOfMeetingMessagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReceiveCopiesOfMeetingMessagesSpecified
        {
            get
            {
                return this.receiveCopiesOfMeetingMessagesFieldSpecified;
            }
            set
            {
                this.receiveCopiesOfMeetingMessagesFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool ViewPrivateItems
        {
            get
            {
                return this.viewPrivateItemsField;
            }
            set
            {
                this.viewPrivateItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ViewPrivateItemsSpecified
        {
            get
            {
                return this.viewPrivateItemsFieldSpecified;
            }
            set
            {
                this.viewPrivateItemsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class UserIdType
    {

        private string sIDField;

        private string primarySmtpAddressField;

        private string displayNameField;

        private DistinguishedUserType distinguishedUserField;

        private bool distinguishedUserFieldSpecified;

        /// <remarks/>
        public string SID
        {
            get
            {
                return this.sIDField;
            }
            set
            {
                this.sIDField = value;
            }
        }

        /// <remarks/>
        public string PrimarySmtpAddress
        {
            get
            {
                return this.primarySmtpAddressField;
            }
            set
            {
                this.primarySmtpAddressField = value;
            }
        }

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public DistinguishedUserType DistinguishedUser
        {
            get
            {
                return this.distinguishedUserField;
            }
            set
            {
                this.distinguishedUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DistinguishedUserSpecified
        {
            get
            {
                return this.distinguishedUserFieldSpecified;
            }
            set
            {
                this.distinguishedUserFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DistinguishedUserType
    {

        /// <remarks/>
        Default,

        /// <remarks/>
        Anonymous,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DelegatePermissionsType
    {

        private DelegateFolderPermissionLevelType calendarFolderPermissionLevelField;

        private bool calendarFolderPermissionLevelFieldSpecified;

        private DelegateFolderPermissionLevelType tasksFolderPermissionLevelField;

        private bool tasksFolderPermissionLevelFieldSpecified;

        private DelegateFolderPermissionLevelType inboxFolderPermissionLevelField;

        private bool inboxFolderPermissionLevelFieldSpecified;

        private DelegateFolderPermissionLevelType contactsFolderPermissionLevelField;

        private bool contactsFolderPermissionLevelFieldSpecified;

        private DelegateFolderPermissionLevelType notesFolderPermissionLevelField;

        private bool notesFolderPermissionLevelFieldSpecified;

        private DelegateFolderPermissionLevelType journalFolderPermissionLevelField;

        private bool journalFolderPermissionLevelFieldSpecified;

        /// <remarks/>
        public DelegateFolderPermissionLevelType CalendarFolderPermissionLevel
        {
            get
            {
                return this.calendarFolderPermissionLevelField;
            }
            set
            {
                this.calendarFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CalendarFolderPermissionLevelSpecified
        {
            get
            {
                return this.calendarFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.calendarFolderPermissionLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DelegateFolderPermissionLevelType TasksFolderPermissionLevel
        {
            get
            {
                return this.tasksFolderPermissionLevelField;
            }
            set
            {
                this.tasksFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TasksFolderPermissionLevelSpecified
        {
            get
            {
                return this.tasksFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.tasksFolderPermissionLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DelegateFolderPermissionLevelType InboxFolderPermissionLevel
        {
            get
            {
                return this.inboxFolderPermissionLevelField;
            }
            set
            {
                this.inboxFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InboxFolderPermissionLevelSpecified
        {
            get
            {
                return this.inboxFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.inboxFolderPermissionLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DelegateFolderPermissionLevelType ContactsFolderPermissionLevel
        {
            get
            {
                return this.contactsFolderPermissionLevelField;
            }
            set
            {
                this.contactsFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContactsFolderPermissionLevelSpecified
        {
            get
            {
                return this.contactsFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.contactsFolderPermissionLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DelegateFolderPermissionLevelType NotesFolderPermissionLevel
        {
            get
            {
                return this.notesFolderPermissionLevelField;
            }
            set
            {
                this.notesFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NotesFolderPermissionLevelSpecified
        {
            get
            {
                return this.notesFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.notesFolderPermissionLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DelegateFolderPermissionLevelType JournalFolderPermissionLevel
        {
            get
            {
                return this.journalFolderPermissionLevelField;
            }
            set
            {
                this.journalFolderPermissionLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool JournalFolderPermissionLevelSpecified
        {
            get
            {
                return this.journalFolderPermissionLevelFieldSpecified;
            }
            set
            {
                this.journalFolderPermissionLevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DelegateFolderPermissionLevelType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Editor,

        /// <remarks/>
        Reviewer,

        /// <remarks/>
        Author,

        /// <remarks/>
        Custom,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateDelegateResponseMessageType : BaseDelegateResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class RemoveDelegateResponseMessageType : BaseDelegateResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class AddDelegateResponseMessageType : BaseDelegateResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetDelegateResponseMessageType : BaseDelegateResponseMessageType
    {

        private DeliverMeetingRequestsType deliverMeetingRequestsField;

        private bool deliverMeetingRequestsFieldSpecified;

        /// <remarks/>
        public DeliverMeetingRequestsType DeliverMeetingRequests
        {
            get
            {
                return this.deliverMeetingRequestsField;
            }
            set
            {
                this.deliverMeetingRequestsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeliverMeetingRequestsSpecified
        {
            get
            {
                return this.deliverMeetingRequestsFieldSpecified;
            }
            set
            {
                this.deliverMeetingRequestsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DeliverMeetingRequestsType
    {

        /// <remarks/>
        DelegatesOnly,

        /// <remarks/>
        DelegatesAndMe,

        /// <remarks/>
        DelegatesAndSendInformationToMe,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ConvertIdResponseMessageType : ResponseMessageType
    {

        private AlternateIdBaseType alternateIdField;

        /// <remarks/>
        public AlternateIdBaseType AlternateId
        {
            get
            {
                return this.alternateIdField;
            }
            set
            {
                this.alternateIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AlternatePublicFolderIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AlternatePublicFolderItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AlternateIdType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class AlternateIdBaseType
    {

        private IdFormatType formatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public IdFormatType Format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum IdFormatType
    {

        /// <remarks/>
        EwsLegacyId,

        /// <remarks/>
        EwsId,

        /// <remarks/>
        EntryId,

        /// <remarks/>
        HexEntryId,

        /// <remarks/>
        StoreId,

        /// <remarks/>
        OwaId,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AlternatePublicFolderItemIdType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AlternatePublicFolderIdType : AlternateIdBaseType
    {

        private string folderIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FolderId
        {
            get
            {
                return this.folderIdField;
            }
            set
            {
                this.folderIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AlternatePublicFolderItemIdType : AlternatePublicFolderIdType
    {

        private string itemIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AlternateIdType : AlternateIdBaseType
    {

        private string idField;

        private string mailboxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderItemsResponseMessageType : ResponseMessageType
    {

        private string syncStateField;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private SyncFolderItemsChangesType changesField;

        /// <remarks/>
        public string SyncState
        {
            get
            {
                return this.syncStateField;
            }
            set
            {
                this.syncStateField = value;
            }
        }

        /// <remarks/>
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SyncFolderItemsChangesType Changes
        {
            get
            {
                return this.changesField;
            }
            set
            {
                this.changesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderItemsChangesType
    {

        private object[] itemsField;

        private ItemsChoiceType2[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Create", typeof(SyncFolderItemsCreateOrUpdateType))]
        [System.Xml.Serialization.XmlElementAttribute("Delete", typeof(SyncFolderItemsDeleteType))]
        [System.Xml.Serialization.XmlElementAttribute("ReadFlagChange", typeof(SyncFolderItemsReadFlagType))]
        [System.Xml.Serialization.XmlElementAttribute("Update", typeof(SyncFolderItemsCreateOrUpdateType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType2[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderItemsCreateOrUpdateType
    {

        private ItemType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DistributionList", typeof(DistributionListType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        public ItemType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarItemType : ItemType
    {

        private string uIDField;

        private System.DateTime recurrenceIdField;

        private bool recurrenceIdFieldSpecified;

        private System.DateTime dateTimeStampField;

        private bool dateTimeStampFieldSpecified;

        private System.DateTime startField;

        private bool startFieldSpecified;

        private System.DateTime endField;

        private bool endFieldSpecified;

        private System.DateTime originalStartField;

        private bool originalStartFieldSpecified;

        private bool isAllDayEventField;

        private bool isAllDayEventFieldSpecified;

        private LegacyFreeBusyType legacyFreeBusyStatusField;

        private bool legacyFreeBusyStatusFieldSpecified;

        private string locationField;

        private string whenField;

        private bool isMeetingField;

        private bool isMeetingFieldSpecified;

        private bool isCancelledField;

        private bool isCancelledFieldSpecified;

        private bool isRecurringField;

        private bool isRecurringFieldSpecified;

        private bool meetingRequestWasSentField;

        private bool meetingRequestWasSentFieldSpecified;

        private bool isResponseRequestedField;

        private bool isResponseRequestedFieldSpecified;

        private CalendarItemTypeType calendarItemType1Field;

        private bool calendarItemType1FieldSpecified;

        private ResponseTypeType myResponseTypeField;

        private bool myResponseTypeFieldSpecified;

        private SingleRecipientType organizerField;

        private AttendeeType[] requiredAttendeesField;

        private AttendeeType[] optionalAttendeesField;

        private AttendeeType[] resourcesField;

        private int conflictingMeetingCountField;

        private bool conflictingMeetingCountFieldSpecified;

        private int adjacentMeetingCountField;

        private bool adjacentMeetingCountFieldSpecified;

        private NonEmptyArrayOfAllItemsType conflictingMeetingsField;

        private NonEmptyArrayOfAllItemsType adjacentMeetingsField;

        private string durationField;

        private string timeZoneField;

        private System.DateTime appointmentReplyTimeField;

        private bool appointmentReplyTimeFieldSpecified;

        private int appointmentSequenceNumberField;

        private bool appointmentSequenceNumberFieldSpecified;

        private int appointmentStateField;

        private bool appointmentStateFieldSpecified;

        private RecurrenceType recurrenceField;

        private OccurrenceInfoType firstOccurrenceField;

        private OccurrenceInfoType lastOccurrenceField;

        private OccurrenceInfoType[] modifiedOccurrencesField;

        private DeletedOccurrenceInfoType[] deletedOccurrencesField;

        private TimeZoneType meetingTimeZoneField;

        private int conferenceTypeField;

        private bool conferenceTypeFieldSpecified;

        private bool allowNewTimeProposalField;

        private bool allowNewTimeProposalFieldSpecified;

        private bool isOnlineMeetingField;

        private bool isOnlineMeetingFieldSpecified;

        private string meetingWorkspaceUrlField;

        private string netShowUrlField;

        /// <remarks/>
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime RecurrenceId
        {
            get
            {
                return this.recurrenceIdField;
            }
            set
            {
                this.recurrenceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RecurrenceIdSpecified
        {
            get
            {
                return this.recurrenceIdFieldSpecified;
            }
            set
            {
                this.recurrenceIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTimeStamp
        {
            get
            {
                return this.dateTimeStampField;
            }
            set
            {
                this.dateTimeStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeStampSpecified
        {
            get
            {
                return this.dateTimeStampFieldSpecified;
            }
            set
            {
                this.dateTimeStampFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime Start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartSpecified
        {
            get
            {
                return this.startFieldSpecified;
            }
            set
            {
                this.startFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime End
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EndSpecified
        {
            get
            {
                return this.endFieldSpecified;
            }
            set
            {
                this.endFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime OriginalStart
        {
            get
            {
                return this.originalStartField;
            }
            set
            {
                this.originalStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OriginalStartSpecified
        {
            get
            {
                return this.originalStartFieldSpecified;
            }
            set
            {
                this.originalStartFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsAllDayEvent
        {
            get
            {
                return this.isAllDayEventField;
            }
            set
            {
                this.isAllDayEventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAllDayEventSpecified
        {
            get
            {
                return this.isAllDayEventFieldSpecified;
            }
            set
            {
                this.isAllDayEventFieldSpecified = value;
            }
        }

        /// <remarks/>
        public LegacyFreeBusyType LegacyFreeBusyStatus
        {
            get
            {
                return this.legacyFreeBusyStatusField;
            }
            set
            {
                this.legacyFreeBusyStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LegacyFreeBusyStatusSpecified
        {
            get
            {
                return this.legacyFreeBusyStatusFieldSpecified;
            }
            set
            {
                this.legacyFreeBusyStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string When
        {
            get
            {
                return this.whenField;
            }
            set
            {
                this.whenField = value;
            }
        }

        /// <remarks/>
        public bool IsMeeting
        {
            get
            {
                return this.isMeetingField;
            }
            set
            {
                this.isMeetingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsMeetingSpecified
        {
            get
            {
                return this.isMeetingFieldSpecified;
            }
            set
            {
                this.isMeetingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCancelled
        {
            get
            {
                return this.isCancelledField;
            }
            set
            {
                this.isCancelledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCancelledSpecified
        {
            get
            {
                return this.isCancelledFieldSpecified;
            }
            set
            {
                this.isCancelledFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsRecurring
        {
            get
            {
                return this.isRecurringField;
            }
            set
            {
                this.isRecurringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRecurringSpecified
        {
            get
            {
                return this.isRecurringFieldSpecified;
            }
            set
            {
                this.isRecurringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool MeetingRequestWasSent
        {
            get
            {
                return this.meetingRequestWasSentField;
            }
            set
            {
                this.meetingRequestWasSentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MeetingRequestWasSentSpecified
        {
            get
            {
                return this.meetingRequestWasSentFieldSpecified;
            }
            set
            {
                this.meetingRequestWasSentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsResponseRequested
        {
            get
            {
                return this.isResponseRequestedField;
            }
            set
            {
                this.isResponseRequestedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsResponseRequestedSpecified
        {
            get
            {
                return this.isResponseRequestedFieldSpecified;
            }
            set
            {
                this.isResponseRequestedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItemType")]
        public CalendarItemTypeType CalendarItemType1
        {
            get
            {
                return this.calendarItemType1Field;
            }
            set
            {
                this.calendarItemType1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CalendarItemType1Specified
        {
            get
            {
                return this.calendarItemType1FieldSpecified;
            }
            set
            {
                this.calendarItemType1FieldSpecified = value;
            }
        }

        /// <remarks/>
        public ResponseTypeType MyResponseType
        {
            get
            {
                return this.myResponseTypeField;
            }
            set
            {
                this.myResponseTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MyResponseTypeSpecified
        {
            get
            {
                return this.myResponseTypeFieldSpecified;
            }
            set
            {
                this.myResponseTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType Organizer
        {
            get
            {
                return this.organizerField;
            }
            set
            {
                this.organizerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] RequiredAttendees
        {
            get
            {
                return this.requiredAttendeesField;
            }
            set
            {
                this.requiredAttendeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] OptionalAttendees
        {
            get
            {
                return this.optionalAttendeesField;
            }
            set
            {
                this.optionalAttendeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] Resources
        {
            get
            {
                return this.resourcesField;
            }
            set
            {
                this.resourcesField = value;
            }
        }

        /// <remarks/>
        public int ConflictingMeetingCount
        {
            get
            {
                return this.conflictingMeetingCountField;
            }
            set
            {
                this.conflictingMeetingCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConflictingMeetingCountSpecified
        {
            get
            {
                return this.conflictingMeetingCountFieldSpecified;
            }
            set
            {
                this.conflictingMeetingCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AdjacentMeetingCount
        {
            get
            {
                return this.adjacentMeetingCountField;
            }
            set
            {
                this.adjacentMeetingCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdjacentMeetingCountSpecified
        {
            get
            {
                return this.adjacentMeetingCountFieldSpecified;
            }
            set
            {
                this.adjacentMeetingCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public NonEmptyArrayOfAllItemsType ConflictingMeetings
        {
            get
            {
                return this.conflictingMeetingsField;
            }
            set
            {
                this.conflictingMeetingsField = value;
            }
        }

        /// <remarks/>
        public NonEmptyArrayOfAllItemsType AdjacentMeetings
        {
            get
            {
                return this.adjacentMeetingsField;
            }
            set
            {
                this.adjacentMeetingsField = value;
            }
        }

        /// <remarks/>
        public string Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        public System.DateTime AppointmentReplyTime
        {
            get
            {
                return this.appointmentReplyTimeField;
            }
            set
            {
                this.appointmentReplyTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentReplyTimeSpecified
        {
            get
            {
                return this.appointmentReplyTimeFieldSpecified;
            }
            set
            {
                this.appointmentReplyTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AppointmentSequenceNumber
        {
            get
            {
                return this.appointmentSequenceNumberField;
            }
            set
            {
                this.appointmentSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentSequenceNumberSpecified
        {
            get
            {
                return this.appointmentSequenceNumberFieldSpecified;
            }
            set
            {
                this.appointmentSequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AppointmentState
        {
            get
            {
                return this.appointmentStateField;
            }
            set
            {
                this.appointmentStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentStateSpecified
        {
            get
            {
                return this.appointmentStateFieldSpecified;
            }
            set
            {
                this.appointmentStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public RecurrenceType Recurrence
        {
            get
            {
                return this.recurrenceField;
            }
            set
            {
                this.recurrenceField = value;
            }
        }

        /// <remarks/>
        public OccurrenceInfoType FirstOccurrence
        {
            get
            {
                return this.firstOccurrenceField;
            }
            set
            {
                this.firstOccurrenceField = value;
            }
        }

        /// <remarks/>
        public OccurrenceInfoType LastOccurrence
        {
            get
            {
                return this.lastOccurrenceField;
            }
            set
            {
                this.lastOccurrenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Occurrence", IsNullable = false)]
        public OccurrenceInfoType[] ModifiedOccurrences
        {
            get
            {
                return this.modifiedOccurrencesField;
            }
            set
            {
                this.modifiedOccurrencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DeletedOccurrence", IsNullable = false)]
        public DeletedOccurrenceInfoType[] DeletedOccurrences
        {
            get
            {
                return this.deletedOccurrencesField;
            }
            set
            {
                this.deletedOccurrencesField = value;
            }
        }

        /// <remarks/>
        public TimeZoneType MeetingTimeZone
        {
            get
            {
                return this.meetingTimeZoneField;
            }
            set
            {
                this.meetingTimeZoneField = value;
            }
        }

        /// <remarks/>
        public int ConferenceType
        {
            get
            {
                return this.conferenceTypeField;
            }
            set
            {
                this.conferenceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConferenceTypeSpecified
        {
            get
            {
                return this.conferenceTypeFieldSpecified;
            }
            set
            {
                this.conferenceTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool AllowNewTimeProposal
        {
            get
            {
                return this.allowNewTimeProposalField;
            }
            set
            {
                this.allowNewTimeProposalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowNewTimeProposalSpecified
        {
            get
            {
                return this.allowNewTimeProposalFieldSpecified;
            }
            set
            {
                this.allowNewTimeProposalFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOnlineMeeting
        {
            get
            {
                return this.isOnlineMeetingField;
            }
            set
            {
                this.isOnlineMeetingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineMeetingSpecified
        {
            get
            {
                return this.isOnlineMeetingFieldSpecified;
            }
            set
            {
                this.isOnlineMeetingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string MeetingWorkspaceUrl
        {
            get
            {
                return this.meetingWorkspaceUrlField;
            }
            set
            {
                this.meetingWorkspaceUrlField = value;
            }
        }

        /// <remarks/>
        public string NetShowUrl
        {
            get
            {
                return this.netShowUrlField;
            }
            set
            {
                this.netShowUrlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum LegacyFreeBusyType
    {

        /// <remarks/>
        Free,

        /// <remarks/>
        Tentative,

        /// <remarks/>
        Busy,

        /// <remarks/>
        OOF,

        /// <remarks/>
        NoData,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum CalendarItemTypeType
    {

        /// <remarks/>
        Single,

        /// <remarks/>
        Occurrence,

        /// <remarks/>
        Exception,

        /// <remarks/>
        RecurringMaster,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ResponseTypeType
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Organizer,

        /// <remarks/>
        Tentative,

        /// <remarks/>
        Accept,

        /// <remarks/>
        Decline,

        /// <remarks/>
        NoResponseReceived,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SingleRecipientType
    {

        private EmailAddressType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Mailbox")]
        public EmailAddressType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class EmailAddressType : BaseEmailAddressType
    {

        private string nameField;

        private string emailAddressField;

        private string routingTypeField;

        private MailboxTypeType mailboxTypeField;

        private bool mailboxTypeFieldSpecified;

        private ItemIdType itemIdField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }

        /// <remarks/>
        public string RoutingType
        {
            get
            {
                return this.routingTypeField;
            }
            set
            {
                this.routingTypeField = value;
            }
        }

        /// <remarks/>
        public MailboxTypeType MailboxType
        {
            get
            {
                return this.mailboxTypeField;
            }
            set
            {
                this.mailboxTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MailboxTypeSpecified
        {
            get
            {
                return this.mailboxTypeFieldSpecified;
            }
            set
            {
                this.mailboxTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ItemIdType ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MailboxTypeType
    {

        /// <remarks/>
        Mailbox,

        /// <remarks/>
        PublicDL,

        /// <remarks/>
        PrivateDL,

        /// <remarks/>
        Contact,

        /// <remarks/>
        PublicFolder,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemIdType : BaseItemIdType
    {

        private string idField;

        private string changeKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChangeKey
        {
            get
            {
                return this.changeKeyField;
            }
            set
            {
                this.changeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RecurringMasterItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OccurrenceItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RootItemIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RequestAttachmentIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentIdType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BaseItemIdType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RecurringMasterItemIdType : BaseItemIdType
    {

        private string occurrenceIdField;

        private string changeKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OccurrenceId
        {
            get
            {
                return this.occurrenceIdField;
            }
            set
            {
                this.occurrenceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChangeKey
        {
            get
            {
                return this.changeKeyField;
            }
            set
            {
                this.changeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class OccurrenceItemIdType : BaseItemIdType
    {

        private string recurringMasterIdField;

        private string changeKeyField;

        private int instanceIndexField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RecurringMasterId
        {
            get
            {
                return this.recurringMasterIdField;
            }
            set
            {
                this.recurringMasterIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChangeKey
        {
            get
            {
                return this.changeKeyField;
            }
            set
            {
                this.changeKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int InstanceIndex
        {
            get
            {
                return this.instanceIndexField;
            }
            set
            {
                this.instanceIndexField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RootItemIdType : BaseItemIdType
    {

        private string rootItemIdField;

        private string rootItemChangeKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RootItemId
        {
            get
            {
                return this.rootItemIdField;
            }
            set
            {
                this.rootItemIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RootItemChangeKey
        {
            get
            {
                return this.rootItemChangeKeyField;
            }
            set
            {
                this.rootItemChangeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentIdType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RequestAttachmentIdType : BaseItemIdType
    {

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AttachmentIdType : RequestAttachmentIdType
    {

        private string rootItemIdField;

        private string rootItemChangeKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RootItemId
        {
            get
            {
                return this.rootItemIdField;
            }
            set
            {
                this.rootItemIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RootItemChangeKey
        {
            get
            {
                return this.rootItemChangeKeyField;
            }
            set
            {
                this.rootItemChangeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EmailAddressType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class BaseEmailAddressType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AttendeeType
    {

        private EmailAddressType mailboxField;

        private ResponseTypeType responseTypeField;

        private bool responseTypeFieldSpecified;

        private System.DateTime lastResponseTimeField;

        private bool lastResponseTimeFieldSpecified;

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }

        /// <remarks/>
        public ResponseTypeType ResponseType
        {
            get
            {
                return this.responseTypeField;
            }
            set
            {
                this.responseTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ResponseTypeSpecified
        {
            get
            {
                return this.responseTypeFieldSpecified;
            }
            set
            {
                this.responseTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastResponseTime
        {
            get
            {
                return this.lastResponseTimeField;
            }
            set
            {
                this.lastResponseTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastResponseTimeSpecified
        {
            get
            {
                return this.lastResponseTimeFieldSpecified;
            }
            set
            {
                this.lastResponseTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NonEmptyArrayOfAllItemsType
    {

        private ItemType[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AcceptItem", typeof(AcceptItemType))]
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("CancelCalendarItem", typeof(CancelCalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DeclineItem", typeof(DeclineItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DistributionList", typeof(DistributionListType))]
        [System.Xml.Serialization.XmlElementAttribute("ForwardItem", typeof(ForwardItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("PostReplyItem", typeof(PostReplyItemType))]
        [System.Xml.Serialization.XmlElementAttribute("RemoveItem", typeof(RemoveItemType))]
        [System.Xml.Serialization.XmlElementAttribute("ReplyAllToItem", typeof(ReplyAllToItemType))]
        [System.Xml.Serialization.XmlElementAttribute("ReplyToItem", typeof(ReplyToItemType))]
        [System.Xml.Serialization.XmlElementAttribute("SuppressReadReceipt", typeof(SuppressReadReceiptType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        [System.Xml.Serialization.XmlElementAttribute("TentativelyAcceptItem", typeof(TentativelyAcceptItemType))]
        public ItemType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AcceptItemType : WellKnownResponseObjectType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclineItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TentativelyAcceptItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AcceptItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class WellKnownResponseObjectType : ResponseObjectType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SuppressReadReceiptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WellKnownResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclineItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TentativelyAcceptItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AcceptItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class ResponseObjectType : ResponseObjectCoreType
    {

        private string objectNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ObjectName
        {
            get
            {
                return this.objectNameField;
            }
            set
            {
                this.objectNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SuppressReadReceiptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WellKnownResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclineItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TentativelyAcceptItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AcceptItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class ResponseObjectCoreType : MessageType
    {

        private ItemIdType referenceItemIdField;

        /// <remarks/>
        public ItemIdType ReferenceItemId
        {
            get
            {
                return this.referenceItemIdField;
            }
            set
            {
                this.referenceItemIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingCancellationMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingRequestMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseObjectCoreType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SuppressReadReceiptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WellKnownResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclineItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TentativelyAcceptItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AcceptItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MessageType : ItemType
    {

        private SingleRecipientType senderField;

        private EmailAddressType[] toRecipientsField;

        private EmailAddressType[] ccRecipientsField;

        private EmailAddressType[] bccRecipientsField;

        private bool isReadReceiptRequestedField;

        private bool isReadReceiptRequestedFieldSpecified;

        private bool isDeliveryReceiptRequestedField;

        private bool isDeliveryReceiptRequestedFieldSpecified;

        private byte[] conversationIndexField;

        private string conversationTopicField;

        private SingleRecipientType fromField;

        private string internetMessageIdField;

        private bool isReadField;

        private bool isReadFieldSpecified;

        private bool isResponseRequestedField;

        private bool isResponseRequestedFieldSpecified;

        private string referencesField;

        private EmailAddressType[] replyToField;

        private SingleRecipientType receivedByField;

        private SingleRecipientType receivedRepresentingField;

        /// <remarks/>
        public SingleRecipientType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Mailbox", IsNullable = false)]
        public EmailAddressType[] ToRecipients
        {
            get
            {
                return this.toRecipientsField;
            }
            set
            {
                this.toRecipientsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Mailbox", IsNullable = false)]
        public EmailAddressType[] CcRecipients
        {
            get
            {
                return this.ccRecipientsField;
            }
            set
            {
                this.ccRecipientsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Mailbox", IsNullable = false)]
        public EmailAddressType[] BccRecipients
        {
            get
            {
                return this.bccRecipientsField;
            }
            set
            {
                this.bccRecipientsField = value;
            }
        }

        /// <remarks/>
        public bool IsReadReceiptRequested
        {
            get
            {
                return this.isReadReceiptRequestedField;
            }
            set
            {
                this.isReadReceiptRequestedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsReadReceiptRequestedSpecified
        {
            get
            {
                return this.isReadReceiptRequestedFieldSpecified;
            }
            set
            {
                this.isReadReceiptRequestedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsDeliveryReceiptRequested
        {
            get
            {
                return this.isDeliveryReceiptRequestedField;
            }
            set
            {
                this.isDeliveryReceiptRequestedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDeliveryReceiptRequestedSpecified
        {
            get
            {
                return this.isDeliveryReceiptRequestedFieldSpecified;
            }
            set
            {
                this.isDeliveryReceiptRequestedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] ConversationIndex
        {
            get
            {
                return this.conversationIndexField;
            }
            set
            {
                this.conversationIndexField = value;
            }
        }

        /// <remarks/>
        public string ConversationTopic
        {
            get
            {
                return this.conversationTopicField;
            }
            set
            {
                this.conversationTopicField = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType From
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
            }
        }

        /// <remarks/>
        public string InternetMessageId
        {
            get
            {
                return this.internetMessageIdField;
            }
            set
            {
                this.internetMessageIdField = value;
            }
        }

        /// <remarks/>
        public bool IsRead
        {
            get
            {
                return this.isReadField;
            }
            set
            {
                this.isReadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsReadSpecified
        {
            get
            {
                return this.isReadFieldSpecified;
            }
            set
            {
                this.isReadFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsResponseRequested
        {
            get
            {
                return this.isResponseRequestedField;
            }
            set
            {
                this.isResponseRequestedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsResponseRequestedSpecified
        {
            get
            {
                return this.isResponseRequestedFieldSpecified;
            }
            set
            {
                this.isResponseRequestedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Mailbox", IsNullable = false)]
        public EmailAddressType[] ReplyTo
        {
            get
            {
                return this.replyToField;
            }
            set
            {
                this.replyToField = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType ReceivedBy
        {
            get
            {
                return this.receivedByField;
            }
            set
            {
                this.receivedByField = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType ReceivedRepresenting
        {
            get
            {
                return this.receivedRepresentingField;
            }
            set
            {
                this.receivedRepresentingField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DistributionListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingCancellationMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingRequestMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseObjectCoreType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReferenceItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SuppressReadReceiptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WellKnownResponseObjectType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeclineItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TentativelyAcceptItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AcceptItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemType
    {

        private MimeContentType mimeContentField;

        private ItemIdType itemIdField;

        private FolderIdType parentFolderIdField;

        private string itemClassField;

        private string subjectField;

        private SensitivityChoicesType sensitivityField;

        private bool sensitivityFieldSpecified;

        private BodyType bodyField;

        private AttachmentType[] attachmentsField;

        private System.DateTime dateTimeReceivedField;

        private bool dateTimeReceivedFieldSpecified;

        private int sizeField;

        private bool sizeFieldSpecified;

        private string[] categoriesField;

        private ImportanceChoicesType importanceField;

        private bool importanceFieldSpecified;

        private string inReplyToField;

        private bool isSubmittedField;

        private bool isSubmittedFieldSpecified;

        private bool isDraftField;

        private bool isDraftFieldSpecified;

        private bool isFromMeField;

        private bool isFromMeFieldSpecified;

        private bool isResendField;

        private bool isResendFieldSpecified;

        private bool isUnmodifiedField;

        private bool isUnmodifiedFieldSpecified;

        private InternetHeaderType[] internetMessageHeadersField;

        private System.DateTime dateTimeSentField;

        private bool dateTimeSentFieldSpecified;

        private System.DateTime dateTimeCreatedField;

        private bool dateTimeCreatedFieldSpecified;

        private ResponseObjectType[] responseObjectsField;

        private System.DateTime reminderDueByField;

        private bool reminderDueByFieldSpecified;

        private bool reminderIsSetField;

        private bool reminderIsSetFieldSpecified;

        private string reminderMinutesBeforeStartField;

        private string displayCcField;

        private string displayToField;

        private bool hasAttachmentsField;

        private bool hasAttachmentsFieldSpecified;

        private ExtendedPropertyType[] extendedPropertyField;

        private string cultureField;

        private EffectiveRightsType effectiveRightsField;

        private string lastModifiedNameField;

        private System.DateTime lastModifiedTimeField;

        private bool lastModifiedTimeFieldSpecified;

        /// <remarks/>
        public MimeContentType MimeContent
        {
            get
            {
                return this.mimeContentField;
            }
            set
            {
                this.mimeContentField = value;
            }
        }

        /// <remarks/>
        public ItemIdType ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public FolderIdType ParentFolderId
        {
            get
            {
                return this.parentFolderIdField;
            }
            set
            {
                this.parentFolderIdField = value;
            }
        }

        /// <remarks/>
        public string ItemClass
        {
            get
            {
                return this.itemClassField;
            }
            set
            {
                this.itemClassField = value;
            }
        }

        /// <remarks/>
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        public SensitivityChoicesType Sensitivity
        {
            get
            {
                return this.sensitivityField;
            }
            set
            {
                this.sensitivityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SensitivitySpecified
        {
            get
            {
                return this.sensitivityFieldSpecified;
            }
            set
            {
                this.sensitivityFieldSpecified = value;
            }
        }

        /// <remarks/>
        public BodyType Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FileAttachment", typeof(FileAttachmentType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemAttachment", typeof(ItemAttachmentType), IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTimeReceived
        {
            get
            {
                return this.dateTimeReceivedField;
            }
            set
            {
                this.dateTimeReceivedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeReceivedSpecified
        {
            get
            {
                return this.dateTimeReceivedFieldSpecified;
            }
            set
            {
                this.dateTimeReceivedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SizeSpecified
        {
            get
            {
                return this.sizeFieldSpecified;
            }
            set
            {
                this.sizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("String", IsNullable = false)]
        public string[] Categories
        {
            get
            {
                return this.categoriesField;
            }
            set
            {
                this.categoriesField = value;
            }
        }

        /// <remarks/>
        public ImportanceChoicesType Importance
        {
            get
            {
                return this.importanceField;
            }
            set
            {
                this.importanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ImportanceSpecified
        {
            get
            {
                return this.importanceFieldSpecified;
            }
            set
            {
                this.importanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string InReplyTo
        {
            get
            {
                return this.inReplyToField;
            }
            set
            {
                this.inReplyToField = value;
            }
        }

        /// <remarks/>
        public bool IsSubmitted
        {
            get
            {
                return this.isSubmittedField;
            }
            set
            {
                this.isSubmittedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsSubmittedSpecified
        {
            get
            {
                return this.isSubmittedFieldSpecified;
            }
            set
            {
                this.isSubmittedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsDraft
        {
            get
            {
                return this.isDraftField;
            }
            set
            {
                this.isDraftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDraftSpecified
        {
            get
            {
                return this.isDraftFieldSpecified;
            }
            set
            {
                this.isDraftFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsFromMe
        {
            get
            {
                return this.isFromMeField;
            }
            set
            {
                this.isFromMeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFromMeSpecified
        {
            get
            {
                return this.isFromMeFieldSpecified;
            }
            set
            {
                this.isFromMeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsResend
        {
            get
            {
                return this.isResendField;
            }
            set
            {
                this.isResendField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsResendSpecified
        {
            get
            {
                return this.isResendFieldSpecified;
            }
            set
            {
                this.isResendFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsUnmodified
        {
            get
            {
                return this.isUnmodifiedField;
            }
            set
            {
                this.isUnmodifiedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsUnmodifiedSpecified
        {
            get
            {
                return this.isUnmodifiedFieldSpecified;
            }
            set
            {
                this.isUnmodifiedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("InternetMessageHeader", IsNullable = false)]
        public InternetHeaderType[] InternetMessageHeaders
        {
            get
            {
                return this.internetMessageHeadersField;
            }
            set
            {
                this.internetMessageHeadersField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTimeSent
        {
            get
            {
                return this.dateTimeSentField;
            }
            set
            {
                this.dateTimeSentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeSentSpecified
        {
            get
            {
                return this.dateTimeSentFieldSpecified;
            }
            set
            {
                this.dateTimeSentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTimeCreated
        {
            get
            {
                return this.dateTimeCreatedField;
            }
            set
            {
                this.dateTimeCreatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeCreatedSpecified
        {
            get
            {
                return this.dateTimeCreatedFieldSpecified;
            }
            set
            {
                this.dateTimeCreatedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AcceptItem", typeof(AcceptItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CancelCalendarItem", typeof(CancelCalendarItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DeclineItem", typeof(DeclineItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ForwardItem", typeof(ForwardItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("PostReplyItem", typeof(PostReplyItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RemoveItem", typeof(RemoveItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReplyAllToItem", typeof(ReplyAllToItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReplyToItem", typeof(ReplyToItemType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SuppressReadReceipt", typeof(SuppressReadReceiptType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TentativelyAcceptItem", typeof(TentativelyAcceptItemType), IsNullable = false)]
        public ResponseObjectType[] ResponseObjects
        {
            get
            {
                return this.responseObjectsField;
            }
            set
            {
                this.responseObjectsField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ReminderDueBy
        {
            get
            {
                return this.reminderDueByField;
            }
            set
            {
                this.reminderDueByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReminderDueBySpecified
        {
            get
            {
                return this.reminderDueByFieldSpecified;
            }
            set
            {
                this.reminderDueByFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool ReminderIsSet
        {
            get
            {
                return this.reminderIsSetField;
            }
            set
            {
                this.reminderIsSetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReminderIsSetSpecified
        {
            get
            {
                return this.reminderIsSetFieldSpecified;
            }
            set
            {
                this.reminderIsSetFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ReminderMinutesBeforeStart
        {
            get
            {
                return this.reminderMinutesBeforeStartField;
            }
            set
            {
                this.reminderMinutesBeforeStartField = value;
            }
        }

        /// <remarks/>
        public string DisplayCc
        {
            get
            {
                return this.displayCcField;
            }
            set
            {
                this.displayCcField = value;
            }
        }

        /// <remarks/>
        public string DisplayTo
        {
            get
            {
                return this.displayToField;
            }
            set
            {
                this.displayToField = value;
            }
        }

        /// <remarks/>
        public bool HasAttachments
        {
            get
            {
                return this.hasAttachmentsField;
            }
            set
            {
                this.hasAttachmentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasAttachmentsSpecified
        {
            get
            {
                return this.hasAttachmentsFieldSpecified;
            }
            set
            {
                this.hasAttachmentsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedProperty")]
        public ExtendedPropertyType[] ExtendedProperty
        {
            get
            {
                return this.extendedPropertyField;
            }
            set
            {
                this.extendedPropertyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "language")]
        public string Culture
        {
            get
            {
                return this.cultureField;
            }
            set
            {
                this.cultureField = value;
            }
        }

        /// <remarks/>
        public EffectiveRightsType EffectiveRights
        {
            get
            {
                return this.effectiveRightsField;
            }
            set
            {
                this.effectiveRightsField = value;
            }
        }

        /// <remarks/>
        public string LastModifiedName
        {
            get
            {
                return this.lastModifiedNameField;
            }
            set
            {
                this.lastModifiedNameField = value;
            }
        }

        /// <remarks/>
        public System.DateTime LastModifiedTime
        {
            get
            {
                return this.lastModifiedTimeField;
            }
            set
            {
                this.lastModifiedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastModifiedTimeSpecified
        {
            get
            {
                return this.lastModifiedTimeFieldSpecified;
            }
            set
            {
                this.lastModifiedTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MimeContentType
    {

        private string characterSetField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CharacterSet
        {
            get
            {
                return this.characterSetField;
            }
            set
            {
                this.characterSetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FolderIdType : BaseFolderIdType
    {

        private string idField;

        private string changeKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChangeKey
        {
            get
            {
                return this.changeKeyField;
            }
            set
            {
                this.changeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FolderIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DistinguishedFolderIdType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BaseFolderIdType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DistinguishedFolderIdType : BaseFolderIdType
    {

        private EmailAddressType mailboxField;

        private DistinguishedFolderIdNameType idField;

        private string changeKeyField;

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DistinguishedFolderIdNameType Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChangeKey
        {
            get
            {
                return this.changeKeyField;
            }
            set
            {
                this.changeKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DistinguishedFolderIdNameType
    {

        /// <remarks/>
        calendar,

        /// <remarks/>
        contacts,

        /// <remarks/>
        deleteditems,

        /// <remarks/>
        drafts,

        /// <remarks/>
        inbox,

        /// <remarks/>
        journal,

        /// <remarks/>
        notes,

        /// <remarks/>
        outbox,

        /// <remarks/>
        sentitems,

        /// <remarks/>
        tasks,

        /// <remarks/>
        msgfolderroot,

        /// <remarks/>
        publicfoldersroot,

        /// <remarks/>
        root,

        /// <remarks/>
        junkemail,

        /// <remarks/>
        searchfolders,

        /// <remarks/>
        voicemail,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum SensitivityChoicesType
    {

        /// <remarks/>
        Normal,

        /// <remarks/>
        Personal,

        /// <remarks/>
        Private,

        /// <remarks/>
        Confidential,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class BodyType
    {

        private BodyTypeType bodyType1Field;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("BodyType")]
        public BodyTypeType BodyType1
        {
            get
            {
                return this.bodyType1Field;
            }
            set
            {
                this.bodyType1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum BodyTypeType
    {

        /// <remarks/>
        HTML,

        /// <remarks/>
        Text,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FileAttachmentType : AttachmentType
    {

        private byte[] contentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FileAttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemAttachmentType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AttachmentType
    {

        private AttachmentIdType attachmentIdField;

        private string nameField;

        private string contentTypeField;

        private string contentIdField;

        private string contentLocationField;

        /// <remarks/>
        public AttachmentIdType AttachmentId
        {
            get
            {
                return this.attachmentIdField;
            }
            set
            {
                this.attachmentIdField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string ContentType
        {
            get
            {
                return this.contentTypeField;
            }
            set
            {
                this.contentTypeField = value;
            }
        }

        /// <remarks/>
        public string ContentId
        {
            get
            {
                return this.contentIdField;
            }
            set
            {
                this.contentIdField = value;
            }
        }

        /// <remarks/>
        public string ContentLocation
        {
            get
            {
                return this.contentLocationField;
            }
            set
            {
                this.contentLocationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemAttachmentType : AttachmentType
    {

        private ItemType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        public ItemType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ContactItemType : ItemType
    {

        private string fileAsField;

        private FileAsMappingType fileAsMappingField;

        private bool fileAsMappingFieldSpecified;

        private string displayNameField;

        private string givenNameField;

        private string initialsField;

        private string middleNameField;

        private string nicknameField;

        private CompleteNameType completeNameField;

        private string companyNameField;

        private EmailAddressDictionaryEntryType[] emailAddressesField;

        private PhysicalAddressDictionaryEntryType[] physicalAddressesField;

        private PhoneNumberDictionaryEntryType[] phoneNumbersField;

        private string assistantNameField;

        private System.DateTime birthdayField;

        private bool birthdayFieldSpecified;

        private string businessHomePageField;

        private string[] childrenField;

        private string[] companiesField;

        private ContactSourceType contactSourceField;

        private bool contactSourceFieldSpecified;

        private string departmentField;

        private string generationField;

        private ImAddressDictionaryEntryType[] imAddressesField;

        private string jobTitleField;

        private string managerField;

        private string mileageField;

        private string officeLocationField;

        private PhysicalAddressIndexType postalAddressIndexField;

        private bool postalAddressIndexFieldSpecified;

        private string professionField;

        private string spouseNameField;

        private string surnameField;

        private System.DateTime weddingAnniversaryField;

        private bool weddingAnniversaryFieldSpecified;

        /// <remarks/>
        public string FileAs
        {
            get
            {
                return this.fileAsField;
            }
            set
            {
                this.fileAsField = value;
            }
        }

        /// <remarks/>
        public FileAsMappingType FileAsMapping
        {
            get
            {
                return this.fileAsMappingField;
            }
            set
            {
                this.fileAsMappingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FileAsMappingSpecified
        {
            get
            {
                return this.fileAsMappingFieldSpecified;
            }
            set
            {
                this.fileAsMappingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }

        /// <remarks/>
        public string Initials
        {
            get
            {
                return this.initialsField;
            }
            set
            {
                this.initialsField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string Nickname
        {
            get
            {
                return this.nicknameField;
            }
            set
            {
                this.nicknameField = value;
            }
        }

        /// <remarks/>
        public CompleteNameType CompleteName
        {
            get
            {
                return this.completeNameField;
            }
            set
            {
                this.completeNameField = value;
            }
        }

        /// <remarks/>
        public string CompanyName
        {
            get
            {
                return this.companyNameField;
            }
            set
            {
                this.companyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Entry", IsNullable = false)]
        public EmailAddressDictionaryEntryType[] EmailAddresses
        {
            get
            {
                return this.emailAddressesField;
            }
            set
            {
                this.emailAddressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Entry", IsNullable = false)]
        public PhysicalAddressDictionaryEntryType[] PhysicalAddresses
        {
            get
            {
                return this.physicalAddressesField;
            }
            set
            {
                this.physicalAddressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Entry", IsNullable = false)]
        public PhoneNumberDictionaryEntryType[] PhoneNumbers
        {
            get
            {
                return this.phoneNumbersField;
            }
            set
            {
                this.phoneNumbersField = value;
            }
        }

        /// <remarks/>
        public string AssistantName
        {
            get
            {
                return this.assistantNameField;
            }
            set
            {
                this.assistantNameField = value;
            }
        }

        /// <remarks/>
        public System.DateTime Birthday
        {
            get
            {
                return this.birthdayField;
            }
            set
            {
                this.birthdayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BirthdaySpecified
        {
            get
            {
                return this.birthdayFieldSpecified;
            }
            set
            {
                this.birthdayFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")]
        public string BusinessHomePage
        {
            get
            {
                return this.businessHomePageField;
            }
            set
            {
                this.businessHomePageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("String", IsNullable = false)]
        public string[] Children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("String", IsNullable = false)]
        public string[] Companies
        {
            get
            {
                return this.companiesField;
            }
            set
            {
                this.companiesField = value;
            }
        }

        /// <remarks/>
        public ContactSourceType ContactSource
        {
            get
            {
                return this.contactSourceField;
            }
            set
            {
                this.contactSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContactSourceSpecified
        {
            get
            {
                return this.contactSourceFieldSpecified;
            }
            set
            {
                this.contactSourceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
            }
        }

        /// <remarks/>
        public string Generation
        {
            get
            {
                return this.generationField;
            }
            set
            {
                this.generationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Entry", IsNullable = false)]
        public ImAddressDictionaryEntryType[] ImAddresses
        {
            get
            {
                return this.imAddressesField;
            }
            set
            {
                this.imAddressesField = value;
            }
        }

        /// <remarks/>
        public string JobTitle
        {
            get
            {
                return this.jobTitleField;
            }
            set
            {
                this.jobTitleField = value;
            }
        }

        /// <remarks/>
        public string Manager
        {
            get
            {
                return this.managerField;
            }
            set
            {
                this.managerField = value;
            }
        }

        /// <remarks/>
        public string Mileage
        {
            get
            {
                return this.mileageField;
            }
            set
            {
                this.mileageField = value;
            }
        }

        /// <remarks/>
        public string OfficeLocation
        {
            get
            {
                return this.officeLocationField;
            }
            set
            {
                this.officeLocationField = value;
            }
        }

        /// <remarks/>
        public PhysicalAddressIndexType PostalAddressIndex
        {
            get
            {
                return this.postalAddressIndexField;
            }
            set
            {
                this.postalAddressIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PostalAddressIndexSpecified
        {
            get
            {
                return this.postalAddressIndexFieldSpecified;
            }
            set
            {
                this.postalAddressIndexFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Profession
        {
            get
            {
                return this.professionField;
            }
            set
            {
                this.professionField = value;
            }
        }

        /// <remarks/>
        public string SpouseName
        {
            get
            {
                return this.spouseNameField;
            }
            set
            {
                this.spouseNameField = value;
            }
        }

        /// <remarks/>
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        public System.DateTime WeddingAnniversary
        {
            get
            {
                return this.weddingAnniversaryField;
            }
            set
            {
                this.weddingAnniversaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WeddingAnniversarySpecified
        {
            get
            {
                return this.weddingAnniversaryFieldSpecified;
            }
            set
            {
                this.weddingAnniversaryFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum FileAsMappingType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        LastCommaFirst,

        /// <remarks/>
        FirstSpaceLast,

        /// <remarks/>
        Company,

        /// <remarks/>
        LastCommaFirstCompany,

        /// <remarks/>
        CompanyLastFirst,

        /// <remarks/>
        LastFirst,

        /// <remarks/>
        LastFirstCompany,

        /// <remarks/>
        CompanyLastCommaFirst,

        /// <remarks/>
        LastFirstSuffix,

        /// <remarks/>
        LastSpaceFirstCompany,

        /// <remarks/>
        CompanyLastSpaceFirst,

        /// <remarks/>
        LastSpaceFirst,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CompleteNameType
    {

        private string titleField;

        private string firstNameField;

        private string middleNameField;

        private string lastNameField;

        private string suffixField;

        private string initialsField;

        private string fullNameField;

        private string nicknameField;

        private string yomiFirstNameField;

        private string yomiLastNameField;

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string MiddleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string LastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string Suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
            }
        }

        /// <remarks/>
        public string Initials
        {
            get
            {
                return this.initialsField;
            }
            set
            {
                this.initialsField = value;
            }
        }

        /// <remarks/>
        public string FullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        public string Nickname
        {
            get
            {
                return this.nicknameField;
            }
            set
            {
                this.nicknameField = value;
            }
        }

        /// <remarks/>
        public string YomiFirstName
        {
            get
            {
                return this.yomiFirstNameField;
            }
            set
            {
                this.yomiFirstNameField = value;
            }
        }

        /// <remarks/>
        public string YomiLastName
        {
            get
            {
                return this.yomiLastNameField;
            }
            set
            {
                this.yomiLastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class EmailAddressDictionaryEntryType
    {

        private EmailAddressKeyType keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EmailAddressKeyType Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum EmailAddressKeyType
    {

        /// <remarks/>
        EmailAddress1,

        /// <remarks/>
        EmailAddress2,

        /// <remarks/>
        EmailAddress3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PhysicalAddressDictionaryEntryType
    {

        private string streetField;

        private string cityField;

        private string stateField;

        private string countryOrRegionField;

        private string postalCodeField;

        private PhysicalAddressKeyType keyField;

        /// <remarks/>
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string CountryOrRegion
        {
            get
            {
                return this.countryOrRegionField;
            }
            set
            {
                this.countryOrRegionField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PhysicalAddressKeyType Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PhysicalAddressKeyType
    {

        /// <remarks/>
        Business,

        /// <remarks/>
        Home,

        /// <remarks/>
        Other,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PhoneNumberDictionaryEntryType
    {

        private PhoneNumberKeyType keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PhoneNumberKeyType Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PhoneNumberKeyType
    {

        /// <remarks/>
        AssistantPhone,

        /// <remarks/>
        BusinessFax,

        /// <remarks/>
        BusinessPhone,

        /// <remarks/>
        BusinessPhone2,

        /// <remarks/>
        Callback,

        /// <remarks/>
        CarPhone,

        /// <remarks/>
        CompanyMainPhone,

        /// <remarks/>
        HomeFax,

        /// <remarks/>
        HomePhone,

        /// <remarks/>
        HomePhone2,

        /// <remarks/>
        Isdn,

        /// <remarks/>
        MobilePhone,

        /// <remarks/>
        OtherFax,

        /// <remarks/>
        OtherTelephone,

        /// <remarks/>
        Pager,

        /// <remarks/>
        PrimaryPhone,

        /// <remarks/>
        RadioPhone,

        /// <remarks/>
        Telex,

        /// <remarks/>
        TtyTddPhone,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ContactSourceType
    {

        /// <remarks/>
        ActiveDirectory,

        /// <remarks/>
        Store,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ImAddressDictionaryEntryType
    {

        private ImAddressKeyType keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ImAddressKeyType Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ImAddressKeyType
    {

        /// <remarks/>
        ImAddress1,

        /// <remarks/>
        ImAddress2,

        /// <remarks/>
        ImAddress3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PhysicalAddressIndexType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Business,

        /// <remarks/>
        Home,

        /// <remarks/>
        Other,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MeetingCancellationMessageType : MeetingMessageType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingCancellationMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingResponseMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeetingRequestMessageType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MeetingMessageType : MessageType
    {

        private ItemIdType associatedCalendarItemIdField;

        private bool isDelegatedField;

        private bool isDelegatedFieldSpecified;

        private bool isOutOfDateField;

        private bool isOutOfDateFieldSpecified;

        private bool hasBeenProcessedField;

        private bool hasBeenProcessedFieldSpecified;

        private ResponseTypeType responseTypeField;

        private bool responseTypeFieldSpecified;

        private string uIDField;

        private System.DateTime recurrenceIdField;

        private bool recurrenceIdFieldSpecified;

        private System.DateTime dateTimeStampField;

        private bool dateTimeStampFieldSpecified;

        /// <remarks/>
        public ItemIdType AssociatedCalendarItemId
        {
            get
            {
                return this.associatedCalendarItemIdField;
            }
            set
            {
                this.associatedCalendarItemIdField = value;
            }
        }

        /// <remarks/>
        public bool IsDelegated
        {
            get
            {
                return this.isDelegatedField;
            }
            set
            {
                this.isDelegatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDelegatedSpecified
        {
            get
            {
                return this.isDelegatedFieldSpecified;
            }
            set
            {
                this.isDelegatedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOutOfDate
        {
            get
            {
                return this.isOutOfDateField;
            }
            set
            {
                this.isOutOfDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOutOfDateSpecified
        {
            get
            {
                return this.isOutOfDateFieldSpecified;
            }
            set
            {
                this.isOutOfDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasBeenProcessed
        {
            get
            {
                return this.hasBeenProcessedField;
            }
            set
            {
                this.hasBeenProcessedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasBeenProcessedSpecified
        {
            get
            {
                return this.hasBeenProcessedFieldSpecified;
            }
            set
            {
                this.hasBeenProcessedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ResponseTypeType ResponseType
        {
            get
            {
                return this.responseTypeField;
            }
            set
            {
                this.responseTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ResponseTypeSpecified
        {
            get
            {
                return this.responseTypeFieldSpecified;
            }
            set
            {
                this.responseTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime RecurrenceId
        {
            get
            {
                return this.recurrenceIdField;
            }
            set
            {
                this.recurrenceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RecurrenceIdSpecified
        {
            get
            {
                return this.recurrenceIdFieldSpecified;
            }
            set
            {
                this.recurrenceIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTimeStamp
        {
            get
            {
                return this.dateTimeStampField;
            }
            set
            {
                this.dateTimeStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateTimeStampSpecified
        {
            get
            {
                return this.dateTimeStampFieldSpecified;
            }
            set
            {
                this.dateTimeStampFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MeetingResponseMessageType : MeetingMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MeetingRequestMessageType : MeetingMessageType
    {

        private MeetingRequestTypeType meetingRequestTypeField;

        private bool meetingRequestTypeFieldSpecified;

        private LegacyFreeBusyType intendedFreeBusyStatusField;

        private bool intendedFreeBusyStatusFieldSpecified;

        private System.DateTime startField;

        private bool startFieldSpecified;

        private System.DateTime endField;

        private bool endFieldSpecified;

        private System.DateTime originalStartField;

        private bool originalStartFieldSpecified;

        private bool isAllDayEventField;

        private bool isAllDayEventFieldSpecified;

        private LegacyFreeBusyType legacyFreeBusyStatusField;

        private bool legacyFreeBusyStatusFieldSpecified;

        private string locationField;

        private string whenField;

        private bool isMeetingField;

        private bool isMeetingFieldSpecified;

        private bool isCancelledField;

        private bool isCancelledFieldSpecified;

        private bool isRecurringField;

        private bool isRecurringFieldSpecified;

        private bool meetingRequestWasSentField;

        private bool meetingRequestWasSentFieldSpecified;

        private CalendarItemTypeType calendarItemTypeField;

        private bool calendarItemTypeFieldSpecified;

        private ResponseTypeType myResponseTypeField;

        private bool myResponseTypeFieldSpecified;

        private SingleRecipientType organizerField;

        private AttendeeType[] requiredAttendeesField;

        private AttendeeType[] optionalAttendeesField;

        private AttendeeType[] resourcesField;

        private int conflictingMeetingCountField;

        private bool conflictingMeetingCountFieldSpecified;

        private int adjacentMeetingCountField;

        private bool adjacentMeetingCountFieldSpecified;

        private NonEmptyArrayOfAllItemsType conflictingMeetingsField;

        private NonEmptyArrayOfAllItemsType adjacentMeetingsField;

        private string durationField;

        private string timeZoneField;

        private System.DateTime appointmentReplyTimeField;

        private bool appointmentReplyTimeFieldSpecified;

        private int appointmentSequenceNumberField;

        private bool appointmentSequenceNumberFieldSpecified;

        private int appointmentStateField;

        private bool appointmentStateFieldSpecified;

        private RecurrenceType recurrenceField;

        private OccurrenceInfoType firstOccurrenceField;

        private OccurrenceInfoType lastOccurrenceField;

        private OccurrenceInfoType[] modifiedOccurrencesField;

        private DeletedOccurrenceInfoType[] deletedOccurrencesField;

        private TimeZoneType meetingTimeZoneField;

        private int conferenceTypeField;

        private bool conferenceTypeFieldSpecified;

        private bool allowNewTimeProposalField;

        private bool allowNewTimeProposalFieldSpecified;

        private bool isOnlineMeetingField;

        private bool isOnlineMeetingFieldSpecified;

        private string meetingWorkspaceUrlField;

        private string netShowUrlField;

        /// <remarks/>
        public MeetingRequestTypeType MeetingRequestType
        {
            get
            {
                return this.meetingRequestTypeField;
            }
            set
            {
                this.meetingRequestTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MeetingRequestTypeSpecified
        {
            get
            {
                return this.meetingRequestTypeFieldSpecified;
            }
            set
            {
                this.meetingRequestTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public LegacyFreeBusyType IntendedFreeBusyStatus
        {
            get
            {
                return this.intendedFreeBusyStatusField;
            }
            set
            {
                this.intendedFreeBusyStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IntendedFreeBusyStatusSpecified
        {
            get
            {
                return this.intendedFreeBusyStatusFieldSpecified;
            }
            set
            {
                this.intendedFreeBusyStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime Start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartSpecified
        {
            get
            {
                return this.startFieldSpecified;
            }
            set
            {
                this.startFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime End
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EndSpecified
        {
            get
            {
                return this.endFieldSpecified;
            }
            set
            {
                this.endFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime OriginalStart
        {
            get
            {
                return this.originalStartField;
            }
            set
            {
                this.originalStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OriginalStartSpecified
        {
            get
            {
                return this.originalStartFieldSpecified;
            }
            set
            {
                this.originalStartFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsAllDayEvent
        {
            get
            {
                return this.isAllDayEventField;
            }
            set
            {
                this.isAllDayEventField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAllDayEventSpecified
        {
            get
            {
                return this.isAllDayEventFieldSpecified;
            }
            set
            {
                this.isAllDayEventFieldSpecified = value;
            }
        }

        /// <remarks/>
        public LegacyFreeBusyType LegacyFreeBusyStatus
        {
            get
            {
                return this.legacyFreeBusyStatusField;
            }
            set
            {
                this.legacyFreeBusyStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LegacyFreeBusyStatusSpecified
        {
            get
            {
                return this.legacyFreeBusyStatusFieldSpecified;
            }
            set
            {
                this.legacyFreeBusyStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string When
        {
            get
            {
                return this.whenField;
            }
            set
            {
                this.whenField = value;
            }
        }

        /// <remarks/>
        public bool IsMeeting
        {
            get
            {
                return this.isMeetingField;
            }
            set
            {
                this.isMeetingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsMeetingSpecified
        {
            get
            {
                return this.isMeetingFieldSpecified;
            }
            set
            {
                this.isMeetingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCancelled
        {
            get
            {
                return this.isCancelledField;
            }
            set
            {
                this.isCancelledField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCancelledSpecified
        {
            get
            {
                return this.isCancelledFieldSpecified;
            }
            set
            {
                this.isCancelledFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsRecurring
        {
            get
            {
                return this.isRecurringField;
            }
            set
            {
                this.isRecurringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRecurringSpecified
        {
            get
            {
                return this.isRecurringFieldSpecified;
            }
            set
            {
                this.isRecurringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool MeetingRequestWasSent
        {
            get
            {
                return this.meetingRequestWasSentField;
            }
            set
            {
                this.meetingRequestWasSentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MeetingRequestWasSentSpecified
        {
            get
            {
                return this.meetingRequestWasSentFieldSpecified;
            }
            set
            {
                this.meetingRequestWasSentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CalendarItemTypeType CalendarItemType
        {
            get
            {
                return this.calendarItemTypeField;
            }
            set
            {
                this.calendarItemTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CalendarItemTypeSpecified
        {
            get
            {
                return this.calendarItemTypeFieldSpecified;
            }
            set
            {
                this.calendarItemTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ResponseTypeType MyResponseType
        {
            get
            {
                return this.myResponseTypeField;
            }
            set
            {
                this.myResponseTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MyResponseTypeSpecified
        {
            get
            {
                return this.myResponseTypeFieldSpecified;
            }
            set
            {
                this.myResponseTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType Organizer
        {
            get
            {
                return this.organizerField;
            }
            set
            {
                this.organizerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] RequiredAttendees
        {
            get
            {
                return this.requiredAttendeesField;
            }
            set
            {
                this.requiredAttendeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] OptionalAttendees
        {
            get
            {
                return this.optionalAttendeesField;
            }
            set
            {
                this.optionalAttendeesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Attendee", IsNullable = false)]
        public AttendeeType[] Resources
        {
            get
            {
                return this.resourcesField;
            }
            set
            {
                this.resourcesField = value;
            }
        }

        /// <remarks/>
        public int ConflictingMeetingCount
        {
            get
            {
                return this.conflictingMeetingCountField;
            }
            set
            {
                this.conflictingMeetingCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConflictingMeetingCountSpecified
        {
            get
            {
                return this.conflictingMeetingCountFieldSpecified;
            }
            set
            {
                this.conflictingMeetingCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AdjacentMeetingCount
        {
            get
            {
                return this.adjacentMeetingCountField;
            }
            set
            {
                this.adjacentMeetingCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdjacentMeetingCountSpecified
        {
            get
            {
                return this.adjacentMeetingCountFieldSpecified;
            }
            set
            {
                this.adjacentMeetingCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public NonEmptyArrayOfAllItemsType ConflictingMeetings
        {
            get
            {
                return this.conflictingMeetingsField;
            }
            set
            {
                this.conflictingMeetingsField = value;
            }
        }

        /// <remarks/>
        public NonEmptyArrayOfAllItemsType AdjacentMeetings
        {
            get
            {
                return this.adjacentMeetingsField;
            }
            set
            {
                this.adjacentMeetingsField = value;
            }
        }

        /// <remarks/>
        public string Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        public string TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        public System.DateTime AppointmentReplyTime
        {
            get
            {
                return this.appointmentReplyTimeField;
            }
            set
            {
                this.appointmentReplyTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentReplyTimeSpecified
        {
            get
            {
                return this.appointmentReplyTimeFieldSpecified;
            }
            set
            {
                this.appointmentReplyTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AppointmentSequenceNumber
        {
            get
            {
                return this.appointmentSequenceNumberField;
            }
            set
            {
                this.appointmentSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentSequenceNumberSpecified
        {
            get
            {
                return this.appointmentSequenceNumberFieldSpecified;
            }
            set
            {
                this.appointmentSequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int AppointmentState
        {
            get
            {
                return this.appointmentStateField;
            }
            set
            {
                this.appointmentStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AppointmentStateSpecified
        {
            get
            {
                return this.appointmentStateFieldSpecified;
            }
            set
            {
                this.appointmentStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public RecurrenceType Recurrence
        {
            get
            {
                return this.recurrenceField;
            }
            set
            {
                this.recurrenceField = value;
            }
        }

        /// <remarks/>
        public OccurrenceInfoType FirstOccurrence
        {
            get
            {
                return this.firstOccurrenceField;
            }
            set
            {
                this.firstOccurrenceField = value;
            }
        }

        /// <remarks/>
        public OccurrenceInfoType LastOccurrence
        {
            get
            {
                return this.lastOccurrenceField;
            }
            set
            {
                this.lastOccurrenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Occurrence", IsNullable = false)]
        public OccurrenceInfoType[] ModifiedOccurrences
        {
            get
            {
                return this.modifiedOccurrencesField;
            }
            set
            {
                this.modifiedOccurrencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DeletedOccurrence", IsNullable = false)]
        public DeletedOccurrenceInfoType[] DeletedOccurrences
        {
            get
            {
                return this.deletedOccurrencesField;
            }
            set
            {
                this.deletedOccurrencesField = value;
            }
        }

        /// <remarks/>
        public TimeZoneType MeetingTimeZone
        {
            get
            {
                return this.meetingTimeZoneField;
            }
            set
            {
                this.meetingTimeZoneField = value;
            }
        }

        /// <remarks/>
        public int ConferenceType
        {
            get
            {
                return this.conferenceTypeField;
            }
            set
            {
                this.conferenceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConferenceTypeSpecified
        {
            get
            {
                return this.conferenceTypeFieldSpecified;
            }
            set
            {
                this.conferenceTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool AllowNewTimeProposal
        {
            get
            {
                return this.allowNewTimeProposalField;
            }
            set
            {
                this.allowNewTimeProposalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowNewTimeProposalSpecified
        {
            get
            {
                return this.allowNewTimeProposalFieldSpecified;
            }
            set
            {
                this.allowNewTimeProposalFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsOnlineMeeting
        {
            get
            {
                return this.isOnlineMeetingField;
            }
            set
            {
                this.isOnlineMeetingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsOnlineMeetingSpecified
        {
            get
            {
                return this.isOnlineMeetingFieldSpecified;
            }
            set
            {
                this.isOnlineMeetingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string MeetingWorkspaceUrl
        {
            get
            {
                return this.meetingWorkspaceUrlField;
            }
            set
            {
                this.meetingWorkspaceUrlField = value;
            }
        }

        /// <remarks/>
        public string NetShowUrl
        {
            get
            {
                return this.netShowUrlField;
            }
            set
            {
                this.netShowUrlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MeetingRequestTypeType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        FullUpdate,

        /// <remarks/>
        InformationalUpdate,

        /// <remarks/>
        NewMeetingRequest,

        /// <remarks/>
        Outdated,

        /// <remarks/>
        SilentUpdate,

        /// <remarks/>
        PrincipalWantsCopy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RecurrenceType
    {

        private RecurrencePatternBaseType itemField;

        private RecurrenceRangeBaseType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AbsoluteMonthlyRecurrence", typeof(AbsoluteMonthlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("AbsoluteYearlyRecurrence", typeof(AbsoluteYearlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("DailyRecurrence", typeof(DailyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("RelativeMonthlyRecurrence", typeof(RelativeMonthlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("RelativeYearlyRecurrence", typeof(RelativeYearlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("WeeklyRecurrence", typeof(WeeklyRecurrencePatternType))]
        public RecurrencePatternBaseType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EndDateRecurrence", typeof(EndDateRecurrenceRangeType))]
        [System.Xml.Serialization.XmlElementAttribute("NoEndRecurrence", typeof(NoEndRecurrenceRangeType))]
        [System.Xml.Serialization.XmlElementAttribute("NumberedRecurrence", typeof(NumberedRecurrenceRangeType))]
        public RecurrenceRangeBaseType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AbsoluteMonthlyRecurrencePatternType : IntervalRecurrencePatternBaseType
    {

        private int dayOfMonthField;

        /// <remarks/>
        public int DayOfMonth
        {
            get
            {
                return this.dayOfMonthField;
            }
            set
            {
                this.dayOfMonthField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DailyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WeeklyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AbsoluteMonthlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RelativeMonthlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegeneratingPatternBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(YearlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MonthlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WeeklyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DailyRegeneratingPatternType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class IntervalRecurrencePatternBaseType : RecurrencePatternBaseType
    {

        private int intervalField;

        /// <remarks/>
        public int Interval
        {
            get
            {
                return this.intervalField;
            }
            set
            {
                this.intervalField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AbsoluteYearlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RelativeYearlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IntervalRecurrencePatternBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DailyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WeeklyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AbsoluteMonthlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RelativeMonthlyRecurrencePatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegeneratingPatternBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(YearlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MonthlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WeeklyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DailyRegeneratingPatternType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class RecurrencePatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AbsoluteYearlyRecurrencePatternType : RecurrencePatternBaseType
    {

        private int dayOfMonthField;

        private MonthNamesType monthField;

        /// <remarks/>
        public int DayOfMonth
        {
            get
            {
                return this.dayOfMonthField;
            }
            set
            {
                this.dayOfMonthField = value;
            }
        }

        /// <remarks/>
        public MonthNamesType Month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MonthNamesType
    {

        /// <remarks/>
        January,

        /// <remarks/>
        February,

        /// <remarks/>
        March,

        /// <remarks/>
        April,

        /// <remarks/>
        May,

        /// <remarks/>
        June,

        /// <remarks/>
        July,

        /// <remarks/>
        August,

        /// <remarks/>
        September,

        /// <remarks/>
        October,

        /// <remarks/>
        November,

        /// <remarks/>
        December,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RelativeYearlyRecurrencePatternType : RecurrencePatternBaseType
    {

        private string daysOfWeekField;

        private DayOfWeekIndexType dayOfWeekIndexField;

        private MonthNamesType monthField;

        /// <remarks/>
        public string DaysOfWeek
        {
            get
            {
                return this.daysOfWeekField;
            }
            set
            {
                this.daysOfWeekField = value;
            }
        }

        /// <remarks/>
        public DayOfWeekIndexType DayOfWeekIndex
        {
            get
            {
                return this.dayOfWeekIndexField;
            }
            set
            {
                this.dayOfWeekIndexField = value;
            }
        }

        /// <remarks/>
        public MonthNamesType Month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DayOfWeekIndexType
    {

        /// <remarks/>
        First,

        /// <remarks/>
        Second,

        /// <remarks/>
        Third,

        /// <remarks/>
        Fourth,

        /// <remarks/>
        Last,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DailyRecurrencePatternType : IntervalRecurrencePatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class WeeklyRecurrencePatternType : IntervalRecurrencePatternBaseType
    {

        private string daysOfWeekField;

        /// <remarks/>
        public string DaysOfWeek
        {
            get
            {
                return this.daysOfWeekField;
            }
            set
            {
                this.daysOfWeekField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RelativeMonthlyRecurrencePatternType : IntervalRecurrencePatternBaseType
    {

        private DayOfWeekType daysOfWeekField;

        private DayOfWeekIndexType dayOfWeekIndexField;

        /// <remarks/>
        public DayOfWeekType DaysOfWeek
        {
            get
            {
                return this.daysOfWeekField;
            }
            set
            {
                this.daysOfWeekField = value;
            }
        }

        /// <remarks/>
        public DayOfWeekIndexType DayOfWeekIndex
        {
            get
            {
                return this.dayOfWeekIndexField;
            }
            set
            {
                this.dayOfWeekIndexField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DayOfWeekType
    {

        /// <remarks/>
        Sunday,

        /// <remarks/>
        Monday,

        /// <remarks/>
        Tuesday,

        /// <remarks/>
        Wednesday,

        /// <remarks/>
        Thursday,

        /// <remarks/>
        Friday,

        /// <remarks/>
        Saturday,

        /// <remarks/>
        Day,

        /// <remarks/>
        Weekday,

        /// <remarks/>
        WeekendDay,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(YearlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MonthlyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(WeeklyRegeneratingPatternType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DailyRegeneratingPatternType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class RegeneratingPatternBaseType : IntervalRecurrencePatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class YearlyRegeneratingPatternType : RegeneratingPatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MonthlyRegeneratingPatternType : RegeneratingPatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class WeeklyRegeneratingPatternType : RegeneratingPatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DailyRegeneratingPatternType : RegeneratingPatternBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class EndDateRecurrenceRangeType : RecurrenceRangeBaseType
    {

        private System.DateTime endDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NumberedRecurrenceRangeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EndDateRecurrenceRangeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NoEndRecurrenceRangeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class RecurrenceRangeBaseType
    {

        private System.DateTime startDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NumberedRecurrenceRangeType : RecurrenceRangeBaseType
    {

        private int numberOfOccurrencesField;

        /// <remarks/>
        public int NumberOfOccurrences
        {
            get
            {
                return this.numberOfOccurrencesField;
            }
            set
            {
                this.numberOfOccurrencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NoEndRecurrenceRangeType : RecurrenceRangeBaseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class OccurrenceInfoType
    {

        private ItemIdType itemIdField;

        private System.DateTime startField;

        private System.DateTime endField;

        private System.DateTime originalStartField;

        /// <remarks/>
        public ItemIdType ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime Start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        public System.DateTime End
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }

        /// <remarks/>
        public System.DateTime OriginalStart
        {
            get
            {
                return this.originalStartField;
            }
            set
            {
                this.originalStartField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DeletedOccurrenceInfoType
    {

        private System.DateTime startField;

        /// <remarks/>
        public System.DateTime Start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TimeZoneType
    {

        private string baseOffsetField;

        private TimeChangeType standardField;

        private TimeChangeType daylightField;

        private string timeZoneNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string BaseOffset
        {
            get
            {
                return this.baseOffsetField;
            }
            set
            {
                this.baseOffsetField = value;
            }
        }

        /// <remarks/>
        public TimeChangeType Standard
        {
            get
            {
                return this.standardField;
            }
            set
            {
                this.standardField = value;
            }
        }

        /// <remarks/>
        public TimeChangeType Daylight
        {
            get
            {
                return this.daylightField;
            }
            set
            {
                this.daylightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TimeZoneName
        {
            get
            {
                return this.timeZoneNameField;
            }
            set
            {
                this.timeZoneNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TimeChangeType
    {

        private string offsetField;

        private object itemField;

        private System.DateTime timeField;

        private string timeZoneNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
        public string Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AbsoluteDate", typeof(System.DateTime), DataType = "date")]
        [System.Xml.Serialization.XmlElementAttribute("RelativeYearlyRecurrence", typeof(RelativeYearlyRecurrencePatternType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
        public System.DateTime Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TimeZoneName
        {
            get
            {
                return this.timeZoneNameField;
            }
            set
            {
                this.timeZoneNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PostItemType : ItemType
    {

        private byte[] conversationIndexField;

        private string conversationTopicField;

        private SingleRecipientType fromField;

        private string internetMessageIdField;

        private bool isReadField;

        private bool isReadFieldSpecified;

        private System.DateTime postedTimeField;

        private bool postedTimeFieldSpecified;

        private string referencesField;

        private SingleRecipientType senderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] ConversationIndex
        {
            get
            {
                return this.conversationIndexField;
            }
            set
            {
                this.conversationIndexField = value;
            }
        }

        /// <remarks/>
        public string ConversationTopic
        {
            get
            {
                return this.conversationTopicField;
            }
            set
            {
                this.conversationTopicField = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType From
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
            }
        }

        /// <remarks/>
        public string InternetMessageId
        {
            get
            {
                return this.internetMessageIdField;
            }
            set
            {
                this.internetMessageIdField = value;
            }
        }

        /// <remarks/>
        public bool IsRead
        {
            get
            {
                return this.isReadField;
            }
            set
            {
                this.isReadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsReadSpecified
        {
            get
            {
                return this.isReadFieldSpecified;
            }
            set
            {
                this.isReadFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime PostedTime
        {
            get
            {
                return this.postedTimeField;
            }
            set
            {
                this.postedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PostedTimeSpecified
        {
            get
            {
                return this.postedTimeFieldSpecified;
            }
            set
            {
                this.postedTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        /// <remarks/>
        public SingleRecipientType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TaskType : ItemType
    {

        private int actualWorkField;

        private bool actualWorkFieldSpecified;

        private System.DateTime assignedTimeField;

        private bool assignedTimeFieldSpecified;

        private string billingInformationField;

        private int changeCountField;

        private bool changeCountFieldSpecified;

        private string[] companiesField;

        private System.DateTime completeDateField;

        private bool completeDateFieldSpecified;

        private string[] contactsField;

        private TaskDelegateStateType delegationStateField;

        private bool delegationStateFieldSpecified;

        private string delegatorField;

        private System.DateTime dueDateField;

        private bool dueDateFieldSpecified;

        private int isAssignmentEditableField;

        private bool isAssignmentEditableFieldSpecified;

        private bool isCompleteField;

        private bool isCompleteFieldSpecified;

        private bool isRecurringField;

        private bool isRecurringFieldSpecified;

        private bool isTeamTaskField;

        private bool isTeamTaskFieldSpecified;

        private string mileageField;

        private string ownerField;

        private double percentCompleteField;

        private bool percentCompleteFieldSpecified;

        private TaskRecurrenceType recurrenceField;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private TaskStatusType statusField;

        private bool statusFieldSpecified;

        private string statusDescriptionField;

        private int totalWorkField;

        private bool totalWorkFieldSpecified;

        /// <remarks/>
        public int ActualWork
        {
            get
            {
                return this.actualWorkField;
            }
            set
            {
                this.actualWorkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActualWorkSpecified
        {
            get
            {
                return this.actualWorkFieldSpecified;
            }
            set
            {
                this.actualWorkFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime AssignedTime
        {
            get
            {
                return this.assignedTimeField;
            }
            set
            {
                this.assignedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AssignedTimeSpecified
        {
            get
            {
                return this.assignedTimeFieldSpecified;
            }
            set
            {
                this.assignedTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string BillingInformation
        {
            get
            {
                return this.billingInformationField;
            }
            set
            {
                this.billingInformationField = value;
            }
        }

        /// <remarks/>
        public int ChangeCount
        {
            get
            {
                return this.changeCountField;
            }
            set
            {
                this.changeCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChangeCountSpecified
        {
            get
            {
                return this.changeCountFieldSpecified;
            }
            set
            {
                this.changeCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("String", IsNullable = false)]
        public string[] Companies
        {
            get
            {
                return this.companiesField;
            }
            set
            {
                this.companiesField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CompleteDate
        {
            get
            {
                return this.completeDateField;
            }
            set
            {
                this.completeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CompleteDateSpecified
        {
            get
            {
                return this.completeDateFieldSpecified;
            }
            set
            {
                this.completeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("String", IsNullable = false)]
        public string[] Contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
            }
        }

        /// <remarks/>
        public TaskDelegateStateType DelegationState
        {
            get
            {
                return this.delegationStateField;
            }
            set
            {
                this.delegationStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DelegationStateSpecified
        {
            get
            {
                return this.delegationStateFieldSpecified;
            }
            set
            {
                this.delegationStateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Delegator
        {
            get
            {
                return this.delegatorField;
            }
            set
            {
                this.delegatorField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DueDateSpecified
        {
            get
            {
                return this.dueDateFieldSpecified;
            }
            set
            {
                this.dueDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int IsAssignmentEditable
        {
            get
            {
                return this.isAssignmentEditableField;
            }
            set
            {
                this.isAssignmentEditableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAssignmentEditableSpecified
        {
            get
            {
                return this.isAssignmentEditableFieldSpecified;
            }
            set
            {
                this.isAssignmentEditableFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsComplete
        {
            get
            {
                return this.isCompleteField;
            }
            set
            {
                this.isCompleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCompleteSpecified
        {
            get
            {
                return this.isCompleteFieldSpecified;
            }
            set
            {
                this.isCompleteFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsRecurring
        {
            get
            {
                return this.isRecurringField;
            }
            set
            {
                this.isRecurringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsRecurringSpecified
        {
            get
            {
                return this.isRecurringFieldSpecified;
            }
            set
            {
                this.isRecurringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsTeamTask
        {
            get
            {
                return this.isTeamTaskField;
            }
            set
            {
                this.isTeamTaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsTeamTaskSpecified
        {
            get
            {
                return this.isTeamTaskFieldSpecified;
            }
            set
            {
                this.isTeamTaskFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string Mileage
        {
            get
            {
                return this.mileageField;
            }
            set
            {
                this.mileageField = value;
            }
        }

        /// <remarks/>
        public string Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }

        /// <remarks/>
        public double PercentComplete
        {
            get
            {
                return this.percentCompleteField;
            }
            set
            {
                this.percentCompleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PercentCompleteSpecified
        {
            get
            {
                return this.percentCompleteFieldSpecified;
            }
            set
            {
                this.percentCompleteFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TaskRecurrenceType Recurrence
        {
            get
            {
                return this.recurrenceField;
            }
            set
            {
                this.recurrenceField = value;
            }
        }

        /// <remarks/>
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TaskStatusType Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StatusSpecified
        {
            get
            {
                return this.statusFieldSpecified;
            }
            set
            {
                this.statusFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return this.statusDescriptionField;
            }
            set
            {
                this.statusDescriptionField = value;
            }
        }

        /// <remarks/>
        public int TotalWork
        {
            get
            {
                return this.totalWorkField;
            }
            set
            {
                this.totalWorkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalWorkSpecified
        {
            get
            {
                return this.totalWorkFieldSpecified;
            }
            set
            {
                this.totalWorkFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum TaskDelegateStateType
    {

        /// <remarks/>
        NoMatch,

        /// <remarks/>
        OwnNew,

        /// <remarks/>
        Owned,

        /// <remarks/>
        Accepted,

        /// <remarks/>
        Declined,

        /// <remarks/>
        Max,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TaskRecurrenceType
    {

        private RecurrencePatternBaseType itemField;

        private RecurrenceRangeBaseType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AbsoluteMonthlyRecurrence", typeof(AbsoluteMonthlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("AbsoluteYearlyRecurrence", typeof(AbsoluteYearlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("DailyRecurrence", typeof(DailyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("DailyRegeneration", typeof(DailyRegeneratingPatternType))]
        [System.Xml.Serialization.XmlElementAttribute("MonthlyRegeneration", typeof(MonthlyRegeneratingPatternType))]
        [System.Xml.Serialization.XmlElementAttribute("RelativeMonthlyRecurrence", typeof(RelativeMonthlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("RelativeYearlyRecurrence", typeof(RelativeYearlyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("WeeklyRecurrence", typeof(WeeklyRecurrencePatternType))]
        [System.Xml.Serialization.XmlElementAttribute("WeeklyRegeneration", typeof(WeeklyRegeneratingPatternType))]
        [System.Xml.Serialization.XmlElementAttribute("YearlyRegeneration", typeof(YearlyRegeneratingPatternType))]
        public RecurrencePatternBaseType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EndDateRecurrence", typeof(EndDateRecurrenceRangeType))]
        [System.Xml.Serialization.XmlElementAttribute("NoEndRecurrence", typeof(NoEndRecurrenceRangeType))]
        [System.Xml.Serialization.XmlElementAttribute("NumberedRecurrence", typeof(NumberedRecurrenceRangeType))]
        public RecurrenceRangeBaseType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum TaskStatusType
    {

        /// <remarks/>
        NotStarted,

        /// <remarks/>
        InProgress,

        /// <remarks/>
        Completed,

        /// <remarks/>
        WaitingOnOthers,

        /// <remarks/>
        Deferred,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ImportanceChoicesType
    {

        /// <remarks/>
        Low,

        /// <remarks/>
        Normal,

        /// <remarks/>
        High,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class InternetHeaderType
    {

        private string headerNameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HeaderName
        {
            get
            {
                return this.headerNameField;
            }
            set
            {
                this.headerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CancelCalendarItemType : SmartResponseType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SmartResponseType : SmartResponseBaseType
    {

        private BodyType newBodyContentField;

        /// <remarks/>
        public BodyType NewBodyContent
        {
            get
            {
                return this.newBodyContentField;
            }
            set
            {
                this.newBodyContentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CancelCalendarItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ForwardItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyAllToItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReplyToItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SmartResponseBaseType : ResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ForwardItemType : SmartResponseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ReplyAllToItemType : SmartResponseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ReplyToItemType : SmartResponseType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DeclineItemType : WellKnownResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PostReplyItemType : PostReplyItemBaseType
    {

        private BodyType newBodyContentField;

        /// <remarks/>
        public BodyType NewBodyContent
        {
            get
            {
                return this.newBodyContentField;
            }
            set
            {
                this.newBodyContentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PostReplyItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PostReplyItemBaseType : ResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RemoveItemType : ResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SuppressReadReceiptType : ReferenceItemResponseType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SuppressReadReceiptType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ReferenceItemResponseType : ResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TentativelyAcceptItemType : WellKnownResponseObjectType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ExtendedPropertyType
    {

        private PathToExtendedFieldType extendedFieldURIField;

        private object itemField;

        /// <remarks/>
        public PathToExtendedFieldType ExtendedFieldURI
        {
            get
            {
                return this.extendedFieldURIField;
            }
            set
            {
                this.extendedFieldURIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Values", typeof(NonEmptyArrayOfPropertyValuesType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PathToExtendedFieldType : BasePathToElementType
    {

        private DistinguishedPropertySetType distinguishedPropertySetIdField;

        private bool distinguishedPropertySetIdFieldSpecified;

        private string propertySetIdField;

        private string propertyTagField;

        private string propertyNameField;

        private int propertyIdField;

        private bool propertyIdFieldSpecified;

        private MapiPropertyTypeType propertyTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DistinguishedPropertySetType DistinguishedPropertySetId
        {
            get
            {
                return this.distinguishedPropertySetIdField;
            }
            set
            {
                this.distinguishedPropertySetIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DistinguishedPropertySetIdSpecified
        {
            get
            {
                return this.distinguishedPropertySetIdFieldSpecified;
            }
            set
            {
                this.distinguishedPropertySetIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertySetId
        {
            get
            {
                return this.propertySetIdField;
            }
            set
            {
                this.propertySetIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyTag
        {
            get
            {
                return this.propertyTagField;
            }
            set
            {
                this.propertyTagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyName
        {
            get
            {
                return this.propertyNameField;
            }
            set
            {
                this.propertyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int PropertyId
        {
            get
            {
                return this.propertyIdField;
            }
            set
            {
                this.propertyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PropertyIdSpecified
        {
            get
            {
                return this.propertyIdFieldSpecified;
            }
            set
            {
                this.propertyIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public MapiPropertyTypeType PropertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DistinguishedPropertySetType
    {

        /// <remarks/>
        Meeting,

        /// <remarks/>
        Appointment,

        /// <remarks/>
        Common,

        /// <remarks/>
        PublicStrings,

        /// <remarks/>
        Address,

        /// <remarks/>
        InternetHeaders,

        /// <remarks/>
        CalendarAssistant,

        /// <remarks/>
        UnifiedMessaging,

        /// <remarks/>
        Task,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MapiPropertyTypeType
    {

        /// <remarks/>
        ApplicationTime,

        /// <remarks/>
        ApplicationTimeArray,

        /// <remarks/>
        Binary,

        /// <remarks/>
        BinaryArray,

        /// <remarks/>
        Boolean,

        /// <remarks/>
        CLSID,

        /// <remarks/>
        CLSIDArray,

        /// <remarks/>
        Currency,

        /// <remarks/>
        CurrencyArray,

        /// <remarks/>
        Double,

        /// <remarks/>
        DoubleArray,

        /// <remarks/>
        Error,

        /// <remarks/>
        Float,

        /// <remarks/>
        FloatArray,

        /// <remarks/>
        Integer,

        /// <remarks/>
        IntegerArray,

        /// <remarks/>
        Long,

        /// <remarks/>
        LongArray,

        /// <remarks/>
        Null,

        /// <remarks/>
        Object,

        /// <remarks/>
        ObjectArray,

        /// <remarks/>
        Short,

        /// <remarks/>
        ShortArray,

        /// <remarks/>
        SystemTime,

        /// <remarks/>
        SystemTimeArray,

        /// <remarks/>
        String,

        /// <remarks/>
        StringArray,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PathToExtendedFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PathToExceptionFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PathToIndexedFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PathToUnindexedFieldType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BasePathToElementType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PathToExceptionFieldType : BasePathToElementType
    {

        private ExceptionPropertyURIType fieldURIField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ExceptionPropertyURIType FieldURI
        {
            get
            {
                return this.fieldURIField;
            }
            set
            {
                this.fieldURIField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ExceptionPropertyURIType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("attachment:Name")]
        attachmentName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("attachment:ContentType")]
        attachmentContentType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("attachment:Content")]
        attachmentContent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:Month")]
        recurrenceMonth,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:DayOfWeekIndex")]
        recurrenceDayOfWeekIndex,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:DaysOfWeek")]
        recurrenceDaysOfWeek,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:DayOfMonth")]
        recurrenceDayOfMonth,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:Interval")]
        recurrenceInterval,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("recurrence:NumberOfOccurrences")]
        recurrenceNumberOfOccurrences,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("timezone:Offset")]
        timezoneOffset,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PathToIndexedFieldType : BasePathToElementType
    {

        private DictionaryURIType fieldURIField;

        private string fieldIndexField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DictionaryURIType FieldURI
        {
            get
            {
                return this.fieldURIField;
            }
            set
            {
                this.fieldURIField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FieldIndex
        {
            get
            {
                return this.fieldIndexField;
            }
            set
            {
                this.fieldIndexField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DictionaryURIType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:InternetMessageHeader")]
        itemInternetMessageHeader,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:ImAddress")]
        contactsImAddress,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddress:Street")]
        contactsPhysicalAddressStreet,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddress:City")]
        contactsPhysicalAddressCity,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddress:State")]
        contactsPhysicalAddressState,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddress:CountryOrRegion")]
        contactsPhysicalAddressCountryOrRegion,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddress:PostalCode")]
        contactsPhysicalAddressPostalCode,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhoneNumber")]
        contactsPhoneNumber,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:EmailAddress")]
        contactsEmailAddress,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PathToUnindexedFieldType : BasePathToElementType
    {

        private UnindexedFieldURIType fieldURIField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public UnindexedFieldURIType FieldURI
        {
            get
            {
                return this.fieldURIField;
            }
            set
            {
                this.fieldURIField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum UnindexedFieldURIType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:FolderId")]
        folderFolderId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:ParentFolderId")]
        folderParentFolderId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:DisplayName")]
        folderDisplayName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:UnreadCount")]
        folderUnreadCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:TotalCount")]
        folderTotalCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:ChildFolderCount")]
        folderChildFolderCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:FolderClass")]
        folderFolderClass,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:SearchParameters")]
        folderSearchParameters,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:ManagedFolderInformation")]
        folderManagedFolderInformation,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:PermissionSet")]
        folderPermissionSet,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("folder:EffectiveRights")]
        folderEffectiveRights,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ItemId")]
        itemItemId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ParentFolderId")]
        itemParentFolderId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ItemClass")]
        itemItemClass,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:MimeContent")]
        itemMimeContent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Attachments")]
        itemAttachments,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Subject")]
        itemSubject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:DateTimeReceived")]
        itemDateTimeReceived,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Size")]
        itemSize,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Categories")]
        itemCategories,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:HasAttachments")]
        itemHasAttachments,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Importance")]
        itemImportance,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:InReplyTo")]
        itemInReplyTo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:InternetMessageHeaders")]
        itemInternetMessageHeaders,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:IsDraft")]
        itemIsDraft,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:IsFromMe")]
        itemIsFromMe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:IsResend")]
        itemIsResend,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:IsSubmitted")]
        itemIsSubmitted,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:IsUnmodified")]
        itemIsUnmodified,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:DateTimeSent")]
        itemDateTimeSent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:DateTimeCreated")]
        itemDateTimeCreated,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Body")]
        itemBody,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ResponseObjects")]
        itemResponseObjects,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Sensitivity")]
        itemSensitivity,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ReminderDueBy")]
        itemReminderDueBy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ReminderIsSet")]
        itemReminderIsSet,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:ReminderMinutesBeforeStart")]
        itemReminderMinutesBeforeStart,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:DisplayTo")]
        itemDisplayTo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:DisplayCc")]
        itemDisplayCc,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:Culture")]
        itemCulture,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:EffectiveRights")]
        itemEffectiveRights,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:LastModifiedName")]
        itemLastModifiedName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("item:LastModifiedTime")]
        itemLastModifiedTime,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ConversationIndex")]
        messageConversationIndex,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ConversationTopic")]
        messageConversationTopic,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:InternetMessageId")]
        messageInternetMessageId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:IsRead")]
        messageIsRead,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:IsResponseRequested")]
        messageIsResponseRequested,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:IsReadReceiptRequested")]
        messageIsReadReceiptRequested,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:IsDeliveryReceiptRequested")]
        messageIsDeliveryReceiptRequested,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ReceivedBy")]
        messageReceivedBy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ReceivedRepresenting")]
        messageReceivedRepresenting,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:References")]
        messageReferences,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ReplyTo")]
        messageReplyTo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:From")]
        messageFrom,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:Sender")]
        messageSender,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:ToRecipients")]
        messageToRecipients,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:CcRecipients")]
        messageCcRecipients,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("message:BccRecipients")]
        messageBccRecipients,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meeting:AssociatedCalendarItemId")]
        meetingAssociatedCalendarItemId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meeting:IsDelegated")]
        meetingIsDelegated,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meeting:IsOutOfDate")]
        meetingIsOutOfDate,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meeting:HasBeenProcessed")]
        meetingHasBeenProcessed,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meeting:ResponseType")]
        meetingResponseType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meetingRequest:MeetingRequestType")]
        meetingRequestMeetingRequestType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("meetingRequest:IntendedFreeBusyStatus")]
        meetingRequestIntendedFreeBusyStatus,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Start")]
        calendarStart,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:End")]
        calendarEnd,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:OriginalStart")]
        calendarOriginalStart,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsAllDayEvent")]
        calendarIsAllDayEvent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:LegacyFreeBusyStatus")]
        calendarLegacyFreeBusyStatus,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Location")]
        calendarLocation,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:When")]
        calendarWhen,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsMeeting")]
        calendarIsMeeting,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsCancelled")]
        calendarIsCancelled,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsRecurring")]
        calendarIsRecurring,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:MeetingRequestWasSent")]
        calendarMeetingRequestWasSent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsResponseRequested")]
        calendarIsResponseRequested,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:CalendarItemType")]
        calendarCalendarItemType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:MyResponseType")]
        calendarMyResponseType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Organizer")]
        calendarOrganizer,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:RequiredAttendees")]
        calendarRequiredAttendees,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:OptionalAttendees")]
        calendarOptionalAttendees,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Resources")]
        calendarResources,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:ConflictingMeetingCount")]
        calendarConflictingMeetingCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AdjacentMeetingCount")]
        calendarAdjacentMeetingCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:ConflictingMeetings")]
        calendarConflictingMeetings,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AdjacentMeetings")]
        calendarAdjacentMeetings,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Duration")]
        calendarDuration,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:TimeZone")]
        calendarTimeZone,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AppointmentReplyTime")]
        calendarAppointmentReplyTime,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AppointmentSequenceNumber")]
        calendarAppointmentSequenceNumber,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AppointmentState")]
        calendarAppointmentState,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:Recurrence")]
        calendarRecurrence,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:FirstOccurrence")]
        calendarFirstOccurrence,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:LastOccurrence")]
        calendarLastOccurrence,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:ModifiedOccurrences")]
        calendarModifiedOccurrences,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:DeletedOccurrences")]
        calendarDeletedOccurrences,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:MeetingTimeZone")]
        calendarMeetingTimeZone,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:ConferenceType")]
        calendarConferenceType,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:AllowNewTimeProposal")]
        calendarAllowNewTimeProposal,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:IsOnlineMeeting")]
        calendarIsOnlineMeeting,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:MeetingWorkspaceUrl")]
        calendarMeetingWorkspaceUrl,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:NetShowUrl")]
        calendarNetShowUrl,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:UID")]
        calendarUID,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:RecurrenceId")]
        calendarRecurrenceId,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("calendar:DateTimeStamp")]
        calendarDateTimeStamp,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:ActualWork")]
        taskActualWork,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:AssignedTime")]
        taskAssignedTime,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:BillingInformation")]
        taskBillingInformation,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:ChangeCount")]
        taskChangeCount,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Companies")]
        taskCompanies,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:CompleteDate")]
        taskCompleteDate,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Contacts")]
        taskContacts,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:DelegationState")]
        taskDelegationState,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Delegator")]
        taskDelegator,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:DueDate")]
        taskDueDate,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:IsAssignmentEditable")]
        taskIsAssignmentEditable,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:IsComplete")]
        taskIsComplete,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:IsRecurring")]
        taskIsRecurring,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:IsTeamTask")]
        taskIsTeamTask,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Mileage")]
        taskMileage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Owner")]
        taskOwner,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:PercentComplete")]
        taskPercentComplete,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Recurrence")]
        taskRecurrence,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:StartDate")]
        taskStartDate,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:Status")]
        taskStatus,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:StatusDescription")]
        taskStatusDescription,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("task:TotalWork")]
        taskTotalWork,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:AssistantName")]
        contactsAssistantName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Birthday")]
        contactsBirthday,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:BusinessHomePage")]
        contactsBusinessHomePage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Children")]
        contactsChildren,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Companies")]
        contactsCompanies,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:CompanyName")]
        contactsCompanyName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:CompleteName")]
        contactsCompleteName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:ContactSource")]
        contactsContactSource,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Culture")]
        contactsCulture,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Department")]
        contactsDepartment,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:DisplayName")]
        contactsDisplayName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:EmailAddresses")]
        contactsEmailAddresses,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:FileAs")]
        contactsFileAs,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:FileAsMapping")]
        contactsFileAsMapping,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Generation")]
        contactsGeneration,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:GivenName")]
        contactsGivenName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:ImAddresses")]
        contactsImAddresses,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Initials")]
        contactsInitials,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:JobTitle")]
        contactsJobTitle,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Manager")]
        contactsManager,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:MiddleName")]
        contactsMiddleName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Mileage")]
        contactsMileage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Nickname")]
        contactsNickname,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:OfficeLocation")]
        contactsOfficeLocation,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhoneNumbers")]
        contactsPhoneNumbers,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PhysicalAddresses")]
        contactsPhysicalAddresses,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:PostalAddressIndex")]
        contactsPostalAddressIndex,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Profession")]
        contactsProfession,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:SpouseName")]
        contactsSpouseName,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:Surname")]
        contactsSurname,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("contacts:WeddingAnniversary")]
        contactsWeddingAnniversary,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("postitem:PostedTime")]
        postitemPostedTime,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NonEmptyArrayOfPropertyValuesType
    {

        private string[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Value")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class EffectiveRightsType
    {

        private bool createAssociatedField;

        private bool createContentsField;

        private bool createHierarchyField;

        private bool deleteField;

        private bool modifyField;

        private bool readField;

        /// <remarks/>
        public bool CreateAssociated
        {
            get
            {
                return this.createAssociatedField;
            }
            set
            {
                this.createAssociatedField = value;
            }
        }

        /// <remarks/>
        public bool CreateContents
        {
            get
            {
                return this.createContentsField;
            }
            set
            {
                this.createContentsField = value;
            }
        }

        /// <remarks/>
        public bool CreateHierarchy
        {
            get
            {
                return this.createHierarchyField;
            }
            set
            {
                this.createHierarchyField = value;
            }
        }

        /// <remarks/>
        public bool Delete
        {
            get
            {
                return this.deleteField;
            }
            set
            {
                this.deleteField = value;
            }
        }

        /// <remarks/>
        public bool Modify
        {
            get
            {
                return this.modifyField;
            }
            set
            {
                this.modifyField = value;
            }
        }

        /// <remarks/>
        public bool Read
        {
            get
            {
                return this.readField;
            }
            set
            {
                this.readField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DistributionListType : ItemType
    {

        private string displayNameField;

        private string fileAsField;

        private ContactSourceType contactSourceField;

        private bool contactSourceFieldSpecified;

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public string FileAs
        {
            get
            {
                return this.fileAsField;
            }
            set
            {
                this.fileAsField = value;
            }
        }

        /// <remarks/>
        public ContactSourceType ContactSource
        {
            get
            {
                return this.contactSourceField;
            }
            set
            {
                this.contactSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContactSourceSpecified
        {
            get
            {
                return this.contactSourceFieldSpecified;
            }
            set
            {
                this.contactSourceFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderItemsDeleteType
    {

        private ItemIdType itemIdField;

        /// <remarks/>
        public ItemIdType ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderItemsReadFlagType
    {

        private ItemIdType itemIdField;

        private bool isReadField;

        /// <remarks/>
        public ItemIdType ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public bool IsRead
        {
            get
            {
                return this.isReadField;
            }
            set
            {
                this.isReadField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IncludeInSchema = false)]
    public enum ItemsChoiceType2
    {

        /// <remarks/>
        Create,

        /// <remarks/>
        Delete,

        /// <remarks/>
        ReadFlagChange,

        /// <remarks/>
        Update,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderHierarchyResponseMessageType : ResponseMessageType
    {

        private string syncStateField;

        private bool includesLastFolderInRangeField;

        private bool includesLastFolderInRangeFieldSpecified;

        private SyncFolderHierarchyChangesType changesField;

        /// <remarks/>
        public string SyncState
        {
            get
            {
                return this.syncStateField;
            }
            set
            {
                this.syncStateField = value;
            }
        }

        /// <remarks/>
        public bool IncludesLastFolderInRange
        {
            get
            {
                return this.includesLastFolderInRangeField;
            }
            set
            {
                this.includesLastFolderInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastFolderInRangeSpecified
        {
            get
            {
                return this.includesLastFolderInRangeFieldSpecified;
            }
            set
            {
                this.includesLastFolderInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SyncFolderHierarchyChangesType Changes
        {
            get
            {
                return this.changesField;
            }
            set
            {
                this.changesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderHierarchyChangesType
    {

        private object[] itemsField;

        private ItemsChoiceType1[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Create", typeof(SyncFolderHierarchyCreateOrUpdateType))]
        [System.Xml.Serialization.XmlElementAttribute("Delete", typeof(SyncFolderHierarchyDeleteType))]
        [System.Xml.Serialization.XmlElementAttribute("Update", typeof(SyncFolderHierarchyCreateOrUpdateType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType1[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderHierarchyCreateOrUpdateType
    {

        private BaseFolderType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarFolder", typeof(CalendarFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("ContactsFolder", typeof(ContactsFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("Folder", typeof(FolderType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchFolder", typeof(SearchFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("TasksFolder", typeof(TasksFolderType))]
        public BaseFolderType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarFolderType : BaseFolderType
    {

        private CalendarPermissionSetType permissionSetField;

        /// <remarks/>
        public CalendarPermissionSetType PermissionSet
        {
            get
            {
                return this.permissionSetField;
            }
            set
            {
                this.permissionSetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarPermissionSetType
    {

        private CalendarPermissionType[] calendarPermissionsField;

        private string[] unknownEntriesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CalendarPermission", IsNullable = false)]
        public CalendarPermissionType[] CalendarPermissions
        {
            get
            {
                return this.calendarPermissionsField;
            }
            set
            {
                this.calendarPermissionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UnknownEntry", IsNullable = false)]
        public string[] UnknownEntries
        {
            get
            {
                return this.unknownEntriesField;
            }
            set
            {
                this.unknownEntriesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarPermissionType : BasePermissionType
    {

        private CalendarPermissionReadAccessType readItemsField;

        private bool readItemsFieldSpecified;

        private CalendarPermissionLevelType calendarPermissionLevelField;

        /// <remarks/>
        public CalendarPermissionReadAccessType ReadItems
        {
            get
            {
                return this.readItemsField;
            }
            set
            {
                this.readItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReadItemsSpecified
        {
            get
            {
                return this.readItemsFieldSpecified;
            }
            set
            {
                this.readItemsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CalendarPermissionLevelType CalendarPermissionLevel
        {
            get
            {
                return this.calendarPermissionLevelField;
            }
            set
            {
                this.calendarPermissionLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum CalendarPermissionReadAccessType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        TimeOnly,

        /// <remarks/>
        TimeAndSubjectAndLocation,

        /// <remarks/>
        FullDetails,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum CalendarPermissionLevelType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Owner,

        /// <remarks/>
        PublishingEditor,

        /// <remarks/>
        Editor,

        /// <remarks/>
        PublishingAuthor,

        /// <remarks/>
        Author,

        /// <remarks/>
        NoneditingAuthor,

        /// <remarks/>
        Reviewer,

        /// <remarks/>
        Contributor,

        /// <remarks/>
        FreeBusyTimeOnly,

        /// <remarks/>
        FreeBusyTimeAndSubjectAndLocation,

        /// <remarks/>
        Custom,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalendarPermissionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PermissionType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BasePermissionType
    {

        private UserIdType userIdField;

        private bool canCreateItemsField;

        private bool canCreateItemsFieldSpecified;

        private bool canCreateSubFoldersField;

        private bool canCreateSubFoldersFieldSpecified;

        private bool isFolderOwnerField;

        private bool isFolderOwnerFieldSpecified;

        private bool isFolderVisibleField;

        private bool isFolderVisibleFieldSpecified;

        private bool isFolderContactField;

        private bool isFolderContactFieldSpecified;

        private PermissionActionType editItemsField;

        private bool editItemsFieldSpecified;

        private PermissionActionType deleteItemsField;

        private bool deleteItemsFieldSpecified;

        /// <remarks/>
        public UserIdType UserId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        public bool CanCreateItems
        {
            get
            {
                return this.canCreateItemsField;
            }
            set
            {
                this.canCreateItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CanCreateItemsSpecified
        {
            get
            {
                return this.canCreateItemsFieldSpecified;
            }
            set
            {
                this.canCreateItemsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool CanCreateSubFolders
        {
            get
            {
                return this.canCreateSubFoldersField;
            }
            set
            {
                this.canCreateSubFoldersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CanCreateSubFoldersSpecified
        {
            get
            {
                return this.canCreateSubFoldersFieldSpecified;
            }
            set
            {
                this.canCreateSubFoldersFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsFolderOwner
        {
            get
            {
                return this.isFolderOwnerField;
            }
            set
            {
                this.isFolderOwnerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFolderOwnerSpecified
        {
            get
            {
                return this.isFolderOwnerFieldSpecified;
            }
            set
            {
                this.isFolderOwnerFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsFolderVisible
        {
            get
            {
                return this.isFolderVisibleField;
            }
            set
            {
                this.isFolderVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFolderVisibleSpecified
        {
            get
            {
                return this.isFolderVisibleFieldSpecified;
            }
            set
            {
                this.isFolderVisibleFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsFolderContact
        {
            get
            {
                return this.isFolderContactField;
            }
            set
            {
                this.isFolderContactField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFolderContactSpecified
        {
            get
            {
                return this.isFolderContactFieldSpecified;
            }
            set
            {
                this.isFolderContactFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PermissionActionType EditItems
        {
            get
            {
                return this.editItemsField;
            }
            set
            {
                this.editItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EditItemsSpecified
        {
            get
            {
                return this.editItemsFieldSpecified;
            }
            set
            {
                this.editItemsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PermissionActionType DeleteItems
        {
            get
            {
                return this.deleteItemsField;
            }
            set
            {
                this.deleteItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeleteItemsSpecified
        {
            get
            {
                return this.deleteItemsFieldSpecified;
            }
            set
            {
                this.deleteItemsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PermissionActionType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Owned,

        /// <remarks/>
        All,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PermissionType : BasePermissionType
    {

        private PermissionReadAccessType readItemsField;

        private bool readItemsFieldSpecified;

        private PermissionLevelType permissionLevelField;

        /// <remarks/>
        public PermissionReadAccessType ReadItems
        {
            get
            {
                return this.readItemsField;
            }
            set
            {
                this.readItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReadItemsSpecified
        {
            get
            {
                return this.readItemsFieldSpecified;
            }
            set
            {
                this.readItemsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PermissionLevelType PermissionLevel
        {
            get
            {
                return this.permissionLevelField;
            }
            set
            {
                this.permissionLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PermissionReadAccessType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        FullDetails,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum PermissionLevelType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Owner,

        /// <remarks/>
        PublishingEditor,

        /// <remarks/>
        Editor,

        /// <remarks/>
        PublishingAuthor,

        /// <remarks/>
        Author,

        /// <remarks/>
        NoneditingAuthor,

        /// <remarks/>
        Reviewer,

        /// <remarks/>
        Contributor,

        /// <remarks/>
        Custom,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactsFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalendarFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TasksFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchFolderType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BaseFolderType
    {

        private FolderIdType folderIdField;

        private FolderIdType parentFolderIdField;

        private string folderClassField;

        private string displayNameField;

        private int totalCountField;

        private bool totalCountFieldSpecified;

        private int childFolderCountField;

        private bool childFolderCountFieldSpecified;

        private ExtendedPropertyType[] extendedPropertyField;

        private ManagedFolderInformationType managedFolderInformationField;

        private EffectiveRightsType effectiveRightsField;

        /// <remarks/>
        public FolderIdType FolderId
        {
            get
            {
                return this.folderIdField;
            }
            set
            {
                this.folderIdField = value;
            }
        }

        /// <remarks/>
        public FolderIdType ParentFolderId
        {
            get
            {
                return this.parentFolderIdField;
            }
            set
            {
                this.parentFolderIdField = value;
            }
        }

        /// <remarks/>
        public string FolderClass
        {
            get
            {
                return this.folderClassField;
            }
            set
            {
                this.folderClassField = value;
            }
        }

        /// <remarks/>
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        public int TotalCount
        {
            get
            {
                return this.totalCountField;
            }
            set
            {
                this.totalCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalCountSpecified
        {
            get
            {
                return this.totalCountFieldSpecified;
            }
            set
            {
                this.totalCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int ChildFolderCount
        {
            get
            {
                return this.childFolderCountField;
            }
            set
            {
                this.childFolderCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChildFolderCountSpecified
        {
            get
            {
                return this.childFolderCountFieldSpecified;
            }
            set
            {
                this.childFolderCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedProperty")]
        public ExtendedPropertyType[] ExtendedProperty
        {
            get
            {
                return this.extendedPropertyField;
            }
            set
            {
                this.extendedPropertyField = value;
            }
        }

        /// <remarks/>
        public ManagedFolderInformationType ManagedFolderInformation
        {
            get
            {
                return this.managedFolderInformationField;
            }
            set
            {
                this.managedFolderInformationField = value;
            }
        }

        /// <remarks/>
        public EffectiveRightsType EffectiveRights
        {
            get
            {
                return this.effectiveRightsField;
            }
            set
            {
                this.effectiveRightsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ManagedFolderInformationType
    {

        private bool canDeleteField;

        private bool canDeleteFieldSpecified;

        private bool canRenameOrMoveField;

        private bool canRenameOrMoveFieldSpecified;

        private bool mustDisplayCommentField;

        private bool mustDisplayCommentFieldSpecified;

        private bool hasQuotaField;

        private bool hasQuotaFieldSpecified;

        private bool isManagedFoldersRootField;

        private bool isManagedFoldersRootFieldSpecified;

        private string managedFolderIdField;

        private string commentField;

        private int storageQuotaField;

        private bool storageQuotaFieldSpecified;

        private int folderSizeField;

        private bool folderSizeFieldSpecified;

        private string homePageField;

        /// <remarks/>
        public bool CanDelete
        {
            get
            {
                return this.canDeleteField;
            }
            set
            {
                this.canDeleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CanDeleteSpecified
        {
            get
            {
                return this.canDeleteFieldSpecified;
            }
            set
            {
                this.canDeleteFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool CanRenameOrMove
        {
            get
            {
                return this.canRenameOrMoveField;
            }
            set
            {
                this.canRenameOrMoveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CanRenameOrMoveSpecified
        {
            get
            {
                return this.canRenameOrMoveFieldSpecified;
            }
            set
            {
                this.canRenameOrMoveFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool MustDisplayComment
        {
            get
            {
                return this.mustDisplayCommentField;
            }
            set
            {
                this.mustDisplayCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MustDisplayCommentSpecified
        {
            get
            {
                return this.mustDisplayCommentFieldSpecified;
            }
            set
            {
                this.mustDisplayCommentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool HasQuota
        {
            get
            {
                return this.hasQuotaField;
            }
            set
            {
                this.hasQuotaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HasQuotaSpecified
        {
            get
            {
                return this.hasQuotaFieldSpecified;
            }
            set
            {
                this.hasQuotaFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsManagedFoldersRoot
        {
            get
            {
                return this.isManagedFoldersRootField;
            }
            set
            {
                this.isManagedFoldersRootField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsManagedFoldersRootSpecified
        {
            get
            {
                return this.isManagedFoldersRootFieldSpecified;
            }
            set
            {
                this.isManagedFoldersRootFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ManagedFolderId
        {
            get
            {
                return this.managedFolderIdField;
            }
            set
            {
                this.managedFolderIdField = value;
            }
        }

        /// <remarks/>
        public string Comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        public int StorageQuota
        {
            get
            {
                return this.storageQuotaField;
            }
            set
            {
                this.storageQuotaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StorageQuotaSpecified
        {
            get
            {
                return this.storageQuotaFieldSpecified;
            }
            set
            {
                this.storageQuotaFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int FolderSize
        {
            get
            {
                return this.folderSizeField;
            }
            set
            {
                this.folderSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FolderSizeSpecified
        {
            get
            {
                return this.folderSizeFieldSpecified;
            }
            set
            {
                this.folderSizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string HomePage
        {
            get
            {
                return this.homePageField;
            }
            set
            {
                this.homePageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ContactsFolderType : BaseFolderType
    {

        private PermissionSetType permissionSetField;

        /// <remarks/>
        public PermissionSetType PermissionSet
        {
            get
            {
                return this.permissionSetField;
            }
            set
            {
                this.permissionSetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PermissionSetType
    {

        private PermissionType[] permissionsField;

        private string[] unknownEntriesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Permission", IsNullable = false)]
        public PermissionType[] Permissions
        {
            get
            {
                return this.permissionsField;
            }
            set
            {
                this.permissionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UnknownEntry", IsNullable = false)]
        public string[] UnknownEntries
        {
            get
            {
                return this.unknownEntriesField;
            }
            set
            {
                this.unknownEntriesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TasksFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchFolderType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FolderType : BaseFolderType
    {

        private PermissionSetType permissionSetField;

        private int unreadCountField;

        private bool unreadCountFieldSpecified;

        /// <remarks/>
        public PermissionSetType PermissionSet
        {
            get
            {
                return this.permissionSetField;
            }
            set
            {
                this.permissionSetField = value;
            }
        }

        /// <remarks/>
        public int UnreadCount
        {
            get
            {
                return this.unreadCountField;
            }
            set
            {
                this.unreadCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnreadCountSpecified
        {
            get
            {
                return this.unreadCountFieldSpecified;
            }
            set
            {
                this.unreadCountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TasksFolderType : FolderType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SearchFolderType : FolderType
    {

        private SearchParametersType searchParametersField;

        /// <remarks/>
        public SearchParametersType SearchParameters
        {
            get
            {
                return this.searchParametersField;
            }
            set
            {
                this.searchParametersField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SearchParametersType
    {

        private RestrictionType restrictionField;

        private BaseFolderIdType[] baseFolderIdsField;

        private SearchFolderTraversalType traversalField;

        private bool traversalFieldSpecified;

        /// <remarks/>
        public RestrictionType Restriction
        {
            get
            {
                return this.restrictionField;
            }
            set
            {
                this.restrictionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), IsNullable = false)]
        public BaseFolderIdType[] BaseFolderIds
        {
            get
            {
                return this.baseFolderIdsField;
            }
            set
            {
                this.baseFolderIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SearchFolderTraversalType Traversal
        {
            get
            {
                return this.traversalField;
            }
            set
            {
                this.traversalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TraversalSpecified
        {
            get
            {
                return this.traversalFieldSpecified;
            }
            set
            {
                this.traversalFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class RestrictionType
    {

        private SearchExpressionType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType))]
        [System.Xml.Serialization.XmlElementAttribute("Contains", typeof(ContainsExpressionType))]
        [System.Xml.Serialization.XmlElementAttribute("Excludes", typeof(ExcludesType))]
        [System.Xml.Serialization.XmlElementAttribute("Exists", typeof(ExistsType))]
        [System.Xml.Serialization.XmlElementAttribute("IsEqualTo", typeof(IsEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThan", typeof(IsGreaterThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThanOrEqualTo", typeof(IsGreaterThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThan", typeof(IsLessThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThanOrEqualTo", typeof(IsLessThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsNotEqualTo", typeof(IsNotEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("Not", typeof(NotType))]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchExpression", typeof(SearchExpressionType))]
        public SearchExpressionType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AndType : MultipleOperandBooleanExpressionType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AndType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class MultipleOperandBooleanExpressionType : SearchExpressionType
    {

        private SearchExpressionType[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType))]
        [System.Xml.Serialization.XmlElementAttribute("Contains", typeof(ContainsExpressionType))]
        [System.Xml.Serialization.XmlElementAttribute("Excludes", typeof(ExcludesType))]
        [System.Xml.Serialization.XmlElementAttribute("Exists", typeof(ExistsType))]
        [System.Xml.Serialization.XmlElementAttribute("IsEqualTo", typeof(IsEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThan", typeof(IsGreaterThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThanOrEqualTo", typeof(IsGreaterThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThan", typeof(IsLessThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThanOrEqualTo", typeof(IsLessThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsNotEqualTo", typeof(IsNotEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("Not", typeof(NotType))]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchExpression", typeof(SearchExpressionType))]
        public SearchExpressionType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ContainsExpressionType : SearchExpressionType
    {

        private BasePathToElementType itemField;

        private ConstantValueType constantField;

        private ContainmentModeType containmentModeField;

        private bool containmentModeFieldSpecified;

        private ContainmentComparisonType containmentComparisonField;

        private bool containmentComparisonFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public ConstantValueType Constant
        {
            get
            {
                return this.constantField;
            }
            set
            {
                this.constantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ContainmentModeType ContainmentMode
        {
            get
            {
                return this.containmentModeField;
            }
            set
            {
                this.containmentModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContainmentModeSpecified
        {
            get
            {
                return this.containmentModeFieldSpecified;
            }
            set
            {
                this.containmentModeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ContainmentComparisonType ContainmentComparison
        {
            get
            {
                return this.containmentComparisonField;
            }
            set
            {
                this.containmentComparisonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ContainmentComparisonSpecified
        {
            get
            {
                return this.containmentComparisonFieldSpecified;
            }
            set
            {
                this.containmentComparisonFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ConstantValueType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ContainmentModeType
    {

        /// <remarks/>
        FullString,

        /// <remarks/>
        Prefixed,

        /// <remarks/>
        Substring,

        /// <remarks/>
        PrefixOnWords,

        /// <remarks/>
        ExactPhrase,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ContainmentComparisonType
    {

        /// <remarks/>
        Exact,

        /// <remarks/>
        IgnoreCase,

        /// <remarks/>
        IgnoreNonSpacingCharacters,

        /// <remarks/>
        Loose,

        /// <remarks/>
        IgnoreCaseAndNonSpacingCharacters,

        /// <remarks/>
        LooseAndIgnoreCase,

        /// <remarks/>
        LooseAndIgnoreNonSpace,

        /// <remarks/>
        LooseAndIgnoreCaseAndIgnoreNonSpace,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MultipleOperandBooleanExpressionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AndType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NotType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContainsExpressionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExcludesType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TwoOperandExpressionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsLessThanOrEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsLessThanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsGreaterThanOrEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsGreaterThanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsNotEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExistsType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class SearchExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NotType : SearchExpressionType
    {

        private SearchExpressionType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType))]
        [System.Xml.Serialization.XmlElementAttribute("Contains", typeof(ContainsExpressionType))]
        [System.Xml.Serialization.XmlElementAttribute("Excludes", typeof(ExcludesType))]
        [System.Xml.Serialization.XmlElementAttribute("Exists", typeof(ExistsType))]
        [System.Xml.Serialization.XmlElementAttribute("IsEqualTo", typeof(IsEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThan", typeof(IsGreaterThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsGreaterThanOrEqualTo", typeof(IsGreaterThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThan", typeof(IsLessThanType))]
        [System.Xml.Serialization.XmlElementAttribute("IsLessThanOrEqualTo", typeof(IsLessThanOrEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("IsNotEqualTo", typeof(IsNotEqualToType))]
        [System.Xml.Serialization.XmlElementAttribute("Not", typeof(NotType))]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchExpression", typeof(SearchExpressionType))]
        public SearchExpressionType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ExcludesType : SearchExpressionType
    {

        private BasePathToElementType itemField;

        private ExcludesValueType bitmaskField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public ExcludesValueType Bitmask
        {
            get
            {
                return this.bitmaskField;
            }
            set
            {
                this.bitmaskField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ExcludesValueType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ExistsType : SearchExpressionType
    {

        private BasePathToElementType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsEqualToType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsLessThanOrEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsLessThanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsGreaterThanOrEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsGreaterThanType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsNotEqualToType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsEqualToType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class TwoOperandExpressionType : SearchExpressionType
    {

        private BasePathToElementType itemField;

        private FieldURIOrConstantType fieldURIOrConstantField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public FieldURIOrConstantType FieldURIOrConstant
        {
            get
            {
                return this.fieldURIOrConstantField;
            }
            set
            {
                this.fieldURIOrConstantField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FieldURIOrConstantType
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Constant", typeof(ConstantValueType))]
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("Path", typeof(BasePathToElementType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsLessThanOrEqualToType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsLessThanType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsGreaterThanOrEqualToType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsGreaterThanType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IsNotEqualToType : TwoOperandExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class OrType : MultipleOperandBooleanExpressionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum SearchFolderTraversalType
    {

        /// <remarks/>
        Shallow,

        /// <remarks/>
        Deep,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SyncFolderHierarchyDeleteType
    {

        private FolderIdType folderIdField;

        /// <remarks/>
        public FolderIdType FolderId
        {
            get
            {
                return this.folderIdField;
            }
            set
            {
                this.folderIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IncludeInSchema = false)]
    public enum ItemsChoiceType1
    {

        /// <remarks/>
        Create,

        /// <remarks/>
        Delete,

        /// <remarks/>
        Update,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SendNotificationResponseMessageType : ResponseMessageType
    {

        private NotificationType notificationField;

        /// <remarks/>
        public NotificationType Notification
        {
            get
            {
                return this.notificationField;
            }
            set
            {
                this.notificationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class NotificationType
    {

        private string subscriptionIdField;

        private string previousWatermarkField;

        private bool moreEventsField;

        private BaseNotificationEventType[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        public string SubscriptionId
        {
            get
            {
                return this.subscriptionIdField;
            }
            set
            {
                this.subscriptionIdField = value;
            }
        }

        /// <remarks/>
        public string PreviousWatermark
        {
            get
            {
                return this.previousWatermarkField;
            }
            set
            {
                this.previousWatermarkField = value;
            }
        }

        /// <remarks/>
        public bool MoreEvents
        {
            get
            {
                return this.moreEventsField;
            }
            set
            {
                this.moreEventsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CopiedEvent", typeof(MovedCopiedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("CreatedEvent", typeof(BaseObjectChangedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("DeletedEvent", typeof(BaseObjectChangedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("ModifiedEvent", typeof(ModifiedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("MovedEvent", typeof(MovedCopiedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("NewMailEvent", typeof(BaseObjectChangedEventType))]
        [System.Xml.Serialization.XmlElementAttribute("StatusEvent", typeof(BaseNotificationEventType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public BaseNotificationEventType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MovedCopiedEventType : BaseObjectChangedEventType
    {

        private object item1Field;

        private FolderIdType oldParentFolderIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OldFolderId", typeof(FolderIdType))]
        [System.Xml.Serialization.XmlElementAttribute("OldItemId", typeof(ItemIdType))]
        public object Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }

        /// <remarks/>
        public FolderIdType OldParentFolderId
        {
            get
            {
                return this.oldParentFolderIdField;
            }
            set
            {
                this.oldParentFolderIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MovedCopiedEventType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ModifiedEventType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class BaseObjectChangedEventType : BaseNotificationEventType
    {

        private System.DateTime timeStampField;

        private object itemField;

        private FolderIdType parentFolderIdField;

        /// <remarks/>
        public System.DateTime TimeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FolderId", typeof(FolderIdType))]
        [System.Xml.Serialization.XmlElementAttribute("ItemId", typeof(ItemIdType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public FolderIdType ParentFolderId
        {
            get
            {
                return this.parentFolderIdField;
            }
            set
            {
                this.parentFolderIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseObjectChangedEventType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MovedCopiedEventType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ModifiedEventType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class BaseNotificationEventType
    {

        private string watermarkField;

        /// <remarks/>
        public string Watermark
        {
            get
            {
                return this.watermarkField;
            }
            set
            {
                this.watermarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ModifiedEventType : BaseObjectChangedEventType
    {

        private int unreadCountField;

        private bool unreadCountFieldSpecified;

        /// <remarks/>
        public int UnreadCount
        {
            get
            {
                return this.unreadCountField;
            }
            set
            {
                this.unreadCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UnreadCountSpecified
        {
            get
            {
                return this.unreadCountFieldSpecified;
            }
            set
            {
                this.unreadCountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        CopiedEvent,

        /// <remarks/>
        CreatedEvent,

        /// <remarks/>
        DeletedEvent,

        /// <remarks/>
        ModifiedEvent,

        /// <remarks/>
        MovedEvent,

        /// <remarks/>
        NewMailEvent,

        /// <remarks/>
        StatusEvent,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetEventsResponseMessageType : ResponseMessageType
    {

        private NotificationType notificationField;

        /// <remarks/>
        public NotificationType Notification
        {
            get
            {
                return this.notificationField;
            }
            set
            {
                this.notificationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SubscribeResponseMessageType : ResponseMessageType
    {

        private string subscriptionIdField;

        private string watermarkField;

        /// <remarks/>
        public string SubscriptionId
        {
            get
            {
                return this.subscriptionIdField;
            }
            set
            {
                this.subscriptionIdField = value;
            }
        }

        /// <remarks/>
        public string Watermark
        {
            get
            {
                return this.watermarkField;
            }
            set
            {
                this.watermarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ExpandDLResponseMessageType : ResponseMessageType
    {

        private ArrayOfDLExpansionType dLExpansionField;

        private int indexedPagingOffsetField;

        private bool indexedPagingOffsetFieldSpecified;

        private int numeratorOffsetField;

        private bool numeratorOffsetFieldSpecified;

        private int absoluteDenominatorField;

        private bool absoluteDenominatorFieldSpecified;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private int totalItemsInViewField;

        private bool totalItemsInViewFieldSpecified;

        /// <remarks/>
        public ArrayOfDLExpansionType DLExpansion
        {
            get
            {
                return this.dLExpansionField;
            }
            set
            {
                this.dLExpansionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IndexedPagingOffset
        {
            get
            {
                return this.indexedPagingOffsetField;
            }
            set
            {
                this.indexedPagingOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndexedPagingOffsetSpecified
        {
            get
            {
                return this.indexedPagingOffsetFieldSpecified;
            }
            set
            {
                this.indexedPagingOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeratorOffset
        {
            get
            {
                return this.numeratorOffsetField;
            }
            set
            {
                this.numeratorOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumeratorOffsetSpecified
        {
            get
            {
                return this.numeratorOffsetFieldSpecified;
            }
            set
            {
                this.numeratorOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AbsoluteDenominator
        {
            get
            {
                return this.absoluteDenominatorField;
            }
            set
            {
                this.absoluteDenominatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AbsoluteDenominatorSpecified
        {
            get
            {
                return this.absoluteDenominatorFieldSpecified;
            }
            set
            {
                this.absoluteDenominatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int TotalItemsInView
        {
            get
            {
                return this.totalItemsInViewField;
            }
            set
            {
                this.totalItemsInViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalItemsInViewSpecified
        {
            get
            {
                return this.totalItemsInViewFieldSpecified;
            }
            set
            {
                this.totalItemsInViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ArrayOfDLExpansionType
    {

        private EmailAddressType[] mailboxField;

        private int indexedPagingOffsetField;

        private bool indexedPagingOffsetFieldSpecified;

        private int numeratorOffsetField;

        private bool numeratorOffsetFieldSpecified;

        private int absoluteDenominatorField;

        private bool absoluteDenominatorFieldSpecified;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private int totalItemsInViewField;

        private bool totalItemsInViewFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Mailbox")]
        public EmailAddressType[] Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IndexedPagingOffset
        {
            get
            {
                return this.indexedPagingOffsetField;
            }
            set
            {
                this.indexedPagingOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndexedPagingOffsetSpecified
        {
            get
            {
                return this.indexedPagingOffsetFieldSpecified;
            }
            set
            {
                this.indexedPagingOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeratorOffset
        {
            get
            {
                return this.numeratorOffsetField;
            }
            set
            {
                this.numeratorOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumeratorOffsetSpecified
        {
            get
            {
                return this.numeratorOffsetFieldSpecified;
            }
            set
            {
                this.numeratorOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AbsoluteDenominator
        {
            get
            {
                return this.absoluteDenominatorField;
            }
            set
            {
                this.absoluteDenominatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AbsoluteDenominatorSpecified
        {
            get
            {
                return this.absoluteDenominatorFieldSpecified;
            }
            set
            {
                this.absoluteDenominatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int TotalItemsInView
        {
            get
            {
                return this.totalItemsInViewField;
            }
            set
            {
                this.totalItemsInViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalItemsInViewSpecified
        {
            get
            {
                return this.totalItemsInViewFieldSpecified;
            }
            set
            {
                this.totalItemsInViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ResolveNamesResponseMessageType : ResponseMessageType
    {

        private ArrayOfResolutionType resolutionSetField;

        /// <remarks/>
        public ArrayOfResolutionType ResolutionSet
        {
            get
            {
                return this.resolutionSetField;
            }
            set
            {
                this.resolutionSetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ArrayOfResolutionType
    {

        private ResolutionType[] resolutionField;

        private int indexedPagingOffsetField;

        private bool indexedPagingOffsetFieldSpecified;

        private int numeratorOffsetField;

        private bool numeratorOffsetFieldSpecified;

        private int absoluteDenominatorField;

        private bool absoluteDenominatorFieldSpecified;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private int totalItemsInViewField;

        private bool totalItemsInViewFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Resolution")]
        public ResolutionType[] Resolution
        {
            get
            {
                return this.resolutionField;
            }
            set
            {
                this.resolutionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IndexedPagingOffset
        {
            get
            {
                return this.indexedPagingOffsetField;
            }
            set
            {
                this.indexedPagingOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndexedPagingOffsetSpecified
        {
            get
            {
                return this.indexedPagingOffsetFieldSpecified;
            }
            set
            {
                this.indexedPagingOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeratorOffset
        {
            get
            {
                return this.numeratorOffsetField;
            }
            set
            {
                this.numeratorOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumeratorOffsetSpecified
        {
            get
            {
                return this.numeratorOffsetFieldSpecified;
            }
            set
            {
                this.numeratorOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AbsoluteDenominator
        {
            get
            {
                return this.absoluteDenominatorField;
            }
            set
            {
                this.absoluteDenominatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AbsoluteDenominatorSpecified
        {
            get
            {
                return this.absoluteDenominatorFieldSpecified;
            }
            set
            {
                this.absoluteDenominatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int TotalItemsInView
        {
            get
            {
                return this.totalItemsInViewField;
            }
            set
            {
                this.totalItemsInViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalItemsInViewSpecified
        {
            get
            {
                return this.totalItemsInViewFieldSpecified;
            }
            set
            {
                this.totalItemsInViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ResolutionType
    {

        private EmailAddressType mailboxField;

        private ContactItemType contactField;

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }

        /// <remarks/>
        public ContactItemType Contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindItemResponseMessageType : ResponseMessageType
    {

        private FindItemParentType rootFolderField;

        /// <remarks/>
        public FindItemParentType RootFolder
        {
            get
            {
                return this.rootFolderField;
            }
            set
            {
                this.rootFolderField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FindItemParentType
    {

        private object itemField;

        private int indexedPagingOffsetField;

        private bool indexedPagingOffsetFieldSpecified;

        private int numeratorOffsetField;

        private bool numeratorOffsetFieldSpecified;

        private int absoluteDenominatorField;

        private bool absoluteDenominatorFieldSpecified;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private int totalItemsInViewField;

        private bool totalItemsInViewFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Groups", typeof(ArrayOfGroupedItemsType))]
        [System.Xml.Serialization.XmlElementAttribute("Items", typeof(ArrayOfRealItemsType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IndexedPagingOffset
        {
            get
            {
                return this.indexedPagingOffsetField;
            }
            set
            {
                this.indexedPagingOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndexedPagingOffsetSpecified
        {
            get
            {
                return this.indexedPagingOffsetFieldSpecified;
            }
            set
            {
                this.indexedPagingOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeratorOffset
        {
            get
            {
                return this.numeratorOffsetField;
            }
            set
            {
                this.numeratorOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumeratorOffsetSpecified
        {
            get
            {
                return this.numeratorOffsetFieldSpecified;
            }
            set
            {
                this.numeratorOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AbsoluteDenominator
        {
            get
            {
                return this.absoluteDenominatorField;
            }
            set
            {
                this.absoluteDenominatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AbsoluteDenominatorSpecified
        {
            get
            {
                return this.absoluteDenominatorFieldSpecified;
            }
            set
            {
                this.absoluteDenominatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int TotalItemsInView
        {
            get
            {
                return this.totalItemsInViewField;
            }
            set
            {
                this.totalItemsInViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalItemsInViewSpecified
        {
            get
            {
                return this.totalItemsInViewFieldSpecified;
            }
            set
            {
                this.totalItemsInViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ArrayOfGroupedItemsType
    {

        private GroupedItemsType[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GroupedItems")]
        public GroupedItemsType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class GroupedItemsType
    {

        private string groupIndexField;

        private ArrayOfRealItemsType itemsField;

        /// <remarks/>
        public string GroupIndex
        {
            get
            {
                return this.groupIndexField;
            }
            set
            {
                this.groupIndexField = value;
            }
        }

        /// <remarks/>
        public ArrayOfRealItemsType Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ArrayOfRealItemsType
    {

        private ItemType[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DistributionList", typeof(DistributionListType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        public ItemType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteAttachmentResponseMessageType : ResponseMessageType
    {

        private RootItemIdType rootItemIdField;

        /// <remarks/>
        public RootItemIdType RootItemId
        {
            get
            {
                return this.rootItemIdField;
            }
            set
            {
                this.rootItemIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class AttachmentInfoResponseMessageType : ResponseMessageType
    {

        private AttachmentType[] attachmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FileAttachment", typeof(FileAttachmentType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemAttachment", typeof(ItemAttachmentType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateItemResponseMessageType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ItemInfoResponseMessageType : ResponseMessageType
    {

        private ArrayOfRealItemsType itemsField;

        /// <remarks/>
        public ArrayOfRealItemsType Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateItemResponseMessageType : ItemInfoResponseMessageType
    {

        private ConflictResultsType conflictResultsField;

        /// <remarks/>
        public ConflictResultsType ConflictResults
        {
            get
            {
                return this.conflictResultsField;
            }
            set
            {
                this.conflictResultsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ConflictResultsType
    {

        private int countField;

        /// <remarks/>
        public int Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindFolderResponseMessageType : ResponseMessageType
    {

        private FindFolderParentType rootFolderField;

        /// <remarks/>
        public FindFolderParentType RootFolder
        {
            get
            {
                return this.rootFolderField;
            }
            set
            {
                this.rootFolderField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FindFolderParentType
    {

        private BaseFolderType[] foldersField;

        private int indexedPagingOffsetField;

        private bool indexedPagingOffsetFieldSpecified;

        private int numeratorOffsetField;

        private bool numeratorOffsetFieldSpecified;

        private int absoluteDenominatorField;

        private bool absoluteDenominatorFieldSpecified;

        private bool includesLastItemInRangeField;

        private bool includesLastItemInRangeFieldSpecified;

        private int totalItemsInViewField;

        private bool totalItemsInViewFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CalendarFolder", typeof(CalendarFolderType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ContactsFolder", typeof(ContactsFolderType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Folder", typeof(FolderType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SearchFolder", typeof(SearchFolderType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TasksFolder", typeof(TasksFolderType), IsNullable = false)]
        public BaseFolderType[] Folders
        {
            get
            {
                return this.foldersField;
            }
            set
            {
                this.foldersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int IndexedPagingOffset
        {
            get
            {
                return this.indexedPagingOffsetField;
            }
            set
            {
                this.indexedPagingOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IndexedPagingOffsetSpecified
        {
            get
            {
                return this.indexedPagingOffsetFieldSpecified;
            }
            set
            {
                this.indexedPagingOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int NumeratorOffset
        {
            get
            {
                return this.numeratorOffsetField;
            }
            set
            {
                this.numeratorOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NumeratorOffsetSpecified
        {
            get
            {
                return this.numeratorOffsetFieldSpecified;
            }
            set
            {
                this.numeratorOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AbsoluteDenominator
        {
            get
            {
                return this.absoluteDenominatorField;
            }
            set
            {
                this.absoluteDenominatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AbsoluteDenominatorSpecified
        {
            get
            {
                return this.absoluteDenominatorFieldSpecified;
            }
            set
            {
                this.absoluteDenominatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludesLastItemInRange
        {
            get
            {
                return this.includesLastItemInRangeField;
            }
            set
            {
                this.includesLastItemInRangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludesLastItemInRangeSpecified
        {
            get
            {
                return this.includesLastItemInRangeFieldSpecified;
            }
            set
            {
                this.includesLastItemInRangeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int TotalItemsInView
        {
            get
            {
                return this.totalItemsInViewField;
            }
            set
            {
                this.totalItemsInViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalItemsInViewSpecified
        {
            get
            {
                return this.totalItemsInViewFieldSpecified;
            }
            set
            {
                this.totalItemsInViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FolderInfoResponseMessageType : ResponseMessageType
    {

        private BaseFolderType[] foldersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CalendarFolder", typeof(CalendarFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ContactsFolder", typeof(ContactsFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Folder", typeof(FolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SearchFolder", typeof(SearchFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TasksFolder", typeof(TasksFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderType[] Folders
        {
            get
            {
                return this.foldersField;
            }
            set
            {
                this.foldersField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetUserOofSettingsResponse
    {

        private ResponseMessageType responseMessageField;

        private UserOofSettings oofSettingsField;

        private ExternalAudience allowExternalOofField;

        private bool allowExternalOofFieldSpecified;

        /// <remarks/>
        public ResponseMessageType ResponseMessage
        {
            get
            {
                return this.responseMessageField;
            }
            set
            {
                this.responseMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public UserOofSettings OofSettings
        {
            get
            {
                return this.oofSettingsField;
            }
            set
            {
                this.oofSettingsField = value;
            }
        }

        /// <remarks/>
        public ExternalAudience AllowExternalOof
        {
            get
            {
                return this.allowExternalOofField;
            }
            set
            {
                this.allowExternalOofField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AllowExternalOofSpecified
        {
            get
            {
                return this.allowExternalOofFieldSpecified;
            }
            set
            {
                this.allowExternalOofFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class UserOofSettings
    {

        private OofState oofStateField;

        private ExternalAudience externalAudienceField;

        private Duration durationField;

        private ReplyBody internalReplyField;

        private ReplyBody externalReplyField;

        /// <remarks/>
        public OofState OofState
        {
            get
            {
                return this.oofStateField;
            }
            set
            {
                this.oofStateField = value;
            }
        }

        /// <remarks/>
        public ExternalAudience ExternalAudience
        {
            get
            {
                return this.externalAudienceField;
            }
            set
            {
                this.externalAudienceField = value;
            }
        }

        /// <remarks/>
        public Duration Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        public ReplyBody InternalReply
        {
            get
            {
                return this.internalReplyField;
            }
            set
            {
                this.internalReplyField = value;
            }
        }

        /// <remarks/>
        public ReplyBody ExternalReply
        {
            get
            {
                return this.externalReplyField;
            }
            set
            {
                this.externalReplyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum OofState
    {

        /// <remarks/>
        Disabled,

        /// <remarks/>
        Enabled,

        /// <remarks/>
        Scheduled,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ExternalAudience
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Known,

        /// <remarks/>
        All,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class Duration
    {

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        /// <remarks/>
        public System.DateTime StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ReplyBody
    {

        private string messageField;

        private string langField;

        /// <remarks/>
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupAttendeeConflictData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IndividualAttendeeConflictData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TooBigGroupAttendeeConflictData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnknownAttendeeConflictData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class AttendeeConflictData
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class GroupAttendeeConflictData : AttendeeConflictData
    {

        private int numberOfMembersField;

        private int numberOfMembersAvailableField;

        private int numberOfMembersWithConflictField;

        private int numberOfMembersWithNoDataField;

        /// <remarks/>
        public int NumberOfMembers
        {
            get
            {
                return this.numberOfMembersField;
            }
            set
            {
                this.numberOfMembersField = value;
            }
        }

        /// <remarks/>
        public int NumberOfMembersAvailable
        {
            get
            {
                return this.numberOfMembersAvailableField;
            }
            set
            {
                this.numberOfMembersAvailableField = value;
            }
        }

        /// <remarks/>
        public int NumberOfMembersWithConflict
        {
            get
            {
                return this.numberOfMembersWithConflictField;
            }
            set
            {
                this.numberOfMembersWithConflictField = value;
            }
        }

        /// <remarks/>
        public int NumberOfMembersWithNoData
        {
            get
            {
                return this.numberOfMembersWithNoDataField;
            }
            set
            {
                this.numberOfMembersWithNoDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IndividualAttendeeConflictData : AttendeeConflictData
    {

        private LegacyFreeBusyType busyTypeField;

        /// <remarks/>
        public LegacyFreeBusyType BusyType
        {
            get
            {
                return this.busyTypeField;
            }
            set
            {
                this.busyTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TooBigGroupAttendeeConflictData : AttendeeConflictData
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class UnknownAttendeeConflictData : AttendeeConflictData
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class Suggestion
    {

        private System.DateTime meetingTimeField;

        private bool isWorkTimeField;

        private SuggestionQuality suggestionQualityField;

        private AttendeeConflictData[] attendeeConflictDataArrayField;

        /// <remarks/>
        public System.DateTime MeetingTime
        {
            get
            {
                return this.meetingTimeField;
            }
            set
            {
                this.meetingTimeField = value;
            }
        }

        /// <remarks/>
        public bool IsWorkTime
        {
            get
            {
                return this.isWorkTimeField;
            }
            set
            {
                this.isWorkTimeField = value;
            }
        }

        /// <remarks/>
        public SuggestionQuality SuggestionQuality
        {
            get
            {
                return this.suggestionQualityField;
            }
            set
            {
                this.suggestionQualityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(GroupAttendeeConflictData))]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(IndividualAttendeeConflictData))]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(TooBigGroupAttendeeConflictData))]
        [System.Xml.Serialization.XmlArrayItemAttribute(typeof(UnknownAttendeeConflictData))]
        public AttendeeConflictData[] AttendeeConflictDataArray
        {
            get
            {
                return this.attendeeConflictDataArrayField;
            }
            set
            {
                this.attendeeConflictDataArrayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum SuggestionQuality
    {

        /// <remarks/>
        Excellent,

        /// <remarks/>
        Good,

        /// <remarks/>
        Fair,

        /// <remarks/>
        Poor,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SuggestionDayResult
    {

        private System.DateTime dateField;

        private SuggestionQuality dayQualityField;

        private Suggestion[] suggestionArrayField;

        /// <remarks/>
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public SuggestionQuality DayQuality
        {
            get
            {
                return this.dayQualityField;
            }
            set
            {
                this.dayQualityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public Suggestion[] SuggestionArray
        {
            get
            {
                return this.suggestionArrayField;
            }
            set
            {
                this.suggestionArrayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SuggestionsResponseType
    {

        private ResponseMessageType responseMessageField;

        private SuggestionDayResult[] suggestionDayResultArrayField;

        /// <remarks/>
        public ResponseMessageType ResponseMessage
        {
            get
            {
                return this.responseMessageField;
            }
            set
            {
                this.responseMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public SuggestionDayResult[] SuggestionDayResultArray
        {
            get
            {
                return this.suggestionDayResultArrayField;
            }
            set
            {
                this.suggestionDayResultArrayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class WorkingPeriod
    {

        private string dayOfWeekField;

        private int startTimeInMinutesField;

        private int endTimeInMinutesField;

        /// <remarks/>
        public string DayOfWeek
        {
            get
            {
                return this.dayOfWeekField;
            }
            set
            {
                this.dayOfWeekField = value;
            }
        }

        /// <remarks/>
        public int StartTimeInMinutes
        {
            get
            {
                return this.startTimeInMinutesField;
            }
            set
            {
                this.startTimeInMinutesField = value;
            }
        }

        /// <remarks/>
        public int EndTimeInMinutes
        {
            get
            {
                return this.endTimeInMinutesField;
            }
            set
            {
                this.endTimeInMinutesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class WorkingHours
    {

        private SerializableTimeZone timeZoneField;

        private WorkingPeriod[] workingPeriodArrayField;

        /// <remarks/>
        public SerializableTimeZone TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public WorkingPeriod[] WorkingPeriodArray
        {
            get
            {
                return this.workingPeriodArrayField;
            }
            set
            {
                this.workingPeriodArrayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SerializableTimeZone
    {

        private int biasField;

        private SerializableTimeZoneTime standardTimeField;

        private SerializableTimeZoneTime daylightTimeField;

        /// <remarks/>
        public int Bias
        {
            get
            {
                return this.biasField;
            }
            set
            {
                this.biasField = value;
            }
        }

        /// <remarks/>
        public SerializableTimeZoneTime StandardTime
        {
            get
            {
                return this.standardTimeField;
            }
            set
            {
                this.standardTimeField = value;
            }
        }

        /// <remarks/>
        public SerializableTimeZoneTime DaylightTime
        {
            get
            {
                return this.daylightTimeField;
            }
            set
            {
                this.daylightTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SerializableTimeZoneTime
    {

        private int biasField;

        private string timeField;

        private short dayOrderField;

        private short monthField;

        private string dayOfWeekField;

        private string yearField;

        /// <remarks/>
        public int Bias
        {
            get
            {
                return this.biasField;
            }
            set
            {
                this.biasField = value;
            }
        }

        /// <remarks/>
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public short DayOrder
        {
            get
            {
                return this.dayOrderField;
            }
            set
            {
                this.dayOrderField = value;
            }
        }

        /// <remarks/>
        public short Month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }

        /// <remarks/>
        public string DayOfWeek
        {
            get
            {
                return this.dayOfWeekField;
            }
            set
            {
                this.dayOfWeekField = value;
            }
        }

        /// <remarks/>
        public string Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarEventDetails
    {

        private string idField;

        private string subjectField;

        private string locationField;

        private bool isMeetingField;

        private bool isRecurringField;

        private bool isExceptionField;

        private bool isReminderSetField;

        private bool isPrivateField;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public bool IsMeeting
        {
            get
            {
                return this.isMeetingField;
            }
            set
            {
                this.isMeetingField = value;
            }
        }

        /// <remarks/>
        public bool IsRecurring
        {
            get
            {
                return this.isRecurringField;
            }
            set
            {
                this.isRecurringField = value;
            }
        }

        /// <remarks/>
        public bool IsException
        {
            get
            {
                return this.isExceptionField;
            }
            set
            {
                this.isExceptionField = value;
            }
        }

        /// <remarks/>
        public bool IsReminderSet
        {
            get
            {
                return this.isReminderSetField;
            }
            set
            {
                this.isReminderSetField = value;
            }
        }

        /// <remarks/>
        public bool IsPrivate
        {
            get
            {
                return this.isPrivateField;
            }
            set
            {
                this.isPrivateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarEvent
    {

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private LegacyFreeBusyType busyTypeField;

        private CalendarEventDetails calendarEventDetailsField;

        /// <remarks/>
        public System.DateTime StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        public LegacyFreeBusyType BusyType
        {
            get
            {
                return this.busyTypeField;
            }
            set
            {
                this.busyTypeField = value;
            }
        }

        /// <remarks/>
        public CalendarEventDetails CalendarEventDetails
        {
            get
            {
                return this.calendarEventDetailsField;
            }
            set
            {
                this.calendarEventDetailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FreeBusyView
    {

        private FreeBusyViewType freeBusyViewTypeField;

        private string mergedFreeBusyField;

        private CalendarEvent[] calendarEventArrayField;

        private WorkingHours workingHoursField;

        /// <remarks/>
        public FreeBusyViewType FreeBusyViewType
        {
            get
            {
                return this.freeBusyViewTypeField;
            }
            set
            {
                this.freeBusyViewTypeField = value;
            }
        }

        /// <remarks/>
        public string MergedFreeBusy
        {
            get
            {
                return this.mergedFreeBusyField;
            }
            set
            {
                this.mergedFreeBusyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public CalendarEvent[] CalendarEventArray
        {
            get
            {
                return this.calendarEventArrayField;
            }
            set
            {
                this.calendarEventArrayField = value;
            }
        }

        /// <remarks/>
        public WorkingHours WorkingHours
        {
            get
            {
                return this.workingHoursField;
            }
            set
            {
                this.workingHoursField = value;
            }
        }
    }

    /// <remarks/>
    [System.FlagsAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum FreeBusyViewType
    {

        /// <remarks/>
        None = 1,

        /// <remarks/>
        MergedOnly = 2,

        /// <remarks/>
        FreeBusy = 4,

        /// <remarks/>
        FreeBusyMerged = 8,

        /// <remarks/>
        Detailed = 16,

        /// <remarks/>
        DetailedMerged = 32,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FreeBusyResponseType
    {

        private ResponseMessageType responseMessageField;

        private FreeBusyView freeBusyViewField;

        /// <remarks/>
        public ResponseMessageType ResponseMessage
        {
            get
            {
                return this.responseMessageField;
            }
            set
            {
                this.responseMessageField = value;
            }
        }

        /// <remarks/>
        public FreeBusyView FreeBusyView
        {
            get
            {
                return this.freeBusyViewField;
            }
            set
            {
                this.freeBusyViewField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetUserAvailabilityResponseType
    {

        private FreeBusyResponseType[] freeBusyResponseArrayField;

        private SuggestionsResponseType suggestionsResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FreeBusyResponse", IsNullable = false)]
        public FreeBusyResponseType[] FreeBusyResponseArray
        {
            get
            {
                return this.freeBusyResponseArrayField;
            }
            set
            {
                this.freeBusyResponseArrayField = value;
            }
        }

        /// <remarks/>
        public SuggestionsResponseType SuggestionsResponse
        {
            get
            {
                return this.suggestionsResponseField;
            }
            set
            {
                this.suggestionsResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ArrayOfResponseMessagesType
    {

        private ResponseMessageType[] itemsField;

        private ItemsChoiceType3[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ConvertIdResponseMessage", typeof(ConvertIdResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CopyFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CopyItemResponseMessage", typeof(ItemInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CreateAttachmentResponseMessage", typeof(AttachmentInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CreateFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CreateItemResponseMessage", typeof(ItemInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("CreateManagedFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("DeleteAttachmentResponseMessage", typeof(DeleteAttachmentResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("DeleteFolderResponseMessage", typeof(ResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("DeleteItemResponseMessage", typeof(ResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("ExpandDLResponseMessage", typeof(ExpandDLResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("FindFolderResponseMessage", typeof(FindFolderResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("FindItemResponseMessage", typeof(FindItemResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("GetAttachmentResponseMessage", typeof(AttachmentInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("GetEventsResponseMessage", typeof(GetEventsResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("GetFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("GetItemResponseMessage", typeof(ItemInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MoveFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MoveItemResponseMessage", typeof(ItemInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("ResolveNamesResponseMessage", typeof(ResolveNamesResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("SendItemResponseMessage", typeof(ResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("SendNotificationResponseMessage", typeof(SendNotificationResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("SubscribeResponseMessage", typeof(SubscribeResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("SyncFolderHierarchyResponseMessage", typeof(SyncFolderHierarchyResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("SyncFolderItemsResponseMessage", typeof(SyncFolderItemsResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("UnsubscribeResponseMessage", typeof(ResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("UpdateFolderResponseMessage", typeof(FolderInfoResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("UpdateItemResponseMessage", typeof(UpdateItemResponseMessageType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public ResponseMessageType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType3[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages", IncludeInSchema = false)]
    public enum ItemsChoiceType3
    {

        /// <remarks/>
        ConvertIdResponseMessage,

        /// <remarks/>
        CopyFolderResponseMessage,

        /// <remarks/>
        CopyItemResponseMessage,

        /// <remarks/>
        CreateAttachmentResponseMessage,

        /// <remarks/>
        CreateFolderResponseMessage,

        /// <remarks/>
        CreateItemResponseMessage,

        /// <remarks/>
        CreateManagedFolderResponseMessage,

        /// <remarks/>
        DeleteAttachmentResponseMessage,

        /// <remarks/>
        DeleteFolderResponseMessage,

        /// <remarks/>
        DeleteItemResponseMessage,

        /// <remarks/>
        ExpandDLResponseMessage,

        /// <remarks/>
        FindFolderResponseMessage,

        /// <remarks/>
        FindItemResponseMessage,

        /// <remarks/>
        GetAttachmentResponseMessage,

        /// <remarks/>
        GetEventsResponseMessage,

        /// <remarks/>
        GetFolderResponseMessage,

        /// <remarks/>
        GetItemResponseMessage,

        /// <remarks/>
        MoveFolderResponseMessage,

        /// <remarks/>
        MoveItemResponseMessage,

        /// <remarks/>
        ResolveNamesResponseMessage,

        /// <remarks/>
        SendItemResponseMessage,

        /// <remarks/>
        SendNotificationResponseMessage,

        /// <remarks/>
        SubscribeResponseMessage,

        /// <remarks/>
        SyncFolderHierarchyResponseMessage,

        /// <remarks/>
        SyncFolderItemsResponseMessage,

        /// <remarks/>
        UnsubscribeResponseMessage,

        /// <remarks/>
        UpdateFolderResponseMessage,

        /// <remarks/>
        UpdateItemResponseMessage,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConvertIdResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderItemsResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderHierarchyResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SendNotificationResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetEventsResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnsubscribeResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SubscribeResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateManagedFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExpandDLResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolveNamesResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetAttachmentResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteAttachmentResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateAttachmentResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SendItemResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteFolderResponseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindFolderResponseType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class BaseResponseMessageType
    {

        private ArrayOfResponseMessagesType responseMessagesField;

        /// <remarks/>
        public ArrayOfResponseMessagesType ResponseMessages
        {
            get
            {
                return this.responseMessagesField;
            }
            set
            {
                this.responseMessagesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ConvertIdResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderItemsResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderHierarchyResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SendNotificationResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetEventsResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UnsubscribeResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SubscribeResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateManagedFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ExpandDLResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ResolveNamesResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CopyItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class MoveItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetAttachmentResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteAttachmentResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateAttachmentResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SendItemResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CopyFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class MoveFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindFolderResponseType : BaseResponseMessageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SuggestionsViewOptionsType
    {

        private int goodThresholdField;

        private bool goodThresholdFieldSpecified;

        private int maximumResultsByDayField;

        private bool maximumResultsByDayFieldSpecified;

        private int maximumNonWorkHourResultsByDayField;

        private bool maximumNonWorkHourResultsByDayFieldSpecified;

        private int meetingDurationInMinutesField;

        private bool meetingDurationInMinutesFieldSpecified;

        private SuggestionQuality minimumSuggestionQualityField;

        private bool minimumSuggestionQualityFieldSpecified;

        private Duration detailedSuggestionsWindowField;

        private System.DateTime currentMeetingTimeField;

        private bool currentMeetingTimeFieldSpecified;

        private string globalObjectIdField;

        /// <remarks/>
        public int GoodThreshold
        {
            get
            {
                return this.goodThresholdField;
            }
            set
            {
                this.goodThresholdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GoodThresholdSpecified
        {
            get
            {
                return this.goodThresholdFieldSpecified;
            }
            set
            {
                this.goodThresholdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int MaximumResultsByDay
        {
            get
            {
                return this.maximumResultsByDayField;
            }
            set
            {
                this.maximumResultsByDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumResultsByDaySpecified
        {
            get
            {
                return this.maximumResultsByDayFieldSpecified;
            }
            set
            {
                this.maximumResultsByDayFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int MaximumNonWorkHourResultsByDay
        {
            get
            {
                return this.maximumNonWorkHourResultsByDayField;
            }
            set
            {
                this.maximumNonWorkHourResultsByDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaximumNonWorkHourResultsByDaySpecified
        {
            get
            {
                return this.maximumNonWorkHourResultsByDayFieldSpecified;
            }
            set
            {
                this.maximumNonWorkHourResultsByDayFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int MeetingDurationInMinutes
        {
            get
            {
                return this.meetingDurationInMinutesField;
            }
            set
            {
                this.meetingDurationInMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MeetingDurationInMinutesSpecified
        {
            get
            {
                return this.meetingDurationInMinutesFieldSpecified;
            }
            set
            {
                this.meetingDurationInMinutesFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SuggestionQuality MinimumSuggestionQuality
        {
            get
            {
                return this.minimumSuggestionQualityField;
            }
            set
            {
                this.minimumSuggestionQualityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MinimumSuggestionQualitySpecified
        {
            get
            {
                return this.minimumSuggestionQualityFieldSpecified;
            }
            set
            {
                this.minimumSuggestionQualityFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Duration DetailedSuggestionsWindow
        {
            get
            {
                return this.detailedSuggestionsWindowField;
            }
            set
            {
                this.detailedSuggestionsWindowField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CurrentMeetingTime
        {
            get
            {
                return this.currentMeetingTimeField;
            }
            set
            {
                this.currentMeetingTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CurrentMeetingTimeSpecified
        {
            get
            {
                return this.currentMeetingTimeFieldSpecified;
            }
            set
            {
                this.currentMeetingTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string GlobalObjectId
        {
            get
            {
                return this.globalObjectIdField;
            }
            set
            {
                this.globalObjectIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FreeBusyViewOptionsType
    {

        private Duration timeWindowField;

        private int mergedFreeBusyIntervalInMinutesField;

        private bool mergedFreeBusyIntervalInMinutesFieldSpecified;

        private FreeBusyViewType requestedViewField;

        private bool requestedViewFieldSpecified;

        /// <remarks/>
        public Duration TimeWindow
        {
            get
            {
                return this.timeWindowField;
            }
            set
            {
                this.timeWindowField = value;
            }
        }

        /// <remarks/>
        public int MergedFreeBusyIntervalInMinutes
        {
            get
            {
                return this.mergedFreeBusyIntervalInMinutesField;
            }
            set
            {
                this.mergedFreeBusyIntervalInMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MergedFreeBusyIntervalInMinutesSpecified
        {
            get
            {
                return this.mergedFreeBusyIntervalInMinutesFieldSpecified;
            }
            set
            {
                this.mergedFreeBusyIntervalInMinutesFieldSpecified = value;
            }
        }

        /// <remarks/>
        public FreeBusyViewType RequestedView
        {
            get
            {
                return this.requestedViewField;
            }
            set
            {
                this.requestedViewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RequestedViewSpecified
        {
            get
            {
                return this.requestedViewFieldSpecified;
            }
            set
            {
                this.requestedViewFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class EmailAddress
    {

        private string nameField;

        private string addressField;

        private string routingTypeField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public string RoutingType
        {
            get
            {
                return this.routingTypeField;
            }
            set
            {
                this.routingTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class MailboxData
    {

        private EmailAddress emailField;

        private MeetingAttendeeType attendeeTypeField;

        private bool excludeConflictsField;

        private bool excludeConflictsFieldSpecified;

        /// <remarks/>
        public EmailAddress Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public MeetingAttendeeType AttendeeType
        {
            get
            {
                return this.attendeeTypeField;
            }
            set
            {
                this.attendeeTypeField = value;
            }
        }

        /// <remarks/>
        public bool ExcludeConflicts
        {
            get
            {
                return this.excludeConflictsField;
            }
            set
            {
                this.excludeConflictsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExcludeConflictsSpecified
        {
            get
            {
                return this.excludeConflictsFieldSpecified;
            }
            set
            {
                this.excludeConflictsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MeetingAttendeeType
    {

        /// <remarks/>
        Organizer,

        /// <remarks/>
        Required,

        /// <remarks/>
        Optional,

        /// <remarks/>
        Room,

        /// <remarks/>
        Resource,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PullSubscriptionRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PushSubscriptionRequestType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BaseSubscriptionRequestType
    {

        private BaseFolderIdType[] folderIdsField;

        private NotificationEventTypeType[] eventTypesField;

        private string watermarkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), IsNullable = false)]
        public BaseFolderIdType[] FolderIds
        {
            get
            {
                return this.folderIdsField;
            }
            set
            {
                this.folderIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("EventType", IsNullable = false)]
        public NotificationEventTypeType[] EventTypes
        {
            get
            {
                return this.eventTypesField;
            }
            set
            {
                this.eventTypesField = value;
            }
        }

        /// <remarks/>
        public string Watermark
        {
            get
            {
                return this.watermarkField;
            }
            set
            {
                this.watermarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum NotificationEventTypeType
    {

        /// <remarks/>
        CopiedEvent,

        /// <remarks/>
        CreatedEvent,

        /// <remarks/>
        DeletedEvent,

        /// <remarks/>
        ModifiedEvent,

        /// <remarks/>
        MovedEvent,

        /// <remarks/>
        NewMailEvent,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PullSubscriptionRequestType : BaseSubscriptionRequestType
    {

        private int timeoutField;

        /// <remarks/>
        public int Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class PushSubscriptionRequestType : BaseSubscriptionRequestType
    {

        private int statusFrequencyField;

        private string uRLField;

        /// <remarks/>
        public int StatusFrequency
        {
            get
            {
                return this.statusFrequencyField;
            }
            set
            {
                this.statusFrequencyField = value;
            }
        }

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AttachmentResponseShapeType
    {

        private bool includeMimeContentField;

        private bool includeMimeContentFieldSpecified;

        private BodyTypeResponseType bodyTypeField;

        private bool bodyTypeFieldSpecified;

        private BasePathToElementType[] additionalPropertiesField;

        /// <remarks/>
        public bool IncludeMimeContent
        {
            get
            {
                return this.includeMimeContentField;
            }
            set
            {
                this.includeMimeContentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeMimeContentSpecified
        {
            get
            {
                return this.includeMimeContentFieldSpecified;
            }
            set
            {
                this.includeMimeContentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public BodyTypeResponseType BodyType
        {
            get
            {
                return this.bodyTypeField;
            }
            set
            {
                this.bodyTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BodyTypeSpecified
        {
            get
            {
                return this.bodyTypeFieldSpecified;
            }
            set
            {
                this.bodyTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FieldURI", typeof(PathToUnindexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Path", IsNullable = false)]
        public BasePathToElementType[] AdditionalProperties
        {
            get
            {
                return this.additionalPropertiesField;
            }
            set
            {
                this.additionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum BodyTypeResponseType
    {

        /// <remarks/>
        Best,

        /// <remarks/>
        HTML,

        /// <remarks/>
        Text,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FieldOrderType
    {

        private BasePathToElementType itemField;

        private SortDirectionType orderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SortDirectionType Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum SortDirectionType
    {

        /// <remarks/>
        Ascending,

        /// <remarks/>
        Descending,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AggregateOnType
    {

        private BasePathToElementType itemField;

        private AggregateType aggregateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AggregateType Aggregate
        {
            get
            {
                return this.aggregateField;
            }
            set
            {
                this.aggregateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum AggregateType
    {

        /// <remarks/>
        Minimum,

        /// <remarks/>
        Maximum,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DistinguishedGroupByType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupByType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BaseGroupByType
    {

        private SortDirectionType orderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SortDirectionType Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DistinguishedGroupByType : BaseGroupByType
    {

        private StandardGroupByType standardGroupByField;

        /// <remarks/>
        public StandardGroupByType StandardGroupBy
        {
            get
            {
                return this.standardGroupByField;
            }
            set
            {
                this.standardGroupByField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum StandardGroupByType
    {

        /// <remarks/>
        ConversationTopic,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class GroupByType : BaseGroupByType
    {

        private BasePathToElementType itemField;

        private AggregateOnType aggregateOnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public AggregateOnType AggregateOn
        {
            get
            {
                return this.aggregateOnField;
            }
            set
            {
                this.aggregateOnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemChangeType
    {

        private BaseItemIdType itemField;

        private ItemChangeDescriptionType[] updatesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemId", typeof(ItemIdType))]
        [System.Xml.Serialization.XmlElementAttribute("OccurrenceItemId", typeof(OccurrenceItemIdType))]
        [System.Xml.Serialization.XmlElementAttribute("RecurringMasterItemId", typeof(RecurringMasterItemIdType))]
        public BaseItemIdType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AppendToItemField", typeof(AppendToItemFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DeleteItemField", typeof(DeleteItemFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SetItemField", typeof(SetItemFieldType), IsNullable = false)]
        public ItemChangeDescriptionType[] Updates
        {
            get
            {
                return this.updatesField;
            }
            set
            {
                this.updatesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AppendToItemFieldType : ItemChangeDescriptionType
    {

        private ItemType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DistributionList", typeof(DistributionListType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        public ItemType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppendToItemFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteItemFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SetItemFieldType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemChangeDescriptionType : ChangeDescriptionType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FolderChangeDescriptionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppendToFolderFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteFolderFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SetFolderFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemChangeDescriptionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppendToItemFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteItemFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SetItemFieldType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class ChangeDescriptionType
    {

        private BasePathToElementType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("FieldURI", typeof(PathToUnindexedFieldType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType))]
        public BasePathToElementType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppendToFolderFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteFolderFieldType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SetFolderFieldType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FolderChangeDescriptionType : ChangeDescriptionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class AppendToFolderFieldType : FolderChangeDescriptionType
    {

        private BaseFolderType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarFolder", typeof(CalendarFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("ContactsFolder", typeof(ContactsFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("Folder", typeof(FolderType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchFolder", typeof(SearchFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("TasksFolder", typeof(TasksFolderType))]
        public BaseFolderType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DeleteFolderFieldType : FolderChangeDescriptionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SetFolderFieldType : FolderChangeDescriptionType
    {

        private BaseFolderType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarFolder", typeof(CalendarFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("ContactsFolder", typeof(ContactsFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("Folder", typeof(FolderType))]
        [System.Xml.Serialization.XmlElementAttribute("SearchFolder", typeof(SearchFolderType))]
        [System.Xml.Serialization.XmlElementAttribute("TasksFolder", typeof(TasksFolderType))]
        public BaseFolderType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class DeleteItemFieldType : ItemChangeDescriptionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SetItemFieldType : ItemChangeDescriptionType
    {

        private ItemType item1Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarItem", typeof(CalendarItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Contact", typeof(ContactItemType))]
        [System.Xml.Serialization.XmlElementAttribute("DistributionList", typeof(DistributionListType))]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingCancellation", typeof(MeetingCancellationMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingMessage", typeof(MeetingMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingRequest", typeof(MeetingRequestMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("MeetingResponse", typeof(MeetingResponseMessageType))]
        [System.Xml.Serialization.XmlElementAttribute("Message", typeof(MessageType))]
        [System.Xml.Serialization.XmlElementAttribute("PostItem", typeof(PostItemType))]
        [System.Xml.Serialization.XmlElementAttribute("Task", typeof(TaskType))]
        public ItemType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ItemResponseShapeType
    {

        private DefaultShapeNamesType baseShapeField;

        private bool includeMimeContentField;

        private bool includeMimeContentFieldSpecified;

        private BodyTypeResponseType bodyTypeField;

        private bool bodyTypeFieldSpecified;

        private BasePathToElementType[] additionalPropertiesField;

        /// <remarks/>
        public DefaultShapeNamesType BaseShape
        {
            get
            {
                return this.baseShapeField;
            }
            set
            {
                this.baseShapeField = value;
            }
        }

        /// <remarks/>
        public bool IncludeMimeContent
        {
            get
            {
                return this.includeMimeContentField;
            }
            set
            {
                this.includeMimeContentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IncludeMimeContentSpecified
        {
            get
            {
                return this.includeMimeContentFieldSpecified;
            }
            set
            {
                this.includeMimeContentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public BodyTypeResponseType BodyType
        {
            get
            {
                return this.bodyTypeField;
            }
            set
            {
                this.bodyTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BodyTypeSpecified
        {
            get
            {
                return this.bodyTypeFieldSpecified;
            }
            set
            {
                this.bodyTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FieldURI", typeof(PathToUnindexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Path", IsNullable = false)]
        public BasePathToElementType[] AdditionalProperties
        {
            get
            {
                return this.additionalPropertiesField;
            }
            set
            {
                this.additionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DefaultShapeNamesType
    {

        /// <remarks/>
        IdOnly,

        /// <remarks/>
        Default,

        /// <remarks/>
        AllProperties,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FolderChangeType
    {

        private BaseFolderIdType itemField;

        private FolderChangeDescriptionType[] updatesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType))]
        [System.Xml.Serialization.XmlElementAttribute("FolderId", typeof(FolderIdType))]
        public BaseFolderIdType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AppendToFolderField", typeof(AppendToFolderFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DeleteFolderField", typeof(DeleteFolderFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SetFolderField", typeof(SetFolderFieldType), IsNullable = false)]
        public FolderChangeDescriptionType[] Updates
        {
            get
            {
                return this.updatesField;
            }
            set
            {
                this.updatesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactsViewType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalendarViewType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FractionalPageViewType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IndexedPageViewType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public abstract partial class BasePagingType
    {

        private int maxEntriesReturnedField;

        private bool maxEntriesReturnedFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int MaxEntriesReturned
        {
            get
            {
                return this.maxEntriesReturnedField;
            }
            set
            {
                this.maxEntriesReturnedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaxEntriesReturnedSpecified
        {
            get
            {
                return this.maxEntriesReturnedFieldSpecified;
            }
            set
            {
                this.maxEntriesReturnedFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ContactsViewType : BasePagingType
    {

        private string initialNameField;

        private string finalNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string InitialName
        {
            get
            {
                return this.initialNameField;
            }
            set
            {
                this.initialNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FinalName
        {
            get
            {
                return this.finalNameField;
            }
            set
            {
                this.finalNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class CalendarViewType : BasePagingType
    {

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FractionalPageViewType : BasePagingType
    {

        private int numeratorField;

        private int denominatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Numerator
        {
            get
            {
                return this.numeratorField;
            }
            set
            {
                this.numeratorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Denominator
        {
            get
            {
                return this.denominatorField;
            }
            set
            {
                this.denominatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class IndexedPageViewType : BasePagingType
    {

        private int offsetField;

        private IndexBasePointType basePointField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public IndexBasePointType BasePoint
        {
            get
            {
                return this.basePointField;
            }
            set
            {
                this.basePointField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum IndexBasePointType
    {

        /// <remarks/>
        Beginning,

        /// <remarks/>
        End,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class TargetFolderIdType
    {

        private BaseFolderIdType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType))]
        [System.Xml.Serialization.XmlElementAttribute("FolderId", typeof(FolderIdType))]
        public BaseFolderIdType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class FolderResponseShapeType
    {

        private DefaultShapeNamesType baseShapeField;

        private BasePathToElementType[] additionalPropertiesField;

        /// <remarks/>
        public DefaultShapeNamesType BaseShape
        {
            get
            {
                return this.baseShapeField;
            }
            set
            {
                this.baseShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ExtendedFieldURI", typeof(PathToExtendedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FieldURI", typeof(PathToUnindexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("IndexedFieldURI", typeof(PathToIndexedFieldType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Path", IsNullable = false)]
        public BasePathToElementType[] AdditionalProperties
        {
            get
            {
                return this.additionalPropertiesField;
            }
            set
            {
                this.additionalPropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConvertIdType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SetUserOofSettingsRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetUserOofSettingsRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetUserAvailabilityRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderItemsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SyncFolderHierarchyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetEventsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnsubscribeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SubscribeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateManagedFolderRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExpandDLType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResolveNamesType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetAttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteAttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateAttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SendItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseMoveCopyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseMoveCopyFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DeleteFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FindFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CreateFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetFolderType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public abstract partial class BaseRequestType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RemoveDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddDelegateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GetDelegateType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public abstract partial class BaseDelegateType : BaseRequestType
    {

        private EmailAddressType mailboxField;

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateDelegateType : BaseDelegateType
    {

        private DelegateUserType[] delegateUsersField;

        private DeliverMeetingRequestsType deliverMeetingRequestsField;

        private bool deliverMeetingRequestsFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DelegateUser", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public DelegateUserType[] DelegateUsers
        {
            get
            {
                return this.delegateUsersField;
            }
            set
            {
                this.delegateUsersField = value;
            }
        }

        /// <remarks/>
        public DeliverMeetingRequestsType DeliverMeetingRequests
        {
            get
            {
                return this.deliverMeetingRequestsField;
            }
            set
            {
                this.deliverMeetingRequestsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeliverMeetingRequestsSpecified
        {
            get
            {
                return this.deliverMeetingRequestsFieldSpecified;
            }
            set
            {
                this.deliverMeetingRequestsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class RemoveDelegateType : BaseDelegateType
    {

        private UserIdType[] userIdsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UserId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public UserIdType[] UserIds
        {
            get
            {
                return this.userIdsField;
            }
            set
            {
                this.userIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class AddDelegateType : BaseDelegateType
    {

        private DelegateUserType[] delegateUsersField;

        private DeliverMeetingRequestsType deliverMeetingRequestsField;

        private bool deliverMeetingRequestsFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DelegateUser", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public DelegateUserType[] DelegateUsers
        {
            get
            {
                return this.delegateUsersField;
            }
            set
            {
                this.delegateUsersField = value;
            }
        }

        /// <remarks/>
        public DeliverMeetingRequestsType DeliverMeetingRequests
        {
            get
            {
                return this.deliverMeetingRequestsField;
            }
            set
            {
                this.deliverMeetingRequestsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeliverMeetingRequestsSpecified
        {
            get
            {
                return this.deliverMeetingRequestsFieldSpecified;
            }
            set
            {
                this.deliverMeetingRequestsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetDelegateType : BaseDelegateType
    {

        private UserIdType[] userIdsField;

        private bool includePermissionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("UserId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public UserIdType[] UserIds
        {
            get
            {
                return this.userIdsField;
            }
            set
            {
                this.userIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IncludePermissions
        {
            get
            {
                return this.includePermissionsField;
            }
            set
            {
                this.includePermissionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ConvertIdType : BaseRequestType
    {

        private AlternateIdBaseType[] sourceIdsField;

        private IdFormatType destinationFormatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AlternateId", typeof(AlternateIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("AlternatePublicFolderId", typeof(AlternatePublicFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("AlternatePublicFolderItemId", typeof(AlternatePublicFolderItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public AlternateIdBaseType[] SourceIds
        {
            get
            {
                return this.sourceIdsField;
            }
            set
            {
                this.sourceIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public IdFormatType DestinationFormat
        {
            get
            {
                return this.destinationFormatField;
            }
            set
            {
                this.destinationFormatField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SetUserOofSettingsRequest : BaseRequestType
    {

        private EmailAddress mailboxField;

        private UserOofSettings userOofSettingsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public EmailAddress Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public UserOofSettings UserOofSettings
        {
            get
            {
                return this.userOofSettingsField;
            }
            set
            {
                this.userOofSettingsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetUserOofSettingsRequest : BaseRequestType
    {

        private EmailAddress mailboxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public EmailAddress Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetUserAvailabilityRequestType : BaseRequestType
    {

        private SerializableTimeZone timeZoneField;

        private MailboxData[] mailboxDataArrayField;

        private FreeBusyViewOptionsType freeBusyViewOptionsField;

        private SuggestionsViewOptionsType suggestionsViewOptionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public SerializableTimeZone TimeZone
        {
            get
            {
                return this.timeZoneField;
            }
            set
            {
                this.timeZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public MailboxData[] MailboxDataArray
        {
            get
            {
                return this.mailboxDataArrayField;
            }
            set
            {
                this.mailboxDataArrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public FreeBusyViewOptionsType FreeBusyViewOptions
        {
            get
            {
                return this.freeBusyViewOptionsField;
            }
            set
            {
                this.freeBusyViewOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
        public SuggestionsViewOptionsType SuggestionsViewOptions
        {
            get
            {
                return this.suggestionsViewOptionsField;
            }
            set
            {
                this.suggestionsViewOptionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderItemsType : BaseRequestType
    {

        private ItemResponseShapeType itemShapeField;

        private TargetFolderIdType syncFolderIdField;

        private string syncStateField;

        private ItemIdType[] ignoreField;

        private int maxChangesReturnedField;

        /// <remarks/>
        public ItemResponseShapeType ItemShape
        {
            get
            {
                return this.itemShapeField;
            }
            set
            {
                this.itemShapeField = value;
            }
        }

        /// <remarks/>
        public TargetFolderIdType SyncFolderId
        {
            get
            {
                return this.syncFolderIdField;
            }
            set
            {
                this.syncFolderIdField = value;
            }
        }

        /// <remarks/>
        public string SyncState
        {
            get
            {
                return this.syncStateField;
            }
            set
            {
                this.syncStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public ItemIdType[] Ignore
        {
            get
            {
                return this.ignoreField;
            }
            set
            {
                this.ignoreField = value;
            }
        }

        /// <remarks/>
        public int MaxChangesReturned
        {
            get
            {
                return this.maxChangesReturnedField;
            }
            set
            {
                this.maxChangesReturnedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SyncFolderHierarchyType : BaseRequestType
    {

        private FolderResponseShapeType folderShapeField;

        private TargetFolderIdType syncFolderIdField;

        private string syncStateField;

        /// <remarks/>
        public FolderResponseShapeType FolderShape
        {
            get
            {
                return this.folderShapeField;
            }
            set
            {
                this.folderShapeField = value;
            }
        }

        /// <remarks/>
        public TargetFolderIdType SyncFolderId
        {
            get
            {
                return this.syncFolderIdField;
            }
            set
            {
                this.syncFolderIdField = value;
            }
        }

        /// <remarks/>
        public string SyncState
        {
            get
            {
                return this.syncStateField;
            }
            set
            {
                this.syncStateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetEventsType : BaseRequestType
    {

        private string subscriptionIdField;

        private string watermarkField;

        /// <remarks/>
        public string SubscriptionId
        {
            get
            {
                return this.subscriptionIdField;
            }
            set
            {
                this.subscriptionIdField = value;
            }
        }

        /// <remarks/>
        public string Watermark
        {
            get
            {
                return this.watermarkField;
            }
            set
            {
                this.watermarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UnsubscribeType : BaseRequestType
    {

        private string subscriptionIdField;

        /// <remarks/>
        public string SubscriptionId
        {
            get
            {
                return this.subscriptionIdField;
            }
            set
            {
                this.subscriptionIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SubscribeType : BaseRequestType
    {

        private BaseSubscriptionRequestType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PullSubscriptionRequest", typeof(PullSubscriptionRequestType))]
        [System.Xml.Serialization.XmlElementAttribute("PushSubscriptionRequest", typeof(PushSubscriptionRequestType))]
        public BaseSubscriptionRequestType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateManagedFolderRequestType : BaseRequestType
    {

        private string[] folderNamesField;

        private EmailAddressType mailboxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderName", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public string[] FolderNames
        {
            get
            {
                return this.folderNamesField;
            }
            set
            {
                this.folderNamesField = value;
            }
        }

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ExpandDLType : BaseRequestType
    {

        private EmailAddressType mailboxField;

        /// <remarks/>
        public EmailAddressType Mailbox
        {
            get
            {
                return this.mailboxField;
            }
            set
            {
                this.mailboxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class ResolveNamesType : BaseRequestType
    {

        private BaseFolderIdType[] parentFolderIdsField;

        private string unresolvedEntryField;

        private bool returnFullContactDataField;

        private ResolveNamesSearchScopeType searchScopeField;

        public ResolveNamesType()
        {
            this.searchScopeField = ResolveNamesSearchScopeType.ActiveDirectoryContacts;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] ParentFolderIds
        {
            get
            {
                return this.parentFolderIdsField;
            }
            set
            {
                this.parentFolderIdsField = value;
            }
        }

        /// <remarks/>
        public string UnresolvedEntry
        {
            get
            {
                return this.unresolvedEntryField;
            }
            set
            {
                this.unresolvedEntryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ReturnFullContactData
        {
            get
            {
                return this.returnFullContactDataField;
            }
            set
            {
                this.returnFullContactDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ResolveNamesSearchScopeType.ActiveDirectoryContacts)]
        public ResolveNamesSearchScopeType SearchScope
        {
            get
            {
                return this.searchScopeField;
            }
            set
            {
                this.searchScopeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ResolveNamesSearchScopeType
    {

        /// <remarks/>
        ActiveDirectory,

        /// <remarks/>
        ActiveDirectoryContacts,

        /// <remarks/>
        Contacts,

        /// <remarks/>
        ContactsActiveDirectory,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetAttachmentType : BaseRequestType
    {

        private AttachmentResponseShapeType attachmentShapeField;

        private RequestAttachmentIdType[] attachmentIdsField;

        /// <remarks/>
        public AttachmentResponseShapeType AttachmentShape
        {
            get
            {
                return this.attachmentShapeField;
            }
            set
            {
                this.attachmentShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AttachmentId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public RequestAttachmentIdType[] AttachmentIds
        {
            get
            {
                return this.attachmentIdsField;
            }
            set
            {
                this.attachmentIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteAttachmentType : BaseRequestType
    {

        private RequestAttachmentIdType[] attachmentIdsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("AttachmentId", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public RequestAttachmentIdType[] AttachmentIds
        {
            get
            {
                return this.attachmentIdsField;
            }
            set
            {
                this.attachmentIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateAttachmentType : BaseRequestType
    {

        private ItemIdType parentItemIdField;

        private AttachmentType[] attachmentsField;

        /// <remarks/>
        public ItemIdType ParentItemId
        {
            get
            {
                return this.parentItemIdField;
            }
            set
            {
                this.parentItemIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FileAttachment", typeof(FileAttachmentType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemAttachment", typeof(ItemAttachmentType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public AttachmentType[] Attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindItemType : BaseRequestType
    {

        private ItemResponseShapeType itemShapeField;

        private BasePagingType itemField;

        private BaseGroupByType item1Field;

        private RestrictionType restrictionField;

        private FieldOrderType[] sortOrderField;

        private BaseFolderIdType[] parentFolderIdsField;

        private ItemQueryTraversalType traversalField;

        /// <remarks/>
        public ItemResponseShapeType ItemShape
        {
            get
            {
                return this.itemShapeField;
            }
            set
            {
                this.itemShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CalendarView", typeof(CalendarViewType))]
        [System.Xml.Serialization.XmlElementAttribute("ContactsView", typeof(ContactsViewType))]
        [System.Xml.Serialization.XmlElementAttribute("FractionalPageItemView", typeof(FractionalPageViewType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedPageItemView", typeof(IndexedPageViewType))]
        public BasePagingType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DistinguishedGroupBy", typeof(DistinguishedGroupByType))]
        [System.Xml.Serialization.XmlElementAttribute("GroupBy", typeof(GroupByType))]
        public BaseGroupByType Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }

        /// <remarks/>
        public RestrictionType Restriction
        {
            get
            {
                return this.restrictionField;
            }
            set
            {
                this.restrictionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FieldOrder", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public FieldOrderType[] SortOrder
        {
            get
            {
                return this.sortOrderField;
            }
            set
            {
                this.sortOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] ParentFolderIds
        {
            get
            {
                return this.parentFolderIdsField;
            }
            set
            {
                this.parentFolderIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ItemQueryTraversalType Traversal
        {
            get
            {
                return this.traversalField;
            }
            set
            {
                this.traversalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ItemQueryTraversalType
    {

        /// <remarks/>
        Shallow,

        /// <remarks/>
        SoftDeleted,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class SendItemType : BaseRequestType
    {

        private BaseItemIdType[] itemIdsField;

        private TargetFolderIdType savedItemFolderIdField;

        private bool saveItemToFolderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemId", typeof(ItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OccurrenceItemId", typeof(OccurrenceItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RecurringMasterItemId", typeof(RecurringMasterItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseItemIdType[] ItemIds
        {
            get
            {
                return this.itemIdsField;
            }
            set
            {
                this.itemIdsField = value;
            }
        }

        /// <remarks/>
        public TargetFolderIdType SavedItemFolderId
        {
            get
            {
                return this.savedItemFolderIdField;
            }
            set
            {
                this.savedItemFolderIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool SaveItemToFolder
        {
            get
            {
                return this.saveItemToFolderField;
            }
            set
            {
                this.saveItemToFolderField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveItemType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class BaseMoveCopyItemType : BaseRequestType
    {

        private TargetFolderIdType toFolderIdField;

        private BaseItemIdType[] itemIdsField;

        /// <remarks/>
        public TargetFolderIdType ToFolderId
        {
            get
            {
                return this.toFolderIdField;
            }
            set
            {
                this.toFolderIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemId", typeof(ItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OccurrenceItemId", typeof(OccurrenceItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RecurringMasterItemId", typeof(RecurringMasterItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseItemIdType[] ItemIds
        {
            get
            {
                return this.itemIdsField;
            }
            set
            {
                this.itemIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CopyItemType : BaseMoveCopyItemType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class MoveItemType : BaseMoveCopyItemType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteItemType : BaseRequestType
    {

        private BaseItemIdType[] itemIdsField;

        private DisposalType deleteTypeField;

        private CalendarItemCreateOrDeleteOperationType sendMeetingCancellationsField;

        private bool sendMeetingCancellationsFieldSpecified;

        private AffectedTaskOccurrencesType affectedTaskOccurrencesField;

        private bool affectedTaskOccurrencesFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemId", typeof(ItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OccurrenceItemId", typeof(OccurrenceItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RecurringMasterItemId", typeof(RecurringMasterItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseItemIdType[] ItemIds
        {
            get
            {
                return this.itemIdsField;
            }
            set
            {
                this.itemIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DisposalType DeleteType
        {
            get
            {
                return this.deleteTypeField;
            }
            set
            {
                this.deleteTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CalendarItemCreateOrDeleteOperationType SendMeetingCancellations
        {
            get
            {
                return this.sendMeetingCancellationsField;
            }
            set
            {
                this.sendMeetingCancellationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SendMeetingCancellationsSpecified
        {
            get
            {
                return this.sendMeetingCancellationsFieldSpecified;
            }
            set
            {
                this.sendMeetingCancellationsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AffectedTaskOccurrencesType AffectedTaskOccurrences
        {
            get
            {
                return this.affectedTaskOccurrencesField;
            }
            set
            {
                this.affectedTaskOccurrencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AffectedTaskOccurrencesSpecified
        {
            get
            {
                return this.affectedTaskOccurrencesFieldSpecified;
            }
            set
            {
                this.affectedTaskOccurrencesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum DisposalType
    {

        /// <remarks/>
        HardDelete,

        /// <remarks/>
        SoftDelete,

        /// <remarks/>
        MoveToDeletedItems,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum CalendarItemCreateOrDeleteOperationType
    {

        /// <remarks/>
        SendToNone,

        /// <remarks/>
        SendOnlyToAll,

        /// <remarks/>
        SendToAllAndSaveCopy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum AffectedTaskOccurrencesType
    {

        /// <remarks/>
        AllOccurrences,

        /// <remarks/>
        SpecifiedOccurrenceOnly,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateItemType : BaseRequestType
    {

        private TargetFolderIdType savedItemFolderIdField;

        private ItemChangeType[] itemChangesField;

        private ConflictResolutionType conflictResolutionField;

        private MessageDispositionType messageDispositionField;

        private bool messageDispositionFieldSpecified;

        private CalendarItemUpdateOperationType sendMeetingInvitationsOrCancellationsField;

        private bool sendMeetingInvitationsOrCancellationsFieldSpecified;

        /// <remarks/>
        public TargetFolderIdType SavedItemFolderId
        {
            get
            {
                return this.savedItemFolderIdField;
            }
            set
            {
                this.savedItemFolderIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemChange", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public ItemChangeType[] ItemChanges
        {
            get
            {
                return this.itemChangesField;
            }
            set
            {
                this.itemChangesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ConflictResolutionType ConflictResolution
        {
            get
            {
                return this.conflictResolutionField;
            }
            set
            {
                this.conflictResolutionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public MessageDispositionType MessageDisposition
        {
            get
            {
                return this.messageDispositionField;
            }
            set
            {
                this.messageDispositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MessageDispositionSpecified
        {
            get
            {
                return this.messageDispositionFieldSpecified;
            }
            set
            {
                this.messageDispositionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CalendarItemUpdateOperationType SendMeetingInvitationsOrCancellations
        {
            get
            {
                return this.sendMeetingInvitationsOrCancellationsField;
            }
            set
            {
                this.sendMeetingInvitationsOrCancellationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SendMeetingInvitationsOrCancellationsSpecified
        {
            get
            {
                return this.sendMeetingInvitationsOrCancellationsFieldSpecified;
            }
            set
            {
                this.sendMeetingInvitationsOrCancellationsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ConflictResolutionType
    {

        /// <remarks/>
        NeverOverwrite,

        /// <remarks/>
        AutoResolve,

        /// <remarks/>
        AlwaysOverwrite,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum MessageDispositionType
    {

        /// <remarks/>
        SaveOnly,

        /// <remarks/>
        SendOnly,

        /// <remarks/>
        SendAndSaveCopy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum CalendarItemUpdateOperationType
    {

        /// <remarks/>
        SendToNone,

        /// <remarks/>
        SendOnlyToAll,

        /// <remarks/>
        SendOnlyToChanged,

        /// <remarks/>
        SendToAllAndSaveCopy,

        /// <remarks/>
        SendToChangedAndSaveCopy,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateItemType : BaseRequestType
    {

        private TargetFolderIdType savedItemFolderIdField;

        private NonEmptyArrayOfAllItemsType itemsField;

        private MessageDispositionType messageDispositionField;

        private bool messageDispositionFieldSpecified;

        private CalendarItemCreateOrDeleteOperationType sendMeetingInvitationsField;

        private bool sendMeetingInvitationsFieldSpecified;

        /// <remarks/>
        public TargetFolderIdType SavedItemFolderId
        {
            get
            {
                return this.savedItemFolderIdField;
            }
            set
            {
                this.savedItemFolderIdField = value;
            }
        }

        /// <remarks/>
        public NonEmptyArrayOfAllItemsType Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public MessageDispositionType MessageDisposition
        {
            get
            {
                return this.messageDispositionField;
            }
            set
            {
                this.messageDispositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MessageDispositionSpecified
        {
            get
            {
                return this.messageDispositionFieldSpecified;
            }
            set
            {
                this.messageDispositionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CalendarItemCreateOrDeleteOperationType SendMeetingInvitations
        {
            get
            {
                return this.sendMeetingInvitationsField;
            }
            set
            {
                this.sendMeetingInvitationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SendMeetingInvitationsSpecified
        {
            get
            {
                return this.sendMeetingInvitationsFieldSpecified;
            }
            set
            {
                this.sendMeetingInvitationsFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetItemType : BaseRequestType
    {

        private ItemResponseShapeType itemShapeField;

        private BaseItemIdType[] itemIdsField;

        /// <remarks/>
        public ItemResponseShapeType ItemShape
        {
            get
            {
                return this.itemShapeField;
            }
            set
            {
                this.itemShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemId", typeof(ItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OccurrenceItemId", typeof(OccurrenceItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RecurringMasterItemId", typeof(RecurringMasterItemIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseItemIdType[] ItemIds
        {
            get
            {
                return this.itemIdsField;
            }
            set
            {
                this.itemIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class UpdateFolderType : BaseRequestType
    {

        private FolderChangeType[] folderChangesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderChange", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public FolderChangeType[] FolderChanges
        {
            get
            {
                return this.folderChangesField;
            }
            set
            {
                this.folderChangesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CopyFolderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MoveFolderType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class BaseMoveCopyFolderType : BaseRequestType
    {

        private TargetFolderIdType toFolderIdField;

        private BaseFolderIdType[] folderIdsField;

        /// <remarks/>
        public TargetFolderIdType ToFolderId
        {
            get
            {
                return this.toFolderIdField;
            }
            set
            {
                this.toFolderIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] FolderIds
        {
            get
            {
                return this.folderIdsField;
            }
            set
            {
                this.folderIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CopyFolderType : BaseMoveCopyFolderType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class MoveFolderType : BaseMoveCopyFolderType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class DeleteFolderType : BaseRequestType
    {

        private BaseFolderIdType[] folderIdsField;

        private DisposalType deleteTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] FolderIds
        {
            get
            {
                return this.folderIdsField;
            }
            set
            {
                this.folderIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DisposalType DeleteType
        {
            get
            {
                return this.deleteTypeField;
            }
            set
            {
                this.deleteTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class FindFolderType : BaseRequestType
    {

        private FolderResponseShapeType folderShapeField;

        private BasePagingType itemField;

        private RestrictionType restrictionField;

        private BaseFolderIdType[] parentFolderIdsField;

        private FolderQueryTraversalType traversalField;

        /// <remarks/>
        public FolderResponseShapeType FolderShape
        {
            get
            {
                return this.folderShapeField;
            }
            set
            {
                this.folderShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FractionalPageFolderView", typeof(FractionalPageViewType))]
        [System.Xml.Serialization.XmlElementAttribute("IndexedPageFolderView", typeof(IndexedPageViewType))]
        public BasePagingType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        public RestrictionType Restriction
        {
            get
            {
                return this.restrictionField;
            }
            set
            {
                this.restrictionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] ParentFolderIds
        {
            get
            {
                return this.parentFolderIdsField;
            }
            set
            {
                this.parentFolderIdsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public FolderQueryTraversalType Traversal
        {
            get
            {
                return this.traversalField;
            }
            set
            {
                this.traversalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum FolderQueryTraversalType
    {

        /// <remarks/>
        Shallow,

        /// <remarks/>
        Deep,

        /// <remarks/>
        SoftDeleted,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class CreateFolderType : BaseRequestType
    {

        private TargetFolderIdType parentFolderIdField;

        private BaseFolderType[] foldersField;

        /// <remarks/>
        public TargetFolderIdType ParentFolderId
        {
            get
            {
                return this.parentFolderIdField;
            }
            set
            {
                this.parentFolderIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CalendarFolder", typeof(CalendarFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ContactsFolder", typeof(ContactsFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Folder", typeof(FolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SearchFolder", typeof(SearchFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("TasksFolder", typeof(TasksFolderType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderType[] Folders
        {
            get
            {
                return this.foldersField;
            }
            set
            {
                this.foldersField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/messages")]
    public partial class GetFolderType : BaseRequestType
    {

        private FolderResponseShapeType folderShapeField;

        private BaseFolderIdType[] folderIdsField;

        /// <remarks/>
        public FolderResponseShapeType FolderShape
        {
            get
            {
                return this.folderShapeField;
            }
            set
            {
                this.folderShapeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DistinguishedFolderId", typeof(DistinguishedFolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("FolderId", typeof(FolderIdType), Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
        public BaseFolderIdType[] FolderIds
        {
            get
            {
                return this.folderIdsField;
            }
            set
            {
                this.folderIdsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class SidAndAttributesType
    {

        private string securityIdentifierField;

        private uint attributesField;

        /// <remarks/>
        public string SecurityIdentifier
        {
            get
            {
                return this.securityIdentifierField;
            }
            set
            {
                this.securityIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint Attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public partial class ConnectingSIDType
    {

        private string principalNameField;

        private string sIDField;

        private string primarySmtpAddressField;

        /// <remarks/>
        public string PrincipalName
        {
            get
            {
                return this.principalNameField;
            }
            set
            {
                this.principalNameField = value;
            }
        }

        /// <remarks/>
        public string SID
        {
            get
            {
                return this.sIDField;
            }
            set
            {
                this.sIDField = value;
            }
        }

        /// <remarks/>
        public string PrimarySmtpAddress
        {
            get
            {
                return this.primarySmtpAddressField;
            }
            set
            {
                this.primarySmtpAddressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("MailboxCulture", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class language : System.Web.Services.Protocols.SoapHeader
    {

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    [System.Xml.Serialization.XmlRootAttribute("SerializedSecurityContext", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class SerializedSecurityContextType : System.Web.Services.Protocols.SoapHeader
    {

        private string userSidField;

        private SidAndAttributesType[] groupSidsField;

        private SidAndAttributesType[] restrictedGroupSidsField;

        private string primarySmtpAddressField;

        /// <remarks/>
        public string UserSid
        {
            get
            {
                return this.userSidField;
            }
            set
            {
                this.userSidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("GroupIdentifier", IsNullable = false)]
        public SidAndAttributesType[] GroupSids
        {
            get
            {
                return this.groupSidsField;
            }
            set
            {
                this.groupSidsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("RestrictedGroupIdentifier", IsNullable = false)]
        public SidAndAttributesType[] RestrictedGroupSids
        {
            get
            {
                return this.restrictedGroupSidsField;
            }
            set
            {
                this.restrictedGroupSidsField = value;
            }
        }

        /// <remarks/>
        public string PrimarySmtpAddress
        {
            get
            {
                return this.primarySmtpAddressField;
            }
            set
            {
                this.primarySmtpAddressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    [System.Xml.Serialization.XmlRootAttribute("ProxyRequestTypeHeader", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class AvailabilityProxyRequestType : System.Web.Services.Protocols.SoapHeader
    {

        private string[] textField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    [System.Xml.Serialization.XmlRootAttribute("ExchangeImpersonation", Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class ExchangeImpersonationType : System.Web.Services.Protocols.SoapHeader
    {

        private ConnectingSIDType connectingSIDField;

        /// <remarks/>
        public ConnectingSIDType ConnectingSID
        {
            get
            {
                return this.connectingSIDField;
            }
            set
            {
                this.connectingSIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types", IsNullable = false)]
    public partial class RequestServerVersion : System.Web.Services.Protocols.SoapHeader
    {

        private ExchangeVersionType versionField;

        private System.Xml.XmlAttribute[] anyAttrField;

        public RequestServerVersion()
        {
            this.versionField = ExchangeVersionType.Exchange2007_SP1;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ExchangeVersionType Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/exchange/services/2006/types")]
    public enum ExchangeVersionType
    {

        /// <remarks/>
        Exchange2007,

        /// <remarks/>
        Exchange2007_SP1,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void ResolveNamesCompletedEventHandler(object sender, ResolveNamesCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ResolveNamesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ResolveNamesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ResolveNamesResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ResolveNamesResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void ExpandDLCompletedEventHandler(object sender, ExpandDLCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExpandDLCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ExpandDLCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ExpandDLResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ExpandDLResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void FindFolderCompletedEventHandler(object sender, FindFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FindFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal FindFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public FindFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((FindFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void FindItemCompletedEventHandler(object sender, FindItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FindItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal FindItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public FindItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((FindItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetFolderCompletedEventHandler(object sender, GetFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void ConvertIdCompletedEventHandler(object sender, ConvertIdCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConvertIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConvertIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public ConvertIdResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((ConvertIdResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CreateFolderCompletedEventHandler(object sender, CreateFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CreateFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CreateFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void DeleteFolderCompletedEventHandler(object sender, DeleteFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void UpdateFolderCompletedEventHandler(object sender, UpdateFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UpdateFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public UpdateFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((UpdateFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void MoveFolderCompletedEventHandler(object sender, MoveFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MoveFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal MoveFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public MoveFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((MoveFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CopyFolderCompletedEventHandler(object sender, CopyFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CopyFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CopyFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CopyFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CopyFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SubscribeCompletedEventHandler(object sender, SubscribeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SubscribeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SubscribeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SubscribeResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SubscribeResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void UnsubscribeCompletedEventHandler(object sender, UnsubscribeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UnsubscribeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UnsubscribeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public UnsubscribeResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((UnsubscribeResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetEventsCompletedEventHandler(object sender, GetEventsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetEventsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetEventsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetEventsResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetEventsResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SyncFolderHierarchyCompletedEventHandler(object sender, SyncFolderHierarchyCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SyncFolderHierarchyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SyncFolderHierarchyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SyncFolderHierarchyResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SyncFolderHierarchyResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SyncFolderItemsCompletedEventHandler(object sender, SyncFolderItemsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SyncFolderItemsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SyncFolderItemsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SyncFolderItemsResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SyncFolderItemsResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CreateManagedFolderCompletedEventHandler(object sender, CreateManagedFolderCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateManagedFolderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateManagedFolderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CreateManagedFolderResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CreateManagedFolderResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetItemCompletedEventHandler(object sender, GetItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CreateItemCompletedEventHandler(object sender, CreateItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CreateItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CreateItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void DeleteItemCompletedEventHandler(object sender, DeleteItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void UpdateItemCompletedEventHandler(object sender, UpdateItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UpdateItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public UpdateItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((UpdateItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SendItemCompletedEventHandler(object sender, SendItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SendItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SendItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void MoveItemCompletedEventHandler(object sender, MoveItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MoveItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal MoveItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public MoveItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((MoveItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CopyItemCompletedEventHandler(object sender, CopyItemCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CopyItemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CopyItemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CopyItemResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CopyItemResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void CreateAttachmentCompletedEventHandler(object sender, CreateAttachmentCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateAttachmentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CreateAttachmentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CreateAttachmentResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CreateAttachmentResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void DeleteAttachmentCompletedEventHandler(object sender, DeleteAttachmentCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteAttachmentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal DeleteAttachmentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public DeleteAttachmentResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((DeleteAttachmentResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetAttachmentCompletedEventHandler(object sender, GetAttachmentCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetAttachmentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetAttachmentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetAttachmentResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetAttachmentResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetDelegateCompletedEventHandler(object sender, GetDelegateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetDelegateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetDelegateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetDelegateResponseMessageType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetDelegateResponseMessageType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void AddDelegateCompletedEventHandler(object sender, AddDelegateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddDelegateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal AddDelegateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public AddDelegateResponseMessageType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((AddDelegateResponseMessageType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void RemoveDelegateCompletedEventHandler(object sender, RemoveDelegateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RemoveDelegateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal RemoveDelegateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public RemoveDelegateResponseMessageType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((RemoveDelegateResponseMessageType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void UpdateDelegateCompletedEventHandler(object sender, UpdateDelegateCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UpdateDelegateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal UpdateDelegateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public UpdateDelegateResponseMessageType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((UpdateDelegateResponseMessageType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetUserAvailabilityCompletedEventHandler(object sender, GetUserAvailabilityCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserAvailabilityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetUserAvailabilityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetUserAvailabilityResponseType Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetUserAvailabilityResponseType)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void GetUserOofSettingsCompletedEventHandler(object sender, GetUserOofSettingsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUserOofSettingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal GetUserOofSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public GetUserOofSettingsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((GetUserOofSettingsResponse)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void SetUserOofSettingsCompletedEventHandler(object sender, SetUserOofSettingsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetUserOofSettingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SetUserOofSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SetUserOofSettingsResponse Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SetUserOofSettingsResponse)(this.results[0]));
            }
        }
    }
}
