using Movies.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Entities
{
    public class MovieRent : Entity
    {
        public List<Movie> Movies { get; set; }
        public string CPFClient { get; set; }
        public DateTime RentDate { get; set; }
    }
}
