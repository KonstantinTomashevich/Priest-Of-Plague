using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public class EquipSpell : ISpell
    {
        public EquipSpell (int id, Sprite icon, string info)
        {
            Id = id;
            Icon = icon;
            Info = info;

            // TODO: Different equip time for different supertypes.
            BasicCastTime = 2.0f;
            CastTimeAdditionPerLevel = 0.0f;
        }

        public int Id { get; }
        public bool CanCast (Unit.Unit unit, int level = 0, Item item = null)
        {
            if (item == null)
            {
                return true;
            }

            return unit.MyEquipment.CanBeSetted ((EquipmentSlot) level, item);
        }

        public void Cast (Unit.Unit caster, UnitsHub unitsHub, SpellCastParameter parameter)
        {
            caster.MyEquipment [(EquipmentSlot) parameter.Level] = parameter.UsedItem;
        }

        public float BasicCastTime { get; private set; }
        public float CastTimeAdditionPerLevel { get; private set; }
        public Sprite Icon { get; private set; }
        public string Info { get; private set; }
    }
}