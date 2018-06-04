using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace PriestOfPlague.Source.Ingame
{
	public class UnitAnimator : MonoBehaviour
	{
		protected NavMeshAgent _navMeshAgent;
		protected Animator _animator;
		protected Unit.Unit _unit;
		private bool _dyingCoroutineWorking;
		private bool _dieAnimated;
		
		protected IEnumerator Start ()
		{
			_navMeshAgent = GetComponent <NavMeshAgent> ();
			_animator = GetComponentInChildren <Animator> ();
			_unit = null;
			_dyingCoroutineWorking = false;
			_dieAnimated = false;
            
			do
			{
				yield return null;
				_unit = GetComponent <Unit.Unit> ();
			} while (_unit == null);
		}

		protected void Update ()
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
			if (_unit.MovementBlocked || _unit.CurrentlyCasting != null || !_unit.Alive)
			{
				_animator.SetBool ("Moving", false);
				if (_navMeshAgent.enabled)
				{
					_navMeshAgent.ResetPath ();
				}
			}

			if (_unit.Alive)
			{
				_dieAnimated = false;
				var rotation = _unit.transform.rotation.eulerAngles;
				rotation.z = 0;
				_unit.transform.rotation = Quaternion.Euler (rotation);
			}

			if (_navMeshAgent.enabled != _unit.Alive)
			{
				_navMeshAgent.enabled = _unit.Alive;
			}
		}

		private void UpdateAnimatorVariables ()
		{
			_animator.SetBool ("Moving", _navMeshAgent.velocity.magnitude > 0.1f);
			_animator.SetBool ("Casting", !_unit.MovementBlocked && _unit.CurrentlyCasting != null);
			_animator.SetBool ("Alive", _unit.Alive);

			if (!_animator.GetBool ("HasDeathAnimation"))
			{
				_animator.speed = _unit.Alive ? 1.0f : 0.0f;
			}

			if (!_unit.Alive && !_animator.GetBool ("HasDeathAnimation") && !_dyingCoroutineWorking && !_dieAnimated)
			{
				StartCoroutine (CustomDyingAnimation ());
			}
		}
		
		private IEnumerator CustomDyingAnimation ()
		{
			_dyingCoroutineWorking = true;
			float zRotation = 0.0f;
			
			while (true)
			{
				zRotation += 90.0f * Time.deltaTime;
				var rotation = _unit.transform.rotation.eulerAngles;
				rotation.z = zRotation;
				_unit.transform.rotation = Quaternion.Euler (rotation);
				yield return null;

				if (zRotation >= 90.0f)
				{
					_dyingCoroutineWorking = false;
					_dieAnimated = true;
					yield break;
				}
			}
		}
	}
}
