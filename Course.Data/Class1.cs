using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;                    

namespace Course.Data
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }   
        public string Details { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Status { get; set; }          
    }

    public class ViewModel
    {
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Declined { get; set; }
    }

    public class CandidateRepository
    {
        private string _connectionString;

        public CandidateRepository(string connectionString)
        {
            _connectionString = connectionString;                          
        }

        public void AddCandidate(Candidate c)
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                context.Candidate.Add(c);
                context.SaveChanges();
            }
        }
       
        public IEnumerable<Candidate> GetPendingCandidates()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {                                                 
                return context.Candidate.Where(c => c.Status==null).ToList();
            }
        }

        public IEnumerable<Candidate> GetConfirmedCandidates()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.Where(c => c.Status == true).ToList();
            }
        }

        public IEnumerable<Candidate> GetDeclinedCandidates()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.Where(c => c.Status == false).ToList();
            }
        }

        public Candidate GetCandidate(int id)
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.FirstOrDefault(c => c.Id == id);
            }
        }
                              
        public void Confirm(int id)
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand("update Candidate set Status='true' where id=@id",
                    new SqlParameter("@id", id));
                context.SaveChanges();
            }
        }

        public void Decline(int id)
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                context.Database.ExecuteSqlCommand("update Candidate set Status='false' where id=@id",
                    new SqlParameter("@id", id));
                context.SaveChanges();
            }
        }

        public int CountsForPending()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.Where(c => c.Status == null).Count();
            }
        }

        public int CountsForConfirmed()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.Where(c=> c.Status == true).Count();
            }
        }

        public int CountsForDeclined()
        {
            using (var context = new CandidateForCourseContext(_connectionString))
            {
                return context.Candidate.Where(c => c.Status == false).Count();
            }
        }

    }                          

    public class CandidateForCourseContext : DbContext
    {
        private string _connectionString;

        public CandidateForCourseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Candidate> Candidate { get; set; }            

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString);
        }    
    }

    public class CandidateForCourseContextFactory : IDesignTimeDbContextFactory<CandidateForCourseContext>
    {
        public CandidateForCourseContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}Homework 5-06-19"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new CandidateForCourseContext(config.GetConnectionString("ConStr"));
        }
    }

    public class LayoutPageAttribute : ActionFilterAttribute
    {
        private string _connectionString;

        public LayoutPageAttribute(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            CandidateRepository cr = new CandidateRepository(_connectionString);
            var controller = (Controller)context.Controller;
            controller.ViewBag.Confirmed= cr.CountsForConfirmed();
            controller.ViewBag.Declined = cr.CountsForDeclined();
            controller.ViewBag.Pending = cr.CountsForPending();
            base.OnActionExecuted(context);
        }
    }
}
