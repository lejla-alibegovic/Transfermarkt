using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly IData<League> _dataLeague;
        private readonly IData<Country> _dataCountry;
        private readonly IData<Club> _dataClub;
        private readonly IData<Stadium> _dataStadium;
        private readonly IData<Match> _dataMatch;

        public LeaguesController(IData<League> dataLeague, IData<Country> dataCountry,
            IData<Club> dataClub, IData<Stadium> dataStadium, IData<Match> dataMatch)
        {
            _dataLeague = dataLeague;
            _dataCountry = dataCountry;
            _dataClub = dataClub;
            _dataStadium = dataStadium;
            _dataMatch = dataMatch;
        }

        [HttpGet]
        public IActionResult Index(int id, int page = 1)
        {
            ViewBag.Country = id;
            return View(_dataLeague.GetByDetails().Where(x => x.CountryId == id).ToPagedList(page, 5));
        }

        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create(int id)
        {
            var country = _dataCountry.Get(id);

            LeagueInputVM league = new LeagueInputVM
            {
                CountryId = country.Id,
                NameCountry = country.Name
            };
            return View(league);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(League model)
        {
            if (!ModelState.IsValid)
            {
                var leagueInput = new LeagueInputVM
                {
                    Name = model.Name,
                    CountryId = model.CountryId,
                    NameCountry = model.Name
                };
                return View("Create", leagueInput);
            }

            IEnumerable<League> leagues = _dataLeague.GetByDetails();
            foreach (var item in leagues)
            {
                if (item.Name == model.Name)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            _dataLeague.Add(model);
            return RedirectToAction(nameof(Index), new { id = model.CountryId });
        }

        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Generate(int id)
        {
            var clubs = _dataClub.GetByDetails().Where(x => x.LeagueId == id).ToList();

            foreach (var item in clubs)
            {
                var stadium = _dataStadium.GetByDetails().First(x => x.ClubId == item.Id);
                foreach (var item2 in clubs)
                {
                    if (item.Name != item2.Name)
                    {
                        var match = new Match
                        {
                            HomeClubId = item.Id,
                            AwayClubId = item2.Id,
                            TimePlayed = DateTime.Now,
                            LeagueId = id,
                            StadiumId = stadium.Id
                        };
                        _dataMatch.Add(match);
                    }
                }
            }
            return RedirectToAction(nameof(Index), "Matches");
        }
    }
}