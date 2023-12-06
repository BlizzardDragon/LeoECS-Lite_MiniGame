using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct EnemySearchSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GunAttackComponent>> _filterGunAttackC;

        private readonly EcsPoolInject<GunAttackComponent> _poolGunAttackC;
        private readonly EcsPoolInject<AttackTargetComponent> _poolAttackTargetC;
        private readonly EcsPoolInject<MoveTargetComponent> _poolMoveTargetC;
        private readonly EcsPoolInject<TeamComponent> _poolTeamC;
        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<StopComponent> _poolStopC;


        public void Run(IEcsSystems systems)
        {
            SplitIntoTeams(out List<int> team_1, out List<int> team_2);

            if (team_1.Count < 1 || team_2.Count < 1) return;

            ConvertEntitiesToPositions(team_1, out Vector3[] positionsTeam_1);
            ConvertEntitiesToPositions(team_2, out Vector3[] positionsTeam_2);

            float[] minDistanceTeam_2 = new float[team_2.Count];
            for (int i = 0; i < minDistanceTeam_2.Length; i++)
            {
                minDistanceTeam_2[i] = Mathf.Infinity;
            }
            int[] nearestEntitiesToTeam_2 = new int[team_2.Count];
            
            int nearestEntity = 0;
            float currentDistanceSquared;
            Vector3 offset;

            for (int first = 0; first < positionsTeam_1.Length; first++)
            {
                float minDistanceSquared = Mathf.Infinity;

                for (int second = 0; second < positionsTeam_2.Length; second++)
                {
                    offset = positionsTeam_1[first] - positionsTeam_2[second];
                    currentDistanceSquared = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                    if (currentDistanceSquared < minDistanceSquared)
                    {
                        minDistanceSquared = currentDistanceSquared;
                        nearestEntity = team_2[second];
                    }

                    if (currentDistanceSquared < minDistanceTeam_2[second])
                    {
                        minDistanceTeam_2[second] = currentDistanceSquared;
                        nearestEntitiesToTeam_2[second] = team_1[first];
                    }
                }

                CustomizeEntity(
                    minDistanceSquared,
                    _poolGunAttackC.Value.Get(team_1[first]).AttackDistance,
                    nearestEntity,
                    team_1,
                    first);
            }

            for (int i = 0; i < nearestEntitiesToTeam_2.Length; i++)
            {
                CustomizeEntity(
                    minDistanceTeam_2[i],
                    _poolGunAttackC.Value.Get(team_2[i]).AttackDistance,
                    nearestEntitiesToTeam_2[i],
                    team_2,
                    i);
            }

            // TrySetTargetsForEntities(team_1, team_2);
            // TrySetTargetsForEntities(team_2, team_1);
        }

        private void SplitIntoTeams(out List<int> team_1, out List<int> team_2)
        {
            team_1 = new();
            team_2 = new();

            foreach (var entity in _filterGunAttackC.Value)
            {
                if (_poolTeamC.Value.Get(entity).Team == Teams.Team_1)
                {
                    team_1.Add(entity);
                }
                else if (_poolTeamC.Value.Get(entity).Team == Teams.Team_2)
                {
                    team_2.Add(entity);
                }
                else
                {
                    throw new Exception("Team not set");
                }
            }
        }

        private void CustomizeEntity(
            float distanceSquared,
            float attackDistance,
            int nearestEntity,
            List<int> team,
            int index)
        {
            GameObject target = _poolViewC.Value.Get(nearestEntity).ViewObject;
            _poolMoveTargetC.Value.Get(team[index]).MoveTarget = target;

            if (distanceSquared <= attackDistance * attackDistance)
            {
                _poolAttackTargetC.Value.Get(team[index]).AttackTarget = target;

                if (!_poolStopC.Value.Has(team[index]))
                {
                    _poolStopC.Value.Add(team[index]);
                }
            }
            else
            {
                if (_poolAttackTargetC.Value.Get(team[index]).AttackTarget == null)
                {
                    if (_poolStopC.Value.Has(team[index]))
                    {
                        _poolStopC.Value.Del(team[index]);
                    }
                }
                else
                {
                    _poolAttackTargetC.Value.Get(team[index]).AttackTarget = null;
                    _poolStopC.Value.Del(team[index]);
                }
            }
        }

        private void ConvertEntitiesToPositions(List<int> entities, out Vector3[] positions)
        {
            positions = new Vector3[entities.Count];

            for (int i = 0; i < entities.Count; i++)
            {
                positions[i] = _poolViewC.Value.Get(entities[i]).ViewObject.transform.position;
            }
        }

        // private bool TryGetNoTargetEntities(List<int> entities, out List<int> nullTargetEntities)
        // {
        //     nullTargetEntities = new();
        //     GunAttackComponent attackC;
        //     Vector3 entityPos;
        //     float distanceToTorget;
        //     Vector3 targetPos;

        //     foreach (var entity in entities)
        //     {
        //         ref var targetC = ref _poolTargetC.Value.Get(entity);

        //         // Сброс цели.
        //         if (targetC.Target)
        //         {
        //             attackC = _poolGunAttackC.Value.Get(entity);
        //             entityPos = _poolViewC.Value.Get(entity).ViewObject.transform.position;

        //             targetPos = targetC.Target.transform.position;
        //             distanceToTorget = Vector3.Distance(entityPos, targetPos);

        //             if (distanceToTorget > attackC.AttackDistance)
        //             {
        //                 targetC.Target = null;
        //             }
        //         }

        //         if (!targetC.Target)
        //         {
        //             nullTargetEntities.Add(entity);
        //         }
        //     }

        //     return nullTargetEntities.Count > 0;
        // }

        // private void TrySetTargetsForEntities(List<int> currentEntities, List<int> targetEntities)
        // {
        //     foreach (var entity in currentEntities)
        //     {
        //         ref var targetC = ref _poolTargetC.Value.Get(entity);
        //         var attackC = _poolGunAttackC.Value.Get(entity);

        //         Vector3 entityPos = _poolViewC.Value.Get(entity).ViewObject.transform.position;

        //         // Сброс цели.
        //         if (targetC.Target)
        //         {
        //             Vector3 targetPos = targetC.Target.transform.position;
        //             float distanceToTorget = Vector3.Distance(entityPos, targetPos);

        //             if (distanceToTorget > attackC.AttackDistance)
        //             {
        //                 targetC.Target = null;
        //             }
        //         }

        //         // Поиск цели.
        //         if (!targetC.Target)
        //         {
        //             float searchRadius = _poolGunAttackC.Value.Get(entity).AttackDistance;
        //             ConvertEntitiesToGameObjects(targetEntities, out List<GameObject> targets);

        //             GameObject target = FinderNearestTarget.FindNearestTarget(searchRadius, entityPos, targets);

        //             if (target == null)
        //             {
        //                 if (_poolStopC.Value.Has(entity))
        //                 {
        //                     _poolStopC.Value.Del(entity);
        //                 }
        //             }
        //             else
        //             {
        //                 targetC.Target = target;
        //                 if (!_poolStopC.Value.Has(entity))
        //                 {
        //                     _poolStopC.Value.Add(entity);
        //                 }
        //             }
        //         }
        //     }
        // }

        // private List<GameObject> ConvertEntitiesToGameObjects(List<int> entities, out List<GameObject> newHashSet)
        // {
        //     newHashSet = new();
        //     foreach (var targetEntity in entities)
        //     {
        //         GameObject targetObj = _poolViewC.Value.Get(targetEntity).ViewObject;
        //         newHashSet.Add(targetObj);
        //     }

        //     return newHashSet;
        // }
    }
}