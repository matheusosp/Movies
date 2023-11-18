using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Domain.Entities.Base;

namespace Movies.Domain.Entities
{
    public class Movie : Entity
    {
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool Active { get; set; }

        public Genre Genre { get; set; }
        public long GenreId { get; set; }

        public List<MovieRent> MovieRents { get; set; }
    }
}
