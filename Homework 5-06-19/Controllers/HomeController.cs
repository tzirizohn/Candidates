using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homework_5_06_19.Models;
using Microsoft.Extensions.Configuration;
using Course.Data;

namespace Homework_5_06_19.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {                                   
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddForm()
        {
            return View();
        }

        public IActionResult Add(Candidate candidate)
        {
            CandidateRepository cr = new CandidateRepository(_connectionString);
            cr.AddCandidate(candidate);
            return Redirect("/");
        }

        public IActionResult Pending()
        {
            CandidateRepository cc = new CandidateRepository(_connectionString);
            IEnumerable<Candidate> pendingCandidates = cc.GetPendingCandidates();
            return View(pendingCandidates);
        }

        public IActionResult Confirmed()
        {
            CandidateRepository cc = new CandidateRepository(_connectionString);
            IEnumerable<Candidate> confirmedCandidates = cc.GetConfirmedCandidates();
            return View(confirmedCandidates);
        }

        public IActionResult Declined()
        {
            CandidateRepository cc = new CandidateRepository(_connectionString);
            IEnumerable<Candidate> declinedCandidates = cc.GetDeclinedCandidates();
            return View(declinedCandidates);
        }

        public IActionResult ViewCandidate(int id)
        {
            CandidateRepository cr = new CandidateRepository(_connectionString);
            Candidate candidate = cr.GetCandidate(id);
            return View(candidate);
        }        

        public IActionResult Confirm(int id)
        {
            CandidateRepository cr = new CandidateRepository(_connectionString);
            cr.Confirm(id);
            ViewModel vm = new ViewModel();
            vm.Confirmed = cr.CountsForConfirmed();
            vm.Declined = cr.CountsForDeclined();
            vm.Pending = cr.CountsForPending(); 
            return Json(vm);
        }

        public IActionResult Decline(int id)
        {
            CandidateRepository cr = new CandidateRepository(_connectionString);
            cr.Decline(id);                      
            object counts = new
            {
                Pending = cr.CountsForPending(),
                Confirmed = cr.CountsForConfirmed(),
                Declined = cr.CountsForDeclined()
            };
            return Json(counts);
        }

    }
}
