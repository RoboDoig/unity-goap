using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    // Interaction params
    public float reachDistance = 1f;  

    private NavMeshAgent navMeshAgent;
    public Inventory inventory;
    private List<Action> actionQueue;
    private Action currentAction;
    public float workTimer {get; private set;}
    public List<Action> agentActions = new List<Action>();
    public WorldAgent worldAgent {get; private set;}

    // Events
    private UnityEvent onActionComplete;

    void Awake() {
        actionQueue = new List<Action>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();
        worldAgent = GetComponent<WorldAgent>();
        workTimer = 0f;
    }

    void Update() {
        // Check if we need to switch actions
        if (currentAction == null && actionQueue.Count > 0) {
            currentAction = actionQueue[0];
        }

        if (currentAction != null) {
            currentAction.PerformAction(this);
            if (currentAction.isComplete) {
                ActionComplete(currentAction);
            }
        }
    }

    // Returns a list of actions that are doable by this agent in its current state
    public List<Action> AvailableActions() {
        List<Action> availableActions = new List<Action>();

        foreach(Action action in Action.availableActions) {
            if (action.CheckProcedural(this)) {
                availableActions.Add(action);
            }
        }

        return availableActions;
    }

    // Adds an action to the queue of actions to be done
    public void AddActionToQueue(Action action) {
        actionQueue.Add(action);
        action.Reserve(this);
    }

    public void ActionComplete(Action action) {
        action.ActionComplete(this);

        actionQueue.Remove(currentAction);
        currentAction = null;
        workTimer = 0f;

        // Fire an action complete event
        onActionComplete.Invoke();
    }

    public float UpdateWorkTimer() {
        workTimer += Time.deltaTime;
        return workTimer;
    }

    // Sets the agent's destination
    public void SetDestination(Vector3 target) {
        navMeshAgent.SetDestination(target);
    }

    // Get the agent's current WorldItem state (inventory + stats) as a dictionary
    public Dictionary<string, int> GetState() {
        Dictionary<string, int> state = new Dictionary<string, int>();
        foreach (WorldItem item in inventory.GetItems()) {
            state.Add(item.itemDefinition.itemName, item.amount);
        }
        return state;
    }

    public void CreateObject(GameObject gameObject, Vector3 position, Quaternion rotation) {
        Instantiate(gameObject, position, rotation);
    }
}