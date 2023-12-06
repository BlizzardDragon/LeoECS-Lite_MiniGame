using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct UnitCollisionStayHandlingSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitCollisionStayComponent>> _filterCollisionStayC;

        private readonly EcsPoolInject<UnitCollisionStayComponent> _poolCollisionStayC;
        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;

        private readonly EcsCustomInject<SharedData> _sharedDtata;
        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterCollisionStayC.Value)
            {
                var collisionStaC = _poolCollisionStayC.Value.Get(entity);

                (var firstEntitiy, var secondEntitiy) =
                    UnpackEntityUtils.UnpackEntity(
                        _world.Value,
                        collisionStaC.FirstCollide.EcsPacked,
                        collisionStaC.SecondCollide.EcsPacked);

                // Проверка на ContactTimer.
                if (_poolContactTimerC.Value.Has(firstEntitiy) && _poolContactTimerC.Value.Has(secondEntitiy))
                {
                    ref var contactTimerC_1 = ref _poolContactTimerC.Value.Get(firstEntitiy);
                    ref var contactTimerC_2 = ref _poolContactTimerC.Value.Get(secondEntitiy);

                    // Проверка на превышение времени контакта.
                    if (contactTimerC_1.Time > _sharedDtata.Value.MaxContactTime &&
                        contactTimerC_2.Time > _sharedDtata.Value.MaxContactTime)
                    {
                        ref var MoveC_1 = ref _poolMoveC.Value.Get(firstEntitiy);
                        ref var MoveC_2 = ref _poolMoveC.Value.Get(secondEntitiy);

                        // Переназначение направлений и сброс контакт таймера.
                        float value = Random.value;
                        if (value < 0.5f)
                        {
                            MoveC_1.MoveDirection = Vector3.left;
                            MoveC_2.MoveDirection = Vector3.right;
                        }
                        else
                        {
                            MoveC_1.MoveDirection = Vector3.right;
                            MoveC_2.MoveDirection = Vector3.left;
                        }

                        contactTimerC_1.Time = 0;
                        contactTimerC_2.Time = 0;
                    }
                }
            }
        }
    }
}
