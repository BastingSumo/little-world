using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;

    List<GameObject> animals = new List<GameObject>();

    public int totalAnimals = 0;
    public int deathfromhunger = 0;
    public int deathfromOldage = 0;

    public static GameManager Instance { get => _instance; }
    public List<GameObject> Animals { get => animals; set => animals = value; }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Update()
    {
        totalAnimals = animals.Count;
        foreach (var item in Animals)
        {
            if (item == null) 
            { 
                animals.Remove(item);
                break;
            }
        }
    }
}
