using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct DestroyComponentsSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<CollisionComponent>> _filterCollisionC;
        private readonly EcsFilterInject<Inc<HitComponent>> _filterHitC;

        private readonly EcsPoolInject<HitComponent> _poolHitC;
        
        private EcsWorld _world;


        public void Init(IEcsSystems systems) => _world = systems.GetWorld();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterCollisionC.Value)
            {
                _world.DelEntity(entity);
            }

            foreach (var entity in _filterHitC.Value)
            {
                _poolHitC.Value.Del(entity);
            }
        }
    }
}