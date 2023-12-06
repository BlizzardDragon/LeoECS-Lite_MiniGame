using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct DamageSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TakeDamageComponent>> _filterTakeDamageC;

        private readonly EcsPoolInject<TakeDamageComponent> _poolTakeDamageC;
        private readonly EcsPoolInject<DamageComponent> _poolDamegeC;
        private readonly EcsPoolInject<HealthComponent> _poolHealthC;
        private readonly EcsPoolInject<TeamComponent> _poolTeamC;
        private readonly EcsPoolInject<HitComponent> _poolHitC;

        private readonly EcsWorldInject _world;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterTakeDamageC.Value)
            {
                var takeDamageC = _poolTakeDamageC.Value.Get(entity);

                (var firstEntity, var secondEntity) =
                    UnpackEntityUtils.UnpackEntity(
                        _world.Value,
                        takeDamageC.FirstCollide.EcsPacked,
                        takeDamageC.SecondCollide.EcsPacked);

                var firstTeamC = _poolTeamC.Value.Get(firstEntity);
                var secondTeamC = _poolTeamC.Value.Get(secondEntity);

                if (firstTeamC.Team != secondTeamC.Team)
                {
                    var damageC = _poolDamegeC.Value.Get(firstEntity);
                    ref var healthC = ref _poolHealthC.Value.Get(secondEntity);
                    healthC.Health -= damageC.DamageValue;

                    if (!_poolHitC.Value.Has(secondEntity))
                    {
                        _poolHitC.Value.Add(secondEntity);
                    }
                }
            }
        }
    }
}