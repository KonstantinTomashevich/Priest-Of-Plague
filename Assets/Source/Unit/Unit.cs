using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using PriestOfPlague.Source.Core;

namespace PriestOfPlague.Source.Unit
{
    public enum CharacteristicsEnum
    {
        Vitality = 0,
        Luck,
        Agility,
        Strength,
        Intelligence,
        Count
    }

    public class Unit : CreationInformer
    {
        public class AppliedModifier
        {
            public AppliedModifier (int id, float time, int level)
            {
                ID = id;
                Time = time;
                Level = level;
            }

            public int ID { get; set; }
            public float Time { get; set; }
            public int Level { get; set; }
        }

        public CharacterModifiersContainer characterModifiersContainer;
        public string Name { get; set; }
        public bool IsMan { get; set; }

        public float NearDamageBust { get; private set; }
        public float OnDistanceDamageBust { get; private set; }
        public float MagicDamageBust { get; private set; }
        public float CriticalDamageChance { get; private set; }
        public float CriticalResistChance { get; private set; }

        public float CurrentHP { get; private set; }
        public float MaxHP { get; private set; }
        public float RegenOfHP { get; private set; }
        public float CurrentMP { get; private set; }
        public float MaxMP { get; private set; }
        public float RegenOfMP { get; private set; }

        public int Experience { get; private set; }
        public int MaxInventoryWeight { get; private set; }

        private int [] _charactiristics;
        private List <AppliedModifier> ModifiersOnUnit;
        private float [] _resists;
        private int _lineageID = -1;

        private bool _hpRegenerationBlocked = false;
        private bool _mpRegenerationBlocked = false;
        private bool _movementBlocked = false;
        private float _unblockableHPRegeneration = 0;
        private float _unblockableMPRegeneration = 0;

        public void ApplyLineage (int lineageIndex, LineagesContainer container)
        {
            if (lineageIndex != -1)
            {
                for (int i = 0; i < 5; i++)
                {
                    _charactiristics [i] -= container.GetLineage (_lineageID).CharcsChanges [i];
                }
            }

            _lineageID = lineageIndex;
            for (int i = 0; i < 5; i++)
            {
                _charactiristics [i] += container.GetLineage (_lineageID).CharcsChanges [i];
            }
        }

        private void ApplyModifier (int id, int level)
        {
            var modifierType = characterModifiersContainer.GetBuff (id);
            var modifier = new AppliedModifier (id, modifierType.timeOfBuff * level, level);
            ModifiersOnUnit.Add (modifier);

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                _charactiristics [i] += modifierType.CharcsChanges [i] * level;

            _unblockableHPRegeneration += modifierType._unblockableHPRegeneration * level;
            _unblockableMPRegeneration += modifierType._unblockableMPRegeneration * level;
            _hpRegenerationBlocked = modifierType._blocksHpRegeneration;
            _mpRegenerationBlocked = modifierType._blocksMpRegeneration;
            _movementBlocked = modifierType._blocksMovement;

            int modifierIndex = 0;
            while (modifierIndex < ModifiersOnUnit.Count)
            {
                var anotherModifier = ModifiersOnUnit [modifierIndex];
                if (modifierType.BuffsToCancel.Contains (anotherModifier.ID))
                {
                    RemoveModifier (anotherModifier);
                    ModifiersOnUnit.RemoveAt (modifierIndex);
                }
                else
                {
                    modifierIndex++;
                }
            }

            foreach (var anotherId in modifierType.BuffsToApply)
            {
                ApplyModifier (anotherId, level);
            }

            RecalculateChildCharacteristics ();
        }

        public int GetCharacteristic (CharacteristicsEnum characteristicIn)
        {
            return _charactiristics [(int) characteristicIn];
        }

        public void SetCharactiristic (CharacteristicsEnum typeOfCharacteristicIn, int valueOfCharacteristicIn)
        {
            _charactiristics [(int) typeOfCharacteristicIn] += valueOfCharacteristicIn;
            RecalculateChildCharacteristics ();
        }

        public void RecalculateChildCharacteristics ()
        {
            NearDamageBust = 0;
            MaxHP = 0;
            MaxMP = 0;
            RegenOfHP = 0;
            RegenOfMP = 0;
            MaxInventoryWeight = 0;

            OnDistanceDamageBust = 0;
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                _resists [index] = 0;
            }

            MagicDamageBust = 0;
            CriticalDamageChance = 0;
            CriticalResistChance = 0;
            
            int strength = GetCharacteristic (CharacteristicsEnum.Strength);
            if (strength <= 0)
                strength = 1;
            int agility = GetCharacteristic (CharacteristicsEnum.Agility);
            if (agility <= 0)
                agility = 1;
            int vitality = GetCharacteristic (CharacteristicsEnum.Vitality);
            if (vitality <= 0)
                vitality = 1;
            int intelligence = GetCharacteristic (CharacteristicsEnum.Intelligence);
            if (intelligence <= 0)
                intelligence = 1;
            int luck = GetCharacteristic (CharacteristicsEnum.Luck);
            if (luck <= 0)
                luck = 1;

            //обработка силы            
            NearDamageBust += (float) ((float) strength * 0.03);
            MaxHP += 5 * strength;
            MaxMP += 2 * strength;
            RegenOfHP += strength;
            MaxInventoryWeight += 3 * strength;

            //ловкость
            OnDistanceDamageBust += (float) (0.03 * (float) agility);
            MaxHP += 2 * agility;
            MaxMP += 5 * agility;
            RegenOfMP += agility;

            //выносливость
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                _resists [index] += (float) (0.03 * vitality);
            }
            
            MaxHP += 4 * vitality;
            MaxMP += 4 * vitality;
            RegenOfHP += vitality;
            RegenOfMP += vitality;
            MaxInventoryWeight += 3 * vitality;

            //разум
            MagicDamageBust += (float) (0.2 * (float) intelligence);
            MaxMP += 3 * intelligence;
            RegenOfHP += intelligence;
            RegenOfMP += intelligence;

            //удачливость
            CriticalDamageChance += (float) (0.03 * (float) luck);
            CriticalResistChance += (float) (0.03 * (float) luck);
            RegenOfHP += luck;
            RegenOfMP += luck;
        }

        private void RemoveModifier (AppliedModifier modifier)
        {
            CharacterModifier modifierType = characterModifiersContainer.GetBuff (modifier.ID);

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                _charactiristics [i] -= modifierType.CharcsChanges [i] * modifier.Level;

            _unblockableHPRegeneration -= modifierType._unblockableHPRegeneration * modifier.Level;
            _unblockableMPRegeneration -= modifierType._unblockableMPRegeneration * modifier.Level;

            _hpRegenerationBlocked = false;
            _mpRegenerationBlocked = false;
            _movementBlocked = false;

            foreach (var anotherModifier in ModifiersOnUnit)
            {
                if (anotherModifier != modifier)
                {
                    _hpRegenerationBlocked |= characterModifiersContainer.GetBuff (modifier.ID)._blocksHpRegeneration;
                    _mpRegenerationBlocked |= characterModifiersContainer.GetBuff (modifier.ID)._blocksMpRegeneration;
                    _movementBlocked |= characterModifiersContainer.GetBuff (modifier.ID)._blocksMovement;
                }
            }

            RecalculateChildCharacteristics ();
        }

        new void Start ()
        {
            _charactiristics = new int[(int) CharacteristicsEnum.Count];
            _resists = new float[(int) DamageTypesEnum.Count];
            ModifiersOnUnit = new List <AppliedModifier> ();
            base.Start ();
        }

        new void OnDestroy ()
        {
            base.OnDestroy ();
        }

        void Update ()
        {
            if (!_hpRegenerationBlocked)
            {
                CurrentHP += RegenOfHP * Time.deltaTime;
            }
            CurrentHP += _unblockableHPRegeneration * Time.deltaTime;
            
            if (!_mpRegenerationBlocked)
            {
                CurrentMP += RegenOfMP * Time.deltaTime;
            }
            CurrentMP += _unblockableMPRegeneration * Time.deltaTime;

            int modifierIndex = 0;
            while (modifierIndex < ModifiersOnUnit.Count)
            {
                var modifier = ModifiersOnUnit [modifierIndex];
                modifier.Time -= Time.deltaTime;

                if (modifier.Time <= 0.0f)
                {
                    RemoveModifier (modifier);
                    ModifiersOnUnit.RemoveAt (modifierIndex);
                }
                else
                {
                    modifierIndex++;
                }
            }
        }
    }
}