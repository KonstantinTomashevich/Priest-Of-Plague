
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;

//enum Chars { Vitality, Lucky, Agility, Strength, Intelligence }

namespace Assets._scripts
{
    /// <summary>
    /// Класс содержащий в себе список происхождений
    /// </summary>
    public class LineagesContainer : MonoBehaviour
    {
        private const int NumberOfLineages = 5;
        Dictionary<string, Lineage> cont = new Dictionary<string, Lineage>(5);
        /// <summary>
        /// Метод возвращает нужное происхождение
        /// </summary>
        /// <param name="index">Индекс происхождения, которое мы хотим получить</param>
        /// <returns></returns>
        public Lineage GetLineage(string Index)
        {
            if (cont.ContainsKey(Index)) // нужно ли
                return cont[Index];
            else throw new System.Exception("Wrong Index!");
        }
        /// <summary>
        /// Метод считывает происхождения из json файла
        /// </summary>
        public void SetLineages()
        {
            Lineage a = new Lineage();
            a.Index = "1";
            a.InfoAboutLineageInString = "Ганераты";
            a.SetArr(0, 2, 0, 0, 2);
            cont.Add(a.Index, a);
            
            a.Index = "2";
            a.InfoAboutLineageInString = "Кантиры";
            a.SetArr(2, 0, 1, 1, 0);
            cont.Add(a.Index, a);

            a.Index = "3";
            a.InfoAboutLineageInString = "Торговцы";
            a.SetArr(1, 1, 1, 0, 1);
            cont.Add(a.Index, a);

            a.Index = "4";
            a.InfoAboutLineageInString = "Крестьяне";
            a.SetArr(2, 0, 0, 2, 0);
            cont.Add(a.Index, a);

            a.Index = "5";
            a.InfoAboutLineageInString = "Преступники";
            a.SetArr(1, 1, 2, 0, 0);
            cont.Add(a.Index, a);
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