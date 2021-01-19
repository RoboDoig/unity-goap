using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase items;

    [Serializable]
    public struct Resources {
        public WorldItemDefinition Food;
        public WorldItemDefinition Wood;
        public WorldItemDefinition Stone;
        public WorldItemDefinition Gold;
    }

    [Serializable]
    public struct Stats {
        public WorldItemDefinition Health;
        public WorldItemDefinition Hunger;
        public WorldItemDefinition Strength;
    }

    [Serializable]
    public struct Tools {
        public WorldItemDefinition Axe;
        public WorldItemDefinition FishingRod;
        public WorldItemDefinition Hammer;
    }

    public Resources resources;
    public Stats stats;
    public Tools tools;

    void Awake() {
        if (items != null) {
            Destroy(gameObject);
            return;
        }

        items = this;
    }

    public Dictionary<string, int> GetZeroState() {
        Dictionary<string, int> zeroState = new Dictionary<string, int>();

        // Reflection trick to get fields
        FieldInfo[] fields = typeof(Resources).GetFields();
        foreach(FieldInfo field in fields) {
            WorldItemDefinition itemDefinition = (WorldItemDefinition)field.GetValue(this.resources);
            zeroState.Add(itemDefinition.itemName, 0);
        }

        fields = typeof(Stats).GetFields();
        foreach(FieldInfo field in fields) {
            WorldItemDefinition itemDefinition = (WorldItemDefinition)field.GetValue(this.stats);
            zeroState.Add(itemDefinition.itemName, 0);
        }

        fields = typeof(Tools).GetFields();
        foreach(FieldInfo field in fields) {
            WorldItemDefinition itemDefinition = (WorldItemDefinition)field.GetValue(this.tools);
            zeroState.Add(itemDefinition.itemName, 0);
        }

        return zeroState;
    }
}
