using System;
using System.Collections.Generic;
using System.Text;

namespace LumiSoft.Net.Media.Codec.Audio
{
    /// <summary>
    /// This class is base calss for audio codecs.
    /// </summary>
    public abstract class AudioCodec : Codec
    {
        #region properties implementation

        /// <summary>
        /// Gets sample number of samples in second(hz). 
        /// </summary>
        public abstract int SampleRate
        {
            get;
        }

        /// <summary>
        /// Gets number of bits per sample.
        /// </summary>
        public abstract int BitsPerSample
        {
            get;
        }
        
        #endregion
    }
}
