using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Menu
{
	public class LightAnimator : MonoBehaviour
	{
		IEnumerator Start ()
		{
			var light = gameObject.GetComponent <Light> ();
			while (true)
			{
				while (light.intensity < 1.0f)
				{
					light.intensity += Time.deltaTime;
					yield return null;
				}
				
				while (light.intensity > 0.0f)
				{
					light.intensity -= Time.deltaTime;
					yield return null;
				}
			}
		}
	}
}
