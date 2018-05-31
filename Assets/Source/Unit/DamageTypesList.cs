using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    public enum DamageTypesEnum
    {
        Cutting,
        Pricking,
        ArmorPiercing,
        Bumping,
        Bright,
        Flamy,
        Icy,
        Lighting,
        Count
    }

    public static class DamageTypesStrings
    {
        public static string [] Names =
        {
            "Cutting",
            "Priking",
            "Armor Piercing",
            "Bumping",
            "Bright",
            "Flamy",
            "Icy",
            "Lighting"
        };
    }
}