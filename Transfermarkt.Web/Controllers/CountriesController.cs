using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Transfermarkt.Web.Extensions;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Models.Enums;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly IData<Country> _dataCountry;

        public CountriesController(IData<Country> dataCountry)
        {
            _dataCountry = dataCountry;
        }

        //list of countries
        public IActionResult Index(int page = 1)
        {
            return View(_dataCountry.GetByDetails().OrderBy(x => x.Name).ToPagedList(page, 5));
        }

        //creating country
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create()
        {
            var list = Globals.ToPairList<Confederations>(typeof(Confederations));

            CountryInputVM model = new CountryInputVM
            {
                Confederations = list.Select(x => new SelectListItem(x.Value, x.Key.ToString()))
            };
            return View(model);
        }

        //adding country to database and validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Country model)
        {
            if (!ModelState.IsValid)
            {
                var list = Globals.ToPairList<Confederations>(typeof(Confederations));

                CountryInputVM viewModel = new CountryInputVM
                {
                    Name = model.Name,
                    Code = model.Code,
                    Confederations = list.Select(x => new SelectListItem(x.Value, x.Key.ToString()))
                };
                return View("Create", viewModel);
            }

            List<Country> countries = _dataCountry.GetByDetails().ToList();
            foreach (var item in countries)
            {
                if (item.Name == model.Name)
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            _dataCountry.Add(model);
            return RedirectToAction("Create","Cities");
        }
    }
}