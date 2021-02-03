using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transfermarkt.Web.Data;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Models.Enums;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;

namespace Transfermarkt.Web.Controllers
{
    public class SearchController : Controller
    {
        private AppDbContext _context;
        public SearchController(AppDbContext db)
        {
            _context = db;
        }
        public IActionResult Index()
        {
            List<SearchMatchVM> list = new List<SearchMatchVM>();
            
            list = _context.RefereeMatches.Include(a=>a.Match).Include(a => a.Match.AwayClub).Include(a => a.Match.HomeClub)
                .Include(a => a.Match.League).Include(a => a.Match.Stadium).Select(w => new SearchMatchVM
            {
                HomeClub=w.Match.HomeClub.Name,
                AwayClub=w.Match.AwayClub.Name,
                League=w.Match.League.Name,
                RefereeName=w.Referee.FirstName+" ("+w.Referee.MiddleName+") "+w.Referee.LastName,
                StadiumName=w.Match.Stadium.Name
            }).ToList();
            return View(list);
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult CreateMatch()
        {
            RefereeMatchAddVM vm = new RefereeMatchAddVM();
            List<Match> mat = _context.Matches.ToList();
            List<Referee> reff = _context.Referees.ToList();
            vm.MatchList = _context.Matches.Include(a=>a.HomeClub).Include(a => a.AwayClub).Select(w => new SelectListItem
            {
                Value=w.Id.ToString(),
                Text=w.TimePlayed.ToString() +" | "+w.HomeClub.Name+" - "+w.AwayClub.Name
            }).ToList();
            vm.RefereeList = _context.Referees.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.FirstName+" ("+w.MiddleName+") "+w.LastName
            }).ToList();
            return View(vm);
        }
        public IActionResult SaveMatch(int refereeid,int matchid)
        {
            RefereeMatch rf = new RefereeMatch();
            rf.MatchId = matchid;
            rf.RefereeId = refereeid;
            _context.Add(rf);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult CreateReferee()
        {
            CreateRefereeVM model = new CreateRefereeVM();
            model.Country = _context.Countries.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text=w.Name
            }).ToList();
            return View(model);
        }
        public IActionResult SaveReferee(string firstname,string lastname,string middlename,int countryid)
        {
            Referee r = new Referee
            {
                FirstName=firstname,
                LastName=lastname,
                MiddleName=middlename,
                CountryId=countryid
            };
            _context.Add(r);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SearchTrainer()
        {
            return View();
        }
        public IActionResult FindTrainer(string parametar,DateTime parametardate)
        {
            List<SearchForTrainerVM> list = new List<SearchForTrainerVM>();
            List<Coach> coac = _context.Coaches.ToList();
            List<Club> colub = _context.Clubs.ToList();
            List<CoachClub> CoachClub = _context.CoachesClubs.ToList();
            string birthdate = _context.Coaches.Select(a => a.Birthdate.ToShortDateString().ToString()).First();
            if (parametar==null && parametardate==null)
            {
                list = _context.CoachesClubs.Include(a => a.Club).Include(a => a.Coach).Select(w => new SearchForTrainerVM
                {
                    FirstName=w.Coach.FirstName,
                    LastName=w.Coach.LastName,
                    BirthDate=w.Coach.Birthdate,
                    MiddleName=w.Coach.MiddleName,
                    ClubName=w.Club.Name
                }).ToList();
            }
            else
            {
                list = _context.CoachesClubs.Include(a => a.Club).Include(a => a.Coach).Where(a=>a.Club.Name==parametar || a.Coach.FirstName==parametar
                || a.Coach.LastName==parametar || a.Coach.MiddleName==parametar || a.Coach.Birthdate.ToString() == parametar).Select(w => new SearchForTrainerVM
                {
                    FirstName = w.Coach.FirstName,
                    LastName = w.Coach.LastName,
                    BirthDate = w.Coach.Birthdate,
                    MiddleName = w.Coach.MiddleName,
                    ClubName = w.Club.Name
                }).ToList();
            }
            return View(list);
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AddNewCoach()
        {
            return View();
        }
        public IActionResult SaveNewCoach(string firstname,string lastname,string middlename,DateTime birthdate)
        {
            Coach nw = new Coach
            {
                FirstName = firstname,
                LastName=lastname,
                MiddleName=middlename,
                Birthdate=birthdate
            };
            _context.Add(nw);
            _context.SaveChanges();
            return RedirectToAction("SearchTrainer");
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AddNewCoachClub()
        {
            CoachClubVM model = new CoachClubVM();
            model.clubs = _context.Clubs.Select(w => new SelectListItem
            {
                Value=w.Id.ToString(),
                Text=w.Name
            }).ToList();
            model.coachs = _context.Coaches.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.FirstName+" ("+w.MiddleName+") "+w.LastName
            }).ToList();
            return View(model);
        }
        public IActionResult SaveNewCoachClub(int coachid,int clubid)
        {
            CoachClub ch = new CoachClub();
            ch.ClubId = clubid;
            ch.CoachId = coachid;
            _context.Add(ch);
            _context.SaveChanges();
            return RedirectToAction("SearchTrainer");
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AddNewClub()
        {
            AddNewClubVM model = new AddNewClubVM();
            model.League = _context.Leagues.Select(w => new SelectListItem
            {
                Value=w.Id.ToString(),
                Text=w.Name
            }).ToList();
            model.Citys = _context.Cities.Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            }).ToList();
            return View(model);
        }
        public IActionResult SaveNewClub(string name,string abbreviation,string nickname,DateTime founded,string logo,int marketvalue,int cityid,int leagueid)
        {
            Club newclub = new Club
            {
                Abbreviation=abbreviation,
                CityId=cityid,
                Founded=founded,
                LeagueId=leagueid,
                Logo=logo,
                MarketValue=marketvalue,
                Name=name,
                Nickname=nickname
            };
            _context.Add(newclub);
            _context.SaveChanges();
            return RedirectToAction("SearchTrainer");
        }
        public IActionResult SearchHistoryPlayer()
        {
            PlayerHistoryVM model = new PlayerHistoryVM();
            model.players = _context.Players.Select(w => new SelectListItem
            {
                Value=w.Id.ToString(),
                Text=w.FirstName+" ("+w.MiddleName+") "+w.LastName
            }).ToList();
            return View(model);
        }
        public IActionResult GetHistoryPlayer(int playerid)
        {
            HistoryOfPlayerVM model = new HistoryOfPlayerVM();
            Player p = _context.Players.Where(a => a.Id == playerid).First();
            model.FullName = p.FirstName + " (" + p.MiddleName + ") " + p.LastName;
            model.list = _context.Contracts.Include(a => a.Club).Where(a => a.PlayerId == playerid).Select(w => new HistoryPlayerVM
            {
                ClubName=w.Club.Name,
                ExpirationDate=w.ExpirationDate,
                SignedDate=w.SignedDate
            }).ToList();
            return View(model);
        }
        public IActionResult SearchHistoryCoach()
        {
            CoachHistoryVM model = new CoachHistoryVM();
            model.coach=_context.Coaches.Select(w=> new SelectListItem{
                Value=w.Id.ToString(),
                Text=w.FirstName+" ("+w.MiddleName+") "+w.LastName
            }).ToList();
            return View(model);
        }
        public IActionResult GetHistoryCoach(int coachid)
        {
            HistoryOfCoachVM model = new HistoryOfCoachVM();
            Coach co = _context.Coaches.Where(a => a.Id == coachid).First();
            string name = co.FirstName + " (" + co.MiddleName + ") " + co.LastName;
            model.FullName = name;
            model.list = _context.CoachesClubs.Include(a => a.Club).Include(a=>a.Club.League).Where(a => a.CoachId == coachid).Select(w => new CoachHistory
            {
                name=w.Club.Name,
                league=w.Club.League.Name,
                ContractSigned=w.ContractSigned,
                ContractExpired=w.ContractExpired
            }).ToList();
            return View(model);
        }
        public IActionResult SearchClubDetails()
        {
            List<ClubDetailsVM> model = new List<ClubDetailsVM>();
            model = _context.Clubs.Include(a => a.City).Include(a => a.League).Select(w => new ClubDetailsVM
            {
                League=w.League.Name,
                Name=w.Name,
                Abbreviation=w.Abbreviation,
                city=w.City.Name,
                Founded=w.Founded,
                MarketValue=w.MarketValue,
                Logo=w.Logo,
                Nickname=w.Nickname
            }).ToList();
            return View(model);
        }
        public IActionResult SearchPlayerByC()
        {
            return View();
        }
        public IActionResult GetPlayerByC(int criteriaid,string criteria,DateTime datee)
        {
            List<PlayerCriteriaVM> model = new List<PlayerCriteriaVM>();
            string temp = datee.ToShortDateString();
            if(criteriaid==0&&criteria==null&& temp == "1. 1. 0001.")
            {
                model = _context.Players.Include(a => a.Birthplace).Select(w => new PlayerCriteriaVM
                {
                    FirstName=w.FirstName,
                    LastName=w.LastName,
                    MiddleName=w.MiddleName,
                    Height=w.Height,
                    Birthplace=w.Birthplace.Name,
                    Birthdate=w.Birthdate,
                    Jersey=w.Jersey,
                    StrongerFoot=w.StrongerFoot.ToString(),
                    Value=w.Value,
                    Weight=w.Weight
                }).ToList();
            }
            if(criteriaid==1)
            {
                model = _context.Players.Include(a => a.Birthplace).Where(a=>a.FirstName== criteria||a.LastName== criteria||a.MiddleName== criteria||a.FirstName+" "+a.LastName==criteria).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.FirstName,
                    LastName = w.LastName,
                    MiddleName = w.MiddleName,
                    Height = w.Height,
                    Birthplace = w.Birthplace.Name,
                    Birthdate = w.Birthdate,
                    Jersey = w.Jersey,
                    StrongerFoot = w.StrongerFoot.ToString(),
                    Value = w.Value,
                    Weight = w.Weight
                }).ToList();
            }
            if (criteriaid == 2)
            {
                model = _context.Goals.Include(a => a.Scorer.Birthplace).Where(a=>a.Minute== int.Parse(criteria)).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.Scorer.FirstName,
                    LastName = w.Scorer.LastName,
                    MiddleName = w.Scorer.MiddleName,
                    Height = w.Scorer.Height,
                    Birthplace = w.Scorer.Birthplace.Name,
                    Birthdate = w.Scorer.Birthdate,
                    Jersey = w.Scorer.Jersey,
                    StrongerFoot = w.Scorer.StrongerFoot.ToString(),
                    Value = w.Scorer.Value,
                    Weight = w.Scorer.Weight
                }).ToList();
            }
            if (criteriaid == 3)
            {
                model = _context.Goals.Include(a => a.Assistant.Birthplace).Where(a => a.Minute == int.Parse(criteria)).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.Assistant.FirstName,
                    LastName = w.Assistant.LastName,
                    MiddleName = w.Assistant.MiddleName,
                    Height = w.Assistant.Height,
                    Birthplace = w.Assistant.Birthplace.Name,
                    Birthdate = w.Assistant.Birthdate,
                    Jersey = w.Assistant.Jersey,
                    StrongerFoot = w.Assistant.StrongerFoot.ToString(),
                    Value = w.Assistant.Value,
                    Weight = w.Assistant.Weight
                }).ToList();
            }
            if (criteriaid == 4)
            {
                model = _context.Players.Include(a => a.Birthplace).Where(a=>a.Weight==int.Parse(criteria)).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.FirstName,
                    LastName = w.LastName,
                    MiddleName = w.MiddleName,
                    Height = w.Height,
                    Birthplace = w.Birthplace.Name,
                    Birthdate = w.Birthdate,
                    Jersey = w.Jersey,
                    StrongerFoot = w.StrongerFoot.ToString(),
                    Value = w.Value,
                    Weight = w.Weight
                }).ToList();
            }
            if (criteriaid == 5)
            {
                model = _context.Players.Include(a => a.Birthplace).Where(a => a.Height == int.Parse(criteria)).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.FirstName,
                    LastName = w.LastName,
                    MiddleName = w.MiddleName,
                    Height = w.Height,
                    Birthplace = w.Birthplace.Name,
                    Birthdate = w.Birthdate,
                    Jersey = w.Jersey,
                    StrongerFoot = w.StrongerFoot.ToString(),
                    Value = w.Value,
                    Weight = w.Weight
                }).ToList();
            }
            if (criteriaid == 6)
            {
                model = _context.PlayerPositions.Include(a=>a.Player).Include(a => a.Player.Birthplace).Where(a => a.Position.Name == criteria).Select(w => new PlayerCriteriaVM
                {
                    FirstName = w.Player.FirstName,
                    LastName = w.Player.LastName,
                    MiddleName = w.Player.MiddleName,
                    Height = w.Player.Height,
                    Birthplace = w.Player.Birthplace.Name,
                    Birthdate = w.Player.Birthdate,
                    Jersey = w.Player.Jersey,
                    StrongerFoot = w.Player.StrongerFoot.ToString(),
                    Value = w.Player.Value,
                    Weight = w.Player.Weight
                }).ToList();
            }
            if (criteriaid == 7)
            {
                if(datee!=null)
                {
                    model = _context.Players.Include(a => a.Birthplace).Where(a => a.Birthdate==datee).Select(w => new PlayerCriteriaVM
                    {
                        FirstName = w.FirstName,
                        LastName = w.LastName,
                        MiddleName = w.MiddleName,
                        Height = w.Height,
                        Birthplace = w.Birthplace.Name,
                        Birthdate = w.Birthdate,
                        Jersey = w.Jersey,
                        StrongerFoot = w.StrongerFoot.ToString(),
                        Value = w.Value,
                        Weight = w.Weight
                    }).ToList();
                }
            }
            return View(model);
        }
    }
}