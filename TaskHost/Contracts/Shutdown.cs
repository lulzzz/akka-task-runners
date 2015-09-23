using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a request to shut down.
	/// </summary>
	public sealed class Shutdown
	{
		/// <summary>
		///		The singleton instance of the shutdown message.
		/// </summary>
		public static readonly Shutdown Instance = new Shutdown();

		/// <summary>
		///		Create a new shut-down request message.
		/// </summary>
		Shutdown()
		{
		}
	}
}
