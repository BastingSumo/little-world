using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    float foodRestore = 10;

    public FoodTypes foodType = FoodTypes.Herbivore;
    public float FoodRestore { get => foodRestore; set => foodRestore = value; }

}