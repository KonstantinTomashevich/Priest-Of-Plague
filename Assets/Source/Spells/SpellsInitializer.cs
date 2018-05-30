using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;

namespace PriestOfPlague.Source.Spells
{
    public static class SpellsInitializer
    {
        public const int FireWallSpellId = 0;
        public const int EquipSpellId = 1;
        
        public static void InitializeSpells (SpellsContainer container)
        {
            // TODO: Add icon.
            container.AddSpell (new MagicDamageWallSpell (FireWallSpellId, 
                /*Icon*/ null, /*Info*/ "Fire wall", /*Movement required*/ true, /*IST*/ ItemSuperType.FireWand,
                /*Charge*/ 1.0f, /*Per level*/ 0.1f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                /*Cast time*/ 3.0f, /*Per level*/ 0.5f,
                /*Angle*/ 45.0f, /*Per level*/ 1.0f,
                /*Distance*/ 5.0f, /*Per level*/ 1.0f, 
                /*Callback*/ (unit, parameter) =>
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