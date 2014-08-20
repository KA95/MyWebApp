using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class CommentRepository: GenericRepository<Comment>,ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context):base(context)
        {

        }
    }
}