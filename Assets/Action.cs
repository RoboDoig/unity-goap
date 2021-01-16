using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Action - something that can be done in the world. This is the template base class.
public class Action
{
    public static List<Action> availableActions = new List<Action>();
    public string description;
    public enum ActionType {Global, AgentOnly, OtherAgentOnly}
    public ActionType actionType;
    public bool inUse = false;
    // for future use, if action deleted need to tell agent that has reserved it
    public Agent agentUsing = null;
    // List of preconditions
    public List<WorldItem> preconditions;
    // List of effectss
    public List<WorldItem> effects;
    public WorldObject parentObject;
    public float baseWorkTime {get; protected set;}
    public bool isComplete {get; protected set;}
    public bool repeatable = false;

    public Action(string _description, List<WorldItem> _preconditions, List<WorldItem> _effects, WorldObject _parentObject) {
        description = _description;
        preconditions = _preconditions;
        effects = _effects;
        parentObject = _parentObject;

        actionType = ActionType.Global;
        availableActions.Add(this);
        isComplete = false;
        baseWorkTime = 1f;
    }

    // Checks if a given agent can perform this action
    public bool CheckProcedural(Agent agent) {
        // Check action is not in use (reserved)
        if (inUse) {
            return false;
        }

        // If this is an agent only action, check the action is attached to the agent
        if (actionType == ActionType.AgentOnly) {
            if (parentObject != agent.worldAgent) {
                return false;
            }
        }

        // If this is is an other agent only action, check the action is attached to a different agent
        if (actionType == ActionType.OtherAgentOnly) {
            if (parentObject == agent.worldAgent) {
                return false;
            }
        }

        // Check agent has required preconditions
        // foreach (WorldItem precondition in preconditions) {
        //     if (!agent.inventory.HasItem(precondition)) {
        //         return false;
        //     }
        // }

        return true;
    }

    // Request to reserve action so that other agents cannot use it. Some actions, might not need to be reserved, e.g. common actions
    public void Reserve(Agent agent) {
        inUse = true;
    }

    public void DeReserve(Agent agent) {
        inUse = false;
    }

    // Tells a given agent how to perform this action
    public virtual void PerformAction(Agent agent) {
        agent.SetDestination(parentObject.transform.position);

        // if the agent is within reach distance of the parent object
        if ((agent.transform.position - parentObject.transform.position).magnitude < agent.reachDistance) {
            // update the agent work timer
            agent.UpdateWorkTimer();

            // if the agent has been at the object for sufficient time, we can say the action is complete
            if (agent.workTimer > baseWorkTime) {
                isComplete = true;
            }
        }
    }

    // When an action completes
    public virtual void ActionComplete(Agent agent) {
        DeReserve(agent);

        // if action is not repeatable (e.g. eating) then it needs to be removed from the available actions list
        if (!repeatable) {
            availableActions.Remove(this);
            parentObject.actions.Remove(this);
        }

        // remove preconditions from the acting agent
        foreach(WorldItem precondition in preconditions) {
            if (precondition.itemDefinition.consumable) {
                agent.inventory.RemoveItem(precondition);
            }
        }

        // add effects for the acting agent
        foreach(WorldItem effect in effects) {
            agent.inventory.AddItem(effect);
        }
    }

    public void Remove() {
        availableActions.Remove(this);
    }

    // What is the cost of doing this action for a given agent?
    public virtual int Cost(Agent agent) {
        return 1;
    }
}
