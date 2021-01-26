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
    }
}
