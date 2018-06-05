namespace PriestOfPlague.Source.Unit.Ai
{
    public interface IGameAi
    {
        void Process (Unit unit);
        void OnDamage (Unit unit, Unit damager);
        void OnDie (Unit unit);
    }
}