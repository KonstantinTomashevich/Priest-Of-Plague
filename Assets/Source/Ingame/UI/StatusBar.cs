using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame.UI
{
    public class StatusBar : MonoBehaviour
    {
        public Unit.Unit SourceUnit;
        public GameObject ModifierUIItemPrefab;
        public Text HealthText;
        public Text MovementsPointsText;

        private List <GameObject> _modifiersUiItems;

        private void Start ()
        {
            _modifiersUiItems = new List <GameObject> ();
        }

        private void Update ()
        {
            UpdateHealthAndMovementPointsTexts ();
            UpdateUiItemsCount ();
            UpdateModifiersUiItems ();
        }

        private void UpdateHealthAndMovementPointsTexts ()
        {
            var builder = new StringBuilder ();
            HealthText.text = builder.Append (Math.Round (SourceUnit.CurrentHp)).Append ("/")
                .Append (Math.Round (SourceUnit.MaxHp)).ToString ();

            builder.Clear ();
            MovementsPointsText.text = builder.Append (Math.Round (SourceUnit.CurrentMp)).Append ("/")
                .Append (Math.Round (SourceUnit.MaxMp)).ToString ();
        }

        private void UpdateUiItemsCount ()
        {
            int modifiersCount = SourceUnit.ModifiersOnUnit.Count;
            while (_modifiersUiItems.Count > modifiersCount)
            {
                Destroy (_modifiersUiItems.Last ());
                _modifiersUiItems.RemoveAt (_modifiersUiItems.Count - 1);
            }

            while (_modifiersUiItems.Count < modifiersCount)
            {
                var modifierUiItem = Instantiate (ModifierUIItemPrefab);
                modifierUiItem.transform.SetParent (transform);
                modifierUiItem.transform.localScale = Vector3.one;
                _modifiersUiItems.Add (modifierUiItem);
            }
        }

        private void UpdateModifiersUiItems ()
        {
            int uiItemIndex = 0;
            foreach (var modifierData in SourceUnit.ModifiersOnUnit)
            {
                var uiItem = _modifiersUiItems [uiItemIndex];
                var text = uiItem.GetComponentInChildren <Text> ();

                var builder = new StringBuilder ();
                builder.Append (modifierData.Level).Append ("L").AppendLine ().Append (Math.Round (modifierData.Time))
                    .Append ("s");
                text.text = builder.ToString ();

                var modifierType = SourceUnit.CharacterModifiersContainerRef.Modifiers [modifierData.Id];
                var image = uiItem.GetComponentInChildren <Image> ();
                image.sprite = modifierType.Icon;
                uiItemIndex++;
            }
        }
    }
}