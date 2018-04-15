using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets._scripts
{
    class DamageTypesList
    {
        Dictionary<string, DamageTypesList> types = new Dictionary<string, DamageTypesList>();
        private string nameOfType;
        string typeOfDamage;

        public DamageTypesList GetDamage(string Index)
        {
            if (types.ContainsKey(Index)) // нужно ли исключение?
                return types[Index];
            else throw new System.Exception("Неверный индекс!");
        }

        public void SetTypeOfDamage()
        {
            int index = 0;
            DamageTypesList a = new DamageTypesList();
            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Рубящий";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Колющий";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Бронебойный";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Толкающий";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Светлый";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Огненный";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Ледяной";
            types.Add(a.nameOfType, a);

            a.typeOfDamage = (++index).ToString();
            a.nameOfType = "Урон чистой магией";
            types.Add(a.nameOfType, a);
        }
    }
}
