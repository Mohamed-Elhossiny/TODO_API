namespace TODO_API.Global
{
	public class RequestResponse<T>
	{
		public int? ResponseID { get; set; }
		public string? ResponseMessage { get; set; }
		public bool State { get; set; } = false;
        public T? ResponseValue { get; set; }
	}
}
