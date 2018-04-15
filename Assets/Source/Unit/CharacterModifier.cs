using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifier
    {
        public BuffsAndDebuffs Index { get; set; }
        public string InfoAboutBuffsInString { get; set; }
        public double timeOfBuff;
        public int[] CharcsChanges = new int[5];
        private static int index = 0;
        public int PlusRegen { set; get; }
        public List<int> BuffsForUsing = new List<int>();
        public List<int> BuffsForCancel = new List<int>();

        /// <summary>
        /// Конструктор для изначальной информации о баффах
        /// </summary>
        /// <param name="infoIn"> Информация о баффе</param>
        /// <param name="timeIn">Время действия баффа</param>
        public CharacterModifier(string infoIn, int timeIn)
        {
            this.Index = (BuffsAndDebuffs)index++;
            this.InfoAboutBuffsInString = infoIn;
            this.timeOfBuff = timeIn;           
        }

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
            CharcsChanges[(int)Charactiristics.Vitality] = Vit;
            CharcsChanges[(int)Charactiristics.Lucky] = Luc;
            CharcsChanges[(int)Charactiristics.Agility] = Ag;
            CharcsChanges[(int)Charactiristics.Strength] = Str;
            CharcsChanges[(int)Charactiristics.Intelligence] = Int;
        }
    }

}
