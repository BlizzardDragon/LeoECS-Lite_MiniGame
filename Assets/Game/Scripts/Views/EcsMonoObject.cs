using Leopotam.EcsLite;
using UnityEngine;

public abstract class EcsMonoObject : MonoBehaviour
{
    public EcsPackedEntity EcsPacked { get; private set; }
    protected EcsWorld _world;

    public void Init(EcsWorld world) => _world = world;
    public void PackEntity(int entity) => EcsPacked = _world.PackEntity(entity);

    public virtual void OnTriggerEnterEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
    {
        if (_world == null) return;
    }

    public virtual void OnTriggerStayEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
    {
        if (_world == null) return;
    }

    public virtual void OnTriggerExitEvent(EcsMonoObject firstCollide, EcsMonoObject secondCollide)
    {
        if (_world == null) return;
    }
}
