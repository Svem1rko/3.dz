using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using drugi.Entities;
using drugi.Models.TodoModels;

namespace drugi.Repositories
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context =  context;
        }

        public TodoViewModel Get(Guid todoId, string userId)
        {
            TodoItem todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoId);

            if (todo != null && todo.UserId != userId)
                throw new TodoAccessDeniedException();

            return new TodoViewModel(todo);
        }

        public void Add(TodoViewModel tvm)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == tvm.Id);

            if (todo == null)
            {
                var tdi = new TodoItem(tvm);

                foreach (TodoItemLabel label in tvm.Labels)
                {
                    bool exists = false;
                    foreach (TodoItemLabel contextLabel in _context.Labels)
                    {
                        exists = contextLabel.Value == label.Value;
                        if (exists) break;
                    }

                    if (!exists)
                    {
                        _context.Labels.Add(label);
                    }

                }

                _context.TodoItems.Add(tdi);
                _context.SaveChanges();
            }
            else
            {
                throw new DuplicateTodoItemException(tvm.Id);
            }
        }

        public bool Remove(Guid todoId, string userId)
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

        public void Update(TodoViewModel todoVM, string userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoVM.Id);

            if (todo != null && todo.UserId != userId)
                throw new TodoAccessDeniedException();

            _context.TodoItems.AddOrUpdate(todo);
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, string userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoId);

            if (todo != null)
            {
                if (todo.UserId != userId)
                    throw new TodoAccessDeniedException();
                else
                {
                    todo.MarkAsCompleted();
                    _context.TodoItems.AddOrUpdate(todo);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public List<TodoViewModel> GetAll(string userId)
        {
            var todoList = _context.TodoItems.
                Where(i => i.UserId == userId).
                OrderByDescending(t => t.DateCreated).
                ToList();

            List<TodoViewModel> list = new List<TodoViewModel>();
            foreach (TodoItem item in todoList)
            {
                list.Add(new TodoViewModel(item));
            }

            return list;
        }

        public List<TodoViewModel> GetActive(string userId)
        {
            var todoList = _context.TodoItems.
                Include(l => l.Labels).
                Where(i => i.UserId == userId).
                Where(t => t.DateCompleted == null).
                OrderBy(x => x.DateDue).
                ToList();

            List<TodoViewModel> list = todoList.Select(t => new TodoViewModel(t)).ToList();

            return list;
        }

        public List<TodoViewModel> GetCompleted(string userId)
        {
            var todoList = _context.TodoItems.
                Include(l => l.Labels).
                Where(i => i.UserId == userId).
                Where(t => t.DateCompleted != null).
                ToList();

            List<TodoViewModel> list = todoList.Select(t => new TodoViewModel(t)).ToList();

            return list;
        }

        public List<TodoViewModel> GetFiltered(Func<TodoItem, bool> filterFunction, string userId)
        {
            var todoList = _context.TodoItems.Where(x => x.UserId == userId).AsEnumerable().Where(filterFunction).ToList();

            List<TodoViewModel> list = todoList.Select(t => new TodoViewModel(t)).ToList();

            return list;
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
