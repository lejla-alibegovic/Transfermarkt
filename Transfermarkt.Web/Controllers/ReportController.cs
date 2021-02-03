using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Transfermarkt.Web.Data;
using Transfermarkt.Web.Reports;
using System;
using System.Data;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;

namespace Transfermarkt.Web.Controllers
{
    public class ReportController : Controller
    {
        private AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public static List<Report1VM> GetMatches(AppDbContext db)
        {
            List<Report1VM> podaci = db.Matches.Select(s => new Report1VM
            {
                TimePlayed=s.TimePlayed,
                HomeClub=s.HomeClub.Name,
                AwayClub=s.AwayClub.Name,
                League=s.League.Name,
                Stadium=s.Stadium.Name
            }).ToList();

            return podaci;
        }
        [Authorize(Roles = RolesGlobal.Admin)]
        public IActionResult Index()
        {
            LocalReport _localReport = new LocalReport("Reports/Report1.rdlc");
            List<Report1VM> podaci = GetMatches(_context);
            DataSet ds = new DataSet();
            _localReport.AddDataSource("DataSet1", podaci);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ReportSastavio", HttpContext.User.Identity.Name);

           
            ReportResult result = _localReport.Execute(RenderType.Pdf, parameters: parameters);
            return File(result.MainStream, "application/pdf");

        }
    }
}
