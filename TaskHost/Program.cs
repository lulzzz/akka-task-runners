using Akka.Actor;
using Serilog;
using System;
using System.Threading;
using TaskHost.Actors;
using TaskHost.Contracts;

namespace TaskHost
{
	/// <summary>
	///		The host for the task-runner actors.
	/// </summary>
	class Program
	{
		/// <summary>
		///		The main program entry-point.
		/// </summary>
		static void Main()
		{
			ConfigureLogging();

			SynchronizationContext.SetSynchronizationContext(
				new SynchronizationContext()
			);

            try
			{
				ActorTest();
			}
			catch (AggregateException eUnexpected)
			{
				eUnexpected.Flatten().Handle(
					exception =>
					{
						Log.Error(
							exception,
							"Unexpected error: {ErrorMessage}",
							exception.Message
						);

						return true;
					}
				);
			}
			catch (Exception eUnexpected)
			{
				Log.Error(
					eUnexpected,
					"Unexpected error: {ErrorMessage}",
					eUnexpected.Message
				);
			}
		}

		/// <summary>
		///		Quick test of actor usage.
		/// </summary>
		public static void ActorTest()
		{
			using (ActorSystem actorSystem = ActorSystem.Create("TaskRunner"))
			{
				IActorRef controller = actorSystem.ActorOf<TaskController>("TaskController");

				const int taskCount = 20;

				Log.Verbose("{ActorName}: Calling controller to run {TaskCount} tasks...", nameof(Program), taskCount);
				for (int taskId = 1; taskId <= taskCount; taskId++)
				{
					controller.Tell(
						new RunTask("do something #" + taskId)
					);
				}

				Log.Verbose(
					"{ActorName}: Stopping controller...",
					nameof(Program)
				);

				controller.Tell(Shutdown.Instance);

				// Wait for task controller to observe the completion of all outstanding task.
				using (Inbox inbox = Inbox.Create(actorSystem))
				{
					while (true)
					{
						inbox.Send(controller, GetOutstandingTaskCount.Instance);
						OutstandingTaskCount outstandingTaskCount = (OutstandingTaskCount)inbox.ReceiveWhere(
							message => message is OutstandingTaskCount
						);

						if (outstandingTaskCount.Count == 0)
							break;

						Thread.Sleep(
							TimeSpan.FromMilliseconds(500)
						);
					}
				}
				
				Log.Verbose("{ActorName}: Done.", nameof(Program));
			}
		}

		/// <summary>
		///		Enable the global logger.
		/// </summary>
		static void ConfigureLogging()
		{
			Log.Logger =
				new LoggerConfiguration()
					.MinimumLevel.Verbose()
					.WriteTo.LiterateConsole()
					.Enrich.WithMachineName()
					.Enrich.WithProcessId()
					.Enrich.WithThreadId()
					.CreateLogger();
		}
	}
}
