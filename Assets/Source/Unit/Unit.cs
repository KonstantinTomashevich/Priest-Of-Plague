﻿using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using PriestOfPlague.Source.Core;
using PriestOfPlague.Source.Items;
using UnityEngine;

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

        public CharacterModifiersContainer CharacterModifiersContainerRef;
        public LineagesContainer LineagesContainerRef;
        public ItemTypesContainer ItemTypesContainerRef;
        public ItemsRegistrator ItemsRegistratorRef;
        
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsMan { get; set; }
        public Storage MyStorage { get; private set; }
        public Equipment MyEquipment { get; private set; }

        public float NearDamageBust { get; private set; }
        public float OnDistanceDamageBust { get; private set; }
        public float MagicDamageBust { get; private set; }
        public float CriticalDamageChance { get; private set; }
        public float CriticalResistChance { get; private set; }

        public float CurrentHp { get; private set; }
        public float MaxHp { get; private set; }
        public float RegenOfHp { get; private set; }
        public float CurrentMp { get; private set; }
        public float MaxMp { get; private set; }
        public float RegenOfMp { get; private set; }

        public int Experience { get; private set; }
        public int MaxStorageWeight { get; private set; }
        
        public List <AppliedModifier> ModifiersOnUnit { get; private set; }
        public List <int> AvailableSpells { get; private set; }

        public int [] Charactiristics { get; private set; }
        public float [] Resists { get; private set; }
        public int LineageId { get; private set; }

        public bool HpRegenerationBlocked { get; private set; }
        public bool MpRegenerationBlocked { get; private set; }
        public bool MovementBlocked { get; private set; }
        public float UnblockableHpRegeneration { get; private set; }
        public float UnblockableMpRegeneration { get; private set; }

        public void ApplyDamage (float damage, DamageTypesEnum type)
        {
            CurrentHp -= damage * Resists [(int) type];
            // TODO: Death logic.
        }

        public void ApplyLineage (int lineageIndex)
        {
            Lineage lineage;
            if (lineageIndex != -1)
            {
                lineage = LineagesContainerRef.LineagesList [LineageId];
                for (int i = 0; i < 5; i++)
                {
                    Charactiristics [i] -= lineage.CharcsChanges [i];
                }
            }

            LineageId = lineageIndex;
            lineage = LineagesContainerRef.LineagesList [LineageId];

            for (int i = 0; i < 5; i++)
            {
                Charactiristics [i] += lineage.CharcsChanges [i];
            }
        }

        public void ApplyModifier (int id, int level)
        {
            var modifierType = CharacterModifiersContainerRef.Modifiers [id];
            var modifier = new AppliedModifier (id, modifierType.TimeOfBuff * level, level);
            ModifiersOnUnit.Add (modifier);

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                Charactiristics [i] += modifierType.CharcsChanges [i] * level;

            UnblockableHpRegeneration += modifierType.UnblockableHpRegeneration * level;
            UnblockableMpRegeneration += modifierType.UnblockableMpRegeneration * level;
            HpRegenerationBlocked |= modifierType.BlocksHpRegeneration;
            MpRegenerationBlocked |= modifierType.BlocksMpRegeneration;
            MovementBlocked |= modifierType.BlocksMovement;

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
            return Charactiristics [(int) characteristicIn];
        }

        public void SetCharactiristic (CharacteristicsEnum typeOfCharacteristicIn, int valueOfCharacteristicIn)
        {
            Charactiristics [(int) typeOfCharacteristicIn] += valueOfCharacteristicIn;
            RecalculateChildCharacteristics ();
        }

        public void RecalculateChildCharacteristics ()
        {
            NearDamageBust = 0;
            MaxHp = 0;
            MaxMp = 0;
            RegenOfHp = 0;
            RegenOfMp = 0;
            MaxStorageWeight = 0;

            OnDistanceDamageBust = 0;
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                Resists [index] = 0;
            }

            MagicDamageBust = 0;
            CriticalDamageChance = 0;
            CriticalResistChance = 0;

            int strength = GetCharacteristic (CharacteristicsEnum.Strength);
            if (strength < 1) strength = 1;
            
            int agility = GetCharacteristic (CharacteristicsEnum.Agility);
            if (agility < 1) agility = 1;
            
            int vitality = GetCharacteristic (CharacteristicsEnum.Vitality);
            if (vitality < 1) vitality = 1;
            
            int intelligence = GetCharacteristic (CharacteristicsEnum.Intelligence);
            if (intelligence < 1) intelligence = 1;
            
            int luck = GetCharacteristic (CharacteristicsEnum.Luck);
            if (luck < 1) luck = 1;

            //обработка силы            
            NearDamageBust += (float) (strength * 0.03);
            MaxHp += 5 * strength;
            MaxMp += 2 * strength;
            RegenOfHp += strength;
            MaxStorageWeight += 3 * strength;

            //ловкость
            OnDistanceDamageBust += (float) (0.03 * agility);
            MaxHp += 2 * agility;
            MaxMp += 5 * agility;
            RegenOfMp += agility;

            //выносливость
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                Resists [index] += (float) (0.03 * vitality);
            }

            MaxHp += 4 * vitality;
            MaxMp += 4 * vitality;
            RegenOfHp += vitality;
            RegenOfMp += vitality;
            MaxStorageWeight += 3 * vitality;

            //разум
            MagicDamageBust += (float) (0.2 * intelligence);
            MaxMp += 3 * intelligence;
            RegenOfHp += intelligence;
            RegenOfMp += intelligence;

            //удачливость
            CriticalDamageChance += (float) (0.03 * luck);
            CriticalResistChance += (float) (0.03 * luck);
            RegenOfHp += luck;
            RegenOfMp += luck;
            MyStorage.MaxWeight = MaxStorageWeight;
        }

        public void LoadFromXML (XmlNode input)
        {
            ID = XmlHelper.GetIntAttribute (input, "LineageID");
            Name = input.Attributes ["Name"].InnerText;
            IsMan = XmlHelper.GetBoolAttribute (input, "IsMan");
            Experience = XmlHelper.GetIntAttribute (input, "Experience");
            
            string charsStringData = input.Attributes ["Characteristics"].InnerText;
            string [] charsSeparated = charsStringData.Split (' ');
            
            for (int index = 0; index < charsSeparated.Length; index++)
            {
                Charactiristics [index] =
                    int.Parse (charsSeparated [index], NumberFormatInfo.InvariantInfo);
            }
            
            ModifiersOnUnit.Clear ();
            foreach (var modifier in XmlHelper.IterateChildren (input, "modifier"))
            {
                // TODO: Not a best way.
                ApplyModifier (XmlHelper.GetIntAttribute (modifier, "ID"),
                    XmlHelper.GetIntAttribute (modifier, "Level"));
            }
            
            string resistsStringData = input.Attributes ["Resists"].InnerText;
            string [] resistsSeparated = resistsStringData.Split (' ');
            
            for (int index = 0; index < resistsSeparated.Length; index++)
            {
                Resists [index] =
                    int.Parse (resistsSeparated [index], NumberFormatInfo.InvariantInfo);
            }
            
            string availableSpellsStringData = input.Attributes ["AvailableSpells"].InnerText;
            string [] availableSpellsSeparated = availableSpellsStringData.Split (' ');
            AvailableSpells.Clear ();
            
            foreach (var spellIdString in availableSpellsSeparated)
            {
                AvailableSpells.Add (int.Parse (spellIdString));
            }
            
            ApplyLineage (XmlHelper.GetIntAttribute (input, "LineageID"));
            RecalculateChildCharacteristics ();
            
            MyStorage.LoadFromXML (ItemsRegistratorRef, XmlHelper.FirstChild (input, "storage"));
            MyEquipment.LoadFromXML (MyStorage, XmlHelper.FirstChild (input, "equipment"));
        }

        public void SaveToXml (XmlElement output)
        {
            output.SetAttribute ("ID", ID.ToString (NumberFormatInfo.InvariantInfo));
            output.SetAttribute ("Name", Name);
            output.SetAttribute ("IsMan", IsMan.ToString ());
            output.SetAttribute ("Experience", Experience.ToString (NumberFormatInfo.InvariantInfo));
            
            foreach (var modifier in ModifiersOnUnit)
            {
                CharacterModifier modifierType = CharacterModifiersContainerRef.Modifiers [modifier.ID];
                for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                {
                    Charactiristics [i] -= modifierType.CharcsChanges [i] * modifier.Level;
                }
            }

            var stringBuilder = new StringBuilder ();
            foreach (var characteristic in Charactiristics)
            {
                stringBuilder.Append (characteristic).Append (' ');
            }

            output.SetAttribute ("Characteristics", stringBuilder.ToString ());
            stringBuilder.Clear ();
            
            foreach (var modifier in ModifiersOnUnit)
            {
                CharacterModifier modifierType = CharacterModifiersContainerRef.Modifiers [modifier.ID];
                for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                {
                    Charactiristics [i] += modifierType.CharcsChanges [i] * modifier.Level;
                }
            }

            foreach (var modifier in ModifiersOnUnit)
            {
                var modifierElement = output.OwnerDocument.CreateElement ("modifier");
                modifierElement.SetAttribute ("ID", modifier.ID.ToString (NumberFormatInfo.InvariantInfo));
                modifierElement.SetAttribute ("Level", modifier.Level.ToString (NumberFormatInfo.InvariantInfo));
                output.AppendChild (modifierElement);
            }

            foreach (var resist in Resists)
            {
                stringBuilder.Append (resist).Append (' ');
            }
            
            output.SetAttribute ("Resists", stringBuilder.ToString ());
            stringBuilder.Clear ();
            
            foreach (var availableSpell in AvailableSpells)
            {
                stringBuilder.Append (availableSpell).Append (' ');
            }
            
            output.SetAttribute ("AvailableSpells", stringBuilder.ToString ());
            stringBuilder.Clear ();

            output.SetAttribute ("LineageID", LineageId.ToString (NumberFormatInfo.InvariantInfo));
            var storageElement = output.OwnerDocument.CreateElement ("storage");
            MyStorage.SaveToXml (storageElement);
            output.AppendChild (storageElement);
            
            var equipmentElement = output.OwnerDocument.CreateElement ("equipment");
            MyEquipment.SaveToXml (equipmentElement);
            output.AppendChild (equipmentElement);
        }

        private void RemoveModifier (AppliedModifier modifier)
        {
            CharacterModifier modifierType = CharacterModifiersContainerRef.Modifiers [modifier.ID];

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
            {
                Charactiristics [i] -= modifierType.CharcsChanges [i] * modifier.Level;
            }

            UnblockableHpRegeneration -= modifierType.UnblockableHpRegeneration * modifier.Level;
            UnblockableMpRegeneration -= modifierType.UnblockableMpRegeneration * modifier.Level;

            HpRegenerationBlocked = false;
            MpRegenerationBlocked = false;
            MovementBlocked = false;

            foreach (var anotherModifier in ModifiersOnUnit)
            {
                if (anotherModifier != modifier)
                {
                    HpRegenerationBlocked |=
                        CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksHpRegeneration;
                    
                    MpRegenerationBlocked |=
                        CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksMpRegeneration;
                    
                    MovementBlocked |= CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksMovement;
                }
            }

            RecalculateChildCharacteristics ();
        }

        new void Start ()
        {
            LineageId = -1;
            Charactiristics = new int[(int) CharacteristicsEnum.Count];
            Resists = new float[(int) DamageTypesEnum.Count];
            
            ModifiersOnUnit = new List <AppliedModifier> ();
            AvailableSpells = new List <int> ();
            
            MyStorage = new Storage (ItemTypesContainerRef);
            MyEquipment = new Equipment (ItemTypesContainerRef);
            
            RecalculateChildCharacteristics ();
            base.Start ();
        }

        new void OnDestroy ()
        {
            base.OnDestroy ();
        }

        void Update ()
        {
            if (!HpRegenerationBlocked)
            {
                CurrentHp += RegenOfHp * Time.deltaTime;
            }

            CurrentHp += UnblockableHpRegeneration * Time.deltaTime;

            if (!MpRegenerationBlocked)
            {
                CurrentMp += RegenOfMp * Time.deltaTime;
            }

            CurrentMp += UnblockableMpRegeneration * Time.deltaTime;

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