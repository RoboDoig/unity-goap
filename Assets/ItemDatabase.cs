using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase items;

    [Header("Resources")]
    public WorldItemDefinition Food;
    public WorldItemDefinition Wood;
    public WorldItemDefinition Stone;
    public WorldItemDefinition Gold;

    [Header("Stats")]
    public WorldItemDefinition Health;
    public WorldItemDefinition Hunger;
    public WorldItemDefinition Strength;

    [Header("Tools")]
    public WorldItemDefinition Axe;
    public WorldItemDefinition FishingRod;
    public WorldItemDefinition Hammer;


    void Awake() {
        if (items != null) {
            Destroy(gameObject);
            return;
        }

        items = this;
    }
}
