using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFactory : MonoBehaviour
{
    public List<Action> actions = new List<Action>();

    void Start() {
        // Just throw a load of actions into the world for GOAP testing

        // Eat action
        actions.Add(
            new Action("Eat", 
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Food, 1)},
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Health, 1)},
            null
            )
        );

        // Pick up wood
        actions.Add(
            new Action("Pick up wood", 
            new List<WorldItem>{},
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Wood, 1)},
            null
            )
        );

        // Pick up stone
        actions.Add(
            new Action("Pick up stone", 
            new List<WorldItem>{},
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Stone, 1)},
            null
            )
        );

        // Pick up gold
        actions.Add(
            new Action("Pick up gold", 
            new List<WorldItem>{},
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Gold, 1)},
            null
            )
        );
    }
}
