using System.Collections;
using System.Collections.Generic;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Spells;
using PriestOfPlague.Source.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellsBar : MonoBehaviour
{
	public Unit SourceUnit;
	public GameObject SpellUIItemPrefab;
	private Dictionary <int, GameObject> _icons;

	private void Start () 
	{
		_icons = new Dictionary <int, GameObject> ();
		foreach (var spellId in SourceUnit.AvailableSpells)
		{
			AddSpellUIItem (spellId);
		}
		
		EventsHub.Instance.Subscribe (this, Unit.EventSpellLearned);
		EventsHub.Instance.Subscribe (this, Unit.EventSpellForgotten);
	}

	private void OnDestroy ()
	{
		EventsHub.Instance.Unsubscribe (this, Unit.EventSpellLearned);
		EventsHub.Instance.Unsubscribe (this, Unit.EventSpellForgotten);
	}

	private void AddSpellUIItem (int spellId)
	{
		if (_icons.ContainsKey (spellId))
		{
			return;
		}
		
		var spellsContainer = GameObject.FindGameObjectWithTag ("GameEngineCore").GetComponent <SpellsContainer> ();
		var spell = spellsContainer.Spells [spellId];
		var spellUIItem = Instantiate (SpellUIItemPrefab);
		_icons.Add (spellId, spellUIItem);
		
		spellUIItem.transform.SetParent (transform);
		spellUIItem.transform.localScale = Vector3.one;

		var image = spellUIItem.GetComponent <Image> ();
		image.sprite = spell.Icon;

		var eventTrigger = spellUIItem.GetComponent <EventTrigger> ();
		var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
		entry.callback.AddListener (argument => { SendMessage ("SpellUIItemClicked", spellId); });
		eventTrigger.triggers.Add (entry);
	}

	private void RemoveSpellUIItem (int spellId)
	{
		if (_icons.ContainsKey (spellId))
		{
			Destroy (_icons [spellId]);
			_icons.Remove (spellId);
		}
	}

	private void SpellLearned (object parameter)
	{
		var argument = parameter as Unit.SpellLearnedOrForgottenEventData;
		if (argument.EventUnit == SourceUnit)
		{
			AddSpellUIItem (argument.SpellId);
		}
	}
	
	private void SpellForgotten (object parameter)
	{
		var argument = parameter as Unit.SpellLearnedOrForgottenEventData;
		if (argument.EventUnit == SourceUnit)
		{
			RemoveSpellUIItem (argument.SpellId);
		}
	}

	private void SpellUIItemClicked (object parameter)
	{
		// TODO: Implement.
	}
}
