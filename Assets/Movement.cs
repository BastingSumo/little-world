using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Movement
{
    public static void MoveTo(Vector3 animal, Vector3 Target, NavMeshAgent navMeshAgent, float StoppingDistance)
    {
        if (navMeshAgent.isOnNavMesh)
        {
            float distance = Vector3.Distance(animal, Target);
            if (distance > StoppingDistance)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(Target);
            }
            else navMeshAgent.isStopped = true;
        }
    }
    public static bool CheckIfInRange(Vector3 animal, Vector3 Target, float InRangeDistance)
    {
        if (Vector3.Distance(animal, Target) <= InRangeDistance) return true;
        return false;
    }
    public static Vector3 MoveToRandomLocation(Animal animal, NavMeshAgent navMeshAgent, float StoppingDistance, Vector3 initialPosition) 
    {
        Vector3 RandomLocation = initialPosition;
        if (!animal.IsMovingTo)
        {
            Debug.Log(RandomLocation);
            animal.IsMovingTo = true;
        }
        if (Vector3.Distance(animal.gameObject.transform.position, RandomLocation) < 2) animal.IsMovingTo = false;
        MoveTo(animal.transform.position, RandomLocation, navMeshAgent, StoppingDistance);
        return RandomLocation;
    }
}
