using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using UnityEngine;
using PriestOfPlague.Source.Core;
using PriestOfPlague.Source.Items;

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
        
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsMan { get; set; }
        public Storage MyStorage { get; set; }

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

        private int [] _charactiristics;
        private List <AppliedModifier> ModifiersOnUnit;
        private float [] _resists;
        private int _lineageID = -1;

        private bool _hpRegenerationBlocked = false;
        private bool _mpRegenerationBlocked = false;
        private bool _movementBlocked = false;
        private float _unblockableHpRegeneration = 0;
        private float _unblockableMpRegeneration = 0;

        public void ApplyLineage (int lineageIndex)
        {
            Lineage lineage = null;
            if (lineageIndex != -1)
            {
                lineage = LineagesContainerRef.LineagesList [_lineageID];
                for (int i = 0; i < 5; i++)
                {
                    _charactiristics [i] -= lineage.CharcsChanges [i];
                }
            }

            _lineageID = lineageIndex;
            lineage = LineagesContainerRef.LineagesList [_lineageID];

            for (int i = 0; i < 5; i++)
            {
                _charactiristics [i] += lineage.CharcsChanges [i];
            }
        }

        public void ApplyModifier (int id, int level)
        {
            var modifierType = CharacterModifiersContainerRef.Modifiers [id];
            var modifier = new AppliedModifier (id, modifierType.TimeOfBuff * level, level);
            ModifiersOnUnit.Add (modifier);

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
                _charactiristics [i] += modifierType.CharcsChanges [i] * level;

            _unblockableHpRegeneration += modifierType.UnblockableHpRegeneration * level;
            _unblockableMpRegeneration += modifierType.UnblockableMpRegeneration * level;
            _hpRegenerationBlocked |= modifierType.BlocksHpRegeneration;
            _mpRegenerationBlocked |= modifierType.BlocksMpRegeneration;
            _movementBlocked |= modifierType.BlocksMovement;

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
            MaxHp = 0;
            MaxMp = 0;
            RegenOfHp = 0;
            RegenOfMp = 0;
            MaxStorageWeight = 0;

            OnDistanceDamageBust = 0;
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                _resists [index] = 0;
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
            NearDamageBust += (float) ((float) strength * 0.03);
            MaxHp += 5 * strength;
            MaxMp += 2 * strength;
            RegenOfHp += strength;
            MaxStorageWeight += 3 * strength;

            //ловкость
            OnDistanceDamageBust += (float) (0.03 * (float) agility);
            MaxHp += 2 * agility;
            MaxMp += 5 * agility;
            RegenOfMp += agility;

            //выносливость
            for (int index = 0; index < (int) DamageTypesEnum.Count; index++)
            {
                _resists [index] += (float) (0.03 * vitality);
            }

            MaxHp += 4 * vitality;
            MaxMp += 4 * vitality;
            RegenOfHp += vitality;
            RegenOfMp += vitality;
            MaxStorageWeight += 3 * vitality;

            //разум
            MagicDamageBust += (float) (0.2 * (float) intelligence);
            MaxMp += 3 * intelligence;
            RegenOfHp += intelligence;
            RegenOfMp += intelligence;

            //удачливость
            CriticalDamageChance += (float) (0.03 * (float) luck);
            CriticalResistChance += (float) (0.03 * (float) luck);
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
                _charactiristics [index] =
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
                _resists [index] =
                    int.Parse (resistsSeparated [index], NumberFormatInfo.InvariantInfo);
            }
            
            ApplyLineage (XmlHelper.GetIntAttribute (input, "LineageID"));
            RecalculateChildCharacteristics ();
            MyStorage.LoadFromXML (XmlHelper.FirstChild (input, "storage"));
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
                    _charactiristics [i] -= modifierType.CharcsChanges [i] * modifier.Level;
                }
            }

            var stringBuilder = new StringBuilder ();
            foreach (var characteristic in _charactiristics)
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
                    _charactiristics [i] += modifierType.CharcsChanges [i] * modifier.Level;
                }
            }

            foreach (var modifier in ModifiersOnUnit)
            {
                var modifierElement = output.OwnerDocument.CreateElement ("modifier");
                modifierElement.SetAttribute ("ID", modifier.ID.ToString (NumberFormatInfo.InvariantInfo));
                modifierElement.SetAttribute ("Level", modifier.Level.ToString (NumberFormatInfo.InvariantInfo));
                output.AppendChild (modifierElement);
            }

            foreach (var resist in _resists)
            {
                stringBuilder.Append (resist).Append (' ');
            }
            
            output.SetAttribute ("Resists", stringBuilder.ToString ());
            stringBuilder.Clear ();

            output.SetAttribute ("LineageID", _lineageID.ToString (NumberFormatInfo.InvariantInfo));
            var storageElement = output.OwnerDocument.CreateElement ("storage");
            MyStorage.SaveToXml (storageElement);
            output.AppendChild (storageElement);
        }

        private void RemoveModifier (AppliedModifier modifier)
        {
            CharacterModifier modifierType = CharacterModifiersContainerRef.Modifiers [modifier.ID];

            for (int i = 0; i < (int) CharacteristicsEnum.Count; i++)
            {
                _charactiristics [i] -= modifierType.CharcsChanges [i] * modifier.Level;
            }

            _unblockableHpRegeneration -= modifierType.UnblockableHpRegeneration * modifier.Level;
            _unblockableMpRegeneration -= modifierType.UnblockableMpRegeneration * modifier.Level;

            _hpRegenerationBlocked = false;
            _mpRegenerationBlocked = false;
            _movementBlocked = false;

            foreach (var anotherModifier in ModifiersOnUnit)
            {
                if (anotherModifier != modifier)
                {
                    _hpRegenerationBlocked |=
                        CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksHpRegeneration;
                    
                    _mpRegenerationBlocked |=
                        CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksMpRegeneration;
                    
                    _movementBlocked |= CharacterModifiersContainerRef.Modifiers [anotherModifier.ID].BlocksMovement;
                }
            }

            RecalculateChildCharacteristics ();
        }

        new void Start ()
        {
            _charactiristics = new int[(int) CharacteristicsEnum.Count];
            _resists = new float[(int) DamageTypesEnum.Count];
            ModifiersOnUnit = new List <AppliedModifier> ();
            MyStorage = new Storage (ItemTypesContainerRef);
            RecalculateChildCharacteristics ();
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
                CurrentHp += RegenOfHp * Time.deltaTime;
            }

            CurrentHp += _unblockableHpRegeneration * Time.deltaTime;

            if (!_mpRegenerationBlocked)
            {
                CurrentMp += RegenOfMp * Time.deltaTime;
            }

            CurrentMp += _unblockableMpRegeneration * Time.deltaTime;

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