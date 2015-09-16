using System;

namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a request to run a task.
	/// </summary>
	public class RunTask
	{
		/// <summary>
		///		Create a new <see cref="RunTask"/> request message.
		/// </summary>
		/// <param name="what">
		///		What to do.
		/// </param>
		public RunTask(string what)
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
