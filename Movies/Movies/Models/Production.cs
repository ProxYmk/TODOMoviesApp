using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models
{
    public class Production
    {
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public String Name { get; set; }
    }
}
