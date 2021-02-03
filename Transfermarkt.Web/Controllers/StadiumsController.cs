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
    public class StadiumsController : Controller
    {
        private readonly IData<Stadium> _dataStadium;
        private readonly IData<Club> _dataClub;

        public StadiumsController(IData<Stadium> dataStadium, IData<Club> dataClub)
        {
            _dataStadium = dataStadium;
            _dataClub = dataClub;
        }

        [HttpGet]
        public IActionResult Index(int page=1)
        {
            return View(_dataStadium.GetByDetails().ToPagedList(page,5));
        }

        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create(int id, int leagueId)
        {
            //var clubs = _dataClub.Get().Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});
            var club = _dataClub.Get(id);
            if (club == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var stadium = new StadiumInputVM
            {
                ClubId = id,
                ClubName = club.Name,
                LeagueId = leagueId
            };
            return View(stadium);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(StadiumInputVM stadium)
        {
            if (!ModelState.IsValid)
            {
                var club = _dataClub.Get(stadium.ClubId);
                if (club == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                StadiumInputVM viewModel = new StadiumInputVM
                {
                    Name = stadium.Name,
                    DateBuilt = stadium.DateBuilt,
                    Capacity = stadium.Capacity,
                    ClubId = stadium.ClubId,
                    ClubName = stadium.Name
                };
                return View(nameof(Create), viewModel);
            }

            if (stadium.Id == 0)
            {
                var stadiumForDB = new Stadium
                {
                    Capacity = stadium.Capacity,
                    ClubId = stadium.ClubId,
                    DateBuilt = stadium.DateBuilt,
                    Name = stadium.Name
                };
                _dataStadium.Add(stadiumForDB);
            }
            else
            {
                var stadiumInDb = _dataStadium.Get(stadium.Id);
                stadiumInDb.Id = stadium.Id;
                stadiumInDb.Name = stadium.Name;
                stadiumInDb.Capacity = stadium.Capacity;
                stadiumInDb.DateBuilt = stadium.DateBuilt;
                stadiumInDb.ClubId = stadium.ClubId;
                _dataStadium.Update(stadiumInDb);
            }
            return RedirectToAction(nameof(Index),"Clubs", new { id = stadium.LeagueId });
        }

        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var stadiumInDb = _dataStadium.Get(id);

            if (stadiumInDb == null) return RedirectToAction("Error", "Home");

            IEnumerable<SelectListItem> clubsList = _dataClub.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            StadiumInputVM viewModel = new StadiumInputVM
            {
                Name = stadiumInDb.Name,
                ClubId = stadiumInDb.ClubId,
                Capacity = stadiumInDb.Capacity,
                DateBuilt = stadiumInDb.DateBuilt
            };

            return View(nameof(Create), viewModel);
        }
    }
}
