using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class DislikeRepository : GenericRepository<Dislike>, IDislikeRepository
    {
        public DislikeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}