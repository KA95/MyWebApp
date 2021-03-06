﻿using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context)
            : base(context)
        {
           
        }

        public Tag GetByName(string name)
        {
            Tag tag = this.GetByID(name);

            if (tag == null)
                return new Tag() { Name = name };
            else
                return tag;
        }
    }
}