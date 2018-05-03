using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace NuMo
{
    public class NutrientThresholds
    {
        IDictionary<String, Double[]> items;

        public NutrientThresholds()
        {
            items = new Dictionary<String, Double[]>();

            double[] QDRI1 = { 0, 0 };
            items.Add("Total Sugar(g)", QDRI1);

            //protein
            //double[] QDRI1 = { 0, 0 }; 
            //items.Add("Protein(g)", QDRI1);

        }
    }
}

