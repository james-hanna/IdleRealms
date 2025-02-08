// Recipe.cs
using UnityEngine;

[System.Serializable]
public class Recipe
{
    [Tooltip("First ingredient required for the recipe.")]
    public ItemData ingredient1;
    
    [Tooltip("Second ingredient required for the recipe.")]
    public ItemData ingredient2;
    
    [Tooltip("The resulting item when combining the ingredients.")]
    public ItemData result;
}
