using System;

namespace zad1
{
    class Test
    {

        public static void Main()
        {

            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=TodoSqlRepository;Trusted_Connection=True;MultipleActiveResultSets=true";

            TodoSqlRepository repository = new TodoSqlRepository(new TodoDbContext(connectionString));

            TodoItem todo = new TodoItem("bla bla");

            repository.Add(todo);
            //repository.Add(todo);

            var a = repository.Get(todo.Id, todo.UserId);

            Console.WriteLine(todo.Id == a.Id);
            /*
            repository.Remove(todo.Id,todo.UserId);

            a = repository.Get(todo.Id, todo.UserId);

            if(a == null) 
                Console.WriteLine("IT'S ALIVE!");
            
            todo.Text = "bi bup";

            repository.Update(todo, todo.UserId);

            a = repository.Get(todo.Id, todo.UserId);

            Console.WriteLine(a.Text);

            Console.WriteLine(repository.MarkAsCompleted(todo.Id, todo.UserId));

            var b = repository.GetAll(todo.UserId);
            Console.WriteLine(b.Count);
            */
        }

    }
}