using System;
using System.Collections.Generic;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Hubs
{
    public static class UnitsHubCriterias
    {
        public static bool MaxDistanceAndMaxAngle (Unit.Unit asker, Unit.Unit unit,
            float maxDistance, float maxAngle = 180.0f)
        {
            return (unit.transform.position - asker.transform.position).magnitude <= maxDistance &&
                   Math.Abs (MathUtils.AngleFrom_0_360To_m180_180 (
                       Quaternion.LookRotation (unit.transform.position - asker.transform.position)
                           .eulerAngles.y - asker.transform.rotation.eulerAngles.y)) <= maxAngle;
        }
    }
}