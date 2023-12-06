using UnityEngine;

public class FPS_Limiter : MonoBehaviour
{
    [Header ("FPS")] 
    [Tooltip ("При выставлении значения равным -1 снимается ограничение на FPS")]
    [SerializeField] private int _targetFrameRate = 144;

    private void OnValidate()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}
