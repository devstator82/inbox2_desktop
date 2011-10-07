using System;

namespace Inbox2.Core.Unmanaged.Unrar
{
	public interface IUnrar : IDisposable
	{
		/// <summary>
		/// Event that is raised when a new chunk of data has been extracted
		/// </summary>
		event DataAvailableHandler DataAvailable;

		/// <summary>
		/// Event that is raised to indicate extraction progress
		/// </summary>
		event ExtractionProgressHandler ExtractionProgress;

		/// <summary>
		/// Event that is raised when a required archive volume is missing
		/// </summary>
		event MissingVolumeHandler MissingVolume;

		/// <summary>
		/// Event that is raised when a new file is encountered during processing
		/// </summary>
		event NewFileHandler NewFile;

		/// <summary>
		/// Event that is raised when a new archive volume is opened for processing
		/// </summary>
		event NewVolumeHandler NewVolume;

		/// <summary>
		/// Event that is raised when a password is required before continuing
		/// </summary>
		event PasswordRequiredHandler PasswordRequired;

		/// <summary>
		/// Path and name of RAR archive to open
		/// </summary>
		string ArchivePathName { get; set; }

		/// <summary>
		/// Archive comment 
		/// </summary>
		string Comment { get; }

		/// <summary>
		/// Current file being processed
		/// </summary>
		RARFileInfo CurrentFile { get; }

		/// <summary>
		/// Default destination path for extraction
		/// </summary>
		string DestinationPath { get; set; }

		/// <summary>
		/// Password for opening encrypted archive
		/// </summary>
		string Password { get; set; }

		/// <summary>
		/// Close the currently open archive
		/// </summary>
		/// <returns></returns>
		void Close();

		/// <summary>
		/// Opens archive specified by the ArchivePathName property for testing or extraction
		/// </summary>
		void Open();

		/// <summary>
		/// Opens archive specified by the ArchivePathName property with a specified mode
		/// </summary>
		/// <param name="openMode">Mode in which archive should be opened</param>
		void Open(Unrar.OpenMode openMode);

		/// <summary>
		/// Opens specified archive using the specified mode.  
		/// </summary>
		/// <param name="archivePathName">Path of archive to open</param>
		/// <param name="openMode">Mode in which to open archive</param>
		void Open(string archivePathName, Unrar.OpenMode openMode);

		/// <summary>
		/// Reads the next archive header and populates CurrentFile property data
		/// </summary>
		/// <returns></returns>
		bool ReadHeader();

		/// <summary>
		/// Returns array of file names contained in archive
		/// </summary>
		/// <returns></returns>
		string[] ListFiles();

		/// <summary>
		/// Moves the current archive position to the next available header
		/// </summary>
		/// <returns></returns>
		void Skip();

		/// <summary>
		/// Tests the ability to extract the current file without saving extracted data to disk
		/// </summary>
		/// <returns></returns>
		void Test();

		/// <summary>
		/// Extracts the current file to the default destination path
		/// </summary>
		/// <returns></returns>
		void Extract();

		/// <summary>
		/// Extracts the current file to a specified destination path and filename
		/// </summary>
		/// <param name="destinationName">Path and name of extracted file</param>
		/// <returns></returns>
		void Extract(string destinationName);

		/// <summary>
		/// Extracts the current file to a specified directory without renaming file
		/// </summary>
		/// <param name="destinationPath"></param>
		/// <returns></returns>
		void ExtractToDirectory(string destinationPath);
	}
}