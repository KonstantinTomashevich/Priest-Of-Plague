using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public enum TypesOfDamageEnum { Near = 0, OnDistance, Magic, Critical }

    public class Unit : MonoBehaviour
    {
        const int numberOfBuffsAndDebuffs = 7;
        private Lineage lineage; // private?
        public string _nameOfCharacter { get; private set; }
        public bool _isMan { get; private set; }

        Dictionary<TypesOfDamageEnum, float> _dictionaryOfResists = new Dictionary<TypesOfDamageEnum, float>();

        public float _nearDamageBust { get; private set; }
        public float _onDistanceDamageBust { get; private set; }
        public float _magicDamageBust { get; private set; }
        public float _criticalDamageBust { get; private set; }

        public float _currentHP { get; private set; }
        public float _maxHP { get; private set; }
        public float _regenOfHP { get; private set; }
        public float _currentMP { get; private set; }
        public float _maxMP { get; private set; }
        public float _regenOfMP { get; private set; }

        public int _experience { get; private set; }
        public int _maxHeightOfInvertory { get; private set; }

        int[] _arrayOfCharactiristics = new int[5];
        private float _regenOfHPForPoisoning;
        //тут конкретно моды, которые в данный момент на юните
        public List<int> ModifiersOnUnit = new List<int>();

        private void ApplyLineage()
        {
            for (int i = 0; i < 5; i++)
                _arrayOfCharactiristics[i] += lineage.CharcsChanges[i];
        }

        struct StructOfModifier
        {
            public int ID { get; set; }
            public float timeOfModifier { get; set; }
            public int levelOfModifier { get; set; }
        }

        //здесь просто все моды, которые могут быть, с базовой информацией о них
        StructOfModifier[] arrOfAllBuffs = new StructOfModifier[numberOfBuffsAndDebuffs];

        public void SetArrOfBuffs()
        {
            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
            {
                arrOfAllBuffs[i].ID = i;
                arrOfAllBuffs[i].timeOfModifier = CharacterModifiersContainer.GetBuff(i).timeOfBuff;
                arrOfAllBuffs[i].levelOfModifier = 1;
            }
        }

        private void ApplyModifier(int indexIn)
        {
            //характеристики мода к характеристикам юнита
            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] += CharacterModifiersContainer.GetBuff(indexIn).CharcsChanges[i] * arrOfAllBuffs[i].levelOfModifier;
            //реген HP для юнита *яд - особая ситуация
            if (indexIn == (int)BuffsAndDebuffsEnum.Poisoning)
            {
                _regenOfHPForPoisoning = _regenOfHP;
                _regenOfHP = CharacterModifiersContainer.GetBuff(indexIn).PlusRegen * arrOfAllBuffs[(int)BuffsAndDebuffsEnum.Poisoning].levelOfModifier;
            }
            else _regenOfHP += CharacterModifiersContainer.GetBuff(indexIn).PlusRegen * arrOfAllBuffs[indexIn].levelOfModifier;
            //для лечения особая ситуация (если таких навыков, вызывающих другие навыки или отменяющих другие навыки станет больше одного, 
            //то можно будет выделить в отдельную функцию всё это, если нужно это сделать сейчас - скажи, пожалуйста.
            if (indexIn == (int)BuffsAndDebuffsEnum.Healing)
                for (int i = 0; i < ModifiersOnUnit.Count; i++)
                    for (int j = 0; j < CharacterModifiersContainer.GetBuff(indexIn).BuffsForCancel.Count; j++)
                        if (ModifiersOnUnit[i] == CharacterModifiersContainer.GetBuff(indexIn).BuffsForCancel[j]) //если в списке баффов, что уже на юните, находит те, что нужно отменить...             
                        {
                            ModifiersOnUnit.RemoveAt(i);
                        }
            if (CharacterModifiersContainer.GetBuff(indexIn).BuffsForUsing.Count != 0)
            {
                int indexOfBuff = IsModInList(indexIn);
                for (int j = 0; j < CharacterModifiersContainer.GetBuff(indexIn).BuffsForUsing.Count; j++)
                {
                    //если накладываемого мода нет на юните, то накладываем, есть - продляем
                    if (indexOfBuff == -1)
                        ModifiersOnUnit.Add(CharacterModifiersContainer.GetBuff(indexIn).BuffsForUsing[j]);
                    else
                        arrOfAllBuffs[ModifiersOnUnit[indexOfBuff]].timeOfModifier = CharacterModifiersContainer.GetBuff(ModifiersOnUnit[indexOfBuff]).timeOfBuff;
                }
            }
        }

        private int IsModInList(int indexIn)
        {
            for (int i = 0; i < ModifiersOnUnit.Count; i++)
                if (ModifiersOnUnit[i] == indexIn)
                    return i;
            return -1;
        }

        public int GetCharacteristics(CharactiristicsEnum characteristicIn)
        {
            return _arrayOfCharactiristics[(int)characteristicIn];
        }
        public void SetCharactiristics(CharactiristicsEnum typeOfCharacteristicIn, int valueOfCharacteristicIn)
        {
            _arrayOfCharactiristics[(int)typeOfCharacteristicIn] += valueOfCharacteristicIn;
            UpdateCharacteristics();
        }

        private void UpdateCharacteristics()
        {
            //обработка силы
            _nearDamageBust += (float)(GetCharacteristics(CharactiristicsEnum.Strength) * 0.03);
            _maxHP += 5 * GetCharacteristics(CharactiristicsEnum.Strength);
            _maxMP += 2 * GetCharacteristics(CharactiristicsEnum.Strength);
            _regenOfHP += GetCharacteristics(CharactiristicsEnum.Strength);
            _maxHeightOfInvertory += 3 * GetCharacteristics(CharactiristicsEnum.Strength);

            //ловкость
            _onDistanceDamageBust += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Agility));
            _maxHP += 2 * GetCharacteristics(CharactiristicsEnum.Agility);
            _maxMP += 5 * GetCharacteristics(CharactiristicsEnum.Agility);
            _regenOfMP += GetCharacteristics(CharactiristicsEnum.Agility);

            //выносливость
            _dictionaryOfResists[TypesOfDamageEnum.Near] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
            _dictionaryOfResists[TypesOfDamageEnum.OnDistance] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
            _dictionaryOfResists[TypesOfDamageEnum.Magic] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
            _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
            _maxHP += 4 * GetCharacteristics(CharactiristicsEnum.Vitality);
            _maxMP += 4 * GetCharacteristics(CharactiristicsEnum.Vitality);
            _regenOfHP += GetCharacteristics(CharactiristicsEnum.Vitality);
            _regenOfMP += GetCharacteristics(CharactiristicsEnum.Vitality);
            _maxHeightOfInvertory += 3 * GetCharacteristics(CharactiristicsEnum.Vitality);

            //разум
            _magicDamageBust += (float)(0.2 * GetCharacteristics(CharactiristicsEnum.Intelligence));
            _maxMP += 3 * GetCharacteristics(CharactiristicsEnum.Intelligence);
            _regenOfHP += GetCharacteristics(CharactiristicsEnum.Intelligence);
            _regenOfMP += GetCharacteristics(CharactiristicsEnum.Intelligence);

            //удачливость
            _criticalDamageBust += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Luck));
            _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Luck));
            _regenOfHP += GetCharacteristics(CharactiristicsEnum.Luck);
            _regenOfMP += GetCharacteristics(CharactiristicsEnum.Luck);
        }

        // Use this for initialization
        void Start()
        {

        }

        private void ReverseCharacteristics(int indexIn)
        {
            //характеристики мода к характеристикам юнита
            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] -= CharacterModifiersContainer.GetBuff(indexIn).CharcsChanges[i] * arrOfAllBuffs[i].levelOfModifier;
            //реген HP для юнита *яд - особая ситуация
            if (indexIn == (int)BuffsAndDebuffsEnum.Poisoning)
                _regenOfHP = _regenOfHPForPoisoning;
            else _regenOfHP -= CharacterModifiersContainer.GetBuff(indexIn).PlusRegen * arrOfAllBuffs[indexIn].levelOfModifier;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < ModifiersOnUnit.Count; i++)
            {
                arrOfAllBuffs[ModifiersOnUnit[i]].timeOfModifier -= Time.deltaTime; //??
                if (arrOfAllBuffs[ModifiersOnUnit[i]].timeOfModifier <= 0)
                {
                    ModifiersOnUnit.RemoveAt(i);
                    arrOfAllBuffs[ModifiersOnUnit[i]].timeOfModifier = CharacterModifiersContainer.GetBuff(ModifiersOnUnit[i]).timeOfBuff;
                    ReverseCharacteristics(ModifiersOnUnit[i]);
                }
            }
        }
    }
}