using System;
using System.Collections.Generic;
using UnityEngine;

//enum Chars { Vitality, Lucky, Agility, Strength, Intelligence }
public enum LineageEnum
{
    Gunerates = 0,
    Canteers,
    Traders,
    Villagers,
    Outlaws
}

namespace PriestOfPlague.Source.Unit
{
    public class LineagesContainer : MonoBehaviour
    {
        private List <Lineage> _lineagesList;

        public Lineage GetLineage (int id)
        {
            if (id >= 0 && id < _lineagesList.Count)
            {
                return _lineagesList [id];
            }

            throw new IndexOutOfRangeException ("Unknown lineage id!");
        }
        
        private void Start ()
        {
            _lineagesList = new List <Lineage> ();
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