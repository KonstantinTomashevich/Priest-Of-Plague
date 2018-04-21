using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifier
    {
        public BuffsAndDebuffsEnum Index { get; set; }
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
            this.Index = (BuffsAndDebuffsEnum)index++;
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
            CharcsChanges[(int)CharactiristicsEnum.Vitality] = Vit;
            CharcsChanges[(int)CharactiristicsEnum.Luck] = Luc;
            CharcsChanges[(int)CharactiristicsEnum.Agility] = Ag;
            CharcsChanges[(int)CharactiristicsEnum.Strength] = Str;
            CharcsChanges[(int)CharactiristicsEnum.Intelligence] = Int;
        }
    }

}
