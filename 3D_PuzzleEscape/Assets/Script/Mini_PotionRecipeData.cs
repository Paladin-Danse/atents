using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PotionRecipeData
{
    public Dictionary<Material, int> Recipe;
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PotionRecipe", order = 1)]
public class Mini_PotionRecipeData : ScriptableObject
{

}
