using UnityEngine;

public class StorageCollidingObject : MonoBehaviour
{
    [SerializeField] private EcsMonoObject _collidingObject;
    public EcsMonoObject CollidingObject => _collidingObject;
}
