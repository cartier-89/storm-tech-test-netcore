using System;

namespace Todo.Exceptions
{
    public class TodoItemNotFoundException : Exception
    {
        public int Id { get; }

        public TodoItemNotFoundException(int id) : base($"TodoItem of id {id} could not be found.")
        {
            Id = id;
        }
    }
}
