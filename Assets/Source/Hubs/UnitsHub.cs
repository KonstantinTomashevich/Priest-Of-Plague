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
        public IReadOnlyCollection <Unit.Unit> Units => _units.Values;

        public delegate bool UnitsSearchCriteria (Unit.Unit toCheck);

        public UnitsHub ()
        {
            _units = new Dictionary <int, Unit.Unit> ();
            _freeId = 0;
        }
        
        public List <Unit.Unit> GetUnitsByCriteria (UnitsSearchCriteria criteria)
        {
            var result = new List <Unit.Unit> ();
            foreach (var unit in _units)
            {
                if (criteria (unit.Value))
                {
                    result.Add (unit.Value);
                }
            }

            return result;
        }

        public int RequestId (int id)
        {
            if (id < _freeId)
            {
                ++_freeId;
                return _freeId - 1;
            }

            _freeId = id + 1;
            return _freeId;
        }
        
        public Unit.Unit this [int id] => (_units.ContainsKey (id) ? _units [id] : null);
        
        private Dictionary <int, Unit.Unit> _units;
        private int _freeId;
        
        private void Start ()
        {
            
            EventsHub.Instance.Subscribe (this, CreationInformer.EventObjectCreated);
            EventsHub.Instance.Subscribe (this, CreationInformer.EventObjectDestroyed);
        }

        private void Update ()
        {
            // TODO: Destroy units after some time, not exactly at death moment.
            /*var toDestroy = new List <GameObject> ();
            
            foreach (var unit in _units)
            {
                if (unit.CurrentHp <= 0.0f)
                {
                    toDestroy.Add (unit.gameObject);
                }
            }

            foreach (var gameObjectToDestroy in toDestroy)
            {
                Destroy (gameObjectToDestroy);
            }*/
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
                var unit = parameter as Unit.Unit;
                if (!_units.ContainsKey (unit.Id))
                {
                    _units.Add (unit.Id, unit);
                }
            }
        }
        
        private void ObjectDestroyed (object parameter)
        {
            if (parameter is Unit.Unit)
            {
                var unit = parameter as Unit.Unit;
                _units.Remove (unit.Id);
            }
        }
    }
}