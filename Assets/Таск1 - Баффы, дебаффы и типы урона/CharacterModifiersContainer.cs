using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Chars { Vitality, Lucky, Agility, Strength, Intelligence }

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

public class CharacterModifier
{
    public string Index { get; set; }
    public string InfoAboutBuffsInString { get; set; }
    public double timeOfBuff;
    public int[] CharcsChanges = new int[5];
    private static int index = 0;
    public int Level { get; set; }
    public int PlusRegen { set; get; }

    /// <summary>
    /// Конструктор для изначальной информации о баффах
    /// </summary>
    /// <param name="infoIn"> Информация о баффе</param>
    /// <param name="timeIn">Время действия баффа</param>
    public CharacterModifier(string infoIn, int timeIn)
    {
        this.Index = (++index).ToString();
        this.InfoAboutBuffsInString = infoIn;
        this.timeOfBuff = timeIn;
        this.Level = 1;
    }

    /// <summary>
    /// Метод заполняет массив-проихождение значениями, на которое данное происхождение меняет указанные характеристики
    /// </summary>
    /// <param name="Vit">Выносливость</param>
    /// <param name="Luc">Удачи</param>
    /// <param name="Ag">Ловкость</param>
    /// <param name="Str">Сила</param>
    /// <param name="Int">Разум</param>
    /// 
    public void SetArr(int Vit, int Luc, int Ag, int Str, int Int)
    {
        CharcsChanges[(int)Chars.Vitality] = Vit;
        CharcsChanges[(int)Chars.Lucky] = Luc;
        CharcsChanges[(int)Chars.Agility] = Ag;
        CharcsChanges[(int)Chars.Strength] = Str;
        CharcsChanges[(int)Chars.Intelligence] = Int;
    }
}

public class CharacterModifiersContainer : MonoBehaviour
{
    Dictionary<string, CharacterModifier> dict = new Dictionary<string, CharacterModifier>(7);

    Dictionary<string, CharacterModifier> listOfUsedBuffs = new Dictionary<string, CharacterModifier>(); //Костя сказал
    Dictionary<string, CharacterModifier> listOfUsedDebuffs = new Dictionary<string, CharacterModifier>(); //С тимлидом не спорят
    /// <summary>
    /// Даёт баффы по индексу
    /// </summary>
    /// <param name="Index">Унивкальынй индекс баффа</param>
    /// <returns>бафф</returns>
    public CharacterModifier GetBuff(string Index)
    {
        if (dict.ContainsKey(Index)) // нужно ли исключение?
            return dict[Index];
        else throw new System.Exception("Неверный индекс!");
    }
    /// <summary>
    /// Устанавливает все баффы
    /// </summary>
    public void SetBuffs()
    {
        //Лечение(Фиг я реализую, пока не буду знать, какие баффы уже наложены на юнита. Будет логично сделать стек, в
        //который будут класться эти баффы. И так уже по ключу нужно будет искать.) (но Костя сказал, что так не надо, но и для его реализации нужны другие таски)
        CharacterModifier a = new CharacterModifier("Лечение", 5);
        a.SetArr(0, 0, 0, 0, 0);
        a.PlusRegen = 3 * a.Level;
        dict.Add(a.Index, a);

        //Сытость++
        a = new CharacterModifier("Сытость", 5);
        a.SetArr(a.Level, 0, a.Level, a.Level, 0);
        dict.Add(a.Index, a);

        //Магическая поддержка++
        a = new CharacterModifier("Магическая поддержка", 5);
        a.SetArr(a.Level, a.Level, a.Level, a.Level, a.Level);
        dict.Add(a.Index, a);

        //Ослаблен++
        a = new CharacterModifier("Ослабление", 5);
        a.SetArr(-a.Level, 0, -a.Level, -a.Level, 0);
        dict.Add(a.Index, a);

        //Отравлен++
        a = new CharacterModifier("Отравление", 5);
        a.SetArr(-a.Level, 0, -a.Level, -a.Level, 0);
        a.PlusRegen = -a.Level;
        dict.Add(a.Index, a);

        //Болен ++
        a = new CharacterModifier("Болезнь", 5);
        a.SetArr(-a.Level, -a.Level, -a.Level, -a.Level, -a.Level);
        dict.Add(a.Index, a);

        //Парализован(Опять же не реализовать, так как нужно понимание действий (класс unit))
        a = new CharacterModifier("Парализация", 5);
        a.SetArr(0, 0, 0, 0, 0);
        dict.Add(a.Index, a);
    }

    /// <summary>
    /// Класс содержит информацию о баффах/дебаффах
    /// </summary>

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
