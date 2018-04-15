
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;

//enum Chars { Vitality, Lucky, Agility, Strength, Intelligence }
public enum LineageEnum { Ганераты = 0, Кантиры, Торговцы, Крестьяне, Преступники }

namespace PriestOfPlague.Source.Unit
{
    /// <summary>
    /// Класс содержащий в себе список происхождений
    /// </summary>
    public class LineagesContainer : MonoBehaviour
    {
        private const int NumberOfLineages = 5;
        //Dictionary<int, Lineage> cont = new Dictionary<int, Lineage>(5);
        Lineage[] cont = new Lineage[5];
        /// <summary>
        /// Метод возвращает нужное происхождение
        /// </summary>
        /// <param name="index">Индекс происхождения, которое мы хотим получить</param>
        /// <returns></returns>
        public Lineage GetLineage(int IndexIn)
        {
            if (cont[IndexIn] != null)
                return cont[IndexIn];
            else throw new System.Exception("Неверный индекс!");
        }

        public void SetLineages()
        {
            int index = 0;
            Lineage a = new Lineage();
            a.Index = (LineageEnum)index++;
            a.InfoAboutLineageInString = "Ганераты";
            a.SetArr(0, 2, 0, 0, 2);
            cont[(int)LineageEnum.Ганераты] = a;

            a.Index = (LineageEnum)index++;
            a.InfoAboutLineageInString = "Кантиры";
            a.SetArr(2, 0, 1, 1, 0);
            cont[(int)LineageEnum.Кантиры] = a;

            a.Index = (LineageEnum)index++;
            a.InfoAboutLineageInString = "Торговцы";
            a.SetArr(1, 1, 1, 0, 1);
            cont[(int)LineageEnum.Крестьяне] = a;

            a.Index = (LineageEnum)index++;
            a.InfoAboutLineageInString = "Крестьяне";
            a.SetArr(2, 0, 0, 2, 0);
            cont[(int)LineageEnum.Преступники] = a;

            a.Index = (LineageEnum)index++;
            a.InfoAboutLineageInString = "Преступники";
            a.SetArr(1, 1, 2, 0, 0);
            cont[(int)LineageEnum.Торговцы] = a;
        }

        // Use this for initialization
        void Start() //тут он должен в массив lineages засунуть из json-файла
        {
        }

        // Update is called once per frame
        void Update()
        {


        }
    }
}