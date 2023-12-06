using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct LifeTimeSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<LifeTimerComponent>> _filterLifeTimerC;

        private readonly EcsPoolInject<LifeTimerComponent> _poolLifeTimerC;
        private readonly EcsPoolInject<HealthComponent> _poolHealthC;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterLifeTimerC.Value)
            {
                ref var lifeTimerC = ref _poolLifeTimerC.Value.Get(entity);

                if (lifeTimerC.Time <= 0)
                {
                    _poolHealthC.Value.Get(entity).Health = 0;
                    _poolLifeTimerC.Value.Del(entity);
                }
            }
        }
    }
}