using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Framework.Network
{
	public interface IPayloadSerializer<R, T>
	{
		T Deserialize(Stream source);

		void Serialize(Stream target, R instance);
	}
}
