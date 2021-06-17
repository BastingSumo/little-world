using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Node
{
    Vector3 TargetLocation;
    public override void RUN(List<GameObject> gameObjects, Animal animal)
    {
        Movement.MoveTo(animal.gameObject.transform.position, TargetLocation, animal.NavMeshAgent, animal.StoppingDistance);
        if (Movement.CheckIfInRange(animal.gameObject.transform.position, TargetLocation, animal.StoppingDistance)) currentNodeState = NodeState.Failure;
        
    }
    public override bool Check(List<GameObject> gameObjects, Animal animal)
    {
        TargetLocation = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
        float ranomdizer = Random.Range(0, 100);
        if (ranomdizer > animal.WonderChance)
        {
            currentNodeState = NodeState.Failure; 
            return false;
        }
        else
        {
            currentNodeState = NodeState.Succes;
            return true;
        }
    }
}
