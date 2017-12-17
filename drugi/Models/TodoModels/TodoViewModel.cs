using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using drugi.Entities;

namespace drugi.Models.TodoModels
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public DateTime? DateCompleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateDue { get; set; }

        public List<TodoItemLabel> Labels;

        public bool IsCompleted => DateCompleted.HasValue;

        public TodoViewModel(TodoItem todoItem)
        {
            this.Id = todoItem.Id;
            this.DateCompleted = todoItem.DateCompleted;
            this.DateCreated = todoItem.DateCreated;
            this.DateDue = todoItem.DateDue;
            this.Text = todoItem.Text;
            this.UserId = todoItem.UserId;
            Labels = todoItem.Labels;
        }

        public TodoViewModel(AddTodoViewModel atvm)
        {
            Id = atvm.Id;
            UserId = atvm.UserId;
            Text = atvm.Text;
            DateCompleted = atvm.DateCompleted;
            DateCreated = DateTime.Now;
            DateDue = atvm.DateDue;
            Labels = atvm.Labels;
        }

        public bool MarkAsCompleted()
        {
            if (IsCompleted) return false;
            DateCompleted = DateTime.Now;
            return true;
        }

        public string DueTime()
        {
            string result = "";

            if (DateDue != null)
            {
                result += " (za ";

                var a = DateTime.Parse(DateDue.ToString());
                var b = DateTime.Parse(DateTime.Now.ToString());

                int c = a.Day - b.Day;

                result += c;
                if (c == 1)
                {
                    result += "dan!)";
                }
                else if (c < 0)
                {
                    result = "The deadline has passed";
                }
                else
                {
                    result += " dana!)";
                }
            }

            return result;
        }

        public string GetDue()
        {
            var a = DateTime.Parse(DateDue.ToString());

            return a.Date.ToString("d");
        }

        public string GetCompleted()
        {
            var a = DateTime.Parse(DateCompleted.ToString());

            return a.Date.ToString("d");
        }

        public string PrintLabels()
        {
            string result = "";

            if (Labels == null || Labels.Count == 0)
                result = "No labels";
            else
            {
                foreach (var label in Labels)
                {
                    result += label.Value;
                    result += ", ";
                }
            }
            return result;
        }
    }
}
