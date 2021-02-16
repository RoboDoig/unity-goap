using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectBuildingPlan : WorldObject
{
    public GameObject finishedBuildingPrefab;
    public List<WorldItem> buildingRequirements;
    public WorldItem rewardPerRequirement;
    private Inventory inventory;

    protected override void Start()
    {
        base.Start();
        inventory = GetComponent<Inventory>();
        actionComplete += CheckRequirementsMet;

        // Add the building requirement actions
        foreach (WorldItem worldItem in buildingRequirements) {
            string descriptionString = "Deposit " + worldItem.amount + " " + worldItem.itemDefinition.itemName;
            actions.Add(new DepositItemAction(descriptionString,
                        new List<WorldItem>{new WorldItem(worldItem.itemDefinition, worldItem.amount)},
                        new List<WorldItem>{new WorldItem(rewardPerRequirement.itemDefinition, rewardPerRequirement.amount)},
                        this)
                        );
        }
    }

    public void CheckRequirementsMet() {
        foreach (WorldItem worldItem in buildingRequirements) {
            if (!inventory.HasItem(worldItem)) {
                return;
            }
        }
        
        Instantiate(finishedBuildingPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
