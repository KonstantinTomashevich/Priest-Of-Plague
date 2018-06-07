using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace PriestOfPlague.Source.Hubs
{
    public class EventsHub
    {
        public static EventsHub Instance = new EventsHub ();

        public void SendGlobalEvent (string eventName, object eventData)
        {
            var list = GetSubscribers (eventName.GetHashCode ());
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
            var list = GetSubscribers (eventName.GetHashCode ());
            if (list == null)
            {
                list = new List <MonoBehaviour> ();
                _subscribers.Add (eventName.GetHashCode (), list);
            }

            list.Add (subscriber);
        }

        public void Unsubscribe (MonoBehaviour subscriber, string eventName)
        {
            var list = GetSubscribers (eventName.GetHashCode ());
            list?.Remove (subscriber);
        }

        private List <MonoBehaviour> GetSubscribers (int hashCode)
        {
            try
            {
                return _subscribers [hashCode];
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private EventsHub ()
        {
            _subscribers = new Dictionary <int, List <MonoBehaviour>> ();
        }

        private Dictionary <int, List <MonoBehaviour>> _subscribers;
    }
}