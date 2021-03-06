﻿using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class SpellsContainer : MonoBehaviour
    {
        public SpellsContainer ()
        {
            _spells = new Dictionary <int, ISpell> ();
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

        private void Start ()
        {
            SpellsInitializer.InitializeSpells (this);
        }

        public Dictionary <int, ISpell> Spells => _spells;
        private Dictionary <int, ISpell> _spells;
    }
}