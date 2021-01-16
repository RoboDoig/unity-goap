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

    void Awake() {
        if (items != null) {
            Destroy(gameObject);
            return;
        }

        items = this;
    }
}
