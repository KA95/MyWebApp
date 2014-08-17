﻿using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp.Repositories
{
    public class UserSolvedRepository :GenericRepository<UserSolvedProblem>, IUserSolvedRepository
    {
        public UserSolvedRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}