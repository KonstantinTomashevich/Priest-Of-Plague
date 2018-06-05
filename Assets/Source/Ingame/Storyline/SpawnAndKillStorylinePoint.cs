using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace PriestOfPlague.Source.Ingame.Storyline
{
    public class SpawnAndKillStorylinePoint : IStorylinePoint
    {
        public bool ShouldExit { get; private set; }
        public GameObject Prefab { get; private set; }
        public SphereCollider Area { get; private set; }
        public int Count { get; private set; }
        public int Alignment { get; private set; }
        
        private List <GameObject> _spawned;
        private float _timeElapsed;
        private const float MinimumTimeBeforeCheck = 1.0f;

        public SpawnAndKillStorylinePoint (GameObject prefab, SphereCollider area, int count, int alignment)
        {
            Prefab = prefab;
            Area = area;
            Count = count;
            Alignment = alignment;
            _spawned = new List <GameObject> ();
            _timeElapsed = 0.0f;
        }

        public void Enter ()
        {
            var random = new Random ();
            for (int index = 0; index < Count; index++)
            {
                var newUnit = Object.Instantiate (Prefab);
                newUnit.transform.SetParent (Area.transform);

                var requestedPosition = new Vector3 ();
                NavMeshHit hit;

                do
                {
                    requestedPosition.x = random.Next ((int) (Area.center.x - Area.radius), (int) (Area.center.x + Area.radius));
                    requestedPosition.z = random.Next ((int) (Area.center.z - Area.radius), (int) (Area.center.z + Area.radius));

                } while (!NavMesh.SamplePosition (requestedPosition, out hit, 1.0f,
                    newUnit.GetComponent <NavMeshAgent> ().areaMask));

                newUnit.transform.position = hit.position;
                newUnit.GetComponent <UnitSpawner> ().RequestedAlignment = Alignment;
                _spawned.Add (newUnit);
            }
        }

        public void Update ()
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed < MinimumTimeBeforeCheck)
            {
                ShouldExit = false;
                return;
            }
            
            ShouldExit = true;
            foreach (var gameObject in _spawned)
            {
                var unit = gameObject.GetComponent <Unit.Unit> ();
                if (unit == null || (unit.Alive && unit.Alignment == Alignment))
                {
                    ShouldExit = false;
                }
            }
        }

        public int Exit ()
        {
            return 1;
        }
    }
}