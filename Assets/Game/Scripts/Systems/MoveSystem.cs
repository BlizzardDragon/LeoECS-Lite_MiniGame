using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct MoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MoveComponent>, Exc<StopComponent>> _filterMoveC;

        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterMoveC.Value)
            {
                ref var viewC = ref _poolViewC.Value.Get(entity);
                ref var moveC = ref _poolMoveC.Value.Get(entity);

                if (moveC.MoveDirection == Vector3.zero)
                {
                    moveC.MoveDirection = Vector3.forward;
                }

                Transform transform = viewC.ViewObject.transform;
                transform.position += transform.TransformVector(moveC.MoveDirection) * moveC.MoveSpeed * Time.deltaTime;
            }
        }
    }
}