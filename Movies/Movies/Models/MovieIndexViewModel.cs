using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movies.Models
{
    public class MovieIndexViewModel
    {
        public List<Movie> movies;
        public SelectList genres;
        public int movieGenre { get; set; }
    }
}
