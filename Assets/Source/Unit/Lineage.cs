using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using PriestOfPlague.Source.Core;

namespace PriestOfPlague.Source.Unit
{
    public class Lineage
    {
        public int ID { get; set; }
        public string InfoAboutLineage { get; set; }
        public int [] CharcsChanges;

        private Lineage (int id)
        {
            ID = id;
            InfoAboutLineage = "";
            CharcsChanges = new int[(int) CharacteristicsEnum.Count];
        }

        public void SetCharacteristicsChanges (int Vit, int Luc, int Ag, int Str, int Int)
        {
            CharcsChanges [(int) CharacteristicsEnum.Vitality] = Vit;
            CharcsChanges [(int) CharacteristicsEnum.Luck] = Luc;
            CharcsChanges [(int) CharacteristicsEnum.Agility] = Ag;
            CharcsChanges [(int) CharacteristicsEnum.Strength] = Str;
            CharcsChanges [(int) CharacteristicsEnum.Intelligence] = Int;
        }

        public static Lineage LoadFromXML (XmlNode input)
        {
            var lineage = new Lineage (XmlHelper.GetIntAttribute (input, "ID"));
            string stringData = input.Attributes ["Value"].InnerText;

            string [] separated = stringData.Split (' ');
            lineage.SetCharacteristicsChanges (
                int.Parse (separated [0], NumberFormatInfo.InvariantInfo),
                int.Parse (separated [1], NumberFormatInfo.InvariantInfo),
                int.Parse (separated [2], NumberFormatInfo.InvariantInfo),
                int.Parse (separated [3], NumberFormatInfo.InvariantInfo),
                int.Parse (separated [4], NumberFormatInfo.InvariantInfo));
            
            return lineage;
        }
    }
}