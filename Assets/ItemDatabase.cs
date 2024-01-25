using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDatabase
{
	public enum Item {Limestone, Root, Berries, Coin}
	public Item item;
	
	public enum ItemType {Ingredient, Collectable}
	public ItemType itemType;
	
	public enum IngredientType {None, White, Blue, Red, Brown}
	public IngredientType ingredientType;
}
