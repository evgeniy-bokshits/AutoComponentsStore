using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoComponentsStore.Domain.Abstract;
using AutoComponentsStore.Domain.Entities;
using AutoComponentsStore.WebUI.Controllers;
using AutoComponentsStore.WebUI.HtmlHelpers;
using AutoComponentsStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoComponentsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1"},
                new Product { ProductId = 2, Name = "Игра2"},
                new Product { ProductId = 3, Name = "Игра3"},
                new Product { ProductId = 4, Name = "Игра4"},
                new Product { ProductId = 5, Name = "Игра5"}
            });
            AutoComponentsController controller = new AutoComponentsController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            AutoComponentsListViewModel result = (AutoComponentsListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Product> games = result.Products.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
                new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
                new Product { ProductId = 3, Name = "Игра3", Category="Cat2"},
                new Product { ProductId = 4, Name = "Игра4", Category="Cat1"},
                new Product { ProductId = 5, Name = "Игра5", Category="Cat1"}
            });
            AutoComponentsController controller = new AutoComponentsController(mock.Object);
            controller.pageSize = 3;

            // Act
            AutoComponentsListViewModel result
                = (AutoComponentsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
                new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
                new Product { ProductId = 3, Name = "Игра3", Category="Cat2"},
                new Product { ProductId = 4, Name = "Игра4", Category="Cat1"},
                new Product { ProductId = 5, Name = "Игра5", Category="Cat1"}
            });

            AutoComponentsController controller = new AutoComponentsController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Product> result = ((AutoComponentsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра3" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация (arrange)
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
                new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
                new Product { ProductId = 3, Name = "Игра3", Category="Cat2"},
                new Product { ProductId = 4, Name = "Игра4", Category="Cat1"},
                new Product { ProductId = 5, Name = "Игра5", Category="Cat1"}
            });

            // Организация - создание контроллера
            NavigationController target = new NavigationController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0], "Cat1");
            Assert.AreEqual(results[1], "Cat2");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
                new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
                new Product { ProductId = 3, Name = "Игра3", Category="Cat2"},
                new Product { ProductId = 4, Name = "Игра4", Category="Cat1"},
                new Product { ProductId = 5, Name = "Игра5", Category="Cat1"}
            });

            // Организация - создание контроллера
            NavigationController target = new NavigationController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Cat2";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            Mock<IAutoComponentsRepository> mock = new Mock<IAutoComponentsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { ProductId = 1, Name = "Игра1", Category="Cat1"},
                new Product { ProductId = 2, Name = "Игра2", Category="Cat2"},
                new Product { ProductId = 3, Name = "Игра3", Category="Cat2"},
                new Product { ProductId = 4, Name = "Игра4", Category="Cat1"},
                new Product { ProductId = 5, Name = "Игра5", Category="Cat3"}
            });

            // Организация - создание контроллера
            AutoComponentsController controller = new AutoComponentsController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((AutoComponentsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((AutoComponentsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((AutoComponentsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((AutoComponentsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

    }
}
