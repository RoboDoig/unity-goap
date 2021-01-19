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

        // start and goal states must have matching keys
        // foreach (KeyValuePair<string, int> stateComponent in startState) {
        //     if (goalState.ContainsKey(stateComponent.Key)) {
        //         goalState[stateComponent.Key] += startState[stateComponent.Key];
        //     } else {
        //         goalState.Add(stateComponent.Key, stateComponent.Value);
        //     }
        // }
    }

    public void GeneratePlan() {
        bool planFound = false;

        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        List<Action> actionPlan = new List<Action>();
        Node currentNode;

        // Node currentStateNode = new Node(null, 0, new List<WorldItem>(currentState), null, new List<Action>());
        Node goalStateNode = new Node(null, 0, new Dictionary<string, int>(goalState), null, availableActions);

        // We begin at the goal state and work back to our current state
        openNodes.Add(goalStateNode);

        // As long as we have no plan and there are still open nodes to search, continue looking for plan
        int iterations = 0;
        while (!planFound && openNodes.Count > 0) {
            iterations++;

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
                    currentNode = currentNode.parent;
                }
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
                    neighborNode.state[effect.AsKey()] -= effect.amount;
                }

                // Update node with action preconditions
                foreach (WorldItem precondition in neighbor.preconditions) {
                    if (!neighborNode.state.ContainsKey(precondition.AsKey())) {
                        neighborNode.state.Add(precondition.AsKey(), 0);
                    }
                    neighborNode.state[precondition.AsKey()] += precondition.amount;
                }

                // Distance cost
                int dCost = 0;
                foreach (KeyValuePair<string, int> stateComponent in startState) {
                    dCost += neighborNode.state[stateComponent.Key] - startState[stateComponent.Key];
                }
                neighborNode.runningCost += dCost;

                openNodes.Add(neighborNode);
            }
        }

        if (!planFound)
            Debug.Log("No plan found");
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

    // For a given node, IsEndStateReached returns true if the node state satisfies the startState, and false otherwise
    // The state is satisfied if all state components of the node state are less than or equal to the start state
    bool IsEndStateReached(Node node) {
        Debug.Log(node.action);
        PrintState(node.state);
        // For all state components
        foreach (KeyValuePair<string, int> stateComponent in node.state) {
            // If this component exists in the start state
            if (startState.ContainsKey(stateComponent.Key)) {
                // if the amount for this component is greater than in the start state, return false
                if (stateComponent.Value > startState[stateComponent.Key]) {
                    return false;
                }
            } else {
                // if this state component is not in the start state return false
                return true;
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
        foreach (WorldItem effect in action.effects) {
            if (node.state.ContainsKey(effect.AsKey())) {
                if (node.state[effect.AsKey()] < effect.amount) {
                    // return false;
                } else {
                    return true;
                }
            } else {
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
