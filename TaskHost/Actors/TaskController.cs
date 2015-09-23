using Akka.Actor;
using Akka.Routing;
using Serilog;
using System;

namespace TaskHost.Actors
{
	using Contracts;
	using Utilities;

	/// <summary>
	///		Actor that receives tasks and distributes them to workers.
	/// </summary>
	public sealed class TaskController
		: ReceiveActor
	{
		/// <summary>
		///		The maximum number of task-runners that the controller should use.
		/// </summary>
		public static readonly int MaximumTaskRunnerCount = 5;

		/// <summary>
		///		The router for the task-runners.
		/// </summary>
		IActorRef _taskRunnerRouter;

		/// <summary>
		///		Create a new task-controller actor.
		/// </summary>
		public TaskController()
		{
			Receive<RunTask>(
				runTask => OnRunTask(runTask)
			);
			Receive<TaskStarted>(
				taskStarted => OnTaskStarted(taskStarted)
			);
			Receive<TaskCompleted>(
				taskCompleted => OnTaskCompleted(taskCompleted)
			);
		}

		/// <summary>
		///		Called when the task-controller actor is started.
		/// </summary>
		protected override void PreStart()
		{
			Log.Information(
				"{ActorPath}: Configuring pool of {TaskRunnerCount} child task-runners...",
				Self.Path.ToUserRelativePath(),
				MaximumTaskRunnerCount
			);

			Props taskRunnerPool =
				Props.Create<TaskRunner>().WithRouter(
					new RoundRobinPool(5)
				);
			_taskRunnerRouter = Context.ActorOf(taskRunnerPool, "TaskRunners");
			
			Log.Information(
				"{ActorPath}: {TaskRunnerCount} child task-runners configured.",
				Self.Path.ToUserRelativePath(),
				MaximumTaskRunnerCount
			);
		}

		/// <summary>
		///		Called to request that the task controller run a task.
		/// </summary>
		/// <param name="runTask">
		///		The <see cref="RunTask"/> request message.
		/// </param>
		void OnRunTask(RunTask runTask)
		{
			if (runTask == null)
				throw new ArgumentNullException("runTask");

			Log.Information(
				"{ActorPath}: Received request to run task: {What}",
				Self.Path.ToUserRelativePath(),
				runTask.What
			);

			_taskRunnerRouter.Tell(
				new StartTask(runTask.What)
			);
		}

		/// <summary>
		///		Called to notify task controller that a task has started.
		/// </summary>
		/// <param name="taskStarted">
		///		The <see cref="TaskStarted"/> notification message.
		/// </param>
		void OnTaskStarted(TaskStarted taskStarted)
		{
			if (taskStarted == null)
				throw new ArgumentNullException("taskStarted");

			Log.Information(
				"{ActorPath}: Task started by task-runner '{TaskRunnerName}': {What}",
				Self.Path.ToUserRelativePath(),
				taskStarted.ByWho,
				taskStarted.What
			);
        }

		/// <summary>
		///		Called to notify task controller that a task has been completed.
		/// </summary>
		/// <param name="taskCompleted">
		///		The <see cref="TaskCompleted"/> notification message.
		/// </param>
		void OnTaskCompleted(TaskCompleted taskCompleted)
		{
			if (taskCompleted == null)
				throw new ArgumentNullException("taskCompleted");

			Log.Information(
				"{ActorPath}: Task completed by task-runner '{TaskRunnerName}' in {RunTime}: {What}",
				Self.Path.ToUserRelativePath(),
				taskCompleted.ByWho,
				taskCompleted.RunTime,
				taskCompleted.What
			);
		}
	}
}
