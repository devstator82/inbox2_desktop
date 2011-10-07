using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Inbox2.Core.Unmanaged.Unrar
{
	/*  Author:  Michael A. McCloskey
 *  Company: Schematrix
 *  Version: 20040714
 *  
 *  Personal Comments:
 *  I created this unrar wrapper class for personal use 
 *  after running into a number of issues trying to use
 *  another COM unrar product via COM interop.  I hope it 
 *  proves as useful to you as it has to me and saves you
 *  some time in building your own products.
 */

	/// <summary>
	/// Wrapper class for unrar DLL supplied by RARSoft.  
	/// Calls unrar DLL via platform invocation services (pinvoke).
	/// DLL is available at http://www.rarlab.com/rar/UnRARDLL.exe
	/// </summary>
	public class Unrar64 : IUnrar, IDisposable
	{
		
		#region Unrar function declarations

		[DllImport("Unrar64.dll")]
		private static extern IntPtr RAROpenArchive(ref Unrar.RAROpenArchiveData archiveData);

		[DllImport("Unrar64.dll")]
		private static extern IntPtr RAROpenArchiveEx(ref Unrar.RAROpenArchiveDataEx archiveData);

		[DllImport("Unrar64.dll")]
		private static extern int RARCloseArchive(IntPtr hArcData);

		[DllImport("Unrar64.dll")]
		private static extern int RARReadHeader(IntPtr hArcData, ref Unrar.RARHeaderData headerData);

		[DllImport("Unrar64.dll")]
		private static extern int RARReadHeaderEx(IntPtr hArcData, ref Unrar.RARHeaderDataEx headerData);

		[DllImport("Unrar64.dll")]
		private static extern int RARProcessFile(IntPtr hArcData, int operation,
			[MarshalAs(UnmanagedType.LPStr)] string destPath,
			[MarshalAs(UnmanagedType.LPStr)] string destName);

		[DllImport("Unrar64.dll")]
		private static extern void RARSetCallback(IntPtr hArcData, UNRARCallback callback, int userData);

		[DllImport("Unrar64.dll")]
		private static extern void RARSetPassword(IntPtr hArcData,
			[MarshalAs(UnmanagedType.LPStr)] string password);

		// Unrar callback delegate signature
		private delegate int UNRARCallback(uint msg, int UserData, IntPtr p1, int p2);

		#endregion

		#region Public event declarations

		/// <summary>
		/// Event that is raised when a new chunk of data has been extracted
		/// </summary>
		public event DataAvailableHandler DataAvailable;
		/// <summary>
		/// Event that is raised to indicate extraction progress
		/// </summary>
		public event ExtractionProgressHandler ExtractionProgress;
		/// <summary>
		/// Event that is raised when a required archive volume is missing
		/// </summary>
		public event MissingVolumeHandler MissingVolume;
		/// <summary>
		/// Event that is raised when a new file is encountered during processing
		/// </summary>
		public event NewFileHandler NewFile;
		/// <summary>
		/// Event that is raised when a new archive volume is opened for processing
		/// </summary>
		public event NewVolumeHandler NewVolume;
		/// <summary>
		/// Event that is raised when a password is required before continuing
		/// </summary>
		public event PasswordRequiredHandler PasswordRequired;

		#endregion

		#region Private fields

		private string archivePathName = string.Empty;
		private IntPtr archiveHandle = new IntPtr(0);
		private bool retrieveComment = true;
		private string password = string.Empty;
		private string comment = string.Empty;
		private Unrar.ArchiveFlags archiveFlags = 0;
		private Unrar.RARHeaderDataEx header = new Unrar.RARHeaderDataEx();
		private string destinationPath = string.Empty;
		private RARFileInfo currentFile = null;
		private UNRARCallback callback = null;

		#endregion

		#region Object lifetime procedures

		public Unrar64()
		{
			this.callback = new UNRARCallback(RARCallback);
		}

		public Unrar64(string archivePathName)
			: this()
		{
			this.archivePathName = archivePathName;
		}

		~Unrar64()
		{
			if (this.archiveHandle != IntPtr.Zero)
			{
				Unrar64.RARCloseArchive(this.archiveHandle);
				this.archiveHandle = IntPtr.Zero;
			}
		}

		public void Dispose()
		{
			if (this.archiveHandle != IntPtr.Zero)
			{
				Unrar64.RARCloseArchive(this.archiveHandle);
				this.archiveHandle = IntPtr.Zero;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Path and name of RAR archive to open
		/// </summary>
		public string ArchivePathName
		{
			get
			{
				return this.archivePathName;
			}
			set
			{
				this.archivePathName = value;
			}
		}

		/// <summary>
		/// Archive comment 
		/// </summary>
		public string Comment
		{
			get
			{
				return (this.comment);
			}
		}

		/// <summary>
		/// Current file being processed
		/// </summary>
		public RARFileInfo CurrentFile
		{
			get
			{
				return (this.currentFile);
			}
		}

		/// <summary>
		/// Default destination path for extraction
		/// </summary>
		public string DestinationPath
		{
			get
			{
				return this.destinationPath;
			}
			set
			{
				this.destinationPath = value;
			}
		}

		/// <summary>
		/// Password for opening encrypted archive
		/// </summary>
		public string Password
		{
			get
			{
				return (this.password);
			}
			set
			{
				this.password = value;
				if (this.archiveHandle != IntPtr.Zero)
					RARSetPassword(this.archiveHandle, value);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Close the currently open archive
		/// </summary>
		/// <returns></returns>
		public void Close()
		{
			// Exit without exception if no archive is open
			if (this.archiveHandle == IntPtr.Zero)
				return;

			// Close archive
			int result = Unrar64.RARCloseArchive(this.archiveHandle);

			// Check result
			if (result != 0)
			{
				ProcessFileError(result);
			}
			else
			{
				this.archiveHandle = IntPtr.Zero;
			}
		}

		/// <summary>
		/// Opens archive specified by the ArchivePathName property for testing or extraction
		/// </summary>
		public void Open()
		{
			if (this.ArchivePathName.Length == 0)
				throw new IOException("Archive name has not been set.");
			this.Open(this.ArchivePathName, Unrar.OpenMode.Extract);
		}

		/// <summary>
		/// Opens archive specified by the ArchivePathName property with a specified mode
		/// </summary>
		/// <param name="openMode">Mode in which archive should be opened</param>
		public void Open(Unrar.OpenMode openMode)
		{
			if (this.ArchivePathName.Length == 0)
				throw new IOException("Archive name has not been set.");
			this.Open(this.ArchivePathName, openMode);
		}

		/// <summary>
		/// Opens specified archive using the specified mode.  
		/// </summary>
		/// <param name="archivePathName">Path of archive to open</param>
		/// <param name="openMode">Mode in which to open archive</param>
		public void Open(string archivePathName, Unrar.OpenMode openMode)
		{
			IntPtr handle = IntPtr.Zero;

			// Close any previously open archives
			if (this.archiveHandle != IntPtr.Zero)
				this.Close();

			// Prepare extended open archive struct
			this.ArchivePathName = archivePathName;
			Unrar.RAROpenArchiveDataEx openStruct = new Unrar.RAROpenArchiveDataEx();
			openStruct.Initialize();
			openStruct.ArcName = this.archivePathName + "\0";
			openStruct.ArcNameW = this.archivePathName + "\0";
			openStruct.OpenMode = (uint)openMode;
			if (this.retrieveComment)
			{
				openStruct.CmtBuf = new string((char)0, 65536);
				openStruct.CmtBufSize = 65536;
			}
			else
			{
				openStruct.CmtBuf = null;
				openStruct.CmtBufSize = 0;
			}

			// Open archive
			handle = Unrar64.RAROpenArchiveEx(ref openStruct);

			// Check for success
			if (openStruct.OpenResult != 0)
			{
				switch ((Unrar.RarError)openStruct.OpenResult)
				{
					case Unrar.RarError.InsufficientMemory:
						throw new OutOfMemoryException("Insufficient memory to perform operation.");

					case Unrar.RarError.BadData:
						throw new IOException("Archive header broken");

					case Unrar.RarError.BadArchive:
						throw new IOException("File is not a valid archive.");

					case Unrar.RarError.OpenError:
						throw new IOException("File could not be opened.");
				}
			}

			// Save handle and flags
			this.archiveHandle = handle;
			this.archiveFlags = (Unrar.ArchiveFlags)openStruct.Flags;

			// Set callback
			Unrar64.RARSetCallback(this.archiveHandle, this.callback, this.GetHashCode());

			// If comment retrieved, save it
			if (openStruct.CmtState == 1)
				this.comment = openStruct.CmtBuf.ToString();

			// If password supplied, set it
			if (this.password.Length != 0)
				Unrar64.RARSetPassword(this.archiveHandle, this.password);

			// Fire NewVolume event for first volume
			this.OnNewVolume(this.archivePathName);
		}

		/// <summary>
		/// Reads the next archive header and populates CurrentFile property data
		/// </summary>
		/// <returns></returns>
		public bool ReadHeader()
		{
			// Throw exception if archive not open
			if (this.archiveHandle == IntPtr.Zero)
				throw new IOException("Archive is not open.");

			// Initialize header struct
			this.header = new Unrar.RARHeaderDataEx();
			header.Initialize();

			// Read next entry
			currentFile = null;
			int result = Unrar64.RARReadHeaderEx(this.archiveHandle, ref this.header);

			// Check for error or end of archive
			if ((Unrar.RarError)result == Unrar.RarError.EndOfArchive)
				return false;
			else if ((Unrar.RarError)result == Unrar.RarError.BadData)
				throw new IOException("Archive data is corrupt.");

			// Determine if new file
			if (((header.Flags & 0x01) != 0) && currentFile != null)
				currentFile.ContinuedFromPrevious = true;
			else
			{
				// New file, prepare header
				currentFile = new RARFileInfo();
				currentFile.FileName = header.FileNameW.ToString();
				if ((header.Flags & 0x02) != 0)
					currentFile.ContinuedOnNext = true;
				if (header.PackSizeHigh != 0)
					currentFile.PackedSize = (header.PackSizeHigh * 0x100000000) + header.PackSize;
				else
					currentFile.PackedSize = header.PackSize;
				if (header.UnpSizeHigh != 0)
					currentFile.UnpackedSize = (header.UnpSizeHigh * 0x100000000) + header.UnpSize;
				else
					currentFile.UnpackedSize = header.UnpSize;
				currentFile.HostOS = (int)header.HostOS;
				currentFile.FileCRC = header.FileCRC;
				currentFile.FileTime = FromMSDOSTime(header.FileTime);
				currentFile.VersionToUnpack = (int)header.UnpVer;
				currentFile.Method = (int)header.Method;
				currentFile.FileAttributes = (int)header.FileAttr;
				currentFile.BytesExtracted = 0;
				if ((header.Flags & 0xE0) == 0xE0)
					currentFile.IsDirectory = true;
				this.OnNewFile();
			}

			// Return success
			return true;
		}

		/// <summary>
		/// Returns array of file names contained in archive
		/// </summary>
		/// <returns></returns>
		public string[] ListFiles()
		{
			ArrayList fileNames = new ArrayList();
			while (this.ReadHeader())
			{
				if (!currentFile.IsDirectory)
					fileNames.Add(currentFile.FileName);
				this.Skip();
			}
			string[] files = new string[fileNames.Count];
			fileNames.CopyTo(files);
			return files;
		}

		/// <summary>
		/// Moves the current archive position to the next available header
		/// </summary>
		/// <returns></returns>
		public void Skip()
		{
			int result = Unrar64.RARProcessFile(this.archiveHandle, (int)Unrar.Operation.Skip, string.Empty, string.Empty);

			// Check result
			if (result != 0)
			{
				ProcessFileError(result);
			}
		}

		/// <summary>
		/// Tests the ability to extract the current file without saving extracted data to disk
		/// </summary>
		/// <returns></returns>
		public void Test()
		{
			int result = Unrar64.RARProcessFile(this.archiveHandle, (int)Unrar.Operation.Test, string.Empty, string.Empty);

			// Check result
			if (result != 0)
			{
				ProcessFileError(result);
			}
		}

		/// <summary>
		/// Extracts the current file to the default destination path
		/// </summary>
		/// <returns></returns>
		public void Extract()
		{
			this.Extract(this.destinationPath, string.Empty);
		}

		/// <summary>
		/// Extracts the current file to a specified destination path and filename
		/// </summary>
		/// <param name="destinationName">Path and name of extracted file</param>
		/// <returns></returns>
		public void Extract(string destinationName)
		{
			this.Extract(string.Empty, destinationName);
		}

		/// <summary>
		/// Extracts the current file to a specified directory without renaming file
		/// </summary>
		/// <param name="destinationPath"></param>
		/// <returns></returns>
		public void ExtractToDirectory(string destinationPath)
		{
			this.Extract(destinationPath, string.Empty);
		}

		#endregion

		#region Private Methods

		private void Extract(string destinationPath, string destinationName)
		{
			int result = Unrar64.RARProcessFile(this.archiveHandle, (int)Unrar.Operation.Extract, destinationPath, destinationName);

			// Check result
			if (result != 0)
			{
				ProcessFileError(result);
			}
		}

		private DateTime FromMSDOSTime(uint dosTime)
		{
			int day = 0;
			int month = 0;
			int year = 0;
			int second = 0;
			int hour = 0;
			int minute = 0;
			ushort hiWord;
			ushort loWord;
			hiWord = (ushort)((dosTime & 0xFFFF0000) >> 16);
			loWord = (ushort)(dosTime & 0xFFFF);
			year = ((hiWord & 0xFE00) >> 9) + 1980;
			month = (hiWord & 0x01E0) >> 5;
			day = hiWord & 0x1F;
			hour = (loWord & 0xF800) >> 11;
			minute = (loWord & 0x07E0) >> 5;
			second = (loWord & 0x1F) << 1;
			return new DateTime(year, month, day, hour, minute, second);
		}

		private void ProcessFileError(int result)
		{
			switch ((Unrar.RarError)result)
			{
				case Unrar.RarError.UnknownFormat:
					throw new OutOfMemoryException("Unknown archive format.");

				case Unrar.RarError.BadData:
					throw new IOException("File CRC Error");

				case Unrar.RarError.BadArchive:
					throw new IOException("File is not a valid archive.");

				case Unrar.RarError.OpenError:
					throw new IOException("File could not be opened.");

				case Unrar.RarError.CreateError:
					throw new IOException("File could not be created.");

				case Unrar.RarError.CloseError:
					throw new IOException("File close error.");

				case Unrar.RarError.ReadError:
					throw new IOException("File read error.");

				case Unrar.RarError.WriteError:
					throw new IOException("File write error.");
			}
		}

		private int RARCallback(uint msg, int UserData, IntPtr p1, int p2)
		{
			string volume = string.Empty;
			string newVolume = string.Empty;
			int result = -1;

			switch ((Unrar.CallbackMessages)msg)
			{
				case Unrar.CallbackMessages.VolumeChange:
					volume = Marshal.PtrToStringAnsi(p1);
					if ((Unrar.VolumeMessage)p2 == Unrar.VolumeMessage.Notify)
						result = OnNewVolume(volume);
					else if ((Unrar.VolumeMessage)p2 == Unrar.VolumeMessage.Ask)
					{
						newVolume = OnMissingVolume(volume);
						if (newVolume.Length == 0)
							result = -1;
						else
						{
							if (newVolume != volume)
							{
								for (int i = 0; i < newVolume.Length; i++)
								{
									Marshal.WriteByte(p1, i, (byte)newVolume[i]);
								}
								Marshal.WriteByte(p1, newVolume.Length, (byte)0);
							}
							result = 1;
						}
					}
					break;

				case Unrar.CallbackMessages.ProcessData:
					result = OnDataAvailable(p1, p2);
					break;

				case Unrar.CallbackMessages.NeedPassword:
					result = OnPasswordRequired(p1, p2);
					break;
			}
			return result;
		}

		#endregion

		#region Protected Virtual (Overridable) Methods

		protected virtual void OnNewFile()
		{
			if (this.NewFile != null)
			{
				NewFileEventArgs e = new NewFileEventArgs(this.currentFile);
				this.NewFile(this, e);
			}
		}

		protected virtual int OnPasswordRequired(IntPtr p1, int p2)
		{
			int result = -1;
			if (this.PasswordRequired != null)
			{
				PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
				this.PasswordRequired(this, e);
				if (e.ContinueOperation && e.Password.Length > 0)
				{
					for (int i = 0; (i < e.Password.Length) && (i < p2); i++)
						Marshal.WriteByte(p1, i, (byte)e.Password[i]);
					Marshal.WriteByte(p1, e.Password.Length, (byte)0);
					result = 1;
				}
			}
			else
			{
				throw new IOException("Password is required for extraction.");
			}
			return result;
		}

		protected virtual int OnDataAvailable(IntPtr p1, int p2)
		{
			int result = 1;
			if (this.currentFile != null)
				this.currentFile.BytesExtracted += p2;
			if (this.DataAvailable != null)
			{
				byte[] data = new byte[p2];
				Marshal.Copy(p1, data, 0, p2);
				DataAvailableEventArgs e = new DataAvailableEventArgs(data);
				this.DataAvailable(this, e);
				if (!e.ContinueOperation)
					result = -1;
			}
			if ((this.ExtractionProgress != null) && (this.currentFile != null))
			{
				ExtractionProgressEventArgs e = new ExtractionProgressEventArgs();
				e.FileName = this.currentFile.FileName;
				e.FileSize = this.currentFile.UnpackedSize;
				e.BytesExtracted = this.currentFile.BytesExtracted;
				e.PercentComplete = this.currentFile.PercentComplete;
				this.ExtractionProgress(this, e);
				if (!e.ContinueOperation)
					result = -1;
			}
			return result;
		}

		protected virtual int OnNewVolume(string volume)
		{
			int result = 1;
			if (this.NewVolume != null)
			{
				NewVolumeEventArgs e = new NewVolumeEventArgs(volume);
				this.NewVolume(this, e);
				if (!e.ContinueOperation)
					result = -1;
			}
			return result;
		}

		protected virtual string OnMissingVolume(string volume)
		{
			string result = string.Empty;
			if (this.MissingVolume != null)
			{
				MissingVolumeEventArgs e = new MissingVolumeEventArgs(volume);
				this.MissingVolume(this, e);
				if (e.ContinueOperation)
					result = e.VolumeName;
			}
			return result;
		}

		#endregion
	}	
}
