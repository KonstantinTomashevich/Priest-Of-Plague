using System;
using System.Collections.Generic;
using UnityEngine;

namespace PriestOfPlague.Source.Hubs
{
    public static class UnitsHubCriterias
    {
        public static bool MaxDistanceAndMaxAngle (Unit.Unit asker, Unit.Unit unit,
            float maxDistance, float maxAngle = 180.0f)
        {
            return (unit.transform.position - asker.transform.position).magnitude <= maxDistance &&
                   Math.Abs (Quaternion.LookRotation (asker.transform.InverseTransformPoint (unit.transform.position))
                       .eulerAngles.y) <= maxAngle;
        }
    }
}