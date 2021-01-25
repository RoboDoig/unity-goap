using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapFactory : MonoBehaviour
{
    public List<Action> actions = new List<Action>();
    public Agent agent;

    void Start() {
        // // State definition - start state
        // // get 'zero state' - all possible ItemDefinitions with amount = 0
        // Dictionary<string, int> startState = agent.inventory.GetItemsAsState();

        // // goal state
        // Dictionary<string, int> goalState = new Dictionary<string, int>{{"wood", 6}};

        // // GOAP generation
        // GOAP goap = new GOAP(agent, startState, goalState);
        // List<Action> actionList = goap.GeneratePlan();

        // // Give to agent
        // foreach (Action action in actionList) {
        //     agent.AddActionToQueue(action);
        // }
    }
}
