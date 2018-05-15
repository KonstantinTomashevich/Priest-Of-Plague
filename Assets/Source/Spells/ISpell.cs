using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public struct SpellCastParameter
    {
        public Item UsedItem;
        public int Level;
        public object Additional;
    }
    
    public interface ISpell
    {
        int Id { get; }
        bool CanCast (Unit.Unit unit, int level = 0, Item item = null);
        void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter);
        float BasicCastTime { get; }
        float CastTimeAdditionPerLevel { get; }
        Sprite Icon { get; }
        string Info { get; }
    }
}