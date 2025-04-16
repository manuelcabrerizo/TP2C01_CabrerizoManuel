using UnityEngine;

[CreateAssetMenu(fileName = "CitizenData", menuName = "Citizen/Data", order = 1)]

public class CitizenData : ScriptableObject
{
    public float ReachRadio = 3.0f;
    public int MaxLife = 3;
    public float walkSpeed = 15.0f;
    public float runSpeed = 30.0f;
}
