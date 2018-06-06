using System.Collections;
using System.Collections.Generic;
using System.Text;
using PriestOfPlague.Source.Unit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PriestOfPlague.Source.Ingame.UI
{
    public class TopBar : MonoBehaviour
    {
        public DialogPanel DialogPanelRef;
        public Text InfoText;
        public GameObject UnitObject;
        private Unit.Unit _unit;

        public void GoToMainMenuPressed ()
        {
            DialogPanelRef.Show ("Вы действительно хотите выйти в главное меню?", "Да.", "Нет.",
                () => SceneManager.LoadScene (0), () => { });
        }

        private IEnumerator Start ()
        {
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

            var builder = new StringBuilder ();
            builder.Append (_unit.Name).Append (" | ").Append (_unit.IsMan ? "Мужчина" : "Женщина").Append (" | ")
                .Append (_unit.LineagesContainerRef.LineagesList [_unit.LineageId].InfoAboutLineage).Append (" | ")
                .Append (_unit.Experience).Append (" опыта | Вес инвентаря ").Append (_unit.MyStorage.CurrentWeight)
                .Append ("/").Append (_unit.MyStorage.MaxWeight);
            InfoText.text = builder.ToString ();
        }
    }
}