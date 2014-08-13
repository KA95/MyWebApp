using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class ProblemRepository: GenericRepository<Problem>,IProblemRepository
    {
        public ProblemRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}