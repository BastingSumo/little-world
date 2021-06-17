using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    List<GameObject> whatAnimalCanSee = new List<GameObject>();

    public bool isMale = true;

    [SerializeField] string species = "Human";

    NavMeshAgent navMeshAgent;

   [SerializeField] Mating mating = new Mating();
   [SerializeField] Eating eating = new Eating();
   [SerializeField] Idle idle = new Idle();

   [Header("Stats")]
   [SerializeField] float stoppingDistance = 2;
   [SerializeField] float animalInteractRange = 3;
   [SerializeField] bool ismovingTo = false;

    [SerializeField] float speed = 3.5f;
    [SerializeField] float offSpring = 1;
    [SerializeField] float wonderChance = 10;
    [SerializeField] float mutationAmount = 10;
    [SerializeField] float lifeLength = 600;
    [SerializeField] float timeBorn = 0;


    [SerializeField] GameObject model;

    Vector3 initialPosition;

    List<Node> behaviour = new List<Node>();
    [SerializeField] Node SelectedNode;

    public FoodTypes FoodEats = FoodTypes.Herbivore;


    public NavMeshAgent NavMeshAgent { get => navMeshAgent; set => navMeshAgent = value; }
    public float StoppingDistance { get => stoppingDistance; set => stoppingDistance = value; }
    public string Species { get => species; set => species = value; }
    public float AnimalInteractRange { get => animalInteractRange; set => animalInteractRange = value; }
    public List<GameObject> WhatAnimalCanSee { get => whatAnimalCanSee; }
    public bool IsMovingTo { get => ismovingTo; set => ismovingTo = value; }
    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public float WonderChance { get => wonderChance; set => wonderChance = value; }
    public float OffSpring { get => offSpring; set => offSpring = value; }
    public float Speed { get => speed; set => speed = value; }
    public float MutationAmount { get => mutationAmount; set => mutationAmount = value; }
    public float LifeLength { get => lifeLength; set => lifeLength = value; }
    public Mating Mating { get => mating; set => mating = value; }
    public Eating Eating { get => eating; set => eating = value; }
    public GameObject Model { get => model; set => model = value; }

    private void Start()
    {
        timeBorn = Time.time;
        SelectedNode = idle;
        InitialPosition = gameObject.transform.position;
        NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        mating.SetInitallastTimneProcreated();
        ContructbehaviourList();
        idle.Check(WhatAnimalCanSee, this);
        mating.LastTimeProcreated = Time.time;
        ApplyMutation();
        GameManager.Instance.Animals.Add(gameObject);
        SetColor();
        InvokeRepeating("whatDo", 1, .1f);
    }
    private void Update()
    {
        eating.ApplyStatusEffects(this);
        //whatDo();
    }
    void SetColor()
    {
        if (isMale) gameObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.blue;
        else gameObject.GetComponentInChildren<MeshRenderer>().materials[0].color = Color.red;
    }
    void whatDo()
    {
        RemoveNullItems();
        DieFromOldAge();
        if (SelectedNode.currentNodeState == Node.NodeState.Failure)
        {
            CheckWhatAnimalCanSee();
            foreach (var item in behaviour)
            {
                if (item.Check(WhatAnimalCanSee, this))
                {
                    SelectedNode = item;
                    break;
                }
            }
        }
        else if (SelectedNode != null) SelectedNode.RUN(WhatAnimalCanSee, this);
    }
    void ContructbehaviourList()
    {
        behaviour.Add(mating);
        behaviour.Add(eating);
        behaviour.Add(idle);
    }
    void ApplyMutation()
    {
        speed = Mutate(Speed);
       // offSpring = Mutate(offSpring);
        WonderChance = Mutate(WonderChance);
        MutationAmount = Mutate(MutationAmount);
        mating.ProcreateCoolDown = Mutate(mating.ProcreateCoolDown);
        LifeLength = Mutate(LifeLength);
        eating.HungerDegrade = Mutate(eating.HungerDegrade);
        navMeshAgent.speed = speed;
    }
    float Mutate(float Original)
    {
        float randomizer = Random.Range(-50, 50);
        if (randomizer < 0) return Original += Original / MutationAmount;
        else return Original -= Original / MutationAmount;
    }
    void RemoveNullItems()
    {
        foreach (var item in whatAnimalCanSee)
        {
            if (item == null)
            {
                whatAnimalCanSee.Remove(item);
                break;
            }
        }
    }
    void DieFromOldAge()
    {
        if (Mathf.Abs(timeBorn - Time.time) > lifeLength)
        {
            GameManager.Instance.deathfromOldage += 1;
            Destroy(gameObject);
        }
    }
    void CheckWhatAnimalCanSee()
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(gameObject.transform.position, 55,Vector3.up, 0);
        foreach (var item in raycastHits)
        {
            if (!WhatAnimalCanSee.Contains(item.collider.gameObject)) whatAnimalCanSee.Add(item.collider.gameObject);
        }
    }
}

