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
        public GameObject UnitObject;
        public GameObject ModifierUIItemPrefab;
        public GameObject ModifiersListContent;
        public Text HealthText;
        public Text MovementsPointsText;

        private Unit.Unit _unit;
        private List <GameObject> _modifiersUiItems;

        private IEnumerator Start ()
        {
            _modifiersUiItems = new List <GameObject> ();
            _unit = null;
            
            do
            {
                yield return null;
                _unit = UnitObject.GetComponent <Unit.Unit> ();
            } while (_unit == null);
        }

        private void Update ()
        {
            if (_unit == null)
            {
                return;
            }
            
            UpdateHealthAndMovementPointsTexts ();
            UpdateUiItemsCount ();
            UpdateModifiersUiItems ();
        }

        private void UpdateHealthAndMovementPointsTexts ()
        {
            var builder = new StringBuilder ();
            HealthText.text = builder.Append (Math.Round (_unit.CurrentHp)).Append ("/")
                .Append (Math.Round (_unit.MaxHp)).ToString ();

            builder.Clear ();
            MovementsPointsText.text = builder.Append (Math.Round (_unit.CurrentMp)).Append ("/")
                .Append (Math.Round (_unit.MaxMp)).ToString ();
        }

        private void UpdateUiItemsCount ()
        {
            int modifiersCount = _unit.ModifiersOnUnit.Count;
            while (_modifiersUiItems.Count > modifiersCount)
            {
                Destroy (_modifiersUiItems.Last ());
                _modifiersUiItems.RemoveAt (_modifiersUiItems.Count - 1);
            }

            while (_modifiersUiItems.Count < modifiersCount)
            {
                var modifierUiItem = Instantiate (ModifierUIItemPrefab);
                modifierUiItem.transform.SetParent (ModifiersListContent.transform);
                modifierUiItem.transform.localScale = Vector3.one;
                _modifiersUiItems.Add (modifierUiItem);
            }

            ModifiersListContent.GetComponent <RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,
                ModifiersListContent.GetComponent <VerticalLayoutGroup> ().preferredHeight);
        }

        private void UpdateModifiersUiItems ()
        {
            int uiItemIndex = 0;
            foreach (var modifierData in _unit.ModifiersOnUnit)
            {
                var uiItem = _modifiersUiItems [uiItemIndex];
                var text = uiItem.GetComponentInChildren <Text> ();

                var builder = new StringBuilder ();
                builder.Append (modifierData.Level).Append ("L").AppendLine ().Append (Math.Round (modifierData.Time))
                    .Append ("s");
                text.text = builder.ToString ();

                var modifierType = _unit.CharacterModifiersContainerRef.Modifiers [modifierData.Id];
                var image = uiItem.GetComponentInChildren <Image> ();
                image.sprite = modifierType.Icon;
                uiItemIndex++;
            }
        }
    }
}