using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactiristicsEnum { Vitality = 0, Luck, Agility, Strength, Intelligence }
public enum TypesOfDamageEnum { Near = 0, OnDistance, Magic, FromItems, Critical  } 

public class Unit : MonoBehaviour
{
    public string _nameOfCharacter { get; private set; }
    public bool _isMan { get; private set; }

    Dictionary<TypesOfDamageEnum, float> _dictionaryOfResists = new Dictionary<TypesOfDamageEnum, float>();
    Dictionary<TypesOfDamageEnum, float> _dictionaryOfBusts = new Dictionary<TypesOfDamageEnum, float>();

    public float _currentHP { get; private set; }
    public float _maxHP { get; private set; }
    public float _regenOfHP { get; private set; }
    public float _currentMP { get; private set; }
    public float _maxMP { get; private set; }
    public float _regenOfMP { get; private set; }

    public int _experience { get; private set; }
    public int _maxHeightOfInvertory { get; private set; }


    int[] _arrayOfCharactiristics = new int[5];

    public int GetCharacteristics(CharactiristicsEnum characteristicIn)
    {
        return _arrayOfCharactiristics[(int)characteristicIn];
    }
    public void SetCharactiristics(CharactiristicsEnum typeOfCharacteristicIn, int valueOfCharacteristicIn)
    {
        _arrayOfCharactiristics[(int)typeOfCharacteristicIn] += valueOfCharacteristicIn;
    }

   // private float[] _arrayOfResistsAndBuffs = new float[8];

    private void UpdateCharacteristics()
    {
        //обработка силы
        _dictionaryOfBusts[TypesOfDamageEnum.Near] += (float)(GetCharacteristics(CharactiristicsEnum.Strength) * 0.03);
        _maxHP += 5 * GetCharacteristics(CharactiristicsEnum.Strength);
        _maxMP += 2 * GetCharacteristics(CharactiristicsEnum.Strength);
        _regenOfHP += GetCharacteristics(CharactiristicsEnum.Strength);
        _maxHeightOfInvertory += 3 * GetCharacteristics(CharactiristicsEnum.Strength);

        //ловкость
        _dictionaryOfBusts[TypesOfDamageEnum.OnDistance] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Agility));
        _maxHP += 2 * GetCharacteristics(CharactiristicsEnum.Agility); 
        _maxMP += 5 * GetCharacteristics(CharactiristicsEnum.Agility);
        _regenOfMP += GetCharacteristics(CharactiristicsEnum.Agility);

        //выносливость
        _dictionaryOfResists[TypesOfDamageEnum.Near] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
        _dictionaryOfResists[TypesOfDamageEnum.OnDistance] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
        _dictionaryOfResists[TypesOfDamageEnum.Magic] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
        _dictionaryOfResists[TypesOfDamageEnum.FromItems] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
        _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Vitality));
        _maxHP += 4 * GetCharacteristics(CharactiristicsEnum.Vitality);
        _maxMP += 4 * GetCharacteristics(CharactiristicsEnum.Vitality);
        _regenOfHP += GetCharacteristics(CharactiristicsEnum.Vitality);
        _regenOfMP += GetCharacteristics(CharactiristicsEnum.Vitality);
        _maxHeightOfInvertory += 3 * GetCharacteristics(CharactiristicsEnum.Vitality);

        //разум
        _dictionaryOfBusts[TypesOfDamageEnum.Magic] += (float)(0.2 * GetCharacteristics(CharactiristicsEnum.Intelligence));
        _dictionaryOfBusts[TypesOfDamageEnum.FromItems] += (float)(0.2 * GetCharacteristics(CharactiristicsEnum.Intelligence));
        _maxMP += 3 * GetCharacteristics(CharactiristicsEnum.Intelligence);
        _regenOfHP += GetCharacteristics(CharactiristicsEnum.Intelligence);
        _regenOfMP += GetCharacteristics(CharactiristicsEnum.Intelligence);

        //удачливость
        _dictionaryOfBusts[TypesOfDamageEnum.Critical] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Luck));
        _dictionaryOfResists[TypesOfDamageEnum.Critical] += (float)(0.03 * GetCharacteristics(CharactiristicsEnum.Luck));
        _regenOfHP += GetCharacteristics(CharactiristicsEnum.Luck);
        _regenOfMP += GetCharacteristics(CharactiristicsEnum.Luck);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
