using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Unit;
using UnityEngine.SceneManagement;

public class TestUnitsHubEvents {
	[UnityTest]
	public IEnumerator TestUnitsHubEventsWithEnumeratorPasses()
	{
		SceneManager.LoadScene ("TestsScene");
		yield return null;
		
		var gameEngineCore = GameObject.FindGameObjectWithTag ("GameEngineCore");
		Assert.NotNull (gameEngineCore);

		var unitsHub = gameEngineCore.GetComponent <UnitsHub> ();
		Assert.NotNull (unitsHub);
		Assert.IsEmpty (unitsHub.Units);
		
		var firstUnitObject = new GameObject ("Unit1");
		var firstUnit = firstUnitObject.AddComponent <Unit> ();
		yield return null;
		
		var secondUnitObject = new GameObject ("Unit2");
		var secondUnit = secondUnitObject.AddComponent <Unit> ();
		yield return null;
		
		Assert.AreEqual (2, unitsHub.Units.Count);
		Assert.IsTrue (unitsHub.Units.Contains (firstUnit));
		Assert.IsTrue (unitsHub.Units.Contains (secondUnit));
	}
}
