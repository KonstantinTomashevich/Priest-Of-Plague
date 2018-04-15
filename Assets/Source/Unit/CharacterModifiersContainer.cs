using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Charactiristics { Vitality, Lucky, Agility, Strength, Intelligence }

namespace Assets._scripts
{
    public class CharacterModifiersContainer : MonoBehaviour
    {
        Dictionary<int, CharacterModifier> dict = new Dictionary<int, CharacterModifier>(7); //тут что-то не так
        /// <summary>
        /// Даёт баффы по индексу
        /// </summary>
        /// <param name="IndexIn">Унивкальынй индекс баффа</param>
        /// <returns>бафф</returns>
        public CharacterModifier GetBuff(int IndexIn)
        {
            if (dict.ContainsKey(IndexIn))
                return dict[IndexIn];
            else throw new System.Exception("Неверный индекс!");
        }
        /// <summary>
        /// Устанавливает все баффы
        /// </summary>
        public void SetBuffs()
        {
            //??
            //может создать enum с именами баффов, раз индексы теперь int?
            //??
            //Лечение
            CharacterModifier a = new CharacterModifier("Лечение", 5);
            a.SetArr(0, 0, 0, 0, 0);
            a.PlusRegen = 3;
            a.BuffsForCancel.Add(5);
            a.BuffsForCancel.Add(3);
            dict.Add(a.Index, a);   

            //Сытость
            a = new CharacterModifier("Сытость", 5);
            a.SetArr(1, 0, 1, 1, 0);
            dict.Add(a.Index, a);

            //Магическая поддержка
            a = new CharacterModifier("Магическая поддержка", 5);
            a.SetArr(1, 1, 1, 1, 1);
            dict.Add(a.Index, a);

            //Ослаблен
            a = new CharacterModifier("Ослабление", 5);
            a.SetArr(-1, 0, -1, -1, 0);
            dict.Add(a.Index, a);

            //Отравлен
            a = new CharacterModifier("Отравление", 5);
            a.SetArr(-1, 0, -1, -1, 0);
            a.PlusRegen = -1;
            dict.Add(a.Index, a);

            //Болен 
            a = new CharacterModifier("Болезнь", 5);
            a.SetArr(-1, -1, -1, -1, -1);
            dict.Add(a.Index, a);

            //Парализован(Опять же не реализовать, так как нужно понимание действий (класс unit))
            a = new CharacterModifier("Парализация", 5);
            a.SetArr(0, 0, 0, 0, 0);
            dict.Add(a.Index, a);
        }

        /// <summary>
        /// Класс содержит информацию о баффах/дебаффах
        /// </summary>

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}