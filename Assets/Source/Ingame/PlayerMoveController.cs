using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame
{
    public enum PlayerCurrentActionType
    {
        Walk = 0,
        Count
    }

    public class PlayerMoveController : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Camera _playerCamera;
        private Unit.Unit _unit;

        public PlayerCurrentActionType PlayerCurrentAction { get; private set; } = PlayerCurrentActionType.Walk;
        public float NavigationAccuracy = 1.0f;

        public void MouseClick ()
        {
            if (PlayerCurrentAction == PlayerCurrentActionType.Walk)
            {
                var ray = _playerCamera.ScreenPointToRay (Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast (ray, out raycastHit))
                {
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition (raycastHit.point, out navMeshHit, NavigationAccuracy,
                        _navMeshAgent.areaMask))
                    {
                        _navMeshAgent.SetDestination (navMeshHit.position);
                        _unit.StartCastingSpell (null);
                    }
                }
            }
        }

        public void SelectWalkAction ()
        {
            PlayerCurrentAction = PlayerCurrentActionType.Walk;
            _unit.StartCastingSpell (null);
        }

        private IEnumerator Start ()
        {
            _navMeshAgent = GetComponent <NavMeshAgent> ();
            _animator = GetComponentInChildren <Animator> ();
            _playerCamera = GetComponentInChildren <Camera> ();
            _unit = null;
            
            do
            {
                yield return null;
                _unit = GetComponent <Unit.Unit> ();
            } while (_unit == null);
        }

        private void Update ()
        {
            if (_unit == null)
            {
                return;
            }
            
            StopUnitIfMovementIsBlocked ();
            UpdateAnimatorVariables ();
        }

        private void StopUnitIfMovementIsBlocked ()
        {
            // NOTE: Block agent movement if unit casts spell.
            if (_unit.MovementBlocked || _unit.CurrentlyCasting != null)
            {
                _animator.SetBool ("Moving", false);
                _navMeshAgent.ResetPath ();
            }
        }

        private void UpdateAnimatorVariables ()
        {
            _animator.SetBool ("Moving", _navMeshAgent.velocity.magnitude > 0.1f);
            _animator.SetBool ("Casting", _unit.CurrentlyCasting != null);
        }
    }
}