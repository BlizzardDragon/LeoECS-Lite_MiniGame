using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Views
{
    public class CollidingBullet : EcsMonoObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CollidingUnit>())
            {
                if (other.TryGetComponent(out EcsMonoObject collide))
                {
                    OnTriggerEnterEvent(this, collide);
                }
            }
        }

        public override void OnTriggerEnterEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
        {
            base.OnTriggerEnterEvent(firstCollide, secondCollide);

            var entity = _world.NewEntity();
            var BulletCollisionEnterC = _world.GetPool<TakeDamageComponent>();
            ref var collisionEnterC = ref BulletCollisionEnterC.Add(entity);
            collisionEnterC.FirstCollide = firstCollide;
            collisionEnterC.SecondCollide = secondCollide;
            
            AddCollisionComponent(entity);
        }

        private void AddCollisionComponent(int entity) => _world.GetPool<CollisionComponent>().Add(entity);
    }
}