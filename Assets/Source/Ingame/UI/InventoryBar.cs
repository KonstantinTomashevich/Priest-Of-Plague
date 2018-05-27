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

    private Dictionary <int, GameObject> _icons;
    private int _selectedItemId;

    public void Toggle ()
    {
        gameObject.SetActive (!gameObject.activeInHierarchy);
    }

    public void DestroySelectedItem ()
    {
        if (_selectedItemId != -1 && SourceUnit.MyStorage [_selectedItemId] != null)
        {
            SourceUnit.MyStorage.RemoveItem (SourceUnit.MyStorage [_selectedItemId]);
        }
    }

    private void Start ()
    {
        _selectedItemId = -1;
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
            
            if (_selectedItemId == argument.ItemId)
            {
                _selectedItemId = -1;
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
            _selectedItemId = itemId;
            SelectedItemInfoObject.SetActive (true);
            DestroySelectedObjectButtonObject.SetActive (true);

            var item = SourceUnit.MyStorage [itemId];
            var itemType = SourceUnit.ItemTypesContainerRef.ItemTypes [item.ItemTypeId];
            var infoBuilder = new StringBuilder ();

            infoBuilder.Append (itemType.ShortInfo).AppendLine ().Append ("Level: ").Append (item.Level).Append (".")
                .AppendLine ().Append ("Charge: ").Append (item.Charge).Append ("/")
                .Append (itemType.MaxCharge + itemType.MaxChargeAdditionPerLevel * item.Level).Append (".")
                .AppendLine ();

            SelectedItemInfoObject.GetComponent <Text> ().text = infoBuilder.ToString ();
        }
        else
        {
            _selectedItemId = -1;
            SelectedItemInfoObject.SetActive (false);
            DestroySelectedObjectButtonObject.SetActive (false);
        }
    }
}