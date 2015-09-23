namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a request to get oustanding task count.
	/// </summary>
	public sealed class GetOutstandingTaskCount
	{
		/// <summary>
		///		The singleton instance of the "get oustanding task count" message.
		/// </summary>
		public static readonly GetOutstandingTaskCount Instance = new GetOutstandingTaskCount();

		/// <summary>
		///		Create a new "get oustanding task count" request message.
		/// </summary>
		GetOutstandingTaskCount()
		{
		}
	}
}
