using System;
using System.Collections.Generic;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public class LineagesContainer : MonoBehaviour
    {
        private Dictionary <int, Lineage> _lineagesList;
        public Dictionary <int, Lineage> LineagesList => _lineagesList;

        public void LoadFromXML (XmlNode input)
        {
            _lineagesList.Clear ();
            foreach (var lineageNode in XmlHelper.IterateChildren (input, "lineage"))
            {
                var lineage = Lineage.LoadFromXML (lineageNode);
                _lineagesList [lineage.ID] = lineage;
            }
        }

        private void Start ()
        {
            _lineagesList = new Dictionary <int, Lineage> ();
        }

        /* Move to XML
        public void SetLineages ()
        {
            int index = 0;
            Lineage a = new Lineage ();
            a.ID = (LineageEnum) index++;
            a.InfoAboutLineage = "Ганераты";
            a.SetCharacteristicsChanges (0, 2, 0, 0, 2);
            cont [(int) LineageEnum.Gunerates] = a;

            a = new Lineage ();
            a.ID = (LineageEnum) index++;
            a.InfoAboutLineage = "Кантиры";
            a.SetCharacteristicsChanges (2, 0, 1, 1, 0);
            cont [(int) LineageEnum.Canteers] = a;

            a = new Lineage ();
            a.ID = (LineageEnum) index++;
            a.InfoAboutLineage = "Торговцы";
            a.SetCharacteristicsChanges (1, 1, 1, 0, 1);
            cont [(int) LineageEnum.Villagers] = a;

            a = new Lineage ();
            a.ID = (LineageEnum) index++;
            a.InfoAboutLineage = "Крестьяне";
            a.SetCharacteristicsChanges (2, 0, 0, 2, 0);
            cont [(int) LineageEnum.Outlaws] = a;

            a = new Lineage ();
            a.ID = (LineageEnum) index++;
            a.InfoAboutLineage = "Преступники";
            a.SetCharacteristicsChanges (1, 1, 2, 0, 0);
            cont [(int) LineageEnum.Traders] = a;
        }*/
    }
}