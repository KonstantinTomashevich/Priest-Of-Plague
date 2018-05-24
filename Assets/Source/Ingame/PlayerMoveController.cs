using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame
{
    public enum PlayerCurrentActionType
    {
        Walk = 0
    }

    public class PlayerMoveController : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Camera _playerCamera;

        public PlayerCurrentActionType PlayerCurrentAction = PlayerCurrentActionType.Walk;
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
                    }
                }
            }
        }

        private void Start ()
        {
            _navMeshAgent = GetComponent <NavMeshAgent> ();
            _animator = GetComponent <Animator> ();
            _playerCamera = GetComponentInChildren <Camera> ();
        }

        private void Update ()
        {
            UpdateAnimatorVariables ();
        }

        private void UpdateAnimatorVariables ()
        {
            if (_navMeshAgent.velocity.magnitude > 0.1f)
            {
                _animator.SetFloat ("Forward", _navMeshAgent.velocity.magnitude);
            }
            else
            {
                _animator.SetFloat ("Forward", 0.0f);
            }
        }
    }
}