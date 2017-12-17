using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using zad1;

namespace zad1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }


        public TodoItem Get(Guid todoId, Guid userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoId);

            if (todo != null && todo.UserId != userId)
                throw new TodoAccessDeniedException();

            return todo;
        }

        public void Add(TodoItem todoItem)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoItem.Id);
            
            if (todo == null)
            {
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
            }
            else
            {
                throw new DuplicateTodoItemException(todoItem.Id);
            }
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todo = _context.TodoItems.First(t => t.Id == todoId);

            if (todo.UserId != userId)
            {
                throw new TodoAccessDeniedException();
            }
            else
            {
                _context.TodoItems.Remove(todo);
                _context.SaveChanges();
                return true;
            }
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoItem.Id);

            if (todo != null && todo.UserId != userId)
                throw new TodoAccessDeniedException();

            _context.TodoItems.AddOrUpdate(todoItem);
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == userId);

            if (todo != null)
            {
                if (todo.UserId != userId)
                    throw new TodoAccessDeniedException();
                else return todo.MarkAsCompleted();
            }
            return false;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.
                Where(i => i.UserId == userId).
                OrderByDescending(t => t.DateCreated).
                ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.
                Where(i => i.UserId == userId).
                Where(t => t.DateCompleted == null).
                ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.
                Where(i => i.UserId == userId).
                Where(t => t.DateCompleted != null).
                ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.
                Where(i => i.UserId == userId).AsEnumerable().
                Where(filterFunction).
                ToList();
        }
    }

    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(Guid id) : base($"duplicate id {id}") { }
    }

    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException() : base("Current user is not the ower of the Todo item")
        {
        }
    }
}
