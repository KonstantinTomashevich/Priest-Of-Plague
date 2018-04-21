using UnityEngine;
using PriestOfPlague.Source.Hubs;

namespace PriestOfPlague.Source.Core
{
    public class CreationInformer : MonoBehaviour
    {
        public const string EventObjectCreated = "ObjectCreated";
        
        protected void Start ()
        {
            EventsHub.Instance.SendGlobalEvent (EventObjectCreated, this);
        }
    }
}