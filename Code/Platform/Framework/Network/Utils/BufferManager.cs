using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network.Utils
{
	/// <summary>
	/// A manager to handle buffers for the socket connections
	/// </summary>
	/// <remarks>
	/// When used in an async call a buffer is pinned. Large numbers of pinned buffers
	/// cause problem with the GC (in particular it causes heap fragmentation).
	///
	/// This class maintains a set of large segments and gives clients pieces of these
	/// segments that they can use for their buffers. The alternative to this would be to
	/// create many small arrays which it then maintained. This methodology should be slightly
	/// better than the many small array methodology because in creating only a few very
	/// large objects it will force these objects to be placed on the LOH. Since the
	/// objects are on the LOH they are at this time not subject to compacting which would
	/// require an update of all GC roots as would be the case with lots of smaller arrays
	/// that were in the normal heap.
	/// </remarks>
	public class BufferManager
	{
		public static BufferManager Current;

		public const int Segments = 100;
		public const int ChunkSize = 1024;

		static BufferManager()
		{
			Current = new BufferManager(8, 1024, 100);
		}

		private readonly int m_SegmentChunks;
		private readonly int m_ChunkSize;
		private readonly int m_SegmentSize;
		private readonly Stack<ArraySegment<byte>> m_Buffers;
		private readonly object m_LockObject = new Object();
		private readonly List<byte[]> m_Segments;

		/// <summary>
		/// The current number of buffers available
		/// </summary>
		public int AvailableBuffers
		{
			get { return m_Buffers.Count; } //do we really care about volatility here?
		}

		/// <summary>
		/// The total size of all buffers
		/// </summary>
		public int TotalBufferSize
		{
			get { return m_Segments.Count * m_SegmentSize; } //do we really care about volatility here?
		}

		/// <summary>
		/// Constructs a new <see cref="BufferManager"></see> object
		/// </summary>
		/// <param name="_SegmentChunks">The number of chunks tocreate per segment</param>
		/// <param name="_ChunkSize">The size of a chunk in bytes</param>
		/// <param name="_InitialSegments">The initial number of segments to create</param>
		public BufferManager(int _SegmentChunks, int _ChunkSize, int _InitialSegments)
		{
			m_SegmentChunks = _SegmentChunks;
			m_ChunkSize = _ChunkSize;
			m_SegmentSize = m_SegmentChunks * m_ChunkSize;
			m_Buffers = new Stack<ArraySegment<byte>>(_SegmentChunks * _InitialSegments);
			m_Segments = new List<byte[]>();
			for (int i = 0; i < _InitialSegments; i++)
			{
				CreateNewSegment();
			}
		}

		/// <summary>
		/// Creates a new segment, makes buffers available
		/// </summary>
		private void CreateNewSegment()
		{
			byte[] bytes = new byte[m_SegmentChunks * m_ChunkSize];
			m_Segments.Add(bytes);
			for (int i = 0; i < m_SegmentChunks; i++)
			{
				ArraySegment<byte> chunk = new ArraySegment<byte>(bytes, i * m_ChunkSize, m_ChunkSize);
				m_Buffers.Push(chunk);
			}
		}

		/// <summary>
		/// Checks out a buffer from the manager
		/// </summary>
		/// <remarks>
		/// It is the client's responsibility to return the buffer to the manger by
		/// calling <see cref="Checkin"></see> on the buffer
		/// </remarks>
		/// <returns>A <see cref="ArraySegment"></see> that can be used as a buffer</returns>
		public ArraySegment<byte> CheckOut()
		{
			lock (m_LockObject)
			{
				if (m_Buffers.Count == 0)
				{
					CreateNewSegment();
				}
				return m_Buffers.Pop();
			}
		}

		/// <summary>
		/// Returns a buffer to the control of the manager
		/// </summary>
		/// <remarks>
		/// It is the client's responsibility to return the buffer to the manger by
		/// calling <see cref="Checkin"></see> on the buffer
		/// </remarks>
		/// <param name="_Buffer">The <see cref="ArraySegment"></see> to return to the cache</param>
		public void CheckIn(ArraySegment<byte> _Buffer)
		{
			lock (m_LockObject)
			{
				m_Buffers.Push(_Buffer);
			}
		}
	}
}
