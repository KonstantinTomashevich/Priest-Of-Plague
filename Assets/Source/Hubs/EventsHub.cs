using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Hubs
{
    public class EventsHub
    {
        public static EventsHub Instance = new EventsHub ();

        public void SendGlobalEvent (string eventName, object eventData)
        {
            var list = _subscribers [eventName.GetHashCode ()];
            if (list != null)
            {
                foreach (var gameObject in list)
                {
                    gameObject.SendMessage (eventName, eventData);
                }
            }
        }

        public void Subscribe (GameObject subscriber, string eventName)
        {
            var list = _subscribers [eventName.GetHashCode ()];
            if (list == null)
            {
                list = new HashSet <GameObject> ();
                _subscribers [eventName.GetHashCode ()] = list;
            }

            list.Add (subscriber);
        }

        public void Unsubscribe (GameObject subscriber, string eventName)
        {
            var list = _subscribers [eventName.GetHashCode ()];
            if (list != null)
            {
                list.Remove (subscriber);
            }
        }
        
        private EventsHub ()
        {
            _subscribers = new Dictionary <int, HashSet <GameObject>> ();
        }

        private Dictionary <int, HashSet <GameObject>> _subscribers;
    }
}