using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public static class SpellsInitializer
    {
        public const int FireWallSpellId = 0;
        public const int DrinkPotionSpellId = 1;
        public const int ImmediateHealSpellId = 2;
        public const int ContiniousHealSpellId = 3;
        public const int StealHealthSpellId = 4;
        public const int FireExplosionSpellId = 5;
        public const int LightingSpellId = 6;
        public const int LightSwordAttackSpellId = 7;
        public const int HeavySwordAttackSpellId = 8;
        public const int ElectricDefenseSpellId = 9;
        public const int RaiseDeadSpellId = 10;
        public const int PoisonSpellId = 11;
        public const int BasiliskEyeSpellId = 12;

        public static void InitializeSpells (SpellsContainer container)
        {
            container.AddSpell (new MagicWallSpell (FireWallSpellId,
                /*Cast time*/ 1.5f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/FireWall"),
                /*Info*/ "Fire Wall", /*IST*/ ItemSuperType.FireWand,
                /*Charge*/ 50.0f, /*Per level*/ 20.0f,
                /*Required base movement points*/ 30.0f, /*Per level*/ 20.0f,
                /*Affect self*/ false,
                /*Angle*/ 45.0f, /*Per level*/ 1.0f,
                /*Distance*/ 6.0f, /*Per level*/ 1.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) *
                        (1.0f + caster.MagicDamageBust), caster, DamageTypesEnum.Flamy);
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Горение"), parameter.Level);
                }));

            container.AddSpell (new DrinkPotion (DrinkPotionSpellId, /*Basic Cast Time*/ 1.0f, 
                /*Icon*/ Resources.Load <Sprite> ("Icons/Items/HealthPotion")));

            container.AddSpell (new TargetedSpell (ImmediateHealSpellId,
                /*Cast time*/ 0.1f, /*Per level*/ 0.1f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/ImmediateHeal"),
                /*Info*/ "Immediate Heal", /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 70.0f, /*Per level*/ 10.0f,
                /*Required base movement points*/ 40.0f, /*Per level*/ 10.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.Heal ((itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level)*
                               (1.0f + caster.MagicDamageBust));
                },
                /*Target Checker*/ (caster, target) => caster.Alignment == target.Alignment));

            container.AddSpell (new TargetedSpell (ContiniousHealSpellId,
                /*Cast time*/ 2.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/ContiniousHeal"),
                /*Info*/ "Continious Heal", /*IST*/ ItemSuperType.HealerSphere,
                /*Charge*/ 40.0f, /*Per level*/ 5.0f,
                /*Required base movement points*/ 20.0f, /*Per level*/ 5.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Лечение"), parameter.Level);
                },
                /*Target Checker*/ (caster, target) => caster.Alignment == target.Alignment));

            container.AddSpell (new TargetedSpell (StealHealthSpellId,
                /*Cast time*/ 0.5f, /*Per level*/ 0.2f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/StealHealth"),
                /*Info*/ "Steal Health", /*IST*/ ItemSuperType.NecromancySphere,
                /*Charge*/ 100.0f, /*Per level*/ 20.0f,
                /*Required base movement points*/ 50.0f, /*Per level*/ 10.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    float unitHealthBefore = unit.CurrentHp;
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) *
                        (1.0f + caster.MagicDamageBust), caster, DamageTypesEnum.Lighting);

                    float damage = unitHealthBefore - unit.CurrentHp;
                    caster.Heal (damage);
                },
                /*Target Checker*/ (caster, target) => target.Alive));

            container.AddSpell (new SingleUnitSpell (FireExplosionSpellId,
                /*Cast time*/ 1.5f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/FireExplosion"),
                /*Info*/ "Fire Explosion", /*IST*/ ItemSuperType.FireWand,
                /*Charge*/ 30.0f, /*Per level*/ 10.0f,
                /*Required base movement points*/ 30.0f, /*Per level*/ 5.0f,
                /*Affect self*/ false,
                /*Angle*/ 10.0f, /*Per level*/ 0.0f,
                /*Distance*/ 15.0f, /*Per level*/ 3.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) * 2.0f *
                        (1.0f + caster.MagicDamageBust), caster, DamageTypesEnum.Flamy);
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Горение"), parameter.Level);
                }));

            container.AddSpell (new SingleUnitSpell (LightingSpellId,
                /*Cast time*/ 2.0f, /*Per level*/ 0.8f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/Lightning"),
                /*Info*/ "Lighting", /*IST*/ ItemSuperType.LightingWand,
                /*Charge*/ 50.0f, /*Per level*/ 10.0f,
                /*Required base movement points*/ 40.0f, /*Per level*/ 10.0f,
                /*Affect self*/ false,
                /*Angle*/ 10.0f, /*Per level*/ 0.0f,
                /*Distance*/ 15.0f, /*Per level*/ 5.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) *
                        (1.0f + caster.MagicDamageBust), caster, DamageTypesEnum.Lighting);
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Паралич"), parameter.Level);
                }));

            container.AddSpell (new SingleUnitSpell (LightSwordAttackSpellId,
                /*Cast time*/ 0.5f, /*Per level*/ -0.05f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/LightSwordAttack"),
                /*Info*/ "Light Sword Attack", /*IST*/ ItemSuperType.OneHandedWeapon,
                /*Charge*/ 0.0f, /*Per level*/ 0.0f,
                /*Required base movement points*/ 0.0f, /*Per level*/ 0.0f,
                /*Affect self*/ false,
                /*Angle*/ 45.0f, /*Per level*/ 0.0f,
                /*Distance*/ 3.5f, /*Per level*/ 0.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        itemType.BasicForce * (1.0f + caster.NearDamageBust), caster, DamageTypesEnum.Cutting);
                }));

            container.AddSpell (new SingleUnitSpell (HeavySwordAttackSpellId,
                /*Cast time*/ 0.4f, /*Per level*/ 0.1f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/HeavySwordAttack"),
                /*Info*/ "Heavy Sword Attack", /*IST*/ ItemSuperType.OneHandedWeapon,
                /*Charge*/ 0.0f, /*Per level*/ 0.0f,
                /*Required base movement points*/ 0.0f, /*Per level*/ 0.0f,
                /*Affect self*/ false,
                /*Angle*/ 45.0f, /*Per level*/ 0.0f,
                /*Distance*/ 3.5f, /*Per level*/ 0.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = unit.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyDamage (
                        (itemType.BasicForce + itemType.ForceAdditionPerLevel * parameter.Level) *
                        (1.0f + caster.NearDamageBust), caster, DamageTypesEnum.Bumping);
                }));

            container.AddSpell (new TargetedSpell (ElectricDefenseSpellId,
                /*Cast time*/ 0.1f, /*Per level*/ 0.1f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/ElectricDefense"),
                /*Info*/ "Electric Defense", /*IST*/ ItemSuperType.LightingWand,
                /*Charge*/ 100.0f, /*Per level*/ 10.0f,
                /*Required base movement points*/ 70.0f, /*Per level*/ 20.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Неуязвимость"), parameter.Level);
                },
                /*Target Checker*/ (caster, target) => caster.Alignment == target.Alignment));

            container.AddSpell (new TargetedSpell (RaiseDeadSpellId,
                /*Cast time*/ 3.0f, /*Per level*/ 0.5f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/RaiseDead"),
                /*Info*/ "Raise Dead", /*IST*/ ItemSuperType.NecromancySphere,
                /*Charge*/ 150.0f, /*Per level*/ 35.0f,
                /*Required base movement points*/ 50.0f, /*Per level*/ 20.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    Debug.Assert (!unit.Alive);
                    unit.Resurrect (caster.Alignment, 0.1f * parameter.Level);
                },
                /*Target Checker*/ (caster, target) => !target.Alive));
            
            container.AddSpell (new TargetedSpell (PoisonSpellId,
                /*Cast time*/ 1.0f, /*Per level*/ 0.1f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/Poison"),
                /*Info*/ "Poison", /*IST*/ ItemSuperType.PoisonWand,
                /*Charge*/ 50.0f, /*Per level*/ 10.0f,
                /*Required base movement points*/ 20.0f, /*Per level*/ 5.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Отравление"), parameter.Level);
                },
                /*Target Checker*/ (caster, target) => target.Alive));
            
            container.AddSpell (new TargetedSpell (BasiliskEyeSpellId,
                /*Cast time*/ 0.3f, /*Per level*/ 0.0f,
                /*Movement required*/ true, /*Icon*/ Resources.Load <Sprite> ("Icons/Spells/BasiliskEye"),
                /*Info*/ "Use basilisk eye", /*IST*/ ItemSuperType.BasiliskEye,
                /*Charge*/ 1.0f, /*Per level*/ 0.0f,
                /*Required base movement points*/ 20.0f, /*Per level*/ 0.0f,
                /*Callback*/ (caster, unit, parameter) =>
                {
                    var itemType = caster.ItemTypesContainerRef.ItemTypes [parameter.UsedItem.ItemTypeId];
                    unit.ApplyModifier (CharacterModifiersContainer.GetIdByInfo ("Паралич"),
                        (int) (parameter.UsedItem.Level * itemType.BasicForce));
                },
                /*Target Checker*/ (caster, target) => target.Alive));
        }
    }
}