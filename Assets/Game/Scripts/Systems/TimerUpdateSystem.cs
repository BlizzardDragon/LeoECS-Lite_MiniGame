using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct TimerUpdateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthRecoveryTimerComponent>> _filterHealthRecoveryTimerC;
        private readonly EcsFilterInject<Inc<BreakContactTimerComponent>> _filterBreakContactTimerC;
        private readonly EcsFilterInject<Inc<RechargeTimerComponent>> _filterRechargeTimerC;
        private readonly EcsFilterInject<Inc<ContactTimerComponent>> _filterContactTimerC;
        private readonly EcsFilterInject<Inc<BlinkTimerComponent>> _filterBlinkTimerC;
        private readonly EcsFilterInject<Inc<LifeTimerComponent>> _filterLifeTimerC;

        private readonly EcsPoolInject<HealthRecoveryTimerComponent> _poolHealthRecoveryTimerC;
        private readonly EcsPoolInject<BreakContactTimerComponent> _poolBreakContactTimerC;
        private readonly EcsPoolInject<RechargeTimerComponent> _poolRechargeTimerC;
        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<BlinkTimerComponent> _poolLifeBlinkTimerC;
        private readonly EcsPoolInject<LifeTimerComponent> _poolLifeTimerC;


        public void Run(IEcsSystems systems)
        {
            float deltaTime = Time.deltaTime;

            foreach (var entity in _filterBreakContactTimerC.Value)
            {
                _poolBreakContactTimerC.Value.Get(entity).Time += deltaTime;
            }

            foreach (var entity in _filterRechargeTimerC.Value)
            {
                _poolRechargeTimerC.Value.Get(entity).Time -= deltaTime;
            }

            foreach (var entity in _filterContactTimerC.Value)
            {
                _poolContactTimerC.Value.Get(entity).Time += deltaTime;
            }

            foreach (var entity in _filterLifeTimerC.Value)
            {
                _poolLifeTimerC.Value.Get(entity).Time -= deltaTime;
            }

            foreach (var entity in _filterBlinkTimerC.Value)
            {
                _poolLifeBlinkTimerC.Value.Get(entity).Time -= deltaTime;
            }

            foreach (var entity in _filterHealthRecoveryTimerC.Value)
            {
                _poolHealthRecoveryTimerC.Value.Get(entity).Time += deltaTime;
            }
        }
    }
}