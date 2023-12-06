using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct RotationSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<RotationSpeedComponent>> _filterRotationC;

        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<RotationSpeedComponent> _poolRotationC;
        private readonly EcsPoolInject<MoveTargetComponent> _poolMoveTargetC;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterRotationC.Value)
            {
                var targetC = _poolMoveTargetC.Value.Get(entity);
                if (!targetC.MoveTarget) continue;

                ref var viewC = ref _poolViewC.Value.Get(entity);
                var rotationC = _poolRotationC.Value.Get(entity);

                Transform transform = viewC.ViewObject.transform;
                Vector3 direction = targetC.MoveTarget.transform.position - transform.position;
                float speed = rotationC.RotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), speed);
            }
        }
    }
}