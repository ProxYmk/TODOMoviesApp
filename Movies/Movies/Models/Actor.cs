using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Actor
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public String Name { get; set; }
        public String LastName { get; set; }

        public List<MovieActor> MovieActors { get; set; }
    }
}
