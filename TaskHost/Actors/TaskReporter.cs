using Serilog;
using Akka.Actor;

namespace TaskHost.Actors
{
	using Contracts;

	/// <summary>
	///		Actor that reports on tasks.
	/// </summary>
	public sealed class TaskReporter
		: ReceiveActor
	{
		/// <summary>
		///		Create a new task-reporter actor.
		/// </summary>
		public TaskReporter()
		{
			Receive<TaskStarted>(
				taskStarted => Log.Information("{ActorPath}: Task started by task-runner '{TaskRunnerName}': {What}", Self.Path, taskStarted.ByWho, taskStarted.What)		
			);
		}
	}
}
