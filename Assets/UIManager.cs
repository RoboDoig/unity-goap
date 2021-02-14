using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton;
    public GameObject interactionPanelPrefab;

    void Awake() {
        if (singleton != null) {
            Destroy(gameObject);
            return;
        }

        singleton = this;
    }

    public void CreateInteractionPanel(WorldObject.InformationStruct informationStruct) {
        GameObject obj = Instantiate(interactionPanelPrefab, transform);
        obj.transform.SetParent(transform);
        obj.transform.position = Input.mousePosition;
        InteractionPanel interactionPanel = obj.GetComponent<InteractionPanel>();

        // Set the title as the object name
        interactionPanel.objectNameText.text = informationStruct.objectName;

        // Set this panel to close on the close button click
        interactionPanel.closeButton.onClick.AddListener(() => {Destroy(obj);});

        // Update the actions panel
        foreach (Action action in informationStruct.actions) {
            ActionIndicator actionIndicator = Instantiate(interactionPanel.actionEntryPrefab, transform);
            actionIndicator.transform.SetParent(interactionPanel.actionListPanel.transform);

            // Add precondition icons
            foreach (WorldItem precondition in action.preconditions) {
                Image itemIcon = Instantiate(actionIndicator.itemIcon, transform);
                itemIcon.transform.SetParent(actionIndicator.preconditionGridLayout.transform);
                itemIcon.sprite = precondition.itemDefinition.icon;
            }

            // Add effect icons
            foreach (WorldItem effect in action.effects) {
                Image itemIcon = Instantiate(actionIndicator.itemIcon, transform);
                itemIcon.transform.SetParent(actionIndicator.effectGridLayout.transform);
                itemIcon.sprite = effect.itemDefinition.icon;
            }
        }
    }
}
