using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoComponentsStore.Domain.Abstract;
using AutoComponentsStore.Domain.Entities;

namespace AutoComponentsStore.Domain.Concreate
{
    public class EFAutoComponentsRepository: IAutoComponentsRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products => context.Products;

    }
}
