using System.Collections.Generic;

namespace PriestOfPlague.Source.Ingame.Storyline
{
    public class Storyline
    {
        public List <IStorylinePoint> Points { get; private set; }
        public int CurrentIndex { get; private set; }
        private bool _shouldEnter;

        public Storyline ()
        {
            Points = new List <IStorylinePoint> ();
            CurrentIndex = 0;
            _shouldEnter = true;
        }

        public void Update ()
        {
            if (CurrentIndex < 0 || CurrentIndex >= Points.Count)
            {
                return;
            }
            
            var point = Points [CurrentIndex];
            if (_shouldEnter)
            {
                point.Enter ();
                _shouldEnter = false;
            }

            point.Update ();
            if (point.ShouldExit)
            {
                CurrentIndex += point.Exit ();
                _shouldEnter = true;
            }
        }
    }
}