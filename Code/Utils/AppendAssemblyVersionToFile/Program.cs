using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppendAssemblyVersionToFile
{
	class Program
	{
		static void Main(string[] args)
		{
			string assemblyname = args[0];
			string outputfilename = args[1];

			Assembly asm = Assembly.LoadFrom(assemblyname);

			var fi = new FileInfo(outputfilename);

			string target = Path.Combine(Path.GetDirectoryName(outputfilename), String.Format("{0} {1}{2}",
				Path.GetFileNameWithoutExtension(outputfilename),
				asm.GetName().Version,
				Path.GetExtension(outputfilename)));

			fi.CopyTo(target);
		}
	}
}
