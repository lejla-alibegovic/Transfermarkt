using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Transfermarkt.Web.Data;
using Transfermarkt.Web.Models;


namespace Transfermarkt.Web.Services
{
    public interface IMatches : IData<Match>
    {
        IEnumerable<Match> GetByDetails(int? id);
    }

    public class MatchesService : Data<Match>, IMatches
    {
        public MatchesService(AppDbContext context) : base(context) { }

        public IEnumerable<Match> GetByDetails(int? id)
        {
            if (!id.HasValue)
            {
                IEnumerable<Match> list = _context.Matches.Include(m => m.HomeClub).Include(m => m.AwayClub)
                                .Include(m => m.League).Include(m => m.Stadium).AsEnumerable();
                return list;
            }
            IEnumerable<Match> list1 = _context.Matches.Include(m => m.HomeClub).Include(m => m.AwayClub)
                .Include(m => m.League).Include(m => m.Stadium).Where(m => m.LeagueId == id).AsEnumerable();
            return list1;
        }
    }
}
