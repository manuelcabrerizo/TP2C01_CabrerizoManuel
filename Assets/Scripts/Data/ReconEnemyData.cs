using UnityEngine;

[CreateAssetMenu(fileName = "ReconEnemyData", menuName = "ReconEnemy/Data", order = 1)]

public class ReconEnemyData : ScriptableObject
{
    public float TargetReachRadio = 2.0f;
    public float Speed = 5.0f;
    public float MaxSpeed = 10.0f;
}
