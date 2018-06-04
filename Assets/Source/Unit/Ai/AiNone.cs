namespace PriestOfPlague.Source.Unit.Ai
{
    public class AiNone : IGameAi
    {
        public void Process (Unit unit)
        {
            /* Do nothing, for player. */
        }

        public void OnDamage (Unit unit, Unit damager)
        {
            /* Do nothing, for player. */
        }

        public void OnDie (Unit unit)
        {
            /* Do nothing, for player. */
        }
    }
}