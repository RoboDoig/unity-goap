using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton;
    public GameObject interactionPanelPrefab;
    public GameObject itemSelectPanelPrefab;

    void Awake() {
        if (singleton != null) {
            Destroy(gameObject);
            return;
        }

        singleton = this;
    }

    // TODO - maybe UI panels should have base and subclasses that implement their own creation methods
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

            // Link the add item button
            actionIndicator.addItemButton.onClick.AddListener(() => {CreateItemSelectPanel(interactionPanel, action);});

            // Add the action title
            actionIndicator.actionNameText.text = action.description;

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

    public void CreateItemSelectPanel(InteractionPanel interactionPanel, Action targetAction) {
        GameObject obj = Instantiate(itemSelectPanelPrefab, transform);
        obj.transform.SetParent(transform);
        obj.transform.position = Input.mousePosition;
        ItemSelectPanel itemSelectPanel = obj.GetComponent<ItemSelectPanel>();

        // Set this panel to close on the close button click
        itemSelectPanel.closeButton.onClick.AddListener(() => {Destroy(obj);});
        // Set parent panel to close select panel on close
        interactionPanel.closeButton.onClick.AddListener(() => {Destroy(obj);});

        // Add the item icons
        List<WorldItem> availableItems = new List<WorldItem>{new WorldItem(ItemDatabase.items.resources.Gold, 1), new WorldItem(ItemDatabase.items.resources.Wood, 1), new WorldItem(ItemDatabase.items.resources.Stone, 1)};
        foreach (WorldItem item in availableItems) {
            Button itemIcon = Instantiate(itemSelectPanel.itemIconButton, transform);
            itemIcon.transform.SetParent(itemSelectPanel.itemIconGridLayout.transform);
            itemIcon.image.sprite = item.itemDefinition.icon;
            // Add function to the icon, adds the world item to the action's effects
            itemIcon.onClick.AddListener(() => {targetAction.AddEffect(item);});
        }
    }
}
