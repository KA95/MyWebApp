using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class AnswerRepository :GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}