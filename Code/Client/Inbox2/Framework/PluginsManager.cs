using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Inbox2.Framework.Interfaces.Plugins;

namespace Inbox2.Framework
{
	public class PluginsManager
	{
		#region Singleton pattern implementation

		private static PluginsManager _Current;

		public static PluginsManager Current
		{
			get
			{
				if (_Current == null)
					_Current = new PluginsManager();

				return _Current;
			}
		}

		private PluginsManager() { }

		#endregion

		[ImportMany]
		public List<PluginPackage> Plugins { get; set; }

		[ImportMany]
		public List<IUrlShortener> UrlShorteners { get; set; }

		public PluginPackage SelectedPlugin { get; set; }

		/// <summary>
		/// Gets the plugin with he given generic type. Throws an exception if not found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetPlugin<T>() where T : PluginPackage
		{
			foreach (var plugin in Plugins)
			{
				if (plugin.GetType() == typeof(T))
					return (T)plugin;
			}

			throw new ApplicationException("Requested plugin could not been found!");
		}

		/// <summary>
		/// Gets the state with he given generic type. Throws an exception if not found.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetState<T>() where T : PluginStateBase
		{
			foreach (var plugin in Plugins)
			{
				if (plugin.State != null && plugin.State.GetType() == typeof(T))
					return (T) plugin.State;
			}

			throw new ApplicationException("Requested state could not been found!");
		}

		/// <summary>
		/// Gets the plugin by the given type. Throws an exception if not found.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public PluginPackage GetPluginBy(Type t)
		{
			foreach (var plugin in Plugins)
			{
				if (plugin.GetType() == t)
					return plugin;
			}

			throw new ApplicationException("Requested plugin could not been found!");
		}
	}
}