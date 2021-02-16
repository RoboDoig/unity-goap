using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<WorldItem> startItems;
    public Agent agent;

    [Serializable]
    public class InventoryItem {
        public WorldItem worldItem;
        public List<Action> associatedActions;

        public InventoryItem(WorldItem _worldItem, Agent _agent) {
            worldItem = new WorldItem(_worldItem.itemDefinition, _worldItem.amount);
            associatedActions = new List<Action>();
        }
    }

    void Awake() {
        agent = GetComponent<Agent>();

        foreach(WorldItem item in startItems) {
            AddItem(item);
        }
    }

    public void AddItem(WorldItem item) {
        // Check ItemDefinition type is in inventory, if it is add to the amount
        foreach(InventoryItem inventoryItem in inventoryItems) {
            // TODO - this structure is repeating alot
            if (item.itemDefinition == inventoryItem.worldItem.itemDefinition) {
                inventoryItem.worldItem.amount += item.amount;

                return;
            }
        }

        InventoryItem newItem = new InventoryItem(item, agent);
        inventoryItems.Add(newItem);
    }

    public void RemoveItem(WorldItem item) {
        // HERE TODO
        foreach(InventoryItem inventoryItem in inventoryItems) {
            if (item.itemDefinition == inventoryItem.worldItem.itemDefinition) {
                inventoryItem.worldItem.amount -= item.amount;
            }
        }
    }

    // Should return true if inventory containes item in >= item amount, fals otherwise
    public bool HasItem(WorldItem item) {
        // AND HERE TODO
        foreach(InventoryItem inventoryItem in inventoryItems) {
            if (item.itemDefinition == inventoryItem.worldItem.itemDefinition) {
                if (inventoryItem.worldItem.amount >= item.amount) {
                    return true;
                }
            }
        }
        return false;
    }

    public void Transfer(WorldItem item, Inventory toInventory) {

    }

    public List<WorldItem> GetItems() {
        List<WorldItem> worldItemList = new List<WorldItem>();
        foreach(InventoryItem inventoryItem in inventoryItems) {
            worldItemList.Add(inventoryItem.worldItem);
        }
        return worldItemList;
    }

    public Dictionary<string, int> GetItemsAsState() {
        Dictionary<string, int> itemsState = new Dictionary<string, int>();
        foreach (InventoryItem inventoryItem in inventoryItems) {
            itemsState.Add(inventoryItem.worldItem.AsKey(), inventoryItem.worldItem.amount);
            // itemsState[inventoryItem.worldItem.AsKey()] += inventoryItem.worldItem.amount;
        }

        return itemsState;
    }
}
