using System;

namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a notification that a task was started.
	/// </summary>
	public class TaskStarted
	{
		/// <summary>
		///		Create a new <see cref="TaskStarted"/> notification message.
		/// </summary>
		/// <param name="what">
		///		What is being done.
		/// </param>
		/// <param name="byWho">
		///		The name of the task-runner started to run the task.
		/// </param>
		public TaskStarted(string what, string byWho)
		{
			if (String.IsNullOrWhiteSpace(what))
				throw new ArgumentException("Argument cannot be null, empty, or entirely componsed of whitespace: 'what'.", "what");

			if (String.IsNullOrWhiteSpace(byWho))
				throw new ArgumentException("Argument cannot be null, empty, or entirely componsed of whitespace: 'byWho'.", "byWho");

			What = what;
			ByWho = byWho;
		}

		/// <summary>
		///		What is being done.
		/// </summary>
		public string What
		{
			get;
		}

		/// <summary>
		///		The name of the task-runner started to run the task.
		/// </summary>
		public string ByWho
		{
			get;
		}
	}
}
