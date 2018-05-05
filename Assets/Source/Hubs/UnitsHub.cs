using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using PriestOfPlague.Source.Core;
using PriestOfPlague.Source.Unit;

namespace PriestOfPlague.Source.Hubs
{
    public class UnitsHub : MonoBehaviour
    {
        public IReadOnlyCollection <Unit.Unit> Units => _units;

        public delegate bool UnitsSearchCriteria (Unit.Unit toCheck);

        public List <Unit.Unit> GetUnitsByCriteria (UnitsSearchCriteria criteria)
        {
            var result = new List <Unit.Unit> ();
            foreach (var unit in _units)
            {
                if (criteria (unit))
                {
                    result.Add (unit);
                }
            }

            return result;
        }
        
        private HashSet <Unit.Unit> _units;
        
        private void Start ()
        {
            _units = new HashSet <Unit.Unit> ();
            EventsHub.Instance.Subscribe (this, CreationInformer.EventObjectCreated);
            EventsHub.Instance.Subscribe (this, CreationInformer.EventObjectDestroyed);
        }

        private void Update ()
        {
            var toDestroy = new List <GameObject> ();
            
            foreach (var unit in _units)
            {
                if (unit.CurrentHP <= 0.0f)
                {
                    toDestroy.Add (unit.gameObject);
                }
            }

            foreach (var gameObjectToDestroy in toDestroy)
            {
                Destroy (gameObjectToDestroy);
            }
        }
        
        private void OnDestroy ()
        {
            EventsHub.Instance.Unsubscribe (this, CreationInformer.EventObjectCreated);
            EventsHub.Instance.Unsubscribe (this, CreationInformer.EventObjectDestroyed);
        }

        private void ObjectCreated (object parameter)
        {
            if (parameter is Unit.Unit)
            {
                _units.Add (parameter as Unit.Unit);
            }
        }
        
        private void ObjectDestroyed (object parameter)
        {
            if (parameter is Unit.Unit)
            {
                _units.Remove (parameter as Unit.Unit);
            }
        }
    }
}