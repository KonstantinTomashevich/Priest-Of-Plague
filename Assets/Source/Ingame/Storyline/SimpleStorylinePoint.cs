namespace PriestOfPlague.Source.Ingame.Storyline
{
    public class SimpleStorylinePoint : IStorylinePoint
    {
        public delegate void VoidDelegate (SimpleStorylinePoint self);
        public delegate int IntDelegate (SimpleStorylinePoint self);
        public bool ShouldExit { get; set; }
        public int ExitCode { get; set; }
        
        public VoidDelegate OnEnter { get; set; }
        public VoidDelegate OnUpdate { get; set; }
        public IntDelegate OnExit { get; set; }

        public SimpleStorylinePoint (VoidDelegate onEnter, VoidDelegate onUpdate, IntDelegate onExit)
        {
            OnEnter = onEnter;
            OnUpdate = onUpdate;
            OnExit = onExit;
        }

        public void Enter ()
        {
            OnEnter (this);
        }

        public void Update ()
        {
            OnUpdate (this);
        }

        public int Exit ()
        {
            return OnExit (this);
        }
    }
}