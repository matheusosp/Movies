﻿using Movies.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Entities
{
    public class Genre : Entity
    {
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool Active { get; set; }
    }
}
