using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct BulletPostInitializationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnComponent, BulletSpawnComponent>> _filterSpawnC;

        private readonly EcsPoolInject<ViewComponent> _poolViewC;
        private readonly EcsPoolInject<ColorComponent> _poolColorC;
        private readonly EcsPoolInject<GunAttackComponent> _poolGunAttackC;
        private readonly EcsPoolInject<TeamComponent> _poolTeamC;
        private readonly EcsPoolInject<AttackTargetComponent> _poolAttackTargetC;
        private readonly EcsPoolInject<BulletSpawnComponent> _poolBulletSpawnC;
        private readonly EcsPoolInject<SpawnComponent> _poolSpawnC;

        private readonly EcsCustomInject<SharedData> _sharedData;
        private EcsWorld _world;


        public void Init(IEcsSystems systems) => _world = systems.GetWorld();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterSpawnC.Value)
            {
                SpawnBullet(entity);
            }
        }

        public void SpawnBullet(int bulletEntity)
        {
            var unitPackedEntity = _poolBulletSpawnC.Value.Get(bulletEntity).UnitEnity;
            int unitEntity = UnpackEntityUtils.UnpackEntity(_world, unitPackedEntity);

            Transform bulletParent;
            Teams team = _poolTeamC.Value.Get(unitEntity).Team;
            Vector3 targerPosition = _poolAttackTargetC.Value.Get(unitEntity).AttackTarget.transform.position;

            ref var viewC = ref _poolViewC.Value.Get(bulletEntity);
            ref var colorC = ref _poolColorC.Value.Get(bulletEntity);
            colorC.MeshRenderer = viewC.ViewObject.GetComponent<MeshRendererComponent>().MeshRenderer;
            colorC.MeshRenderer.material.color = colorC.OriginColor;

            if (team == Teams.Team_1)
            {
                bulletParent = _sharedData.Value.BulletsParentTeam_1;
            }
            else if (team == Teams.Team_2)
            {
                bulletParent = _sharedData.Value.BulletsParentTeam_2;
            }
            else
            {
                throw new Exception("Team not set");
            }

            var bullet = viewC.ViewObject;
            bullet.transform.position = _poolGunAttackC.Value.Get(unitEntity).BulletSpawn.position;
            bullet.transform.rotation = GetRotation(unitEntity, targerPosition);
            bullet.transform.parent = bulletParent;

            EcsMonoObject cObj = bullet.GetComponent<StorageCollidingObject>().CollidingObject;
            cObj.Init(_world);
            cObj.PackEntity(bulletEntity);

            _poolBulletSpawnC.Value.Del(bulletEntity);
            _poolSpawnC.Value.Del(bulletEntity);
        }

        private Quaternion GetRotation(int entity, Vector3 targerPos)
        {
            Vector3 currenPos = _poolViewC.Value.Get(entity).ViewObject.transform.position;
            Vector3 targetDirection = targerPos - currenPos;
            float randomEngle = Random.Range(-_sharedData.Value.ErrorAngle, _sharedData.Value.ErrorAngle);
            float engle = Vector3.SignedAngle(Vector3.forward, targetDirection, Vector3.up) + randomEngle;

            Quaternion rotation = Quaternion.Euler(new Vector3(0, engle, 0)); //Quaternion.LookRotation(direction); 
            return rotation;
        }
    }
}