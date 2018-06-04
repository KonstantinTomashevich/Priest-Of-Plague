using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PriestOfPlague.Source.Menu
{
	public class UiActionsProcessor : MonoBehaviour
	{
		public void LevelClicked (int id)
		{
			SceneManager.LoadScene (id);
		}
		
		public void ExitClicked ()
		{
			Application.Quit ();
		}
	}
}
