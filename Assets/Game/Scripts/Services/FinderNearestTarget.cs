using System.Collections.Generic;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services
{
    public struct FinderNearestTarget
    {
        public static GameObject FindNearestTarget(float searchRadius, Vector3 currentPosition, GameObject[] targets)
        {
            GameObject nearestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(currentPosition, target.transform.position);

                if (distance > searchRadius) continue;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }

        public static GameObject FindNearestTarget(float searchRadius, Vector3 currentPosition, List<GameObject> targets)
        {
            GameObject nearestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(currentPosition, target.transform.position);

                if (distance > searchRadius) continue;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }
    }
}