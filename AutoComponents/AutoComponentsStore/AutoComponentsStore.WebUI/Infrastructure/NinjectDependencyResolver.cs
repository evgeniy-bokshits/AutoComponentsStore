using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoComponentsStore.Domain.Abstract;
using AutoComponentsStore.Domain.Concreate;
using AutoComponentsStore.Domain.Entities;
using Moq;
using Ninject;

namespace AutoComponentsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // Здесь размещаются привязки

            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
                {
                    new Product { ProductId = 1, Name = "SimCity", Price = 1499, Description = "asdfasdfasdf sadfasdf asdfasdf", Category = "Cat1"},
                    new Product { ProductId = 2, Name = "TITANFALL", Price=2299, Category = "Cat2" },
                    new Product { ProductId = 3, Name = "Battlefield 4", Price=899.4M, Category = "Cat1" },
                    new Product { ProductId = 4, Name = "SimCity4", Price = 1499, Category = "Cat3" },
                    new Product { ProductId = 5, Name = "TITANFALL5", Price=2299, Category = "Cat2" },
                    new Product { ProductId = 6, Name = "Battlefield 4 6", Price=899.4M, Category = "Cat1" },
                    new Product { ProductId = 7, Name = "SimCity7", Price = 1499, Category = "Cat2" },
                    new Product { ProductId = 8, Name = "TITANFALL8", Price=2299, Category = "Cat3" },
                    new Product { ProductId = 9, Name = "Battlefield 4 9", Price=899.4M, Category = "Cat1" }
                });
            kernel.Bind<IAutoComponentsRepository>().ToConstant(mock.Object);

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

        }
    }
}

