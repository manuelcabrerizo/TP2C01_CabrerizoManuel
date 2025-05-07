using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> points;

    public Vector3 GetRandomPoint()
    {
        int index = Random.Range(0, points.Count);
        return points[index].position;
    }

}