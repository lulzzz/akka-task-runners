using Serilog;
using Akka.Actor;
using System;
using System.Threading;

namespace TaskHost.Actors
{
	using Contracts;

	/// <summary>
	///		Actor that runs tasks.
	/// </summary>
	public sealed class TaskRunner
		: ReceiveActor
	{
		/// <summary>
		///		Create a new task-runner actor.
		/// </summary>
		public TaskRunner()
		{
			Receive<StartTask>(OnStartTask, shouldHandle: null);
		}

		/// <summary>
		///		Called to request that the task runner start a task.
		/// </summary>
		/// <param name="startTask">
		///		The <see cref="StartTask"/> request message.
		/// </param>
		void OnStartTask(StartTask startTask)
		{
			if (startTask == null)
				throw new ArgumentNullException("taskStarted");

			Log.Information("{ActorPath}: Runner is starting task: {What}", Self.Path, startTask.What);
			Thread.Sleep(200);

			Context.System
				.ActorOf<TaskReporter>("TaskReporter")
				.Tell(
					new TaskStarted(
						startTask.What,
						byWho: Self.Path.Name
					)
				);
		}
	}
}
