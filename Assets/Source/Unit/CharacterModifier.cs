using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifier
    {
        public int Id { get; set; }
        public string InfoAboutBuffsInString { get; set; }
        public float TimeOfBuff { get; set; }
        public int [] CharcsChanges;
        public float [] ResistsChanges;
        public List <int> BuffsToApply;
        public HashSet <int> BuffsToCancel;

        public bool BlocksHpRegeneration { get; set; }
        public bool BlocksMpRegeneration { get; set; }
        public bool BlocksMovement { get; set; }
        public float UnblockableHpRegeneration { get; set; }
        public float UnblockableMpRegeneration { get; set; }
        public Sprite Icon { get; set; }

        private CharacterModifier (int id)
        {
            Id = id;
            InfoAboutBuffsInString = "";
            TimeOfBuff = 0;
            
            CharcsChanges = new int[(int) CharacteristicsEnum.Count];
            ResistsChanges = new float[(int) DamageTypesEnum.Count];
            BuffsToApply = new List <int> ();
            BuffsToCancel = new HashSet <int> ();

            BlocksHpRegeneration = false;
            BlocksMpRegeneration = false;
            BlocksMovement = false;
            UnblockableHpRegeneration = 0;
            UnblockableMpRegeneration = 0;
        }

        public void SetCharacteristicsChanges (int Vit, int Luc, int Ag, int Str, int Int)
        {
            CharcsChanges [(int) CharacteristicsEnum.Vitality] = Vit;
            CharcsChanges [(int) CharacteristicsEnum.Luck] = Luc;
            CharcsChanges [(int) CharacteristicsEnum.Agility] = Ag;
            CharcsChanges [(int) CharacteristicsEnum.Strength] = Str;
            CharcsChanges [(int) CharacteristicsEnum.Intelligence] = Int;
        }
        
        public static CharacterModifier LoadFromXML (XmlNode input)
        {
            var modifier = new CharacterModifier (XmlHelper.GetIntAttribute (input, "Id"));
            modifier.InfoAboutBuffsInString = input.Attributes ["Info"].InnerText;
            modifier.TimeOfBuff = XmlHelper.GetFloatAttribute (input, "TimeOfBuff");
            
            string charsChangesStringData = input.Attributes ["CharacteristicsChanges"].InnerText;
            string [] charsChangesSeparated = charsChangesStringData.Split (' ');
            
            for (int index = 0; index < charsChangesSeparated.Length; index++)
            {
                modifier.CharcsChanges [index] =
                    int.Parse (charsChangesSeparated [index], NumberFormatInfo.InvariantInfo);
            }
            
            string resistsChangesStringData = input.Attributes ["ResistsChanges"].InnerText;
            string [] resistsChangesSeparated = resistsChangesStringData.Split (' ');
            
            for (int index = 0; index < resistsChangesSeparated.Length; index++)
            {
                modifier.ResistsChanges [index] =
                    float.Parse (resistsChangesSeparated [index], NumberFormatInfo.InvariantInfo);
            }

            modifier.BuffsToApply.Clear ();;
            foreach (var toApplyNode in XmlHelper.IterateChildren (input, "applies"))
            {
                modifier.BuffsToApply.Add (XmlHelper.GetIntAttribute (toApplyNode, "Id"));
            }
            
            modifier.BuffsToCancel.Clear ();;
            foreach (var toCancelNode in XmlHelper.IterateChildren (input, "cancels"))
            {
                modifier.BuffsToCancel.Add (XmlHelper.GetIntAttribute (toCancelNode, "Id"));
            }
            
            modifier.BlocksHpRegeneration = XmlHelper.GetBoolAttribute (input, "BlocksHpRegeneration");
            modifier.BlocksMpRegeneration = XmlHelper.GetBoolAttribute (input, "BlocksMpRegeneration");
            modifier.BlocksMovement = XmlHelper.GetBoolAttribute (input, "BlocksMovement");

            modifier.UnblockableHpRegeneration = XmlHelper.GetFloatAttribute (input, "UnblockableHpRegeneration");
            modifier.UnblockableMpRegeneration = XmlHelper.GetFloatAttribute (input, "UnblockableMpRegeneration");
            modifier.Icon = Resources.Load <Sprite> (input.Attributes ["Icon"].InnerText);
            return modifier;
        }
    }
}