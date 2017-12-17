using System;
using System.Collections.Generic;
using drugi.Entities;
using drugi.Models.TodoModels;

namespace drugi.Repositories
{
    public interface ITodoRepository
    {
        TodoViewModel Get(Guid todoId, string userId);

        void Add(TodoViewModel todoItem);

        bool Remove(Guid todoId, string userId);

        void Update(TodoViewModel todoVM, string userId);

        bool MarkAsCompleted(Guid todoId, string userId);

        List<TodoViewModel> GetAll(string userId);

        List<TodoViewModel> GetActive(string userId);

        List<TodoViewModel> GetCompleted(string userId);

        List<TodoViewModel> GetFiltered(Func<TodoItem, bool> filterFunction, string userId);
    }
}
