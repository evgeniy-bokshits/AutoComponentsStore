using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoComponentsStore.Domain.Entities;

namespace AutoComponentsStore.Domain.Concreate
{
    public class EFDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
