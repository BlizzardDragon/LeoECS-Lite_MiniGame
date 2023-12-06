using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct UnitCollisionEnterHandlingSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitCollisionEnterComponent>> _filterCollisionEnterC;

        private readonly EcsPoolInject<UnitCollisionEnterComponent> _poolCollisionEnterC;
        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;
        private readonly EcsPoolInject<StopComponent> _poolStopC;
        private readonly EcsPoolInject<RetreatComponent> _poolRetreatC;

        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterCollisionEnterC.Value)
            {
                var collisionEnterC = _poolCollisionEnterC.Value.Get(entity);

                (var firstEntitiy, var secondEntitiy) =
                    UnpackEntityUtils.UnpackEntity(
                        _world.Value,
                        collisionEnterC.FirstCollide.EcsPacked,
                        collisionEnterC.SecondCollide.EcsPacked);

                // Отмена добавления ContactTimerComponent.
                if (_poolRetreatC.Value.Has(firstEntitiy) || _poolRetreatC.Value.Has(secondEntitiy)) return;

                // Установка направления и добавление ContactTimerComponent.
                if (_poolStopC.Value.Has(firstEntitiy))
                {
                    RedirectUnit(secondEntitiy);
                }
                else
                {
                    if (_poolStopC.Value.Has(secondEntitiy))
                    {
                        RedirectUnit(firstEntitiy);
                    }
                    else
                    {
                        if (!_poolContactTimerC.Value.Has(firstEntitiy))
                        {
                            AddContactTimer(firstEntitiy);

                            if (!_poolContactTimerC.Value.Has(secondEntitiy))
                            {
                                AddContactTimer(secondEntitiy);
                                SetDirectionForTwo(firstEntitiy, secondEntitiy);
                            }
                            else
                            {
                                SetDirectionForTarget(firstEntitiy, secondEntitiy);
                            }
                        }
                        else
                        {
                            if (!_poolContactTimerC.Value.Has(secondEntitiy))
                            {
                                AddContactTimer(secondEntitiy);
                                SetDirectionForTarget(secondEntitiy, firstEntitiy);
                            }
                        }
                    }
                }
            }
        }

        private void AddContactTimer(int entity) => _poolContactTimerC.Value.Add(entity);

        private void RedirectUnit(int entity)
        {
            // Debug.Log($"ID = {_poolUnitC.Value.Get(entity).ID}, MoveC = {_poolMoveC.Value.Has(entity)}");

            // Проверка на вращение.
            if (_poolStopC.Value.Has(entity)) return;

            _poolMoveC.Value.Get(entity).MoveDirection = GetRandomDirection();

            if (!_poolContactTimerC.Value.Has(entity))
            {
                AddContactTimer(entity);
            }
        }

        private Vector3 GetRandomDirection()
        {
            float value = Random.value;
            Vector3 direction = value < 0.5f ? Vector3.left : Vector3.right;
            return direction;
        }

        private void SetDirectionForTwo(int entity_1, int entity_2)
        {
            ref var MoveC_1 = ref _poolMoveC.Value.Get(entity_1);
            ref var MoveC_2 = ref _poolMoveC.Value.Get(entity_2);

            float value = Random.value;
            if (value < 0.5f)
            {
                MoveC_1.MoveDirection = Vector3.left;
                MoveC_2.MoveDirection = Vector3.right;
            }
            else
            {
                MoveC_1.MoveDirection = Vector3.right;
                MoveC_2.MoveDirection = Vector3.left;
            }
        }

        private void SetDirectionForTarget(int targetEntity, int originEntity)
        {
            ref var origin = ref _poolMoveC.Value.Get(originEntity);
            ref var target = ref _poolMoveC.Value.Get(targetEntity);

            if (origin.MoveDirection == Vector3.left)
            {
                target.MoveDirection = Vector3.right;
            }
            else if (origin.MoveDirection == Vector3.right)
            {
                target.MoveDirection = Vector3.left;
            }
        }
    }
}