using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class CitiesController : Controller
    {
        private readonly IData<City> _dataCity;
        private readonly IData<Country> _dataCountry;


        public CitiesController(IData<City> dataCity, IData<Country> dataCountry,IMapper mapper)
        {
            _dataCity = dataCity;
            _dataCountry = dataCountry;
        }

        //list of cities
        public IActionResult Index(int page=1)
        {
            return View(_dataCity.GetByDetails().OrderBy(x=>x.Name).ToPagedList(page,5));
        }

        [HttpGet]
        [Authorize(Roles=RolesGlobal.Admin)]
        //creating city view model and passing it to view
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _dataCountry.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            CityInputVM city = new CityInputVM
            {
                Countries = list.ToList()
            };

            return View(city);
        }

        //adding city to database and validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(City model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> list = _dataCountry.GetByDetails().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                var cityInput = new CityInputVM
                {
                    Name = model.Name,
                    PostalCode = model.PostalCode,
                    CountryId = model.CountryId,
                    Countries = list.ToList()
                };
                return View("Create", cityInput);
            }

            IEnumerable<City> cities = _dataCity.GetByDetails();
            foreach (var item in cities)
            {
                if (item.Name == model.Name)
                {
                    return RedirectToAction(nameof(Index), "Home"); 
                }
            }

            _dataCity.Add(model);
            return RedirectToAction("Create","Players");
        }
    }
}