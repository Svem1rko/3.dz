using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using drugi.Entities;

namespace drugi.Models.TodoModels
{
    public class AddTodoViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [Required, MinLength(3)]
        public string Text { get; set; }

        public DateTime? DateCompleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateDue { get; set; }
        
        public string LabelsString { get; set; }

        public List<TodoItemLabel> Labels;

        public AddTodoViewModel(TodoItem todo)
        {
            Id = todo.Id;
            UserId = todo.UserId;
            Text = todo.Text;
            DateCompleted = todo.DateCompleted;
            DateCreated = todo.DateCreated;
            DateDue = todo.DateDue;
        }

        public AddTodoViewModel()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
        }

        public void DetermineLabels()
        {
            Labels = new List<TodoItemLabel>();

            var s = LabelsString.Split(',');

            foreach (string label in s)
            {
                Labels.Add(new TodoItemLabel(label.ToLower()));
            }
        }
    }
}
