namespace PriestOfPlague.Source.Ingame.Storyline
{
    public interface IStorylinePoint
    {
        bool ShouldExit { get; }
        void Enter ();
        void Update ();
        int Exit ();
    }
}