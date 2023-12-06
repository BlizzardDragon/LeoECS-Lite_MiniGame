using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct UnitCollisionExitHandlingSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<UnitCollisionExitComponent>> _filterCollisionExitC;

        private readonly EcsPoolInject<UnitCollisionExitComponent> _poolCollisionExitC;
        private readonly EcsPoolInject<ContactTimerComponent> _poolContactTimerC;
        private readonly EcsPoolInject<BreakContactTimerComponent> _poolBreakContactTimerC;

        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterCollisionExitC.Value)
            {
                var collisionExitC = _poolCollisionExitC.Value.Get(entity);

                var firstEntitiy =
                    UnpackEntityUtils.UnpackEntity( _world.Value, collisionExitC.FirstCollide.EcsPacked);

                // Сброс контакт тайма.
                if (_poolContactTimerC.Value.Has(firstEntitiy))
                {
                    _poolContactTimerC.Value.Get(firstEntitiy).Time = 0;

                    // Добавление таймера разрыва контакта.
                    if (!_poolBreakContactTimerC.Value.Has(firstEntitiy))
                    {
                        _poolBreakContactTimerC.Value.Add(firstEntitiy);
                    }
                }
            }
        }
    }
}