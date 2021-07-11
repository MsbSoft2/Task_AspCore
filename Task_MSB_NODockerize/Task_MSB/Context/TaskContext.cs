using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_MSB.Models;

namespace Task_MSB.Context
{
    public class TaskContext:DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
