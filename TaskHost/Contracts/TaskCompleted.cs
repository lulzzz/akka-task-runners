using System;

namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a notification that a task was completed.
	/// </summary>
	public class TaskCompleted
	{
		/// <summary>
		///		Create a new <see cref="TaskCompleted"/> notification message.
		/// </summary>
		/// <param name="what">
		///		What is being done.
		/// </param>
		/// <param name="byWho">
		///		The name of the task-runner completed to run the task.
		/// </param>
		/// <param name="runTime">
		///		The span of time for which the task was running.
		/// </param>
		public TaskCompleted(string what, string byWho, TimeSpan runTime)
		{
			if (String.IsNullOrWhiteSpace(what))
				throw new ArgumentException("Argument cannot be null, empty, or entirely componsed of whitespace: 'what'.", "what");

			if (String.IsNullOrWhiteSpace(byWho))
				throw new ArgumentException("Argument cannot be null, empty, or entirely componsed of whitespace: 'byWho'.", "byWho");

			What = what;
			ByWho = byWho;
			RunTime = runTime;
		}

		/// <summary>
		///		What is being done.
		/// </summary>
		public string What
		{
			get;
		}

		/// <summary>
		///		The name of the task-runner completed to run the task.
		/// </summary>
		public string ByWho
		{
			get;
		}

		/// <summary>
		///		The span of time for which the task was running.
		/// </summary>
		public TimeSpan RunTime
		{
			get;
		}
    }
}
