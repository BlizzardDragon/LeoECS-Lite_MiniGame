using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct HealthSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthComponent>> _filterHealthC;

        private readonly EcsPoolInject<HealthComponent> _poolHealthC;
        private readonly EcsPoolInject<DestroyComponent> _poolDestroyC;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterHealthC.Value)
            {
                var healthC = _poolHealthC.Value.Get(entity);

                if (healthC.Health <= 0)
                {
                    _poolDestroyC.Value.Add(entity);
                }
            }
        }
    }
}