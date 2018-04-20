using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ResistsAndBusts { damageResist = 0, damageBustPhisical, damageBustOnDistance, damageBustMagic, damageBustItems, criticalDamageChance, halfOfDamageChance }
//У меня только сопротивление к урону в принципе
//нужно же разделение на сопротивление магическому и физическому уронам? Или ещё и сопротивление к урону на дистанции?


public class Unit : MonoBehaviour
{

    private string _nameOfCharacter;
    private string _gender; //а можно не string, а bool?

    private int _currentHP; //так много одинаковых private int'ов, может в массив бахнуть?
    private int _maxHP;
    private int _regenOfHP;
    private int _currentMP;
    private int _maxMP;
    private int _regenOfMP;
    private int _experience;
    private int _maxHeightOfInvertory;

    private int _strength; //Тут та же фигня, почему бы не в массиве?
    private int _agility;
    private int _vitality;
    private int _intelligence;
    private int _luck;

    double[] ArrayOfResistsAndBuffs = new double[8];

    public void SetCharacteristics()
    {
        //обработка силы
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.damageBustPhisical] += 0.03 *_strength;
        _maxHP += 5 * _strength;
        _maxMP += 2 * _strength;
        _regenOfHP += _strength;
        _maxHeightOfInvertory += 3 * _strength;

        //ловкость
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.damageBustOnDistance] += 0.03 * _agility;
        _maxHP += 2 * _agility;
        _maxMP += 5 * _agility;
        _regenOfMP += _agility;

        //выносливость
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.damageResist] += 0.03 * _vitality;
        _maxHP += 4 * _vitality;
        _maxMP += 4 * _vitality;
        _regenOfHP += _vitality;
        _regenOfMP += _vitality;
        _maxHeightOfInvertory += 3 * _vitality;

        //разум
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.damageBustMagic] += 0.2 * _intelligence;
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.damageBustItems] += 0.2 * _intelligence;
        _maxMP += 3 * _intelligence;
        _regenOfHP += _intelligence;
        _regenOfMP += _intelligence;

        //удачливость
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.criticalDamageChance] += 0.03 * _luck;
        ArrayOfResistsAndBuffs[(int)ResistsAndBusts.halfOfDamageChance] += 0.03 * _luck;
        _regenOfHP += _luck;
        _regenOfMP += _luck;
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
