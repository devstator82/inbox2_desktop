using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Core.Unmanaged.Unrar
{
	public class UnrarExecutable
	{
		public event EventHandler<MissingVolumeEventArgs> VolumeMissing;
		public event EventHandler<ExtractionProgressEventArgs> ExtractionProgress;

		private List<string> skip = new List<string>();
		private int progress;

		public List<string> Skip
		{
			get { return skip; }
		}

		/// <summary>
		/// Gets the progress.
		/// </summary>
		/// <value>The progress.</value>
		public int Progress
		{
			get { return progress; }
		}

		/// <summary>
		/// Executes the specified rar filename.
		/// </summary>
		/// <param name="rarFilename">The rar filename.</param>
		/// <param name="destinationPath"></param>
		public void Execute(string rarFilename, string destinationPath)
		{
			IUnrar unrar = Is64BitMode() ? (IUnrar)new Unrar64() : (IUnrar)new Unrar();
			unrar.ExtractionProgress += OnExtractionProgress;
			unrar.MissingVolume += OnMissingVolume;
			unrar.Open(rarFilename, Unrar.OpenMode.Extract);
			unrar.DestinationPath = destinationPath;

			while (unrar.ReadHeader())
			{
				if (skip.Contains(unrar.CurrentFile.FileName))
					unrar.Skip();
				else
					unrar.Extract();
			}

			unrar.Close();
		}

		/// <summary>
		/// Executes the specified rar filename.
		/// </summary>
		/// <param name="rarFilename"></param>
		/// <param name="filenames"></param>
		/// <param name="destFilenames"></param>
		/// <param name="destinationPath"></param>
		public void ExecuteFiles(string rarFilename, string[] filenames, string[] destFilenames, string destinationPath)
		{
			if (filenames == null || destFilenames == null || String.IsNullOrEmpty(destinationPath))
				throw new ArgumentNullException("One of the required arguments is missing");

			if (filenames.Length == 0 || destFilenames.Length == 0 || filenames.Length != destFilenames.Length)
				throw new ArgumentException("Filenames or destFilenames parameters were empty or did not contain the same amount of strings");

			IUnrar unrar = Is64BitMode() ? (IUnrar)new Unrar64() : (IUnrar)new Unrar();
			unrar.ExtractionProgress += OnExtractionProgress;
			unrar.MissingVolume += OnMissingVolume;
			unrar.Open(rarFilename, Unrar.OpenMode.Extract);
			unrar.DestinationPath = destinationPath;

			while (unrar.ReadHeader())
			{
				string filenameInArchive = unrar.CurrentFile.FileName;
				string filename = Path.GetFileName(filenameInArchive);

				if (filenames.Contains(filename))
				{
					// Get the target filename
					int index = Array.IndexOf(filenames, filenameInArchive);
					string targetFilename = destFilenames[index];

					unrar.Extract(Path.Combine(destinationPath, targetFilename));
				}
			}

			unrar.Close();
		}

		/// <summary>
		/// Gets a list of all the files in the arcive.
		/// </summary>
		/// <param name="rarFilename"></param>
		/// <returns></returns>
		public List<ArchivedFile> GetFilesInArchive(string rarFilename)
		{
			var results = new List<ArchivedFile>();

			IUnrar unrar = Is64BitMode() ? (IUnrar)new Unrar64() : (IUnrar)new Unrar();
			unrar.ExtractionProgress += OnExtractionProgress;
			unrar.MissingVolume += OnMissingVolume;
			unrar.Open(rarFilename, Unrar.OpenMode.List);

			while (unrar.ReadHeader())
			{
				var file = new ArchivedFile();
				file.Filename = unrar.CurrentFile.FileName;
				file.FileSize = unrar.CurrentFile.UnpackedSize;

				results.Add(file);
			}

			unrar.Close();

			return results;
		}

		protected void OnMissingVolume(object sender, MissingVolumeEventArgs e)
		{
			e.ContinueOperation = false;

			if (VolumeMissing != null)
				VolumeMissing(this, e);
		}

		protected void OnExtractionProgress(object sender, ExtractionProgressEventArgs e)
		{
			int currentProgress = (int)e.PercentComplete;

			// Only raise event on whole percent progress increase
			if (currentProgress > progress)
			{
				progress = currentProgress;

				if (ExtractionProgress != null)
					ExtractionProgress(this, e);
			}
		}

		static bool Is64BitMode()
		{
			return System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8;
		}
	}

	public class ArchivedFile
	{
		public string Filename { get; set; }
		public long FileSize { get; set; }
	}
}
