using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectShop : WorldObject
{
    public List<SellDefinition> sellDefinitions;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        List<WorldItem> currentInventory = GetComponent<Inventory>().GetItems();

        foreach (SellDefinition sellDefinition in sellDefinitions) {
            // Do we have enough inventory for this sell definition
            while ( currentInventory.Where(x => x.itemDefinition == sellDefinition.sellItem.itemDefinition).First().amount >= sellDefinition.sellItem.amount ) {
                string descriptionString = "Buy " + sellDefinition.sellItem.amount.ToString() + " " +
                sellDefinition.sellItem.itemDefinition.itemName;

                actions.Add(new BuyAction(descriptionString,
                new List<WorldItem>{sellDefinition.cost},
                new List<WorldItem>{sellDefinition.sellItem},
                this));

                currentInventory.Where(x => x.itemDefinition == sellDefinition.sellItem.itemDefinition).First().amount -= sellDefinition.sellItem.amount;
            }
        }
    }

    [Serializable]
    public class SellDefinition {
        public WorldItem sellItem;
        public WorldItem cost;
    }
}
