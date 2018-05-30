using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;

namespace PriestOfPlague.Source.Spells
{
    public static class SpellsInitializer
    {
        public const int FireWallSpellId = 0;
        public const int DrinkPotionSpellId = 1;
        public const int ImmediateHealSpellId = 2;
        public const int ContiniousSpellId = 3;
        
        public static void InitializeSpells (SpellsContainer container)
        {
            // TODO: Add icon.
            container.AddSpell (new MagicDamageWallSpell (FireWallSpellId, 
                /*Icon*/ null, /*Info*/ "Fire wall", 
                /*Movement required*/ true,  /*Affect self*/ false, /*IST*/ ItemSuperType.FireWand,
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

            // TODO: Add icon.
            container.AddSpell (new DrinkPotion (DrinkPotionSpellId, /*Basic Cast Time*/ 1.0f, /*Icon*/ null));
            
            // TODO: Add icon.
            container.AddSpell (new SingleUnitSpell (ImmediateHealSpellId, 
                /*Icon*/ null, /*Info*/ "Immediate Heal", 
                /*Movement required*/ true,  /*Affect self*/ true, /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 1.0f, /*Per level*/ 0.5f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                /*Cast time*/ 1.0f, /*Per level*/ 0.5f,
                /*Angle*/ 45.0f, /*Per level*/ 1.0f,
                /*Distance*/ 0.5f, /*Per level*/ 0.3f, 
                /*Callback*/ (unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.Heal ((itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) * 10.0f);
                }));
            
            // TODO: Add icon.
            container.AddSpell (new SingleUnitSpell (ContiniousSpellId, 
                /*Icon*/ null, /*Info*/ "Continious Heal", 
                /*Movement required*/ true,  /*Affect self*/ true, /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 2.0f, /*Per level*/ 1.0f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                /*Cast time*/ 2.0f, /*Per level*/ 1.0f,
                /*Angle*/ 45.0f, /*Per level*/ 1.0f,
                /*Distance*/ 0.5f, /*Per level*/ 0.3f, 
                /*Callback*/ (unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyModifier (0, parameter.Level);
                }));
        }
    }
}