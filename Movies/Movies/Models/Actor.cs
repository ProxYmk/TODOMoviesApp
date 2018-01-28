using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Actor
    {
        public int ID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), Required, StringLength(30)]
        public String Name { get; set; }
        [Display(Name = "Last Name"), StringLength(60, MinimumLength = 3)]
        public String LastName { get; set; }

        public List<MovieActor> MovieActors { get; set; }
    }
}
