using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct BreakContactSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BreakContactTimerComponent>> _filterBreakContactTimerC;
        private readonly EcsPoolInject<BreakContactTimerComponent> _poolBreakContactTimerC;
        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<AttackTargetComponent> _poolAttackTargetC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;
        private readonly EcsPoolInject<StopComponent> _poolStopC;

        private readonly EcsFilterInject<Inc<UnitCollisionStayComponent>> _filterUnitCollisionStayC;
        private readonly EcsPoolInject<UnitCollisionStayComponent> _poolCollisionStayC;

        private readonly EcsCustomInject<SharedData> _sharedDtata;
        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            TryBreakContact();
            TryCancelBreakContact();
        }

        private void TryBreakContact()
        {
            foreach (var entity in _filterBreakContactTimerC.Value)
            {
                var breakContactTimerC = _poolBreakContactTimerC.Value.Get(entity);

                if (breakContactTimerC.Time >= _sharedDtata.Value.ContactBreakTime)
                {
                    if (_poolAttackTargetC.Value.Get(entity).AttackTarget == null)
                    {
                        _poolMoveC.Value.Get(entity).MoveDirection = Vector3.zero;
                    }
                    else
                    {
                        if (!_poolStopC.Value.Has(entity))
                        {
                            _poolStopC.Value.Add(entity);
                        }
                    }

                    _poolContactTimerC.Value.Del(entity);
                    _poolBreakContactTimerC.Value.Del(entity);
                }
            }
        }

        private void TryCancelBreakContact()
        {
            foreach (var entity in _filterUnitCollisionStayC.Value)
            {
                var collisionStayC = _poolCollisionStayC.Value.Get(entity);
                int firstEntitiy = UnpackEntityUtils.UnpackEntity(_world.Value, collisionStayC.FirstCollide.EcsPacked);
                TryDeliteBreakContactTimerComponent(firstEntitiy);
            }
        }

        private void TryDeliteBreakContactTimerComponent(int entity)
        {
            // Можно убрать эту проверку, если прикреплять UnitCollisionStayComponent к сущности юнита. И делать 
            // фильтр по UnitCollisionStayComponent + BreakContactTimerComponent. Но тогда придется в CollidingUnit
            // делать проверку, весит ли уже на сущности юнита UnitCollisionStayComponent.
            if (_poolBreakContactTimerC.Value.Has(entity))
            {
                _poolBreakContactTimerC.Value.Del(entity);
            }
        }
    }
}