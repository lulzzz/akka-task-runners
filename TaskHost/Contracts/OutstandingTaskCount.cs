namespace TaskHost.Contracts
{
	/// <summary>
	///		Message that represents a response from the task controller that indicates its oustanding task count.
	/// </summary>
	public sealed class OutstandingTaskCount
	{
		/// <summary>
		///		Create a new "oustanding task count" response message.
		/// </summary>
		/// <param name="count">
		///		The controller's outstanding task count.
		/// </param>
		public OutstandingTaskCount(int count)
		{
			Count = count;
		}

		/// <summary>
		///		The controller's outstanding task count.
		/// </summary>
		public int Count
		{
			get;
		}
	}
}
