using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int citizenCount;
    [Range(0, 1)]
    public float alienPercentage;
    public int assaultEnemyCount;
    public int reconEnemyCount;
}

[CreateAssetMenu(fileName = "LevelsData", menuName = "Levels/Data", order = 1)]
public class LevelsData : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();
}
