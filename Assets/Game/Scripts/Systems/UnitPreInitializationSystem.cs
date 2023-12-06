using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct UnitPreInitializationSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<SharedData> _sharedData;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            EcsPool<RechargeTimerComponent> poolRechargeTimerC = world.GetPool<RechargeTimerComponent>();
            EcsPool<AttackTargetComponent> poolAttackTargetC = world.GetPool<AttackTargetComponent>();
            EcsPool<RotationSpeedComponent> poolRotationC = world.GetPool<RotationSpeedComponent>();
            EcsPool<MoveTargetComponent> poolMoveTargetC = world.GetPool<MoveTargetComponent>();
            EcsPool<GunAttackComponent> poolGunAttackC = world.GetPool<GunAttackComponent>();
            EcsPool<HealthComponent> poolHealthC = world.GetPool<HealthComponent>();
            EcsPool<DamageComponent> poolDamageC = world.GetPool<DamageComponent>();
            EcsPool<SpawnComponent> poolSpawnC = world.GetPool<SpawnComponent>();
            EcsPool<ColorComponent> poolColorC = world.GetPool<ColorComponent>();
            EcsPool<ViewComponent> poolViewC = world.GetPool<ViewComponent>();
            EcsPool<MoveComponent> poolMoveC = world.GetPool<MoveComponent>();
            EcsPool<TeamComponent> poolTeamC = world.GetPool<TeamComponent>();
            EcsPool<UnitComponent> poolUnitC = world.GetPool<UnitComponent>();

            int teamCount = _sharedData.Value.TeamCount;
            int entityCount = _sharedData.Value.UnitsPerTeam * teamCount;

            for (int i = 0; i < entityCount; i++)
            {
                int entity = world.NewEntity();

                poolRechargeTimerC.Add(entity);
                poolAttackTargetC.Add(entity);
                poolMoveTargetC.Add(entity);
                poolGunAttackC.Add(entity);
                poolRotationC.Add(entity).RotationSpeed = _sharedData.Value.RotationSpeed;
                poolHealthC.Add(entity).Health = _sharedData.Value.UnitHealth;
                poolDamageC.Add(entity).DamageValue = _sharedData.Value.UnitDamage;
                poolSpawnC.Add(entity).Path = _sharedData.Value.UnitPrefabPath;
                poolColorC.Add(entity);
                poolMoveC.Add(entity).MoveSpeed = _sharedData.Value.UnitMoveSpeed;
                poolUnitC.Add(entity).ID = i;
                poolViewC.Add(entity);

                ref var attackC = ref poolGunAttackC.Get(entity);
                attackC.AttackPeriod = _sharedData.Value.UnitAttackPeriod;
                attackC.AttackDistance = _sharedData.Value.UnitAttackDistance;
                
                ref var colorC = ref poolColorC.Get(entity);
                colorC.TargetColor = _sharedData.Value.BlinkColor;

                if (i < entityCount / teamCount)
                {
                    colorC.OriginColor = _sharedData.Value.ColorTeam1;
                    poolTeamC.Add(entity).Team = Teams.Team_1;
                }
                else
                {
                    colorC.OriginColor = _sharedData.Value.ColorTeam2;
                    poolTeamC.Add(entity).Team = Teams.Team_2;
                }
                // Debug.Log($"Init = {poolTeamC.Get(entity).Team}");
            }
        }
    }
}