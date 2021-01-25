using System;
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
        foreach (SellDefinition sellDefinition in sellDefinitions) {
            string descriptionString = "Buy " + sellDefinition.sellItem.amount.ToString() +
            sellDefinition.sellItem.itemDefinition.itemName;

            actions.Add(new BuyAction(descriptionString,
            new List<WorldItem>{sellDefinition.cost},
            new List<WorldItem>{sellDefinition.sellItem},
            this));
        }
    }

    [Serializable]
    public class SellDefinition {
        public WorldItem sellItem;
        public WorldItem cost;
    }
}
