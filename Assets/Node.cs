using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    public NodeState currentNodeState;
    public enum NodeState
    {
        Succes, Failure
    }
    public abstract void RUN(List<GameObject> gameObjects, Animal animal);
    public abstract bool Check(List<GameObject> gameObjects, Animal animal);
}
