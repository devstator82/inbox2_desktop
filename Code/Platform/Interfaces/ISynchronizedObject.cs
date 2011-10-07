using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces
{
	public interface ISynchronizedObject
	{
		void AcquireReaderLock();
		void ReleaseReaderLock();

		void AcquireWriterLock();
		void ReleaseWriterLock();
	}
}
