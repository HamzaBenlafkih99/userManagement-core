using Microsoft.EntityFrameworkCore;
using Please.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Please
{
    public class ConnectionDbClass : DbContext
    {
        public ConnectionDbClass(DbContextOptions<ConnectionDbClass> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
