﻿namespace TODO_API.DTOs
{
	public class UpdateTaskDto
	{
		public string? Title { get; set; }
		public string? Description { get; set; }
		public bool IsComplete {  get; set; }
	}
}
