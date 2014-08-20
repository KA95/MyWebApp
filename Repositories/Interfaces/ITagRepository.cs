﻿using MyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Repositories.Interfaces
{
    public interface ITagRepository: IRepository<Tag>
    {
        Tag GetByName(string name);
    }
}
