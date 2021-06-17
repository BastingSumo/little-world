using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Food;
    List<GameObject> FoodList = new List<GameObject>();
    void Update()
    {
        if (FoodList.Count < 100)
        {
            FoodList.Add(Instantiate(Food, new Vector3(Random.Range(-40,40), 1, Random.Range(-40,40)), Quaternion.identity, transform));
        }
        foreach (var item in FoodList)
        {
            if (item == null)
            {
                FoodList.Remove(item);
                break;
            }
        }
    }
}
