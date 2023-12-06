using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems;
using UnityEngine;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SharedData _sharedData;
        EcsWorld _world;
        IEcsSystems _systems;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems

                // Пре-инициализация юнитов.
                .Add(new UnitPreInitializationSystem())

                // Пре-инициализация пуль.
                .Add(new GunAttackSystem())
                .Add(new BulletPreInitializationSystem())

                // Спавн.
                .Add(new SpawnInitSystem())
                .Add(new SpawnRunSystem())

                // Пост-инициализация.
                .Add(new UnitPostInitializationSystem())
                .Add(new BulletPostInitializationSystem())

                // Обработчкики для юнитов и пуль.
                .Add(new EnemySearchSystem())
                .Add(new DamageSystem())
                .Add(new BlinkSystem())
                .Add(new LifeTimeSystem())

                // Обработка столкновений юнитов.
                .Add(new UnitCollisionEnterHandlingSystem())
                .Add(new UnitCollisionStayHandlingSystem())
                .Add(new BreakContactSystem())
                .Add(new UnitCollisionExitHandlingSystem())

                // Отступление.
                .Add(new RetreatSystem())
                .Add(new HealthRecoverySystem())

                // Обновление таймеров.
                .Add(new TimerUpdateSystem())

                // Системы движения.
                .Add(new RotationSystem())
                .Add(new MoveSystem())

                // Системы уничтожения.
                .Add(new DestroyComponentsSystem())
                .Add(new HealthSystem())
                .Add(new DestroySystem())


                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_sharedData)
                .Init();
        }

        void Update()
        {
            // process systems here.
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }

            // cleanup custom worlds here.

            // cleanup default world.
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}

[Serializable]
public class SharedData
{
    [Header("Game")]
    public int UnitsPerTeam;
    public int ColumnCount;
    [Space]
    public Color ColorTeam1;
    public Color ColorTeam2;
    [Space]
    public float BlinkTime;
    public Color BlinkColor;
    [Space]
    public Transform SpawnPointUnitsTeam_1;
    public Transform SpawnPointUnitsTeam_2;
    [Space]
    public Transform BulletsParentTeam_1;
    public Transform BulletsParentTeam_2;
    [Space]
    [Header("Units")]
    public int UnitHealth;
    public int UnitDamage;
    public float UnitMoveSpeed;
    public float RotationSpeed;
    public float UnitAttackPeriod;
    public float UnitAttackDistance;
    public float UnitAttackPeriodRandomMultiplier;
    public float MaxContactTime;
    public float ContactBreakTime;
    public float HealthRecoveryTime;
    [Space]
    public string UnitPrefabPath;
    public float UnitSpawnOffset;
    [Space]
    [Header("Bullets")]
    public int BulletHealth;
    public int BulletDamage;
    public float BulletLiveTime;
    public float BulletMoveSpeed;
    public float ErrorAngle;
    public string BulletPrefabPath;

    public int TeamCount { get; private set; } = 2;
}