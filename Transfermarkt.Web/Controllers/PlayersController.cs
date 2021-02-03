using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
    public class PlayersController : Controller
    {
        private readonly IData<Player> _dataPlayer;
        private readonly IData<City> _dataCity;
        private readonly IData<Position> _dataPositions;
        private readonly IData<PlayerPosition> _dataPlayerPosition;
        private readonly IData<Club> _dataClub;
        private readonly IData<Contract> _dataContract;
        private readonly IData<Goal> _dataGoal;
        private readonly IMapper _mapper;


        public PlayersController(IData<Player> dataPlayer, IData<City> dataCity,
            IData<Position> dataPositions, IData<PlayerPosition> dataPlayerPosition, IData<Club> dataClub,
            IData<Contract> dataContract,IData<Goal> dataGoal, IMapper mapper)
        {
            _dataPlayer = dataPlayer;
            _dataCity = dataCity;
            _dataPositions = dataPositions;
            _dataPlayerPosition = dataPlayerPosition;
            _dataClub = dataClub;
            _dataContract = dataContract;
            _dataGoal = dataGoal;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            return View(_dataPlayer.GetByDetails().ToPagedList(page, 5));
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

            var list = Globals.ToPairList<StrongerFoot>(typeof(StrongerFoot));

            var player = new PlayerInputVM
            {
                Cities = citiesList.ToList(),
                StrongerFoots = list.Select(x => new SelectListItem(x.Value, x.Key.ToString()))
            };

            return View(player);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Player player)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> citiesList = _dataCity.GetByDetails().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

                var list = Globals.ToPairList<StrongerFoot>(typeof(StrongerFoot));

                PlayerInputVM viewModel = new PlayerInputVM();
                viewModel.Cities = citiesList.ToList();
                viewModel.StrongerFoots = list.Select(x => new SelectListItem(x.Value, x.Key.ToString()));
                var mappedForView = _mapper.Map<Player, PlayerInputVM>(player, viewModel);

                return View(nameof(Create), mappedForView);
            }

            if (player.Id == 0) _dataPlayer.Add(player);
            else
            {
                var playerInDb = _dataPlayer.Get(player.Id);
                var mappedPlayer = _mapper.Map<Player, Player>(player, playerInDb);
                _dataPlayer.Update(mappedPlayer);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Edit(int id)
        {
            var playerInDb = _dataPlayer.Get(id);

            if (playerInDb == null) return RedirectToAction("Error", "Home");

            IEnumerable<SelectListItem> citiesList = _dataCity.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            var list = Globals.ToPairList<StrongerFoot>(typeof(StrongerFoot));

            PlayerInputVM viewModel = new PlayerInputVM();
            viewModel.Cities = citiesList.ToList();
            viewModel.StrongerFoots = list.Select(x => new SelectListItem(x.Value, x.Key.ToString()));
            var mappedForView = _mapper.Map<Player, PlayerInputVM>(playerInDb, viewModel);
            return View(nameof(Create), mappedForView);
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignPosition(int id)
        {
            var player = _dataPlayer.Get(id);
            if (player == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var playersPositions = _dataPositions.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name + " - " + x.Abbreviation,
                Value = x.Id.ToString()
            });
            PlayerPositionsInputVM viewModel = new PlayerPositionsInputVM
            {
                PlayerId = id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Positions = playersPositions.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignPosition(PlayerPositionsInputVM model)
        {
            for (var i = 0; i < model.Ids.Count(); i++)
            {
                var playerPosition = new PlayerPosition
                {
                    PlayerId = model.PlayerId,
                    PositionId = model.Ids[i]
                };
                _dataPlayerPosition.Add(playerPosition);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignContract(int id, int? message)
        {
            var player = _dataPlayer.Get(id);
            if (player == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var playerClubs = _dataClub.GetByDetails().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ContractInputVM viewModel = new ContractInputVM();
            viewModel.Clubs = playerClubs.ToList();
            viewModel.PlayerId = player.Id;
            var mappedForView = _mapper.Map<Player, ContractInputVM>(player, viewModel);
            if (message.HasValue)
                ViewBag.Mistake = "The signed date can't be higher than expiration date.";
            return View(mappedForView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult AssignContract(ContractInputVM model)
        {
            if (model.SignedDate > model.ExpirationDate)
            {
                return RedirectToAction(nameof(AssignContract), new { id = model.PlayerId, message = 1 });
            }
            var playerClubs = new Contract();
            var mappedForDatabase = _mapper.Map<ContractInputVM, Contract>(model, playerClubs);
            _dataContract.Add(mappedForDatabase);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            Player player = new Player();

            var tempPlayer = _dataPlayer.Get(id);
            var playerForView = _mapper.Map<Player, Player>(tempPlayer, player);

            var goals = _dataGoal.GetByDetails();

            var goalsScored = goals.Count(x => x.ScorerId == id);
            var assistant = goals.Count(x => x.AssistantId == id);

            ViewBag.Goals = goalsScored;
            ViewBag.Assistant = assistant;

            return PartialView(playerForView);
        }
        public IActionResult DetailsPlayer(int id)
        {
            Player player = new Player();

            var tempPlayer = _dataPlayer.Get(id);
            var playerForView = _mapper.Map<Player, Player>(tempPlayer, player);

            var goals = _dataGoal.GetByDetails();

            var goalsScored = goals.Count(x => x.ScorerId == id);
            var assistant = goals.Count(x => x.AssistantId == id);

            ViewBag.Goals = goalsScored;
            ViewBag.Assistant = assistant;

            return View(playerForView);
        }
    }

}
