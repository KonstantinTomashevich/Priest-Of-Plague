using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum typesOfDamageEnum { Chopping, Stitching, Armor_piercing, Pushing, Light, Fiery, Icy, Damage_pure_magic }

namespace PriestOfPlague.Source.Unit
{
    class DamageTypesList
    {
        DamageTypesList[] types = new DamageTypesList[8];
        private string nameOfType;

        public DamageTypesList GetDamage(int IndexIn)
        {
            if (types[IndexIn] != null)
                return types[IndexIn];
            else throw new System.Exception("Неверный индекс!");
        }

        public void SetTypeOfDamage()
        {
            DamageTypesList a = new DamageTypesList();
            a.nameOfType = "Рубящий";
            types[(int)typesOfDamageEnum.Chopping] = a;

            a = new DamageTypesList();
            a.nameOfType = "Колющий";
            types[(int)typesOfDamageEnum.Stitching] = a;

            a = new DamageTypesList();
            a.nameOfType = "Бронебойный";
            types[(int)typesOfDamageEnum.Armor_piercing] = a;

            a = new DamageTypesList();
            a.nameOfType = "Толкающий";
            types[(int)typesOfDamageEnum.Pushing] = a;

            a = new DamageTypesList();
            a.nameOfType = "Светлый";
            types[(int)typesOfDamageEnum.Light] = a;

            a = new DamageTypesList();
            a.nameOfType = "Огненный";
            types[(int)typesOfDamageEnum.Fiery] = a;

            a = new DamageTypesList();
            a.nameOfType = "Ледяной";
            types[(int)typesOfDamageEnum.Icy] = a;

            a = new DamageTypesList();
            a.nameOfType = "Урон чистой магией";
            types[(int)typesOfDamageEnum.Damage_pure_magic] = a;
        }
    }
}
