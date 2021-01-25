using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAgent : WorldObject
{
    protected override void Start() {
        base.Start();

        actions.Add(new EatAction("Eat",
                                  new List<WorldItem>{new WorldItem(ItemDatabase.items.resources.Food, 1)},
                                  new List<WorldItem>{new WorldItem(ItemDatabase.items.stats.Energy, 1)},
                                  this));
    }
}