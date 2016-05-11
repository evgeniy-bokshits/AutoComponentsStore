using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoComponentsStore.Domain.Entities;

namespace AutoComponentsStore.Domain.Abstract
{
    public interface IAutoComponentsRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
