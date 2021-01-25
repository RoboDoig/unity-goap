using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : Action
{
    public EatAction(string _description, List<WorldItem> _preconditions, List<WorldItem> _effects, WorldObject _parentObject) 
    : base(_description, _preconditions, _effects, _parentObject) {
        actionType = ActionType.AgentOnly;
        repeatable = true;
    }

    public override void PerformAction(Agent agent) {
        base.PerformAction(agent);
    }

    public override void ActionComplete(Agent agent) {
        base.ActionComplete(agent);
    }
}
