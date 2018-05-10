using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Unit;
using UnityEngine;

namespace PriestOfPlague.Source.Spells
{
    public interface ISpell
    {
        int Id { get; }
        bool CanCast (Unit.Unit unit);
        void Cast (Unit.Unit caster, UnitsHub unitsHub, object parameter);
        Sprite Icon { get; set; }
        string Info { get; }
    }
}