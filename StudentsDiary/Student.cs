﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Math { get; set; }
        public string Technilogy { get; set; }
        public string Physics { get; set; }
        public string PolishLang { get; set; }
        public string ForeingLang { get; set; }
        public bool Activities { get; set; }
        public Group Group { get; set; }

    }
}
