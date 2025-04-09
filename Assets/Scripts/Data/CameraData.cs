using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Camera/Data", order = 1)]

public class CameraData : ScriptableObject
{
    public float MouseSpeed = 1.0f;
    public float MaxDistance = 5.0f;
    public float CameraRadius = 0.5f;
}
