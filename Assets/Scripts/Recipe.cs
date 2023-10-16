using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public float craftTime;
    public List<RecipeData> input = new List<RecipeData>();
    
    public List<RecipeData> output = new List<RecipeData>();
}
