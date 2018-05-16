using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class SpellsContainer : MonoBehaviour
    {
        public SpellsContainer ()
        {
            _spells = new Dictionary <int, ISpell> ();
            SpellsInitializer.InitializeSpells (this);
        }

        public bool AddSpell (ISpell spell)
        {
            if (Spells.ContainsKey (spell.Id))
            {
                return false;
            }

            Spells [spell.Id] = spell;
            return true;
        }

        public bool RemoveSpell (ISpell spell)
        {
            return Spells.Remove (spell.Id);
        }

        public Dictionary <int, ISpell> Spells => _spells;
        private Dictionary <int, ISpell> _spells;
    }
}