using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PriestOfPlague.Source.Hubs;
using UnityEngine;

namespace PriestOfPlague.Source.Ingame.UI
{
    public class HealthLabels : MonoBehaviour
    {
        public GameObject UnitObject;
        public UnitsHub UnitsHubRef;
        public Color AllyColor;
        public Color EnemyColor;

        private Unit.Unit _unit;
        private Camera _camera;

        private IEnumerator Start ()
        {
            do
            {
                yield return null;
                _unit = UnitObject.GetComponent <Unit.Unit> ();
            } while (_unit == null);

            _camera = UnitObject.GetComponentInChildren <Camera> ();
        }

        private void OnGUI ()
        {
            if (_unit == null)
            {
                return;
            }

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = Screen.height / 30;

            foreach (var unit in UnitsHubRef.GetUnitsByCriteria (unitToCheck =>
                UnitsHubCriterias.MaxDistanceAndMaxAngle (_unit, unitToCheck, 30.0f)))
            {
                if (unit.Id != _unit.Id)
                {
                    var screenPoint = _camera.WorldToScreenPoint (unit.transform.position + Vector3.up * 5);
                    var rect = new Rect (screenPoint.x - 200, Screen.height - screenPoint.y, 400, 100);

                    var builder = new StringBuilder ();
                    if (unit.Alive)
                    {
                        builder.Append (unit.Name).AppendLine ().Append ("ОЗ: ")
                            .Append (Math.Round (unit.CurrentHp * 10.0f) / 10.0f).Append ("/")
                            .Append (Math.Round (unit.MaxHp * 10.0f) / 10.0f).Append (".");
                    }
                    else
                    {
                        builder.Append ("Мёртв.");
                    }

                    GUI.skin.label.normal.textColor = unit.Alignment == _unit.Alignment ? AllyColor : EnemyColor;
                    GUI.Label (rect, builder.ToString ());
                }
            }
        }
    }
}