using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GOAP
{
    // TODO define maximum plan length
    public Agent agent;
    public List<Action> availableActions;
    public Dictionary<string, int> startState;
    public Dictionary<string, int> goalState;

    public GOAP(Agent _agent, Dictionary<string, int> _startState, Dictionary<string, int> _goalState) {
        agent = _agent;
        availableActions = agent.AvailableActions();
        startState = _startState;
        goalState = _goalState;
    }

    public List<Action> GeneratePlan() {
        bool planFound = false;
        List<Action> actionList = new List<Action>();

        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        List<Action> actionPlan = new List<Action>();
        Node currentNode;

        Node startStateNode = new Node(null, 0, new Dictionary<string, int>(startState), null, availableActions);

        // We begin at the goal state and work back to our current state
        openNodes.Add(startStateNode);

        // As long as we have no plan and there are still open nodes to search, continue looking for plan
        int iterations = 0;
        while (!planFound && openNodes.Count > 0) {
            iterations++;

            // DEBUG BREAK
            if (iterations > 10000) {
                break;
            }

            // start at the lowest cost node of the open nodes, check if we have reached plan
            currentNode = LowestCostNode(openNodes);
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            planFound = IsEndStateReached(currentNode);

            // If we found a plan
            if (planFound) {
                Debug.Log("Plan found in " + iterations.ToString() + " iterations");
                while (currentNode.parent != null) {
                    Debug.Log(currentNode.action.description);
                    actionList.Add(currentNode.action);
                    currentNode = currentNode.parent;
                }
                actionList.Reverse();
                return actionList;
            }

            // What actions are possible to take from this node?
            List<Action> neighbors = GetNeighbors(currentNode);

            // Add these actions as nodes to the tree
            foreach (Action neighbor in neighbors) {
                // First generate a fresh child node, tick over the running cost, copy the parent's state.
                // Add the neighbor action, copy the available actions, then remove the neighbor action from available
                Node neighborNode = new Node(currentNode, 
                                             currentNode.runningCost+neighbor.Cost(agent), 
                                             new Dictionary<string, int>(currentNode.state),
                                             neighbor,
                                             new List<Action>(currentNode.availableActions));
                neighborNode.availableActions.Remove(neighbor);

                // Update node with action effects
                foreach (WorldItem effect in neighbor.effects) {
                    if (!neighborNode.state.ContainsKey(effect.AsKey())) {
                        neighborNode.state.Add(effect.AsKey(), 0);
                    }
                    neighborNode.state[effect.AsKey()] += effect.amount;
                }

                // Update node with action preconditions
                foreach (WorldItem precondition in neighbor.preconditions) {
                    // consumable needs to be extended, what if we are actively dropping item? maybe then it just needs to be a negative effect
                    if (precondition.itemDefinition.consumable)
                        neighborNode.state[precondition.AsKey()] -= precondition.amount;
                }

                // Distance cost
                int dCost = 0;
                foreach (KeyValuePair<string, int> stateComponent in goalState) {
                    if (neighborNode.state.ContainsKey(stateComponent.Key)) {
                        // add distance between state and goal state components, should be smaller if node state is closer to goal state
                        dCost += stateComponent.Value - neighborNode.state[stateComponent.Key];
                    } else {
                        // if goal state is not present in node state, set maximum distance
                        dCost += stateComponent.Value;
                    }
                    // standard penalty for this agent
                    dCost += neighborNode.action.Cost(agent);
                }
                neighborNode.runningCost += dCost;

                openNodes.Add(neighborNode);
            }
        }

        if (!planFound)
            Debug.Log("No plan found");

        return null;
    }

    Node LowestCostNode(List<Node> nodes) {
        Node lowestCostNode = nodes[0];
        foreach (Node node in nodes) {
            if (node.runningCost < lowestCostNode.runningCost) {
                lowestCostNode = node;
            }
        }

        return lowestCostNode;
    }

    // For a given node, IsEndStateReached returns true if the node state satisfies the goal state
    bool IsEndStateReached(Node node) {
        Debug.Log(node.action);
        PrintState(node.state);
        // For all state components in the goal state
        foreach (KeyValuePair<string, int> stateComponent in goalState) {
            // If the node state doesn't have a component of the goal state, end is not reached
            if (!node.state.ContainsKey(stateComponent.Key)) {
                return false;
            }

            // If the node state has less than the target goal state, end is not reached
            if (node.state[stateComponent.Key] < stateComponent.Value) {
                return false;
            }
        }

        return true;
    }

    List<Action> GetNeighbors(Node node) {
        List<Action> neighbors = new List<Action>();
        foreach (Action action in node.availableActions) {
            if (IsNeighbor(action, node)) {
                neighbors.Add(action);
            }
        }

        return neighbors;
    }

    bool IsNeighbor(Action action, Node node) {
        foreach (WorldItem precondition in action.preconditions) {
            // if node state does not have component of action preconditions, action is not a neighbor
            if (!node.state.ContainsKey(precondition.AsKey())) {
                return false;
            }

            // if the node state has less than the precondition amount, action is not a neighbor
            if (node.state[precondition.AsKey()] < precondition.amount) {
                return false;
            }
        }

        return true;
    }

    void PrintState(Dictionary<string, int> state) {
        string stateString = "";
        foreach (KeyValuePair<string, int> stateComponent in state) {
            stateString += stateComponent.Key + ": " + stateComponent.Value.ToString() + ", ";
        }
        Debug.Log(stateString);
    }

    void PrintActions(List<Action> actions) {
        string actionsString  = "";
        foreach (Action action in actions) {
            actionsString += action.description + ", ";
        }
    }

    public class Node {
        public Node parent;
        public int runningCost;
        public Dictionary<string, int> state;
        public Action action;
        public List<Action> availableActions;

        public Node(Node _parent, int _runningCost, Dictionary<string, int> _state, Action _action, List<Action> _availableActions) {
            parent = _parent;
            runningCost = _runningCost;
            state = _state;
            action = _action;
            availableActions = _availableActions;
        }
    }
}
