using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
         public ImageRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}