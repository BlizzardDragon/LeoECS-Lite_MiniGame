using Leopotam.EcsLite.Di;
using Leopotam.EcsLite;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using System;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct BulletPreInitializationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BulletSpawnComponent>> _filterBulletSpawnC;

        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<DamageComponent> _poolDamageC;
        private readonly EcsPoolInject<ColorComponent> _poolColorC;
        private readonly EcsPoolInject<MoveComponent> _poolMoveC;
        private readonly EcsPoolInject<TeamComponent> _poolTeamC;
        private readonly EcsPoolInject<LifeTimerComponent> _poolLifeTimerC;
        private readonly EcsPoolInject<HealthComponent> _poolHealthC;
        private readonly EcsPoolInject<BulletComponent> _poolBulletC;
        private readonly EcsPoolInject<BulletSpawnComponent> _poolBulletSpawnC;
        private readonly EcsPoolInject<SpawnComponent> _poolSpawnC;

        private readonly EcsCustomInject<SharedData> _sharedData;
        private EcsWorld _world;


        public void Init(IEcsSystems systems) => _world = systems.GetWorld();

        public void Run(IEcsSystems systems)
        {
            foreach (var bulletEntity in _filterBulletSpawnC.Value)
            {
                var unitEntity =
                    UnpackEntityUtils.UnpackEntity(_world, _poolBulletSpawnC.Value.Get(bulletEntity).UnitEnity);

                Teams team = _poolTeamC.Value.Get(unitEntity).Team;

                _poolLifeTimerC.Value.Add(bulletEntity).Time = _sharedData.Value.BulletLiveTime;
                _poolDamageC.Value.Add(bulletEntity).DamageValue = _sharedData.Value.BulletDamage;
                _poolHealthC.Value.Add(bulletEntity).Health = _sharedData.Value.BulletHealth;
                _poolBulletC.Value.Add(bulletEntity);
                _poolSpawnC.Value.Add(bulletEntity).Path = _sharedData.Value.BulletPrefabPath;
                _poolMoveC.Value.Add(bulletEntity).MoveSpeed = _sharedData.Value.BulletMoveSpeed;
                _poolTeamC.Value.Add(bulletEntity).Team = team;
                _poolViewC.Value.Add(bulletEntity);

                ref var colorC = ref _poolColorC.Value.Add(bulletEntity);
                if (team == Teams.Team_1)
                {
                    colorC.OriginColor = _sharedData.Value.ColorTeam1;
                }
                else if (team == Teams.Team_2)
                {
                    colorC.OriginColor = _sharedData.Value.ColorTeam2;
                }
                else
                {
                    throw new Exception("Team not set");
                }
            }
        }
    }
}