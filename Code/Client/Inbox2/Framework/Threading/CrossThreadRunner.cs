using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Inbox2.Framework.Threading
{
	/// <summary>
	/// From: http://www.hedgate.net/articles/2007/01/08/instantiating-a-wpf-control-from-an-nunit-test/
	/// </summary>
	public class CrossThreadRunner
	{
		private Exception lastException;

		public void RunInMTA(ThreadStart userDelegate)
		{
			Run(userDelegate, ApartmentState.MTA);
		}

		public void RunInSTA(ThreadStart userDelegate)
		{
			Run(userDelegate, ApartmentState.STA);
		}

		private void Run(ThreadStart userDelegate, ApartmentState apartmentState)
		{
			lastException = null;

			Thread thread = new Thread(
				delegate()
					{
						try
						{
							userDelegate.Invoke();
						}
						catch (Exception e)
						{
							lastException = e;
						}
					});
			thread.SetApartmentState(apartmentState);

			thread.Start();
			thread.Join();

			if (ExceptionWasThrown())
				ThrowExceptionPreservingStack(lastException);
		}

		private bool ExceptionWasThrown()
		{
			return lastException != null;
		}

		[ReflectionPermission(SecurityAction.Demand)]
		private static void ThrowExceptionPreservingStack(Exception exception)
		{
			FieldInfo remoteStackTraceString = typeof(Exception).GetField(
				"_remoteStackTraceString",
				BindingFlags.Instance | BindingFlags.NonPublic);
			remoteStackTraceString.SetValue(exception, exception.StackTrace + Environment.NewLine);
			throw exception;
		}
	}
}