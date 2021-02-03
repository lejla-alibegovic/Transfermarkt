using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Transfermarkt.Web.Extensions;
using Transfermarkt.Web.Hubs;
using Transfermarkt.Web.Models;
using Transfermarkt.Web.Models.Enums;
using Transfermarkt.Web.Services;
using Transfermarkt.Web.ViewModels;
using X.PagedList;

namespace Transfermarkt.Web.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IData<Match> _dataMatches;
        private readonly IData<League> _dataLeagues;
        private readonly IData<Stadium> _dataStadiums;
        private readonly IData<Club> _dataClubs;
        private readonly IData<Referee> _dataReferee;
        private readonly IData<RefereeMatch> _dateRefereeMatch;
        private readonly IData<Player> _dataPlayer;
        private readonly IData<Foul> _dataFoul;
        private readonly IData<Corner> _dataCorner;
        private readonly IData<Goal> _dataGoal;
        private readonly IData<Contract> _dataContract;
        private readonly IMatches _Matches;
        private static IHubContext<Notification> _hubContext;

        public MatchesController(IData<Match> dataMatches, IData<League> dataLeagues,
            IData<Stadium> dataStadiums, IData<Club> dataClubs,
            IData<Referee> dataReferee, IData<RefereeMatch> dateRefereeMatch,
            IData<Player> dataPlayer, IData<Foul> dataFoul, IData<Corner> dataCorner,
            IData<Goal> dataGoal, IData<Contract> dataContract, IMatches matches, IHubContext<Notification>hubContext)
        {
            _dataMatches = dataMatches;
            _dataLeagues = dataLeagues;
            _dataStadiums = dataStadiums;
            _dataClubs = dataClubs;
            _dataReferee = dataReferee;
            _dateRefereeMatch = dateRefereeMatch;
            _dataPlayer = dataPlayer;
            _dataFoul = dataFoul;
            _dataCorner = dataCorner;
            _dataGoal = dataGoal;
            _dataContract = dataContract;
            _Matches = matches;
            _hubContext = hubContext;
        }
        public static void NotificirajSignalR(string poruka = "")
        {
            _hubContext.Clients.All.SendAsync("ReceiveNotification", poruka);
        }
        [HttpGet]
        public IActionResult Index(int? id, int page = 1)
        {
            var games = _Matches.GetByDetails(id);
            List<MatchesOutputVM> list = new List<MatchesOutputVM>();
            foreach (var item in games)
            {
                var game = new MatchesOutputVM
                {
                    AwayClub = item.AwayClub.Name,
                    HomeClub = item.HomeClub.Name,
                    TimePlayed = item.TimePlayed,
                    League = item.League.Name,
                    Stadium = item.Stadium.Name,
                    MatchId = item.Id,
                    LeagueId = id ?? 0
                };
                list.Add(game);
            }
            return View(list.ToPagedList(page, 5));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Create()
        {
            var clubsList = _dataClubs.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var leaguesList = _dataLeagues.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var stadiumsList = _dataStadiums.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var viewModel = new MatchInputVM
            {
                AwayClubs = clubsList.ToList(),
                HomeClubs = clubsList.ToList(),
                Leagues = leaguesList.ToList(),
                Stadiums = stadiumsList.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Match model)
        {
            if (model.Id == 0) _dataMatches.Add(model);
            else
            {
                var matchInDb = _dataMatches.Get(model.Id);
                matchInDb.Id = model.Id;
                matchInDb.HomeClubId = model.HomeClubId;
                matchInDb.AwayClubId = model.AwayClubId;
                matchInDb.TimePlayed = model.TimePlayed;
                matchInDb.LeagueId = model.LeagueId;
                matchInDb.StadiumId = model.StadiumId;
                _dataMatches.Update(matchInDb);
            }
            NotificirajSignalR("Nova utakmica dodana");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var matchInDb = _dataMatches.Get(id);

            if (matchInDb == null) return RedirectToAction("Error", "Home");

            var clubsList = _dataClubs.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var leaguesList = _dataLeagues.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var stadiumsList = _dataStadiums.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            MatchInputVM viewModel = new MatchInputVM
            {
                Id = id,
                AwayClubId = matchInDb.AwayClubId,
                AwayClubs = clubsList.ToList(),
                HomeClubId = matchInDb.HomeClubId,
                HomeClubs = clubsList.ToList(),
                LeagueId = matchInDb.LeagueId,
                Leagues = leaguesList.ToList(),
                StadiumId = matchInDb.StadiumId,
                Stadiums = stadiumsList.ToList(),
                TimePlayed = matchInDb.TimePlayed
            };
            return View(nameof(Create), viewModel);
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignReferee(int id)
        {
            var referee = _dataReferee.Get(id);
            if (referee == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var refereeMatch = _dataReferee.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            });
            RefereeMatchInputVM viewModel = new RefereeMatchInputVM
            {
                MatchId = id,
                Referees = refereeMatch.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignReferee(RefereeMatchInputVM model)
        {
            for (var i = 0; i < model.Ids.Count(); i++)
            {
                var refereeMatch = new RefereeMatch
                {
                    MatchId = model.MatchId,
                    RefereeId = model.Ids[i]
                };
                _dateRefereeMatch.Add(refereeMatch);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignFouls(int id, int leagueId, int? message)
        {
            var match = _dataMatches.Get(id);
            var homeClub = _dataClubs.Get(m => m.Id == match.HomeClubId);
            var awayClub = _dataClubs.Get(m => m.Id == match.AwayClubId);
            if (match == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var playerList = _dataPlayer.GetByDetails();
            List<Player> playersList = new List<Player>();

            var contracts = _dataContract.GetByDetails().ToList();
            foreach (var item in playerList)
            {
                for (int i = contracts.Count() - 1; i >= 0; i--)
                {
                    if (item.Id == contracts[i].PlayerId
                        && (contracts[i].ClubId == homeClub.Id || contracts[i].ClubId == awayClub.Id))
                    {
                        playersList.Add(item);
                    }
                }
            }
            var list = Globals.ToPairList<HalfTime>(typeof(HalfTime));
            FoulInputVM viewmodel = new FoulInputVM
            {
                MatchId = id,
                Players = playersList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString()
                }).ToList(),
                Victims = playersList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString()
                }).ToList(),
                LeagueId = leagueId
            };
            if (message.HasValue)
                ViewBag.Mistake = "You cant add foul because input minute is lower than the last minute foul" +
                    " or entered minute is not correct.";
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignFouls(FoulInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(AssignFouls), new { id = model.MatchId, message = 1 });
            }
            var fouls = _dataFoul.GetByDetails();
            var lastFoul = fouls.LastOrDefault(x => x.MatchId == model.MatchId);
            var foul = new Foul
            {
                Minute = model.Minute,
                MatchId = model.MatchId,
                Penalty = model.Penalty,
                PlayerId = model.PlayerId,
                VictimId = model.VictimId
            };
            if (lastFoul == null && model.Minute >= 1 && model.Minute <= 93)
                _dataFoul.Add(foul);
            else if (lastFoul != null && model.Minute >= 1 && model.Minute <= 93
                && model.Minute > lastFoul.Minute)
                _dataFoul.Add(foul);
            else
                return RedirectToAction(nameof(AssignFouls), new { id = model.MatchId, leagueId = model.LeagueId, message = 1 });

            return RedirectToAction(nameof(Index), new { id = model.LeagueId });
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignCorners(int id, int leagueId, int? message)
        {
            var match = _dataMatches.Get(id);
            var homeClub = _dataClubs.Get(m => m.Id == match.HomeClubId);
            var awayClub = _dataClubs.Get(m => m.Id == match.AwayClubId);
            if (match == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var playerList = _dataPlayer.GetByDetails();
            List<Player> playersList = new List<Player>();

            var contracts = _dataContract.GetByDetails().ToList();
            foreach (var item in playerList)
            {
                for (int i = contracts.Count() - 1; i >= 0; i--)
                {
                    if (item.Id == contracts[i].PlayerId
                        && (contracts[i].ClubId == homeClub.Id || contracts[i].ClubId == awayClub.Id))
                    {
                        playersList.Add(item);
                    }
                }
            }
            var list = Globals.ToPairList<HalfTime>(typeof(HalfTime));

            CornerInputVM viewmodel = new CornerInputVM
            {
                MatchId = id,
                Players = playersList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString()
                }).ToList(),
                HalfTimes = list.Select(x => new SelectListItem(x.Value, x.Key.ToString())),
                LeagueId = leagueId
            };
            if (message.HasValue)
                ViewBag.Mistake = "You cant add corner because input minute is lower than the last minute corner" +
                    " or entered minute is not in correct halftime.";
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignCorners(CornerInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(AssignCorners), new { id = model.MatchId, message = 1 });
            }
            var corners = _dataCorner.GetByDetails();
            var lastCorner = corners.LastOrDefault(x => x.MatchId == model.MatchId);
            var corner = new Corner
            {
                MinuteAwarded = model.MinuteAwarded,
                HalfTime = model.HalfTime,
                TakerId = model.TakerId,
                MatchId = model.MatchId
            };
            if (lastCorner == null &&
                ((model.MinuteAwarded >= 1 && model.MinuteAwarded <= 45 && model.HalfTime.ToString() == "First") ||
                (model.MinuteAwarded >= 46 && model.MinuteAwarded <= 93 && model.HalfTime.ToString() == "Second")))
                _dataCorner.Add(corner);
            else if (lastCorner != null && model.MinuteAwarded > lastCorner.MinuteAwarded
                && model.MinuteAwarded >= 1 && model.MinuteAwarded <= 45 && model.HalfTime.ToString() == "First")
                _dataCorner.Add(corner);
            else if (lastCorner != null && model.MinuteAwarded > lastCorner.MinuteAwarded
                && model.MinuteAwarded >= 46 && model.MinuteAwarded <= 93 && model.HalfTime.ToString() == "Second")
                _dataCorner.Add(corner);
            else
                return RedirectToAction(nameof(AssignCorners), new { id = model.MatchId, leagueId = model.LeagueId, message = 1 });

            return RedirectToAction(nameof(Index), new { id = model.LeagueId });
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignGoals(int id, int leagueId, int? message)
        {
            var match = _dataMatches.Get(id);
            var homeClub = _dataClubs.Get(m => m.Id == match.HomeClubId);
            var awayClub = _dataClubs.Get(m => m.Id == match.AwayClubId);

            if (match == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var playerList = _dataPlayer.GetByDetails();
            List<Player> playersList = new List<Player>();

            var contracts = _dataContract.GetByDetails().ToList();
            foreach (var item in playerList)
            {
                for (int i = contracts.Count() - 1; i >= 0; i--)
                {
                    if (item.Id == contracts[i].PlayerId
                        && (contracts[i].ClubId == homeClub.Id || contracts[i].ClubId == awayClub.Id))
                    {
                        playersList.Add(item);
                    }
                }
            }

            var list = Globals.ToPairList<HalfTime>(typeof(HalfTime));

            GoalInputVM viewmodel = new GoalInputVM
            {
                MatchId = id,
                ScoredPlayers = playersList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString()
                }).ToList(),
                AssistedPlayers = playersList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString()
                }).ToList(),
                Times = list.Select(x => new SelectListItem(x.Value, x.Key.ToString())),
                HomeTeam = homeClub.Name,
                AwayTeam = awayClub.Name,
                LeagueId = leagueId
            };
            if (message.HasValue)
                ViewBag.Mistake = "You cant add goal because input minute is lower than the last minute goal" +
                    " or entered minute is not in correct halftime.";
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AddGoal(GoalInputVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(AssignGoals), new { id = model.MatchId, message = 1 });
            }
            var goals = _dataGoal.GetByDetails();
            var lastGoal = goals.LastOrDefault(x => x.MatchId == model.MatchId);
            var goal = new Goal
            {
                Minute = model.Minute,
                Time = model.Time,
                AssistantId = model.AssistantId,
                ScorerId = model.ScorerId,
                MatchId = model.MatchId
            };
            if (lastGoal == null &&
                ((model.Minute >= 1 && model.Minute <= 45 && model.Time.ToString() == "First") ||
                (model.Minute >= 46 && model.Minute <= 93 && model.Time.ToString() == "Second")))
                _dataGoal.Add(goal);
            else if (lastGoal != null && model.Minute > lastGoal.Minute
                && model.Minute >= 1 && model.Minute <= 45 && model.Time.ToString() == "First")
                _dataGoal.Add(goal);
            else if (lastGoal != null && model.Minute > lastGoal.Minute
                && model.Minute >= 46 && model.Minute <= 93 && model.Time.ToString() == "Second")
                _dataGoal.Add(goal);
            else
                return RedirectToAction(nameof(AssignGoals), new { id = model.MatchId, leagueId = model.LeagueId, message = 1 });

            return RedirectToAction(nameof(Index),new { id = model.LeagueId });
        }

        public IActionResult MatchDetails(int id)
        {
            var match = _Matches.Get(id);

            var homeClub = _dataClubs.Get(x => x.Id == match.HomeClubId);
            var awayClub = _dataClubs.Get(x => x.Id == match.AwayClubId);
            var stadium = _dataStadiums.Get(x => x.Id == match.StadiumId);
            var league = _dataLeagues.Get(x => x.Id == match.LeagueId);

            var corners = _dataCorner.GetByDetails();
            var fouls = _dataFoul.GetByDetails();
            var goals = _dataGoal.GetByDetails();
            var players = _dataPlayer.GetByDetails();

            var cornersList = PlayersCornersList(id, corners, players);
            var foulsList = PlayersFoulsList(id, fouls, players);
            var goalsList = PlayersGoalsList(id, goals, players);

            var details = new MatchDetails
            {
                AwayClub = awayClub.Name,
                HomeClub = homeClub.Name,
                LeagueName = league.Name,
                MatchId = id,
                StadiumName = stadium.Name,
                AwayClubLogo = awayClub.Logo,
                HomeClubLogo = homeClub.Logo,
                CornersMatches = cornersList,
                FoulsMatch = foulsList,
                GoalsMatch = goalsList,
                GoalsScored = goalsList.Count(),
                TimePlayed=match.TimePlayed
            };

            return View(details);
        }
        public List<GoalsMatch> PlayersGoalsList(int matchId, IEnumerable<Goal> goals,
            IEnumerable<Player> players)
        {
            var list = new List<GoalsMatch>();

            foreach (var item in goals.Where(x => x.MatchId == matchId))
            {
                var goalMatch = new GoalsMatch
                {
                    Minute = item.Minute,
                    MatchId = matchId,
                    ScorerId = item.ScorerId
                };
                list.Add(goalMatch);
            }

            foreach (var item in list)
            {
                foreach (var item2 in players.Where(x => x.Id == item.ScorerId))
                {
                    item.ScorerName = item2.FirstName + " " + item2.LastName;
                }
            }
            return list;
        }
        public List<CornersMatch> PlayersCornersList(int matchId, IEnumerable<Corner> corners,
            IEnumerable<Player> players)
        {
            var list = new List<CornersMatch>();

            foreach (var item in corners.Where(x => x.MatchId == matchId))
            {
                var cornerMatch = new CornersMatch
                {
                    MatchId = matchId,
                    Minute = item.MinuteAwarded,
                    TakerId = item.TakerId
                };
                list.Add(cornerMatch);
            }

            foreach (var item in list)
            {
                foreach (var item2 in players.Where(x => x.Id == item.TakerId))
                {
                    item.TakerName = item2.FirstName + " " + item2.LastName;
                }
            }
            return list;
        }
        public List<FoulsMatch> PlayersFoulsList(int matchId, IEnumerable<Foul> fouls,
            IEnumerable<Player> players)
        {
            var list = new List<FoulsMatch>();

            foreach (var item in fouls.Where(x => x.MatchId == matchId))
            {
                var foulMatch = new FoulsMatch
                {
                    MatchId = matchId,
                    Minute = item.Minute,
                    Penalty = item.Penalty,
                    TakerId = item.PlayerId
                };
                list.Add(foulMatch);
            }

            foreach (var item in list)
            {
                foreach (var item2 in players.Where(x => x.Id == item.TakerId))
                {
                    item.TakerName = item2.FirstName + " " + item2.LastName;
                }
            }
            return list;
        }
    }
}