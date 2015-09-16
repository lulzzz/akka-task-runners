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
		///		Create a new task-runner actor.
		/// </summary>
		public TaskRunner()
		{
			Receive<StartTask>(startTask => OnStartTask(startTask));
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

			Log.Information(
				"{ActorPath}: Runner is starting task: {What}",
				Self.Path.ToUserRelativePath(),
				startTask.What
			);

			// Report to controller.
			Context.Parent.Tell(
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
			Context.Parent.Tell(
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
