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
    public class CoachesController : Controller
    {
        private readonly IData<Coach> _dataCoach;
        private readonly IData<Club> _dataClub;
        private readonly IData<CoachClub> _dataCoachClub;
        public CoachesController(IData<Coach> dataCoach, IData<Club> dataClub, IData<CoachClub> dataCoachClub)
        {
            _dataCoach = dataCoach;
            _dataClub = dataClub;
            _dataCoachClub = dataCoachClub;
        }
        [HttpGet]
        public IActionResult Index(int page=1)
        {
            return View(_dataCoach.GetByDetails().ToPagedList(page,5));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create()
        {
            var coach = new Coach();
            return View(coach);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Coach coach)
        {
            if (!ModelState.IsValid)
            {
                Coach model = new Coach
                {
                    FirstName=coach.FirstName,
                    MiddleName=coach.MiddleName,
                    LastName=coach.LastName,
                    Birthdate=coach.Birthdate
                };
                return View(nameof(Create), model);
            }
            if (coach.Id == 0)
            {
                var model = new Coach
                {
                    LastName=coach.LastName,
                    MiddleName=coach.MiddleName,
                    FirstName=coach.FirstName,
                    Birthdate=coach.Birthdate
                };
                _dataCoach.Add(model);
            }
            else
            {
                var coachInDB = _dataCoach.Get(coach.Id);
                coachInDB.Id = coach.Id;
                coachInDB.FirstName = coach.FirstName;
                coachInDB.MiddleName = coach.MiddleName;
                coachInDB.LastName = coach.LastName;
                coachInDB.Birthdate = coach.Birthdate;
                _dataCoach.Update(coachInDB);
            }
            return RedirectToAction(nameof(Index));  
        } 
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var coachInDB = _dataCoach.Get(id);
            if (coachInDB == null)
                return RedirectToAction("Error", "Home");
            
            Coach viewmodel = new Coach
            {
                FirstName = coachInDB.FirstName,
                MiddleName = coachInDB.MiddleName,
                LastName = coachInDB.LastName,
                Birthdate = coachInDB.Birthdate
            };
            return View(nameof(Create), viewmodel);
        }

        public IActionResult Details(int? id)
        {
            Coach coach = new Coach();
            if (id.HasValue && id != 0)
            {
                var tempCoach = _dataCoach.Get(id.Value);
                coach.Id = tempCoach.Id;
                coach.FirstName = tempCoach.FirstName;
                coach.MiddleName = tempCoach.MiddleName;
                coach.LastName = tempCoach.LastName;
                coach.Birthdate = tempCoach.Birthdate;
            }
            return View(coach);
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignClub(int id)
        {
            var coach = _dataCoach.Get(id);
            if (coach == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var coachesClubs = _dataClub.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            CoachClubInputVM viewModel = new CoachClubInputVM
            {
                CoachId = id,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Clubs = coachesClubs.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignClub(CoachClubInputVM model)
        {
            for (var i = 0; i < model.Ids.Count(); i++)
            {
                var coachesClubs = new CoachClub
                {
                    CoachId = model.CoachId,
                    ClubId = model.Ids[i]
                };
                _dataCoachClub.Add(coachesClubs);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
