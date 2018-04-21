using UnityEngine;
using PriestOfPlague.Source.Hubs;

namespace PriestOfPlague.Source.Core
{
    public class CreationInformer : MonoBehaviour
    {
        public const string EventObjectCreated = "ObjectCreated";
        public const string EventObjectDestroyed = "ObjectDestroyed";
        
        protected void Start ()
        {
            EventsHub.Instance.SendGlobalEvent (EventObjectCreated, this);
        }

        protected void OnDestroy ()
        {
            EventsHub.Instance.SendGlobalEvent (EventObjectDestroyed, this);
        }
    }
}