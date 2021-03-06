﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Genre
    {
        public int ID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), Required, StringLength(30)]
        public String Name { get; set; }
    }
}
