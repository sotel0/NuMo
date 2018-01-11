﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuMo
{
    //This class holds data on a single nutrient and can be displayed on the MyDay page.
    public class Nutrient : IMyDayViewItem
    {
        public String name { get; set; }
        public String quantifier { get; set; }
        public double quantity { get; set; }
        public int dbNo { get; set; }
    }
}
