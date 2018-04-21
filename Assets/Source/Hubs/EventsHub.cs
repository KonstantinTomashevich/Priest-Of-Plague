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
                foreach (var monoBehaviour in list)
                {
                    monoBehaviour.SendMessage (eventName, eventData);
                }
            }
        }

        public void Subscribe (MonoBehaviour subscriber, string eventName)
        {
            var list = _subscribers [eventName.GetHashCode ()];
            if (list == null)
            {
                list = new HashSet <MonoBehaviour> ();
                _subscribers [eventName.GetHashCode ()] = list;
            }

            list.Add (subscriber);
        }

        public void Unsubscribe (MonoBehaviour subscriber, string eventName)
        {
            var list = _subscribers [eventName.GetHashCode ()];
            if (list != null)
            {
                list.Remove (subscriber);
            }
        }
        
        private EventsHub ()
        {
            _subscribers = new Dictionary <int, HashSet <MonoBehaviour>> ();
        }

        private Dictionary <int, HashSet <MonoBehaviour>> _subscribers;
    }
}