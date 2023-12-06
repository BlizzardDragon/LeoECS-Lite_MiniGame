using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct DestroySystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DestroyComponent>> _filterDestroyC;
        private readonly EcsPoolInject<ViewComponent> _poolViewC;

        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterDestroyC.Value)
            {
                var view = _poolViewC.Value.Get(entity).ViewObject;

                Object.DestroyImmediate(view);
                _world.Value.DelEntity(entity);
            }
        }
    }
}