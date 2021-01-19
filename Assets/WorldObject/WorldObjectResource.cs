using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectResource : WorldObject
{
    [Header("Collect Resource Conditions/Effects")]
    public List<WorldItem> preconditions;
    public List<WorldItem> effects;

    protected override void Start() {
        base.Start();
        string descriptionString = "Collect Resource: " + effects[0].Description();

        actions.Add(new CollectResourceAction(descriptionString, new List<WorldItem>(preconditions), new List<WorldItem>(effects), this));
    }
}
