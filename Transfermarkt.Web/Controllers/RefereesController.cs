using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class RefereesController : Controller
    {
        private readonly IData<Referee> _dataReferee;
        private readonly IData<Country> _dataCountry;
        public RefereesController(IData<Referee>dataReferee,IData<Country>dataCountry)
        {
            _dataReferee = dataReferee;
            _dataCountry = dataCountry;
        }
        [HttpGet]
        public IActionResult Index(int page=5)
        {
            return View(_dataReferee.GetByDetails().ToPagedList(page,5));
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> countriesList = _dataCountry.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var referee = new RefereeInputVM
            {
                Countries = countriesList.ToList()
            };
            return View(referee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Referee referee)
        {
            if(!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> countriesList = _dataCountry.GetByDetails().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                RefereeInputVM viewmodel = new RefereeInputVM
                {
                    Countries = countriesList.ToList(),
                    FirstName = referee.FirstName,
                    MiddleName = referee.MiddleName,
                    LastName = referee.LastName,
                    CountryId = referee.CountryId
                };
                return View(nameof(Create), viewmodel);
            }
            if (referee.Id == 0)
                _dataReferee.Add(referee);
            else
            {
                var refereeInDB = _dataReferee.Get(referee.Id);
                refereeInDB.Id = referee.Id;
                refereeInDB.FirstName = referee.FirstName;
                refereeInDB.MiddleName = referee.MiddleName;
                refereeInDB.LastName = referee.LastName;
                refereeInDB.CountryId = referee.CountryId;
                _dataReferee.Update(refereeInDB);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var refereeInDB = _dataReferee.Get(id);
            if (refereeInDB == null)
                return RedirectToAction("Error", "Home");
            IEnumerable<SelectListItem> countriesList = _dataCountry.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            RefereeInputVM viewmodel = new RefereeInputVM
            {
                Countries = countriesList.ToList(),
                FirstName = refereeInDB.FirstName,
                MiddleName = refereeInDB.MiddleName,
                LastName = refereeInDB.LastName,
                CountryId = refereeInDB.CountryId
            };
            return View(nameof(Create), viewmodel);
        }
        public IActionResult Details(int? id)
        {
            Referee referee = new Referee();
            if(id.HasValue && id != 0)
            {
                var tempRef= _dataReferee.Get(id.Value);
                referee.Id = tempRef.Id;
                referee.FirstName = tempRef.FirstName;
                referee.MiddleName = tempRef.MiddleName;
                referee.LastName = tempRef.LastName;
                referee.CountryId = tempRef.CountryId;
            }
            return View(referee);
        }
     
    }
}