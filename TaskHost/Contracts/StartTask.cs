using System;

namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a request to start a task.
	/// </summary>
	public class StartTask
	{
		/// <summary>
		///		Create a new <see cref="StartTask"/> request message.
		/// </summary>
		/// <param name="what">
		///		What to do.
		/// </param>
		public StartTask(string what)
		{
			if (String.IsNullOrWhiteSpace(what))
				throw new ArgumentException("Argument cannot be null, empty, or entirely componsed of whitespace: 'what'.", "what");

			What = what;
		}

		/// <summary>
		///		What to do.
		/// </summary>
		public string What
		{
			get;
		}
	}
}
