﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DATN.DataContextCF.Entities
{
    public class Campus
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //Danh sách chưa các tập con
        public List<Building> Buildings { get; set; }
    }
}
