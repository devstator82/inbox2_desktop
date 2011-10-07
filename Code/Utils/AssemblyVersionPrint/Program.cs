using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyVersionPrint
{
	class Program
	{
		static void Main(string[] args)
		{
			string assemblyname = args[0];
			string outputfilename = args[1];

			Assembly asm = Assembly.LoadFrom(assemblyname);

			File.WriteAllText(outputfilename, asm.GetName().Version.ToString());
		}
	}
}
