using System.Threading;
using UnityEngine;

namespace PriestOfPlague.Source.Items
{
    public class ItemsRegistrator : MonoBehaviour
    {
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
        public int FreeId { get; private set; }
    }
}