using UnityEngine;

public class MeshRendererComponent : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer => _meshRenderer;
}
