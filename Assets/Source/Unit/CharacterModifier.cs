using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriestOfPlague.Source.Unit
{
    public class CharacterModifier
    {
        public int ID { get; set; }
        public string InfoAboutBuffsInString { get; set; }
        public float timeOfBuff { get; set; }
        public int [] CharcsChanges = new int[(int) CharacteristicsEnum.Count];
        public List <int> BuffsToApply = new List <int> ();
        public HashSet <int> BuffsToCancel = new HashSet <int> ();

        public bool _blocksHpRegeneration { get; set; }
        public bool _blocksMpRegeneration { get; set; }
        public bool _blocksMovement { get; set; }
        public float _unblockableHPRegeneration { get; set; }
        public float _unblockableMPRegeneration { get; set; }

        public CharacterModifier (int id, string infoIn, int timeIn)
        {
            ID = id;
            InfoAboutBuffsInString = infoIn;
            timeOfBuff = timeIn;

            _blocksHpRegeneration = false;
            _blocksMpRegeneration = false;
            _blocksMovement = false;
            _unblockableHPRegeneration = 0;
            _unblockableMPRegeneration = 0;
        }

        public void SetCharacteristicsChanges (int Vit, int Luc, int Ag, int Str, int Int)
        {
            CharcsChanges [(int) CharacteristicsEnum.Vitality] = Vit;
            CharcsChanges [(int) CharacteristicsEnum.Luck] = Luc;
            CharcsChanges [(int) CharacteristicsEnum.Agility] = Ag;
            CharcsChanges [(int) CharacteristicsEnum.Strength] = Str;
            CharcsChanges [(int) CharacteristicsEnum.Intelligence] = Int;
        }
    }
}