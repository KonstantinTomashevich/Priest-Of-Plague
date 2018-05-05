﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    /// <summary>
    /// Класс происхождений
    /// </summary>
    public class Lineage
    {
        public LineageEnum Index { get; set; }
        public string InfoAboutLineageInString { get; set; }
        public int[] CharcsChanges = new int[5];

        /// <summary>
        /// Метод заполняет массив-проихождение значениями, на которое данное происхождение меняет указанные характеристики
        /// </summary>
        /// <param name="Vit">Выносливость</param>
        /// <param name="Luc">Удачи</param>
        /// <param name="Ag">Ловкость</param>
        /// <param name="Str">Сила</param>
        /// <param name="Int">Разум</param>
        /// 
        public void SetArr(int Vit, int Luc, int Ag, int Str, int Int)
        {
            CharcsChanges[(int)CharactiristicsEnum.Vitality] = Vit;
            CharcsChanges[(int)CharactiristicsEnum.Luck] = Luc;
            CharcsChanges[(int)CharactiristicsEnum.Agility] = Ag;
            CharcsChanges[(int)CharactiristicsEnum.Strength] = Str;
            CharcsChanges[(int)CharactiristicsEnum.Intelligence] = Int;
        }
    }

}