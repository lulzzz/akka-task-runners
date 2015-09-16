using Akka;
using Akka.Actor;
using Akka.Configuration;
using Serilog;
using System;
using System.Threading;

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
				// TODO: Define actor types.
			}
		}

		/// <summary>
		///		Enable the global logger.
		/// </summary>
		static void ConfigureLogging()
		{
			Log.Logger =
				new LoggerConfiguration()
					.MinimumLevel.Information()
					.WriteTo.LiterateConsole()
					.Enrich.WithMachineName()
					.Enrich.WithProcessId()
					.Enrich.WithThreadId()
					.CreateLogger();
		}
	}
}
