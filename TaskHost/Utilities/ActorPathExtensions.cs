using Akka.Actor;
using System;

namespace TaskHost.Utilities
{
	/// <summary>
	///		Extension methods for working with actor paths.
	/// </summary>
	public static class ActorPathExtensions
	{
		/// <summary>
		///		Convert the <see cref="ActorPath"/> to a user-relative string (e.g. without the "/user/" prefix).
		/// </summary>
		/// <param name="actorPath">
		///		The actor path.
		/// </param>
		/// <returns>
		///		The path, as a string, without address or user prefix.
		/// </returns>
		public static string ToUserRelativePath(this ActorPath actorPath)
		{
			if (actorPath == null)
				throw new ArgumentNullException("actorPath");

			return actorPath.ToStringWithoutAddress().Replace("/user/", String.Empty);
        }
	}
}
