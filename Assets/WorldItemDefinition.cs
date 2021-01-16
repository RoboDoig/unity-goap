using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Basic data about things in the world, e.g. wood, health, gold. Distinct from WorldItem, which specifies a WorldItemDefinition and an amount. Distinct from WorldObject which is a GameObject in the world made with actions
[CreateAssetMenu(fileName = "WorldItem", menuName = "ScriptableObjects/WorldItemDefinition", order = 1)]
public class WorldItemDefinition : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public enum BaseType {Resource, Tool, Stat, Building}
    public BaseType baseType;
    public int maxStack;
    // Does use of this item deplete it (true, e.g. food, gold, wood) or does it have multiple uses (false, e.g. axe, sword) - TODO, this might cause problems with inventory transfer of tools.
    public bool consumable;
}