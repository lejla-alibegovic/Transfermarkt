using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.ViewModels;
using Transfermarkt.Web.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IData<Club> _dataClub;
        private readonly IData<City> _dataCity;
        private readonly IData<League> _dataLeague;
        private readonly IImagesService _imagesService;

        public ClubsController(IData<Club> dataClub, IData<City> dataCity, 
            IImagesService imagesService,
           IData<League> dataLeague)
        {
            _dataClub = dataClub;
            _dataCity = dataCity;
            _imagesService = imagesService;
            _dataLeague = dataLeague;
        }
        [HttpGet]
        public IActionResult Index(int id, int page=1)
        {
            return View(_dataClub.GetByDetails().Where(x => x.LeagueId == id).ToPagedList(page,5));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> citiesList = _dataCity.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            IEnumerable<SelectListItem> leaguesList = _dataLeague.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            var club = new ClubInputVM
            {
                Cities = citiesList.ToList(),
                Leagues = leaguesList.ToList()
            };
            return View(club);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ClubInputVM model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> citiesList = _dataCity.GetByDetails().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

                IEnumerable<SelectListItem> leaguesList = _dataLeague.GetByDetails().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

                ClubInputVM viewmodel = new ClubInputVM
                {
                    Cities = citiesList.ToList(),
                    Name = model.Name,
                    Abbreviation = model.Abbreviation,
                    Nickname = model.Nickname,
                    Founded = model.Founded,
                    Logo = model.Logo,
                    MarketValue = model.MarketValue,
                    CityId = model.CityId,
                    LeagueId=model.LeagueId,
                    Leagues= leaguesList.ToList()
                };
                return View(nameof(Create), viewmodel);
            }

            if (model.Id == 0)
            {
                string uniqueFileName = null;
                if (model.Logo != null)
                {
                    uniqueFileName = _imagesService.Upload(model.Logo, model.Logo.FileName);
                }
                Club club = new Club
                {
                    Name = model.Name,
                    Abbreviation = model.Abbreviation,
                    CityId = model.CityId,
                    Founded = model.Founded,
                    MarketValue = model.MarketValue,
                    Nickname = model.Nickname,
                    Logo = uniqueFileName,
                    LeagueId=model.LeagueId
                };
                _dataClub.Add(club);
                return RedirectToAction(nameof(Create),"Stadiums",new { id = club.Id, leagueId = model.LeagueId });
            }
            else
            {
                var clubInDB = _dataClub.Get(model.Id);
                clubInDB.Id = model.Id;
                clubInDB.Name = model.Name;
                clubInDB.Abbreviation = model.Abbreviation;
                clubInDB.Nickname = model.Nickname;
                clubInDB.Founded = model.Founded;
                clubInDB.CityId = model.CityId;
                clubInDB.LeagueId = model.LeagueId;
                clubInDB.MarketValue = model.MarketValue;

                clubInDB.Logo = _imagesService.Upload(model.Logo, model.Logo.FileName, true);

                _dataClub.Update(clubInDB);
            }
            return RedirectToAction(nameof(Index), new { id = model.LeagueId });
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var clubInDB = _dataClub.Get(id);
            if (clubInDB == null)
                return RedirectToAction("Error", "Home");

            IEnumerable<SelectListItem> citiesList = _dataCity.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            IEnumerable<SelectListItem> leaguesList = _dataLeague.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ClubInputVM viewmodel = new ClubInputVM
            {
                Cities = citiesList.ToList(),
                Name = clubInDB.Name,
                Abbreviation = clubInDB.Abbreviation,
                Nickname = clubInDB.Nickname,
                Founded = clubInDB.Founded,
                MarketValue = clubInDB.MarketValue,
                CityId = clubInDB.CityId,
                LeagueId=clubInDB.LeagueId,
                Leagues=leaguesList.ToList()
            };
            return View(nameof(Create), viewmodel);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            Club club = new Club();
            if (id.HasValue && id != 0)
            {
                var tempClub = _dataClub.Get(id.Value);
                club.Id = tempClub.Id;
                club.Name = tempClub.Name;
                club.Abbreviation = tempClub.Abbreviation;
                club.Nickname = tempClub.Nickname;
                club.MarketValue = tempClub.MarketValue;
                club.Logo = tempClub.Logo;
                club.MarketValue = tempClub.MarketValue;
                club.Founded = tempClub.Founded;
                club.CityId = tempClub.CityId;
                club.LeagueId = tempClub.LeagueId;
            }
            return View(club);
        }
    }

}
