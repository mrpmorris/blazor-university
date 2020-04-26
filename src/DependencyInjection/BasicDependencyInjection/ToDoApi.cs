using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasicDependencyInjection
{
	public interface IToDoApi
	{
		Task<IEnumerable<ToDo>> GetToDosAsync();
	}

	public class ToDoApi : IToDoApi
	{
		private readonly IEnumerable<ToDo> Data;

		public ToDoApi()
		{
			Data = new ToDo[]
			{
				new ToDo { Id = 1, Title = "To do 1", Completed = true},
				new ToDo { Id = 2, Title = "To do 2", Completed = false},
				new ToDo { Id = 3, Title = "To do 3", Completed = false},
			};
		}

		public Task<IEnumerable<ToDo>> GetToDosAsync() => Task.FromResult(Data);
	}
}
