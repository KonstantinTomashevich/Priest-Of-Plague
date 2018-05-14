using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;

namespace PriestOfPlague.Source.Spells
{
    public static class SpellsInitializer
    {
        public static void InitializeSpells (SpellsContainer container)
        {
            // TODO: Add icon.
            container.AddSpell (new MagicDamageWallSpell (0, null, "Fire wall", ItemSuperType.FireWand,
                1.0f, 0.1f, 45.0f, 5.0f, 1.0f, 1.0f,
                (unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (1.0f + parameter.Level) *
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level),
                        DamageTypesEnum.Flamy);
                    unit.ApplyModifier (7, parameter.Level);
                }));
        }
    }
}