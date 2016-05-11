using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoComponentsStore.Domain.Abstract;
using AutoComponentsStore.WebUI.Models;

namespace AutoComponentsStore.WebUI.Controllers
{
    public class AutoComponentsController : Controller
    {
        private IAutoComponentsRepository repository;
        public int pageSize = 4;

        // GET: AutoComponents
        public AutoComponentsController(IAutoComponentsRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            //return View(repository.Products
            //    .OrderBy(game => game.ProductId)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize));

            AutoComponentsListViewModel model = new AutoComponentsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(game => game.ProductId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Products.Count() :
                    repository.Products.Count(prod => prod.Category == category)
            },
                CurrentCategory = category
            };
            return View(model);

        }
    }
}