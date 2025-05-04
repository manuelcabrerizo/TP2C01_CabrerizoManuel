using System.Collections.Generic;
using UnityEngine;

public class MovementGraph : MonoBehaviour
{
    [SerializeField] private List<MovementGraphNode> nodes;

    public MovementGraphNode GetRandomNode()
    {
        int index = Random.Range(0, nodes.Count);
        return nodes[index];
    }
}
