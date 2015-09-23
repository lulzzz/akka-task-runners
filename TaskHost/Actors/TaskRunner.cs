using Akka.Actor;
using Serilog;
using System;
using System.Threading;

namespace TaskHost.Actors
{
	using Contracts;
	using Utilities;

	/// <summary>
	///		Actor that runs tasks.
	/// </summary>
	public sealed class TaskRunner
		: ReceiveActor
	{
		/// <summary>
		///		Random-number generator used to get pseudo-delay time for each task being "run" by the task runner.
		/// </summary>
		readonly Random _delayGenerator = new Random();

		/// <summary>
		///		The task-controller actor.
		/// </summary>
		ActorSelection _taskController;

		/// <summary>
		///		Create a new task-runner actor.
		/// </summary>
		public TaskRunner()
		{
			Receive<StartTask>(startTask => OnStartTask(startTask));
        }

		/// <summary>
		///		Called when the actor is being started.
		/// </summary>
		protected override void PreStart()
		{
			_taskController = Context.ActorSelection("../..");
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
				throw new ArgumentNullException("startTask");

			Log.Information(
				"{ActorPath}: Runner is starting task: {What}",
				Self.Path.ToUserRelativePath(),
				startTask.What
			);

			// Report to controller.
			_taskController.Tell(
				new TaskStarted(
					startTask.What,
					byWho: Self.Path.Name
				)
			);

			TimeSpan simulatedWorkDelay = GetRandomDelay();
			Log.Information(
				"{ActorPath}: Runner is pausing for {SimulatedDelay}ms",
				Self.Path.ToUserRelativePath(),
				simulatedWorkDelay.TotalMilliseconds
			);
			Thread.Sleep(simulatedWorkDelay);

			Log.Information(
				"{ActorPath}: Runner has completed task: {What} after {SimulatedDelay}ms",
				Self.Path.ToUserRelativePath(),
				startTask.What,
				simulatedWorkDelay.TotalMilliseconds
			);

			// Report to controller.
			_taskController.Tell(
				new TaskCompleted(
					startTask.What,
					byWho: Self.Path.Name,
					runTime: simulatedWorkDelay
				)
			);
		}

		/// <summary>
		///		Get a random span of time to pause in order to simulate task workload.
		/// </summary>
		/// <returns>
		///		A <see cref="TimeSpan"/> between 100ms and 350ms.
		/// </returns>
		TimeSpan GetRandomDelay()
		{
			return TimeSpan.FromMilliseconds(
				_delayGenerator.Next(100, 350)
			);
		}
	}
}
