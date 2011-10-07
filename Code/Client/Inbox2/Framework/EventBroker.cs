using System;
using System.Collections.Generic;
using System.Linq;

namespace Inbox2.Framework
{
	public static class EventBroker
	{
		readonly static List<IEventReg> _Registrations = new List<IEventReg>();
		private static object _synclock = new object();

		public static IEventReg Subscribe(string eventName, Action action)
		{
			var registration = new EventReg();
			registration.EventName = eventName;
			registration.Action = action;

			lock (_synclock)
				_Registrations.Add(registration);

			return registration;
		}

		public static IEventReg Subscribe<T>(string eventName, Action<T> action)
		{
			var registration = new EventReg<T>();
			registration.EventName = eventName;
			registration.Action = action;

			lock (_synclock)
				_Registrations.Add(registration);

			return registration;
		}

		public static IEventReg Subscribe<T1, T2>(string eventName, Action<T1, T2> action)
		{
			var registration = new EventReg<T1, T2>();
			registration.EventName = eventName;
			registration.Action = action;

			lock (_synclock)
				_Registrations.Add(registration);

			return registration;
		}

		public static IEventReg Subscribe<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
		{
			var registration = new EventReg<T1, T2, T3>();
			registration.EventName = eventName;
			registration.Action = action;

			lock (_synclock)
				_Registrations.Add(registration);

			return registration;
		}

		public static void Unregister(IEventReg registration)
		{
			lock (_synclock)
				_Registrations.Remove(registration);
		}

		public static void Publish(string eventName)
		{
			List<EventReg> list;

			lock (_synclock)
				list = (_Registrations.OfType<EventReg>()
					.Where(e => e.EventName == eventName))
					.ToList();

			list.ForEach(reg => reg.Action());
		}

		public static void Publish<T>(string eventName, T obj)
		{
			List<EventReg<T>> list;

			lock (_synclock)
				list = (_Registrations.OfType<EventReg<T>>()
					.Where(e => e.EventName == eventName))
					.ToList();

			list.ForEach(reg => reg.Action(obj));
		}

		public static void Publish<T1, T2>(string eventName, T1 obj1, T2 obj2)
		{
			List<EventReg<T1, T2>> list;

			lock (_synclock) 
				list = (_Registrations.OfType<EventReg<T1, T2>>()
					.Where(e => e.EventName == eventName))
					.ToList();

			list.ForEach(reg => reg.Action(obj1, obj2));
		}

		public static void Publish<T1, T2, T3>(string eventName, T1 obj1, T2 obj2, T3 obj3)
		{
			List<EventReg<T1, T2, T3>> list;

			lock (_synclock) 
				list = (_Registrations.OfType<EventReg<T1, T2, T3>>()
					.Where(e => e.EventName == eventName))
					.ToList();

			list.ForEach(reg => reg.Action(obj1, obj2, obj3));
		}	
	}

	public interface IEventReg
	{
		string EventName { get; set; }
	}

	public class EventReg : IEventReg
	{
		public string EventName { get; set; }

		public Action Action { get; set; }
	}

	public class EventReg<T> : IEventReg
	{
		public string EventName { get; set; }

		public Action<T> Action { get; set; }
	}

	public class EventReg<T1, T2> : IEventReg
	{
		public string EventName { get; set; }

		public Action<T1, T2> Action { get; set; }
	}

	public class EventReg<T1, T2, T3> : IEventReg
	{
		public string EventName { get; set; }

		public Action<T1, T2, T3> Action { get; set; }
	}
}
