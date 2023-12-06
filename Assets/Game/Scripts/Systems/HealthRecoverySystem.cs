using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct HealthRecoverySystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<HealthRecoveryTimerComponent, UnitComponent>> _filterHealthRecoveryTimerC;

        private readonly EcsPoolInject<HealthRecoveryTimerComponent> _poolHealthRecoveryTimerC;
        private readonly EcsPoolInject<HealthComponent> _poolHealthC;

        private readonly EcsCustomInject<SharedData> _sharedDtata;
        private float _recoveryTime;
        private int _maxHealth;

        public void Init(IEcsSystems systems)
        {
            _recoveryTime = _sharedDtata.Value.HealthRecoveryTime;
            _maxHealth = _sharedDtata.Value.UnitHealth;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterHealthRecoveryTimerC.Value)
            {
                if (_poolHealthRecoveryTimerC.Value.Get(entity).Time >= _recoveryTime)
                {
                    ref int health = ref _poolHealthC.Value.Get(entity).Health;
                    if (health < _maxHealth)
                    {
                        health++;
                        _poolHealthRecoveryTimerC.Value.Get(entity).Time = 0;
                    }
                }
            }
        }
    }
}