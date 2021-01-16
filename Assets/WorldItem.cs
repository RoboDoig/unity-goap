using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WorldItem
{
    public WorldItemDefinition itemDefinition;
    public int amount;

    public WorldItem(WorldItemDefinition _itemDefinition, int _amount) {
        itemDefinition = _itemDefinition;
        amount = _amount;
    }

    public string Description() {
        string descriptorString = amount + " " + itemDefinition.itemName;

        return descriptorString;
    }

    public string AsKey() {
        return itemDefinition.itemName;
    }
}
