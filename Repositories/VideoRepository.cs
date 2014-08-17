using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}