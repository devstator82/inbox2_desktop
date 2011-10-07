using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace Inbox2.Framework.Interop.Windows
{
	public class DefaultMailClientHandler
	{
		public bool IsDefaultMailClient()
		{
			var root = Registry.ClassesRoot.OpenSubKey("mailto", RegistryKeyPermissionCheck.ReadSubTree);
			if (root == null) return false;

			var shell = root.OpenSubKey("shell", RegistryKeyPermissionCheck.ReadSubTree);
			if (shell == null) return false;

			var open = shell.OpenSubKey("open", RegistryKeyPermissionCheck.ReadSubTree);
			if (open == null) return false;

			var command = open.OpenSubKey("command", RegistryKeyPermissionCheck.ReadSubTree);
			if (command == null) return false;

			var value = command.GetValue(String.Empty, null) as string;
			if (value == null) return false;

			return value.Contains("inbox2.exe");
		}

		/// <summary>
		/// See http://msdn.microsoft.com/en-us/library/aa767914(v=VS.85).aspx for documentation.
		/// </summary>
		public void AttachMailToHandler()
		{
			var executable = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "inbox2.exe");

			if (Registry.ClassesRoot.GetSubKeyNames().Contains("mailto"))
				Registry.ClassesRoot.DeleteSubKeyTree("mailto");

			var root = CreateAndCheckSubKey(Registry.ClassesRoot, "mailto");

			root.SetValue(String.Empty, "URL:MailTo Protocol");
			root.SetValue("URL Protocol", String.Empty);

			var icon = CreateAndCheckSubKey(root, "DefaultIcon");
			icon.SetValue(String.Empty, String.Format("\"{0}\",0", executable));

			var shell = CreateAndCheckSubKey(root, "shell");
			var open = CreateAndCheckSubKey(shell, "open");
			var command = CreateAndCheckSubKey(open, "command");

			command.SetValue(String.Empty, String.Format("\"{0}\" /mlt \"%1\"", executable));
			root.Flush();

			command.Close();
			open.Close();
			shell.Close();
			icon.Close();
			root.Close();
		}

		public void AddSystemMailClientSettings()
		{
			var executable = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "inbox2.exe");
			var root = Registry.LocalMachine.OpenSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree)
				.OpenSubKey("Clients", RegistryKeyPermissionCheck.ReadWriteSubTree)
				.OpenSubKey("Mail", RegistryKeyPermissionCheck.ReadWriteSubTree);

			if (root == null)
				throw new ApplicationException("Missing required key HKEY_LOCAL_MACHINE/SOFTWARE/Clients/Mail");

			if (root.GetSubKeyNames().Contains("Inbox2"))
				root.DeleteSubKeyTree("Inbox2");

			var inbox2 = CreateAndCheckSubKey(root, "Inbox2");
			inbox2.SetValue(String.Empty, "Inbox2 Desktop Client");
			inbox2.SetValue("LocalizedString", "Inbox2 Desktop Client");
			inbox2.SetValue("DefaultIcon", String.Format("\"{0}\",0", executable));

			var shell = CreateAndCheckSubKey(inbox2, "shell");
			var open = CreateAndCheckSubKey(shell, "open");
			var command = CreateAndCheckSubKey(open, "command");

			command.SetValue(String.Empty, executable);

			command.Close();
			open.Close();
			shell.Close();
			inbox2.Close();
			root.Flush();
			root.Close();
		}

		public void AddUserMailClientSettings()
		{
			var root = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);

			if (root == null)
				throw new ApplicationException("Unable to open HKEY_CURRENT_USER\\Software");

			var clients = root.OpenSubKey("Clients", RegistryKeyPermissionCheck.ReadWriteSubTree);

			if (clients == null)
				clients = CreateAndCheckSubKey(root, "Clients");

			var mail = clients.OpenSubKey("Mail", RegistryKeyPermissionCheck.ReadWriteSubTree);

			if (mail == null)
				mail = CreateAndCheckSubKey(clients, "Mail");

			mail.SetValue(String.Empty, "Inbox2");
			mail.Close();
			clients.Close();
			root.Close();

			root.Flush();
			root.Close();
		}

		RegistryKey CreateAndCheckSubKey(RegistryKey parent, string name)
		{
			var key = parent.CreateSubKey(name, RegistryKeyPermissionCheck.ReadWriteSubTree);

			if (key == null)
				throw new ApplicationException(String.Format("Failed to create {0} subkey in ClassesRoot", name));

			return key;
		}
	}
}
