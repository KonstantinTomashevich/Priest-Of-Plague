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

public class InventoryBar : MonoBehaviour
{
    public Unit SourceUnit;
    public GameObject ItemUIItemPrefab;
    public GameObject ItemsListContentObject;
    public GameObject SelectedItemInfoObject;
    public GameObject DestroySelectedObjectButtonObject;

    public int SelectedItemId { get; private set; }
    private Dictionary <int, GameObject> _icons;

    public void Toggle ()
    {
        gameObject.SetActive (!gameObject.activeInHierarchy);
    }

    public void DestroySelectedItem ()
    {
        if (SelectedItemId != -1 && SourceUnit.MyStorage [SelectedItemId] != null)
        {
            SourceUnit.MyStorage.RemoveItem (SourceUnit.MyStorage [SelectedItemId]);
        }
    }

    private void Start ()
    {
        SelectedItemId = -1;
        _icons = new Dictionary <int, GameObject> ();
        SelectedItemInfoObject.SetActive (false);
        DestroySelectedObjectButtonObject.SetActive (false);

        if (SourceUnit.MyStorage != null)
        {
            foreach (var item in SourceUnit.MyStorage.Items)
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
        if (SelectedItemId != -1)
        {
            var item = SourceUnit.MyStorage [SelectedItemId];
            var itemType = SourceUnit.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
            var infoBuilder = new StringBuilder ();

            infoBuilder.Append (itemType.ShortInfo).AppendLine ().Append ("Level: ").Append (item.Level).Append (".")
                .AppendLine ().Append ("Charge: ").Append (Math.Round (item.Charge * 10.0f) / 10.0f).Append ("/")
                .Append (Math.Round (itemType.MaxCharge + itemType.MaxChargeAdditionPerLevel * item.Level * 10.0f) /
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
        _icons.Add (item.Id, itemUIItem);

        itemUIItem.transform.SetParent (ItemsListContentObject.transform);
        itemUIItem.transform.localScale = Vector3.one;

        var itemType = SourceUnit.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
        var image = itemUIItem.GetComponent <Image> ();
        image.sprite = itemType.Icon;

        var eventTrigger = itemUIItem.GetComponent <EventTrigger> ();
        var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
        entry.callback.AddListener (argument => { SendMessage ("ItemUIItemClicked", item.Id); });
        eventTrigger.triggers.Add (entry);
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
        if (argument.EventStorage == SourceUnit.MyStorage)
        {
            AddItemUIItem (argument.EventStorage [argument.ItemId]);
        }
    }

    private void ItemRemoved (object parameter)
    {
        var argument = parameter as Storage.ItemAddedOrRemovedEventData;
        if (argument.EventStorage == SourceUnit.MyStorage)
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
        if (SourceUnit.MyStorage [itemId] != null)
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