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
        public float _criticalDamageChance { get; private set; }

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

        private bool _hpRegenerationBlocked = false;
        private bool _mpRegenerationBlocked = false;
        private bool _movementBlocked = false;
        private float _unblockableHPRegeneration = 0;
        private float _unblockableMPRegeneration = 0;

        public void ApplyLineage(int lineageIndex, LineagesContainer container)
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

        public class StructOfModifier
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

        private void ApplyModifier(int indexIn, CharacterModifiersContainer container)
        {
            //Вопрос: в какой момент задаётся умножение на уровень? По идее в этом месте нужно положить в ModifiersOnUnit структуру с нужным баффом, однако где взять level?
            //Это вопрос чисто по механике игры: что будет происходить внутри программы, когда на другого персонажа будет кастоваться скилл? Что именно и как пошлётся?
            CharacterModifier modIn = container.GetBuff(indexIn);

            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] += modIn.CharcsChanges[i] * ModifiersOnUnit[i].levelOfModifier;

            _regenOfHP += modIn.PlusRegen;//а где аналог для MP?
            _unblockableHPRegeneration += modIn._unblockableHPRegeneration; //умножение на level
            _unblockableMPRegeneration += modIn._unblockableMPRegeneration; //умножение на level
            _hpRegenerationBlocked = modIn._blocksHpRegeneration;
            _mpRegenerationBlocked = modIn._blocksMpRegeneration;
            _movementBlocked = modIn._blocksMovement;

            for (int i = 0; i < ModifiersOnUnit.Count; i++)
                for (int j = 0; j < modIn.BuffsForCancel.Count; j++)
                    if (ModifiersOnUnit[i].ID == modIn.BuffsForCancel[j])
                    {
                        ReverseCharacteristics(ModifiersOnUnit[i].ID, new CharacterModifiersContainer());
                        ModifiersOnUnit.RemoveAt(i);
                    }

            for (int j = 0; j < modIn.BuffsForUsing.Count; j++)
                ApplyModifier(modIn.BuffsForUsing[j], new CharacterModifiersContainer());
            UpdateCharacteristics();
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
            _criticalDamageChance += (float)(0.03 * (float)luck);
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
            CharacterModifier modIn = container.GetBuff(indexIn);

            for (int i = 0; i < numberOfBuffsAndDebuffs; i++)
                _arrayOfCharactiristics[i] -= modIn.CharcsChanges[i] * ModifiersOnUnit[i].levelOfModifier;

            _regenOfHP -= modIn.PlusRegen;
            _unblockableHPRegeneration -= modIn._unblockableHPRegeneration; //умножение на level
            _unblockableMPRegeneration -= modIn._unblockableMPRegeneration; //умножение на level

            if (modIn._blocksHpRegeneration)
                _hpRegenerationBlocked = false;
            if (modIn._blocksMpRegeneration)
                _mpRegenerationBlocked = false;
            if (modIn._blocksMovement)
                _movementBlocked = false;
            UpdateCharacteristics();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_hpRegenerationBlocked)
            {
                _currentHP += _regenOfHP * Time.deltaTime;
            }
            _currentHP += _unblockableHPRegeneration * Time.deltaTime;
            if (!_mpRegenerationBlocked)
            {
                _currentMP += _regenOfMP * Time.deltaTime;
            }
            _currentMP += _unblockableMPRegeneration * Time.deltaTime;

            List<int> arrOfRemoving = new List<int>();
            for (int i = 0; i < ModifiersOnUnit.Count; i++)
            {
                ModifiersOnUnit[i].timeOfModifier -= Time.deltaTime;
                if (ModifiersOnUnit[i].timeOfModifier <= 0)
                {
                    ReverseCharacteristics(ModifiersOnUnit[i].ID, new CharacterModifiersContainer());
                    arrOfRemoving.Add(i);
                    UpdateCharacteristics();
                }
            }
            for (int i = arrOfRemoving.Count - 1; i >= 0; i--)
                ModifiersOnUnit.RemoveAt(i);
        }
    }
}