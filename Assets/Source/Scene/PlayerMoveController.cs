using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PriestOfPlague.Source.Core
{
	public class PlayerMoveController : MonoBehaviour
	{
		private NavMeshAgent _navMeshAgent;
		private Animator _animator;
		
		void Start ()
		{
			_navMeshAgent = GetComponent <NavMeshAgent> ();
			_animator = GetComponent <Animator> ();
		}

		void Update ()
		{
			float moveModifier = 0.0f;
			if (Input.GetKey (KeyCode.W)) moveModifier += 1.0f;
			_navMeshAgent.velocity = transform.forward * _navMeshAgent.speed * moveModifier;

			float rotationModifer = 0.0f;
			if (Input.GetKey (KeyCode.D)) rotationModifer += 1.0f;
			if (Input.GetKey (KeyCode.A)) rotationModifer -= 1.0f;
			transform.Rotate (transform.up, _navMeshAgent.angularSpeed * Time.deltaTime * rotationModifer);

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
