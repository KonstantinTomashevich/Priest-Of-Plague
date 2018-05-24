using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Spells;
using PriestOfPlague.Source.Unit;
using UnityEngine.SceneManagement;

public class TestFireWallSpell {
	[UnityTest]
	public IEnumerator TestFireWallSpellPasses()
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
		firstUnitObject.transform.position = new Vector3(3.0f, 0.0f, -2.0f);
		yield return null;
		
		var secondUnitObject = new GameObject ("Unit2");
		var secondUnit = secondUnitObject.AddComponent <Unit> ();
		secondUnitObject.transform.position = new Vector3(3.0f, 0.0f, 2.0f);
		yield return null;

		var itemsTypesContainer = gameEngineCore.GetComponent <ItemTypesContainer> ();
		Assert.NotNull (itemsTypesContainer);
		var itemsRegistrator = gameEngineCore.GetComponent <ItemsRegistrator> ();
		Assert.NotNull (itemsRegistrator);
		
		var thirdUnitObject = new GameObject ("Unit3");
		var thirdUnit = thirdUnitObject.AddComponent <Unit> ();
		thirdUnitObject.transform.position = Vector3.zero;
		thirdUnitObject.transform.rotation = Quaternion.LookRotation (Vector3.right);
		yield return null;

		thirdUnit.SetCharactiristic (CharacteristicsEnum.Strength, 15);
		thirdUnit.AvailableSpells.Add (SpellsInitializer.FireWallSpellId);

		var wandItem = new Item (itemsRegistrator, 0, 10.0f, 1);
		Assert.True (thirdUnit.MyStorage.AddItem (wandItem));
		yield return null;
		
		Assert.NotNull (firstUnit);
		Assert.NotNull (secondUnit);
		Assert.NotNull (thirdUnit);

		var spellsContainer = gameEngineCore.GetComponent <SpellsContainer> ();
		Assert.NotNull (spellsContainer);
		var fireWallSpell = spellsContainer.Spells [SpellsInitializer.FireWallSpellId];
		Assert.NotNull (fireWallSpell);

		Assert.True (fireWallSpell.CanCast (thirdUnit, 1, wandItem));
		fireWallSpell.Cast (thirdUnit, unitsHub, new SpellCastParameter (wandItem, 1, null));

		for (int index = 0; index < 100; index++)
		{
			yield return null;
		}
		
		Assert.True (firstUnitObject == null);
		Assert.True (secondUnitObject == null);
		Assert.False (thirdUnit == null);
	}
}
