using System.Collections.Generic;
using UnityEngine;


public class MovementGraphNode : MonoBehaviour
{
    [SerializeField] private List<MovementGraphNode> childrens;

    public MovementGraphNode GetRandomChild()
    {
        int index = Random.Range(0, childrens.Count);
        return childrens[index];
    }

}
