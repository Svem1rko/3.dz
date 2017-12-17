using System.Data.Entity;

namespace zad1
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> Labels { get; set; }

        public TodoDbContext(string cnnstr) : base(cnnstr)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoItem>().Property(t => t.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasRequired(t => t.Labels);
            modelBuilder.Entity<TodoItem>().HasMany(t => t.Labels).WithMany(l => l.LabelTodoItems);

            modelBuilder.Entity<TodoItemLabel>().HasKey(l => l.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(l => l.Value).IsRequired();
            modelBuilder.Entity<TodoItemLabel>().HasRequired(l => l.LabelTodoItems);
        }
    }
}
