using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Eating : Node
{
    GameObject SuitableFood;
    [SerializeField] float hunger = 100;
    [SerializeField] float hungerDegrade = 1;
    float eatAt = 99;
    float DieAt = 0;

    public float Hunger { get => hunger; set => hunger = value; }
    public float HungerDegrade { get => hungerDegrade; set => hungerDegrade = value; }

    public override void RUN(List<GameObject> gameObjects, Animal animal)
    {
        if (SuitableFood != null)
        {
            Movement.MoveTo(animal.transform.position, SuitableFood.transform.position, animal.NavMeshAgent, animal.StoppingDistance);
            if (Movement.CheckIfInRange(animal.gameObject.transform.position, SuitableFood.transform.position, animal.AnimalInteractRange)) CONSUME(SuitableFood);
        }
        else currentNodeState = NodeState.Failure;
    }
    public void CONSUME(GameObject SuitableFood)
    {
        hunger += SuitableFood.GetComponent<Food>().FoodRestore;
        Object.Destroy(SuitableFood);
        SuitableFood = null;
        currentNodeState = NodeState.Failure;
    }
    public override bool Check(List<GameObject> gameObjects, Animal animal)
    {
        float closestFoodDistance = float.MaxValue;
        SuitableFood = null;
        foreach (GameObject item in gameObjects)
        {
            if (item != null)
            {
                if (item.tag == "Herbivore Food")
                {
                    if (item.GetComponent<Food>().foodType == animal.FoodEats && hunger <= eatAt)
                    {
                        float tempDistance = Vector3.Distance(animal.gameObject.transform.position, item.transform.position);
                        if (closestFoodDistance > tempDistance)
                        {
                            closestFoodDistance = tempDistance;
                            SuitableFood = item;
                        }
                    }
                }
            }
        }
        if (SuitableFood != null)
        {
            currentNodeState = NodeState.Succes;
            return true;
        }
        else
        {
            currentNodeState = NodeState.Failure;
            return false;
        }
    }
    public void ApplyStatusEffects(Animal animal)
    {
        hunger -= hungerDegrade * Time.deltaTime;
        if (hunger < DieAt) 
        {
            Object.Destroy(animal.gameObject);
            GameManager.Instance.deathfromhunger += 1;
        }
    }
}
