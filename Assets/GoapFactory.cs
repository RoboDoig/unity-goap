using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapFactory : MonoBehaviour
{
    public List<Action> actions = new List<Action>();
    public Agent agent;

    void Start() {
        GenerateActions();

        // State definition - start state
        Dictionary<string, int> startState = new Dictionary<string, int>{{"wood", 1}, {"gold", 0}};

        // goal state
        Dictionary<string, int> goalState = new Dictionary<string, int>{{"wood", 1}, {"gold", 1}};

        // GOAP generation
        GOAP goap = new GOAP(agent, startState, goalState);
        goap.GeneratePlan();
    }

    void GenerateActions() {
        // Just throw a load of actions into the world for GOAP testing

        // Pick up wood
        actions.Add(
            new Action("Pick up wood", 
            new List<WorldItem>(),
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Wood, 1)},
            null
            )
        );

        // Pick up stone
        actions.Add(
            new Action("Pick up stone", 
            new List<WorldItem>(),
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Stone, 1)},
            null
            )
        );

        // Pick up gold
        actions.Add(
            new Action("Pick up gold", 
            new List<WorldItem>(),
            new List<WorldItem>{new WorldItem(ItemDatabase.items.Gold, 1)},
            null
            )
        );
    }
}
