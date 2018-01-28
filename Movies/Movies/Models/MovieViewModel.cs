using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movies.Models
{
    public class MovieViewModel
    {
        public MovieViewModel(){
            Movie = new Movie();
            MovieActors = new List<int>();
        }

        public Movie Movie;

        public SelectList Genres;
        public string MovieGenre { get; set; }

        public MultiSelectList ActorList;
        public List<int> MovieActors { get; set; }

        public string MovieProduction { get; set; }
    }
}