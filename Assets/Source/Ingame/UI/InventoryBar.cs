using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame.UI
{
    public class InventoryBar : MonoBehaviour
    {
        public GameObject UnitObject;
        public GameObject ItemUIItemPrefab;
        public GameObject ItemsListContentObject;
        public GameObject SelectedItemInfoObject;
        public GameObject DestroySelectedObjectButtonObject;

        public int SelectedItemId { get; private set; }
        private Unit.Unit _unit;
        private Dictionary <int, GameObject> _icons;

        public void Toggle ()
        {
            gameObject.SetActive (!gameObject.activeInHierarchy);
        }

        public void DestroySelectedItem ()
        {
            if (SelectedItemId != -1 && _unit.MyStorage [SelectedItemId] != null)
            {
                _unit.MyStorage.RemoveItem (_unit.MyStorage [SelectedItemId]);
            }
        }

        private IEnumerator Start ()
        {
            SelectedItemId = -1;
            _icons = new Dictionary <int, GameObject> ();
            SelectedItemInfoObject.SetActive (false);
            DestroySelectedObjectButtonObject.SetActive (false);
            _unit = null;

            do
            {
                yield return null;
                _unit = UnitObject.GetComponent <Unit.Unit> ();
            } while (_unit == null);

            if (_unit.MyStorage != null)
            {
                foreach (var item in _unit.MyStorage.Items)
                {
                    AddItemUIItem (item);
                }
            }

            EventsHub.Instance.Subscribe (this, Storage.EventItemAdded);
            EventsHub.Instance.Subscribe (this, Storage.EventItemRemoved);
        }

        private void OnDestroy ()
        {
            EventsHub.Instance.Subscribe (this, Storage.EventItemAdded);
            EventsHub.Instance.Subscribe (this, Storage.EventItemRemoved);
        }

        private void Update ()
        {
            if (_unit == null)
            {
                return;
            }

            if (SelectedItemId != -1)
            {
                var item = _unit.MyStorage [SelectedItemId];
                var itemType = _unit.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
                var infoBuilder = new StringBuilder ();

                infoBuilder.Append (itemType.ShortInfo).AppendLine ().Append ("Level: ").Append (item.Level)
                    .Append (".")
                    .AppendLine ().Append ("Charge: ").Append (Math.Round (item.Charge * 10.0f) / 10.0f).Append ("/")
                    .Append (
                        Math.Round ((itemType.MaxCharge + itemType.MaxChargeAdditionPerLevel * item.Level) * 10.0f) /
                        10.0f).Append (".").AppendLine ();

                SelectedItemInfoObject.GetComponent <Text> ().text = infoBuilder.ToString ();
            }
        }

        private void AddItemUIItem (Item item)
        {
            if (_icons.ContainsKey (item.Id))
            {
                return;
            }

            var itemUIItem = Instantiate (ItemUIItemPrefab);
            itemUIItem.transform.SetParent (ItemsListContentObject.transform);
            itemUIItem.transform.localScale = Vector3.one;
            _icons.Add (item.Id, itemUIItem);

            var itemType = _unit.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
            var image = itemUIItem.GetComponent <Image> ();
            image.sprite = itemType.Icon;

            var eventTrigger = itemUIItem.GetComponent <EventTrigger> ();
            var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
            entry.callback.AddListener (argument => { SendMessage ("ItemUIItemClicked", item.Id); });
            eventTrigger.triggers.Add (entry);

            ItemsListContentObject.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (
                RectTransform.Axis.Vertical, _icons.Count * 170);
        }

        private void RemoveItemUIItem (int itemId)
        {
            if (_icons.ContainsKey (itemId))
            {
                Destroy (_icons [itemId]);
                _icons.Remove (itemId);
            }
        }

        private void ItemAdded (object parameter)
        {
            var argument = parameter as Storage.ItemAddedOrRemovedEventData;
            if (argument.EventStorage == _unit.MyStorage)
            {
                AddItemUIItem (argument.EventStorage [argument.ItemId]);
            }
        }

        private void ItemRemoved (object parameter)
        {
            var argument = parameter as Storage.ItemAddedOrRemovedEventData;
            if (argument.EventStorage == _unit.MyStorage)
            {
                RemoveItemUIItem (argument.ItemId);

                if (SelectedItemId == argument.ItemId)
                {
                    SelectedItemId = -1;
                    SelectedItemInfoObject.SetActive (false);
                    DestroySelectedObjectButtonObject.SetActive (false);
                }
            }
        }

        private void ItemUIItemClicked (object parameter)
        {
            int itemId = (int) parameter;
            if (_unit.MyStorage [itemId] != null)
            {
                SelectedItemId = itemId;
                SelectedItemInfoObject.SetActive (true);
                DestroySelectedObjectButtonObject.SetActive (true);
            }
            else
            {
                SelectedItemId = -1;
                SelectedItemInfoObject.SetActive (false);
                DestroySelectedObjectButtonObject.SetActive (false);
            }
        }
    }
}