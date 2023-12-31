﻿using Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Models
{
    public class MovieResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool Active { get; set; }

        public Genre Genre { get; set; }

    }
}
