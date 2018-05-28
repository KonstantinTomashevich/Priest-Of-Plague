using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Spells;
using PriestOfPlague.Source.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame.UI
{
	public class SpellsBar : MonoBehaviour
	{
		public Unit.Unit SourceUnit;
		public GameObject SpellUIItemPrefab;
		public Text SpellMaxLevelIndicatorTextObject;
		public InventoryBar InventoryBarRef;

		public const int MaxCastLevel = 10;
		private Dictionary <int, GameObject> _icons;

		private void Start ()
		{
			_icons = new Dictionary <int, GameObject> ();
			if (SourceUnit.AvailableSpells != null)
			{
				foreach (var spellId in SourceUnit.AvailableSpells)
				{
					AddSpellUIItem (spellId);
				}
			}

			EventsHub.Instance.Subscribe (this, Unit.Unit.EventSpellLearned);
			EventsHub.Instance.Subscribe (this, Unit.Unit.EventSpellForgotten);
		}

		private void OnDestroy ()
		{
			EventsHub.Instance.Unsubscribe (this, Unit.Unit.EventSpellLearned);
			EventsHub.Instance.Unsubscribe (this, Unit.Unit.EventSpellForgotten);
		}

		private void Update ()
		{
			int maxCastLevelWithSelectedItem;
			Item selectedItem = SourceUnit.MyStorage [InventoryBarRef.SelectedItemId];

			UpdateMaxLevelIndicatorText (out maxCastLevelWithSelectedItem, selectedItem);
			ProcessInput (maxCastLevelWithSelectedItem, selectedItem);
		}

		private void UpdateMaxLevelIndicatorText (out int maxCastLevelWithSelectedItem, Item selectedItem)
		{
			if (SourceUnit.CurrentlyCasting == null)
			{
				SpellMaxLevelIndicatorTextObject.text = "~";
			}

			maxCastLevelWithSelectedItem = 0;
			selectedItem = SourceUnit.MyStorage [InventoryBarRef.SelectedItemId];

			if (selectedItem == null)
			{
				SpellMaxLevelIndicatorTextObject.text = "~";
				return;
			}

			for (int index = 1; index <= Math.Min (MaxCastLevel, selectedItem.Level); index++)
			{
				if (SourceUnit.CanCast (index, selectedItem))
				{
					maxCastLevelWithSelectedItem = index;
				}
			}

			if (maxCastLevelWithSelectedItem > 0)
			{
				SpellMaxLevelIndicatorTextObject.text = maxCastLevelWithSelectedItem.ToString ();
			}
			else
			{
				SpellMaxLevelIndicatorTextObject.text = "~";
			}
		}

		private void ProcessInput (int maxCastLevelWithSelectedItem, Item selectedItem)
		{
			if (maxCastLevelWithSelectedItem > 0)
			{
				for (int code = 1; code <= maxCastLevelWithSelectedItem; code++)
				{
					var keyCode = KeyCode.Alpha0 + (code % 10);
					if (Input.GetKeyDown (keyCode))
					{
						SourceUnit.CastSpell (code, selectedItem);
						break;
					}
				}
			}
		}

		private void AddSpellUIItem (int spellId)
		{
			if (_icons.ContainsKey (spellId))
			{
				return;
			}

			var spell = SourceUnit.SpellsContainerRef.Spells [spellId];
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
			var argument = parameter as Unit.Unit.SpellLearnedOrForgottenEventData;
			if (argument.EventUnit == SourceUnit)
			{
				AddSpellUIItem (argument.SpellId);
			}
		}

		private void SpellForgotten (object parameter)
		{
			var argument = parameter as Unit.Unit.SpellLearnedOrForgottenEventData;
			if (argument.EventUnit == SourceUnit)
			{
				RemoveSpellUIItem (argument.SpellId);
			}
		}

		private void SpellUIItemClicked (object parameter)
		{
			SourceUnit.StartCastingSpell (SourceUnit.SpellsContainerRef.Spells [(int) parameter]);
		}
	}
}
