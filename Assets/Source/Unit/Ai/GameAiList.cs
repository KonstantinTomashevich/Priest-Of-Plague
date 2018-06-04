using System.Collections.Generic;

namespace PriestOfPlague.Source.Unit.Ai
{
    public static class GameAiList
    {
        public delegate IGameAi GameAiConstructor ();
        public static Dictionary <string, GameAiConstructor> Ais = new Dictionary <string, GameAiConstructor> ()
        {
            {"None", () => new AiNone ()},
            {"Swordsman", () => new AiSwordsman ()}
        };
    }
}