using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BuildTools
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 1)
				throw new ApplicationException("Invalid application start parameters");

			string outputFolder = @"D:\Development\inbox2_deploy";
			string latestFolder = Path.Combine(outputFolder, "latest");

			switch (args[0].ToLower())
			{
				case "postbuildoutput":
					{
						// Copy the files in the latest folder to a directory with the current timestamp
						var files = Directory.GetFiles(latestFolder);

						int loop = 0;
						var outputdirname = Path.Combine(outputFolder, DateTime.Now.ToString("yyyyMMdd"));

						while (Directory.Exists(outputdirname))
							outputdirname = Path.Combine(outputFolder, String.Concat(DateTime.Now.ToString("yyyyMMdd"), "-", ++loop));

						var currentOutputFolder = Directory.CreateDirectory(outputdirname);

						foreach (var file in files)
						{
							var fi = new FileInfo(file);

							fi.CopyTo(Path.Combine(currentOutputFolder.FullName, fi.Name));
						}

						break;
					}
			}
		}
	}
}
