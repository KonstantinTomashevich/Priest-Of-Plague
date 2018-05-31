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
        public const int StealHealthSpellId = 4;
        public const int FireExplosionSpellId = 5;
        public const int LightingSpellId = 5;
        
        public static void InitializeSpells (SpellsContainer container)
        {
            // TODO: Add icon.
            container.AddSpell (new MagicWallSpell (FireWallSpellId,
                /*Cast time*/ 3.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Fire Wall", /*IST*/ ItemSuperType.FireWand,
                /*Charge*/ 1.0f, /*Per level*/ 0.1f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                /*Affect self*/ false, 
                /*Angle*/ 45.0f, /*Per level*/ 1.0f,
                /*Distance*/ 5.0f, /*Per level*/ 1.0f, 
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level,
                        DamageTypesEnum.Flamy);
                    unit.ApplyModifier (7, parameter.Level);
                }));

            // TODO: Add icon.
            container.AddSpell (new DrinkPotion (DrinkPotionSpellId, /*Basic Cast Time*/ 1.0f, /*Icon*/ null));
            
            // TODO: Add icon.
            container.AddSpell (new TargetedSpell (ImmediateHealSpellId,
                /*Cast time*/ 1.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Immediate Heal", 
                /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 1.0f, /*Per level*/ 0.5f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.Heal ((itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) * 10.0f);
                }));
            
            // TODO: Add icon.
            container.AddSpell (new TargetedSpell (ContiniousSpellId, 
                /*Cast time*/ 2.0f, /*Per level*/ 1.0f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Continious Heal", 
                /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 2.0f, /*Per level*/ 1.0f, 
                /*Required base movement points*/ 1.0f, /*Per level*/ 0.5f, 
                
                /*Callback*/ (caster, unit, parameter) =>
                {
                    unit.ApplyModifier (0, parameter.Level);
                }));

            // TODO: Add icon.
            container.AddSpell (new TargetedSpell (StealHealthSpellId, 
                /*Cast time*/ 4.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Steal Health", 
                /*IST*/ ItemSuperType.NecromancySphere,
                /*Charge*/ 5.0f, /*Per level*/ 1.0f, 
                /*Required base movement points*/ 5.0f, /*Per level*/ 2.0f, 
                
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    float unitHealthBefore = unit.CurrentHp;
                    unit.ApplyDamage ((itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) * 10.0f,
                        DamageTypesEnum.PureMagic);

                    float damage = unitHealthBefore - unit.CurrentHp;
                    caster.Heal (damage);
                }));
            
            // TODO: Add Icon.
            container.AddSpell (new SingleUnitSpell (FireExplosionSpellId,
                /*Cast time*/ 3.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Fire Explosion", /*IST*/ ItemSuperType.FireWand,
                /*Charge*/ 2.0f, /*Per level*/ 0.5f, 
                /*Required base movement points*/ 2.0f, /*Per level*/ 0.5f, 
                /*Affect self*/ false, 
                /*Angle*/ 10.0f, /*Per level*/ 0.0f,
                /*Distance*/ 10.0f, /*Per level*/ 3.0f, 
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) * 2.0f,
                        DamageTypesEnum.Flamy);
                    unit.ApplyModifier (7, parameter.Level);
                }));
            
            // TODO: Add Icon.
            container.AddSpell (new SingleUnitSpell (LightingSpellId,
                /*Cast time*/ 3.0f, /*Per level*/ 1.0f,
                /*Movement required*/ true, /*Icon*/ null, /*Info*/ "Lighting", /*IST*/ ItemSuperType.LightingWand,
                /*Charge*/ 3.0f, /*Per level*/ 1.0f, 
                /*Required base movement points*/ 4.0f, /*Per level*/ 1.0f, 
                /*Affect self*/ false, 
                /*Angle*/ 10.0f, /*Per level*/ 0.0f,
                /*Distance*/ 10.0f, /*Per level*/ 5.0f, 
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level,
                        DamageTypesEnum.PureMagic);
                    unit.ApplyModifier (6, parameter.Level);
                }));
            
            
        }
    }
}