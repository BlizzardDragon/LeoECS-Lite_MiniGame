using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public class GunAttackSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<GunAttackComponent>> _filterGunAttackC;

        private readonly EcsPoolInject<GunAttackComponent> _poolGunAttackC;
        private readonly EcsPoolInject<AttackTargetComponent> _poolAttackTargetC;
        private readonly EcsPoolInject<RechargeTimerComponent> _poolRechargeTimerC;
        private readonly EcsPoolInject<BulletSpawnComponent> _poolbulletSpawnC;

        private readonly EcsCustomInject<SharedData> _sharedData;
        private EcsWorld _world;


        public void Init(IEcsSystems systems) => _world = systems.GetWorld();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterGunAttackC.Value)
            {
                ref var rechargeTimerC = ref _poolRechargeTimerC.Value.Get(entity);
                var attackC = _poolGunAttackC.Value.Get(entity);

                if (rechargeTimerC.Time <= 0)
                {
                    var targetC = _poolAttackTargetC.Value.Get(entity);
                    if (targetC.AttackTarget == null) continue;

                    float rate = attackC.AttackPeriod;
                    float multiplier = _sharedData.Value.UnitAttackPeriodRandomMultiplier;
                    
                    rechargeTimerC.Time = Random.Range(rate - rate * multiplier, rate + rate * multiplier);
                    
                    CreateBulletEnetity(entity);
                }
            }
        }

        private void CreateBulletEnetity(int unitEnetity)
        {
            int newBullet = _world.NewEntity();
            EcsPackedEntity unitPackedEntity = _world.PackEntity(unitEnetity);
            _poolbulletSpawnC.Value.Add(newBullet).UnitEnity = unitPackedEntity;
        }
    }
}