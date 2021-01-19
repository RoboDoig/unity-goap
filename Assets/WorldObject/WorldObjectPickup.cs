using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectPickup : WorldObject
{
    public List<WorldItem> items;

    protected override void Start()
    {
        base.Start();

        foreach (WorldItem item in items) {
            actions.Add(new ItemPickupAction("Pick up " + item.itemDefinition.itemName,
                                             new List<WorldItem>(),
                                             new List<WorldItem>{item},
                                             this));
        }
    }
}
