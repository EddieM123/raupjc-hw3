using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zad1;

namespace TodoSqlRepository
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(string cnnstr) : base(cnnstr)
        {

        }

        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoItemLabels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItem>().Property(s => s.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(s => s.UserId).IsRequired();

            modelBuilder.Entity<TodoItemLabel>().HasKey(s => s.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(s => s.Value).IsRequired();


            modelBuilder.Entity<TodoItem>().HasMany(s => s.Labels).WithMany(m => m.LabelTodoItems);
        }
    }
}
