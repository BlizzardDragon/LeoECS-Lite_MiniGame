using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Components;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Systems
{
    public struct BlinkSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HitComponent, UnitComponent>> _filterHitC;
        private readonly EcsFilterInject<Inc<BlinkTimerComponent>> _filterBlinkTimerC;

        private readonly EcsPoolInject<ColorComponent> _poolViewC;
        private readonly EcsPoolInject<BlinkTimerComponent> _poolBlinkTimerC;

        private readonly EcsCustomInject<SharedData> _sharedDtata;


        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterHitC.Value)
            {
                if (_poolBlinkTimerC.Value.Has(entity))
                {
                    _poolBlinkTimerC.Value.Get(entity).Time = _sharedDtata.Value.BlinkTime;
                }
                else
                {
                    _poolViewC.Value.Get(entity).MeshRenderer.material.color = _poolViewC.Value.Get(entity).TargetColor;
                    _poolBlinkTimerC.Value.Add(entity).Time = _sharedDtata.Value.BlinkTime;
                }
            }

            foreach (var entity in _filterBlinkTimerC.Value)
            {
                if (_poolBlinkTimerC.Value.Get(entity).Time <= 0)
                {
                    _poolBlinkTimerC.Value.Del(entity);
                    _poolViewC.Value.Get(entity).MeshRenderer.material.color = _poolViewC.Value.Get(entity).OriginColor;
                }
            }
        }
    }
}