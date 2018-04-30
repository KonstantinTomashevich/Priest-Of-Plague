using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public enum TypesOfDamageEnum { Near = 0, OnDistance, Magic, Critical }

    public class Unit : MonoBehaviour
    {
        const int numberOfBuffsAndDebuffs = 7;
        private int lineage = -1;
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

        //Контейнер для хранения модов, которые сейчас на юните
        public List<StructOfModifier> ModifiersOnUnit = new List<StructOfModifier>();

        private bool _hpRegenerationBlocked;
        private bool _mpRegenerationBlocked;
        private bool _movementBlocked;
        private float _unblockableHPRegeneration;
        private float _unblockableMPRegeneration;

        private void ApplyLineage(int lineageIndex, LineagesContainer container)
        {
            if (lineageIndex != -1)
            {
                for (int i = 0; i < 5; i++)
                {
                    _arrayOfCharactiristics[i] += container.GetLineage(lineageIndex).CharcsChanges[i];
                    _arrayOfCharactiristics[i] -= container.GetLineage(lineage).CharcsChanges[i];
                }
                lineage = lineageIndex;
            }
        }

        public class StructOfModifier //Было сказано использовать структуру, но с ней работало только под костылями
        {
            public StructOfModifier(int IDin, float timeOfModifierIn, int levelOfModifierIn)
            {
                ID = IDin;
                timeOfModifier = timeOfModifierIn;
                levelOfModifier = levelOfModifierIn;
            }
            public int ID { get; set; }
            public float timeOfModifier { get; set; }
            public int levelOfModifier { get; set; }
        }

        //всё переделать к чёртовой матери
        private void ApplyModifier(int indexIn, CharacterModifiersContainer container)
        {
            //характеристики мода к характеристикам юнита
            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] += container.GetBuff(indexIn).CharcsChanges[i] * ModifiersOnUnit[i].levelOfModifier;
            //реген HP для юнита *яд - особая ситуация
            if (indexIn == (int)BuffsAndDebuffsEnum.Poisoning)
            {
                _hpRegenerationBlocked = true;
                _unblockableHPRegeneration += container.GetBuff(indexIn)._unblockableHPRegeneration * ModifiersOnUnit[(int)BuffsAndDebuffsEnum.Poisoning].levelOfModifier;
            }
            else _regenOfHP += container.GetBuff(indexIn).PlusRegen * ModifiersOnUnit[indexIn].levelOfModifier; //Хил идёт сюда же, так можно? Иначе яд после хила будет едва заметен
            //для лечения особая ситуация (если таких навыков, вызывающих другие навыки или отменяющих другие навыки станет больше одного, 
            //то можно будет выделить в отдельную функцию всё это, если нужно это сделать сейчас - скажи, пожалуйста.
            if (indexIn == (int)BuffsAndDebuffsEnum.Healing)
                for (int i = 0; i < ModifiersOnUnit.Count; i++)
                    for (int j = 0; j < container.GetBuff(indexIn).BuffsForCancel.Count; j++)
                        if (ModifiersOnUnit[i].ID == container.GetBuff(indexIn).BuffsForCancel[j]) //если в списке баффов, которые уже на юните, находит те, что нужно отменить...             
                        {
                            ModifiersOnUnit.RemoveAt(i);
                            if (ModifiersOnUnit[i].ID == (int)BuffsAndDebuffsEnum.Poisoning)
                                _hpRegenerationBlocked = false; //только для хила
                        }
            if (container.GetBuff(indexIn).BuffsForUsing.Count != 0)
            {
                int indexOfBuff = IsModInList(indexIn);
                for (int j = 0; j < container.GetBuff(indexIn).BuffsForUsing.Count; j++)
                {
                    //если накладываемого мода нет на юните, то накладываем, есть - продляем
                    int IndexOfUsingMod = container.GetBuff(indexIn).BuffsForUsing[j]; //индекс мода, который мы хотим наложить дополнительно
                    if (indexOfBuff == -1)
                        ModifiersOnUnit.Add(new StructOfModifier(IndexOfUsingMod, container.GetBuff(IndexOfUsingMod).timeOfBuff, ModifiersOnUnit[IndexOfUsingMod].levelOfModifier));
                    else
                        ModifiersOnUnit[indexOfBuff].timeOfModifier = container.GetBuff(ModifiersOnUnit[indexOfBuff].ID).timeOfBuff;
                }
            }
            if (container.GetBuff(indexIn)._blocksMovement)
                _movementBlocked = true;
            UpdateCharacteristics();
        }

        private int IsModInList(int indexIn)
        {
            for (int i = 0; i < ModifiersOnUnit.Count; i++)
                if (ModifiersOnUnit[i].ID == indexIn)
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
            int strength = GetCharacteristics(CharactiristicsEnum.Strength);
            if (strength <= 0)
                strength = 1;
            int agility = GetCharacteristics(CharactiristicsEnum.Agility);
            if (agility <= 0)
                agility = 1;
            int vitality = GetCharacteristics(CharactiristicsEnum.Vitality);
            if (vitality <= 0)
                vitality = 1;
            int intelligence = GetCharacteristics(CharactiristicsEnum.Intelligence);
            if (intelligence <= 0)
                intelligence = 1;
            int luck = GetCharacteristics(CharactiristicsEnum.Luck);
            if (luck <= 0)
                luck = 1;

            //обработка силы            
            _nearDamageBust += (float)((float)strength * 0.03);
            _maxHP += 5 * strength;
            _maxMP += 2 * strength;
            _regenOfHP += strength;
            _maxHeightOfInvertory += 3 * strength;

            //ловкость
            _onDistanceDamageBust += (float)(0.03 * (float)agility);
            _maxHP += 2 * agility;
            _maxMP += 5 * agility;
            _regenOfMP += agility;

            //выносливость
            _dictionaryOfResists[TypesOfDamageEnum.Near] += (float)(0.03 * (float)vitality);
            _dictionaryOfResists[TypesOfDamageEnum.OnDistance] += (float)(0.03 * (float)vitality);
            _dictionaryOfResists[TypesOfDamageEnum.Magic] += (float)(0.03 * (float)vitality);
            _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * (float)vitality);
            _maxHP += 4 * vitality;
            _maxMP += 4 * vitality;
            _regenOfHP += vitality;
            _regenOfMP += vitality;
            _maxHeightOfInvertory += 3 * vitality;

            //разум
            _magicDamageBust += (float)(0.2 * (float)intelligence);
            _maxMP += 3 * intelligence;
            _regenOfHP += intelligence;
            _regenOfMP += intelligence;

            //удачливость
            _criticalDamageBust += (float)(0.03 * (float)luck);
            _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * (float)luck);
            _regenOfHP += luck;
            _regenOfMP += luck;
        }

        // Use this for initialization
        void Start()
        {

        }

        private void ReverseCharacteristics(int indexIn, CharacterModifiersContainer container)
        {
            //характеристики мода к характеристикам юнита
            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] -= container.GetBuff(indexIn).CharcsChanges[i] * ModifiersOnUnit[i].levelOfModifier;
            _regenOfHP -= container.GetBuff(indexIn).PlusRegen * ModifiersOnUnit[indexIn].levelOfModifier;
            _unblockableHPRegeneration -= container.GetBuff(indexIn)._unblockableHPRegeneration * ModifiersOnUnit[indexIn].levelOfModifier;
            if (indexIn == (int)BuffsAndDebuffsEnum.Poisoning)
                _hpRegenerationBlocked = false;
            if (container.GetBuff(indexIn)._blocksMovement)
                _movementBlocked = false;
        }

        // Update is called once per frame
        void Update()
        {
            //сделать, чтобы делалось не за раз, а за секунду столько хилилось
            if (!_hpRegenerationBlocked)
            {
                _currentHP += _regenOfHP;
            }
            _currentHP += _unblockableHPRegeneration;
            if (!_mpRegenerationBlocked)
            {
                _currentMP += _regenOfMP;
            }
            _currentMP += _unblockableMPRegeneration;
            //проходится по всем наложенным баффам
            for (int i = 0; i < ModifiersOnUnit.Count; i++)
            {
                ModifiersOnUnit[i].timeOfModifier -= Time.deltaTime;
                if (ModifiersOnUnit[i].timeOfModifier <= 0)
                {
                    ReverseCharacteristics(ModifiersOnUnit[i].ID, new CharacterModifiersContainer());
                    ModifiersOnUnit.RemoveAt(i);
                    UpdateCharacteristics();
                }
            }
        }
    }
}