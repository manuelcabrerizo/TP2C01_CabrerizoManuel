using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Data", order = 1)]

public class PlayerData : ScriptableObject
{
    public int MaxLife = 100;
    public float MaxSpeed = 40;
    public float Speed = 20;
    public float MaxDamageRecived = 20;
    public float RotationSpeed = 3;
    public float ShootDistance = 100;
    public float BulletSpeed = 3;
    public Vector3 SpawPosition = new Vector3(24, 10, 10);
}
