using System.Threading;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class ItemsRegistrator : MonoBehaviour
    {
        public GameObject SpawnedItemGameObject;
        
        public ItemsRegistrator ()
        {
            FreeId = 0;
        }

        public void Register (Item item)
        {
            if (item.Id >= FreeId)
            {
                FreeId = item.Id + 1;
            }
        }

        public void SpawnItemAsObject (ItemTypesContainer itemTypesContainer, Item item, Vector3 position)
        {
            var newObject = Instantiate (SpawnedItemGameObject);
            newObject.transform.position = position;
            newObject.GetComponent <SpawnedItemContainer> ().SpawnedItem = item;
            newObject.GetComponentInChildren <SpriteRenderer> ().sprite =
                itemTypesContainer.ItemTypes [item.ItemTypeId].Icon;
        }
        public int FreeId { get; private set; }
    }
}