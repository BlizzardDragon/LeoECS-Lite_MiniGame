using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct SpawnRunSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnComponent>> _filterSpawnC;

        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<SpawnComponent> _poolSpawnC;
        

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterSpawnC.Value)
            {
                var spawnC = _poolSpawnC.Value.Get(entity);
                ref var viewC = ref _poolViewC.Value.Get(entity);

                var newUnit = Object.Instantiate(Resources.Load<GameObject>(spawnC.Path));
                viewC.ViewObject = newUnit;
            }
        }
    }
}