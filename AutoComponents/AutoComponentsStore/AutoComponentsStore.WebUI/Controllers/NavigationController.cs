using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoComponentsStore.Domain.Abstract;

namespace AutoComponentsStore.WebUI.Controllers
{
    public class NavigationController : Controller
    {
        private IAutoComponentsRepository repository;

        public NavigationController(IAutoComponentsRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}