using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO_API.DTOs;
using TODO_API.Global;
using TODO_API.Models;
using Task = TODO_API.Models.Task;

namespace TODO_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TasksController : ControllerBase
	{
		private readonly TODOContext _context;

		public TasksController(TODOContext context)
		{
			_context = context;
		}

		// GET: api/Tasks
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Task>>> GetTask()
		{
			var response = new RequestResponse<IEnumerable<Task>>() { ResponseID = 0,ResponseMessage = "No Tasks"};
			if (_context.Task == null)
			{
				return Ok(response);
			}
			response.ResponseValue = await _context.Task.ToListAsync();
			response.ResponseID = 1;
			return Ok(response);
		}

		// GET: api/Tasks/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Task>> GetTask(int id)
		{
			var response = new RequestResponse<Task>() { ResponseID = 0, ResponseMessage = "No Tasks Found" };
			if (_context.Task == null)
			{
				return Ok(response);
			}
			var task = await _context.Task.FindAsync(id);

			if (task == null)
			{
				response.ResponseMessage = $"No Task found with this id {id}";
				return Ok(response);
			}
			response.ResponseValue = task;
			response.ResponseID = 1;
			return Ok(response);
		}

		// PUT: api/Tasks/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTask(int id, UpdateTaskDto dto)
		{
			var response = new RequestResponse<Task>() { ResponseID = 0, ResponseMessage = "No Tasks Found" };
			if (id == 0)
			{
				response.ResponseMessage = "Please enter valid id";
				return Ok(response);
			}

			var task = await _context.Task.FindAsync(id);

			if (task == null)
			{
				response.ResponseMessage = $"No Task found with this id {id}";
				return Ok(response);
			}

			task.Description = dto.Description;
			task.Title = dto.Title;
			task.IsComplete = dto.IsComplete;

			_context.Entry(task).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TaskExists(id))
				{
					return Ok(response);
				}
				else
				{
					response.ResponseMessage = "Error in updating task";
					return Ok(response);
				}
			}
			response.ResponseValue = task;
			response.ResponseID = 1;
			return Ok(response);
		}

		// POST: api/Tasks
		[HttpPost]
		public async Task<ActionResult<Task>> PostTask(AddTaskDto dto)
		{
			var response = new RequestResponse<Task>() { ResponseID = 0 };
			if (_context.Task == null)
			{
				return Ok(response);
			}
			var task = new Task
			{
				Title = dto.Title,
				Description = dto.Description,
				IsComplete = dto.IsComplete
			};
			_context.Task.Add(task);
			await _context.SaveChangesAsync();

			response.ResponseID = 1;
			response.ResponseValue = task;

			return Ok(response);
			//return CreatedAtAction("GetTask", new { id = task.Id }, task);
		}

		// DELETE: api/Tasks/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTask(int id)
		{
			var response = new RequestResponse<Task>() { ResponseID = 0, ResponseMessage = "No Task Found" };
			if (_context.Task == null)
			{
				return Ok(response);
			}
			var task = await _context.Task.FindAsync(id);
			if (task == null)
			{
				return Ok(response);
			}

			_context.Task.Remove(task);
			await _context.SaveChangesAsync();
			response.ResponseID = 1;
			response.ResponseMessage = "Deleted Success";
			return Ok(response);
		}

		private bool TaskExists(int id)
		{
			return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
