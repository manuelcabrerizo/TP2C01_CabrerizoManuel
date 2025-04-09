using UnityEngine;

[CreateAssetMenu(fileName = "AssaultEnemyData", menuName = "AssaultEnemy/Data", order = 1)]

public class AssaultEnemyData : ScriptableObject
{
    public float TimePerUpdate = 3.0f;
    public float TargetReachRadio = 5.0f;
    public float ViewRadio = 40.0f;
    public float Speed = 1.0f;
    public float MaxSpeed = 3.0f;
}
