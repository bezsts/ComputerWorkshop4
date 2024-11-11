﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiDomain.Enums;

namespace ApiDomain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Director { get; set; }
        public Genre Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
