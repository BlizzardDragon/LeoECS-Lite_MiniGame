using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct RetreatSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<HitComponent>, Exc<RetreatComponent>> _filterHitC;
        private readonly EcsFilterInject<Inc<RetreatComponent>> _filterRetreatC;
        private readonly EcsFilterInject<Inc<RetreatComponent, StopComponent>> _filterStopC;
        private readonly EcsFilterInject<Inc<RetreatComponent, ContactTimerComponent>> _filterContactTimerC;
        private readonly EcsFilterInject<Inc<RetreatComponent, BreakContactTimerComponent>> _filterBreakContactTimerC;
        private readonly EcsFilterInject<Inc<RetreatComponent, ContactTimerComponent, BreakContactTimerComponent>> _filterTimers;

        private readonly EcsPoolInject<HealthComponent> _poolHealthC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;
        private readonly EcsPoolInject<StopComponent> _poolStopC;
        private readonly EcsPoolInject<HealthRecoveryTimerComponent> _poolHealthRecoveryTimerC;

        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<BreakContactTimerComponent> _poolBreakContactTimerC;

        private readonly EcsPoolInject<RetreatComponent> _poolRetreatC;

        private readonly EcsCustomInject<SharedData> _sharedDtata;
        
        private int _maxHealth;
        float _minValue;


        public void Init(IEcsSystems systems)
        {
            _maxHealth = _sharedDtata.Value.UnitHealth;
            _minValue = _maxHealth * 0.2f;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterHitC.Value)
            {
                int health = _poolHealthC.Value.Get(entity).Health;
                if (health <= _minValue)
                {
                    _poolRetreatC.Value.Add(entity);
                    _poolHealthRecoveryTimerC.Value.Add(entity);

                    ref var moveC = ref _poolMoveC.Value.Get(entity);
                    moveC.MoveDirection = Vector3.back;
                    moveC.MoveSpeed /= 2;
                }
            }

            foreach (var entity in _filterRetreatC.Value)
            {
                int health = _poolHealthC.Value.Get(entity).Health;
                if (health >= _maxHealth)
                {
                    _poolRetreatC.Value.Del(entity);
                    _poolHealthRecoveryTimerC.Value.Del(entity);

                    ref var moveC = ref _poolMoveC.Value.Get(entity);
                    moveC.MoveDirection = Vector3.zero;
                    moveC.MoveSpeed *= 2;
                }
            }

            foreach (var entity in _filterStopC.Value)
            {
                _poolStopC.Value.Del(entity);
            }

            foreach (var entity in _filterContactTimerC.Value)
            {
                _poolContactTimerC.Value.Del(entity);
            }

            foreach (var entity in _filterBreakContactTimerC.Value)
            {
                _poolBreakContactTimerC.Value.Del(entity);
            }

            foreach (var entity in _filterTimers.Value)
            {
                _poolContactTimerC.Value.Del(entity);
                _poolBreakContactTimerC.Value.Del(entity);
            }
        }
    }
}