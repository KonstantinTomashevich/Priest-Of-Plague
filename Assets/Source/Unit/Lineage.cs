using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    public class Lineage
    {
        public LineageEnum ID { get; set; }
        public string InfoAboutLineage { get; set; }
        public int [] CharcsChanges = new int[(int) CharacteristicsEnum.Count];

        public void SetCharacteristicsChanges (int Vit, int Luc, int Ag, int Str, int Int)
        {
            CharcsChanges [(int) CharacteristicsEnum.Vitality] = Vit;
            CharcsChanges [(int) CharacteristicsEnum.Luck] = Luc;
            CharcsChanges [(int) CharacteristicsEnum.Agility] = Ag;
            CharcsChanges [(int) CharacteristicsEnum.Strength] = Str;
            CharcsChanges [(int) CharacteristicsEnum.Intelligence] = Int;
        }
    }
}