using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Views
{
    public class CollidingUnit : EcsMonoObject
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
            else if (other.GetComponent<CollidingBullet>())
            {
                if (other.TryGetComponent(out EcsMonoObject collide))
                {
                    OnTakeDamage(this, collide);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<CollidingUnit>())
            {
                if (other.TryGetComponent(out EcsMonoObject collide))
                {
                    OnTriggerStayEvent(this, collide);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CollidingUnit>())
            {
                if (other.TryGetComponent(out EcsMonoObject collide))
                {
                    OnTriggerExitEvent(this, collide);
                }
            }
        }

        private void OnTakeDamage(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
        {
            if (_world == null) return;

            var entity = _world.NewEntity();
            var poolTakeDamageC = _world.GetPool<TakeDamageComponent>();
            ref var takeDamageC = ref poolTakeDamageC.Add(entity);
            takeDamageC.FirstCollide = firstCollide;
            takeDamageC.SecondCollide = secondCollide;

            AddCollisionComponent(entity);
        }

        public override void OnTriggerEnterEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
        {
            base.OnTriggerEnterEvent(firstCollide, secondCollide);

            var entity = _world.NewEntity();
            var poolCollisionEnterC = _world.GetPool<UnitCollisionEnterComponent>();
            ref var collisionEnterC = ref poolCollisionEnterC.Add(entity);
            collisionEnterC.FirstCollide = firstCollide;
            collisionEnterC.SecondCollide = secondCollide;

            AddCollisionComponent(entity);
        }

        public override void OnTriggerStayEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
        {
            base.OnTriggerStayEvent(firstCollide, secondCollide);

            var entity = _world.NewEntity();
            var poolCollisionStayC = _world.GetPool<UnitCollisionStayComponent>();
            ref var CollisionStayC = ref poolCollisionStayC.Add(entity);
            CollisionStayC.FirstCollide = firstCollide;
            CollisionStayC.SecondCollide = secondCollide;

            AddCollisionComponent(entity);
        }

        public override void OnTriggerExitEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
        {
            base.OnTriggerExitEvent(firstCollide, secondCollide);

            var entity = _world.NewEntity();
            var poolCollisionExitC = _world.GetPool<UnitCollisionExitComponent>();
            ref var CollisionExitC = ref poolCollisionExitC.Add(entity);
            CollisionExitC.FirstCollide = firstCollide;
            CollisionExitC.SecondCollide = secondCollide;

            AddCollisionComponent(entity);
        }

        private void AddCollisionComponent(int entity) => _world.GetPool<CollisionComponent>().Add(entity);
    }
}
