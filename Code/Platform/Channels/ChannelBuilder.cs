using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inbox2.Platform.Channels.Configuration;
using Inbox2.Platform.Channels.Interfaces;

namespace Inbox2.Platform.Channels
{
	public static class ChannelBuilder
	{
		public static T BuildWithCredentials<T>(Channel configuration) where T : IClientChannel
		{
			var instance = (T)Activator.CreateInstance(configuration.Type);

			SetConnectionParameters(instance, configuration);
			SetAuthenticationParameters(instance, configuration.Authentication);

			return instance;
		}

		public static void SetChannelHostname(string hostname, params Channel[] channels)
		{
			foreach (var channel in channels)
			{
				if (channel == null)
					continue;

				// Only set hostname if it hasn't been set before (to prevent overrides)
				if (String.IsNullOrEmpty(channel.Hostname))
					channel.Hostname = hostname;
			}
		}

		public static void SetChannelAuthentication(string username, string password, params Channel[] channels)
		{
			foreach (var channel in channels)
			{
				if (channel == null)
					continue;

				if (channel.Authentication == null)
				{
					channel.Authentication = new Authentication {Username = username, Password = password};
				}
				else
				{
					// In case channel defines their own custom CredentialsProvider
					channel.Authentication.Username = username;
					channel.Authentication.Password = password;
				}
			}
		}

		/// <summary>
		/// Sets the connection parameters.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="configuration">The configuration.</param>
		static void SetConnectionParameters(IClientChannel channel, Channel configuration)
		{
			if (channel == null) throw new ArgumentNullException("channel");
			if (configuration == null) throw new ArgumentNullException("configuration");

			channel.Hostname = configuration.Hostname;
			channel.Port = configuration.Port;
			channel.IsSecured = configuration.IsSecured;
			channel.MaxConcurrentConnections = configuration.MaxConcurrentConnections;
		}

		/// <summary>
		/// Sets the authentication parameters.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <param name="authentication">The authentication.</param>
		static void SetAuthenticationParameters(IClientChannel channel, Authentication authentication)
		{
			if (authentication == null)
				throw new ArgumentNullException("authentication");

			if (String.IsNullOrEmpty(authentication.TypeSurrogate))
			{
				channel.CredentialsProvider = new DefaultCredentialsProvider(
					authentication.Username, authentication.Password);
			}
			else
			{
				channel.CredentialsProvider =
					(DefaultCredentialsProvider)Activator.CreateInstance(authentication.Type,
					                                                     authentication.Username, authentication.Password);				
			}
		}
	}
}