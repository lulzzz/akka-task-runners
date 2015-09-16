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
				Log.Verbose("About to get handle to actor...");
				IActorRef taskRunner = actorSystem.ActorOf<TaskRunner>("Runner1");

				Log.Verbose("About to invoke actor...");
				taskRunner.Tell(
					new StartTask("do something")
				);

				Log.Verbose("Sleeping for 2 seconds...");
				Thread.Sleep(2000);
				Log.Verbose("Done.");
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
