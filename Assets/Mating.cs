using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Mating : Node
{
    [SerializeField] GameObject AnimalPrefab;
    [SerializeField] float lastTimeProcreated = 0;
    [SerializeField] float procreateCoolDown = 60;
    GameObject SuitableMate;
    bool LastChildsSex = true;

    public float ProcreateCoolDown { get => procreateCoolDown; set => procreateCoolDown = value; }
    public float LastTimeProcreated { get => lastTimeProcreated; set => lastTimeProcreated = value; }

    
    public override void RUN(List<GameObject> gameObjects, Animal animal)
    {
        if (SuitableMate != null)
        {
            Movement.MoveTo(animal.transform.position, SuitableMate.transform.position, animal.NavMeshAgent, animal.StoppingDistance);
            CreateOffSpring(animal);
        }
        else
        {
            currentNodeState = NodeState.Failure;
        }
    }

    public override bool Check(List<GameObject> gameObjects, Animal animal)
    {
        if (SuitableMate != null && SuitableMate.GetComponent<Animal>().Mating.currentNodeState == NodeState.Succes) return true;
        foreach (GameObject item in gameObjects)
        {
            if (item != null)
            {
                if (item.tag == "Animal")
                {
                    Animal script = item.GetComponent<Animal>();
                    if (script.isMale != animal.isMale && CheckMateTime() && animal.Species == script.Species && script.Mating.CheckMateTime())
                    {
                        SuitableMate = item;
                        currentNodeState = NodeState.Succes;
                        return true;
                    }
                }
            }
        }
        currentNodeState = NodeState.Failure;
        return false;
    }
    void CreateOffSpring(Animal animal)
    {
        if (Movement.CheckIfInRange(animal.gameObject.transform.position, SuitableMate.transform.position, animal.AnimalInteractRange))
        {
            if (animal.isMale == false)
            {
                animal.gameObject.transform.LookAt(SuitableMate.transform.position);
                for (int i = 0; i < Mathf.RoundToInt(animal.OffSpring); i++)
                {
                    AnimalPrefab = animal.gameObject;
                    GameObject child = Object.Instantiate(AnimalPrefab, animal.transform.position, animal.transform.rotation);
                    Animal childScript = child.GetComponent<Animal>();
                    childScript.isMale = !LastChildsSex;
                    LastChildsSex = childScript.isMale;
                }
                lastTimeProcreated = Time.time;
                SuitableMate = null;
                currentNodeState = NodeState.Failure;
            }
            else
            {
                lastTimeProcreated = Time.time;
                SuitableMate = null;
                currentNodeState = NodeState.Failure;
            }
        }
    }
    bool CheckMateTime() => Mathf.Abs(Time.time - lastTimeProcreated) > ProcreateCoolDown;
    public void SetInitallastTimneProcreated() => lastTimeProcreated = Time.time;

}
