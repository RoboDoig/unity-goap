using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInterface : MonoBehaviour
{
    float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontalMovement = Input.GetAxis("Horizontal") * transform.transform.right;
        Vector3 forwardMovement = Input.GetAxis("Vertical") * Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        transform.position += (horizontalMovement + forwardMovement).normalized * Time.deltaTime * moveSpeed;

        // Open item panel
        if (Input.GetKeyDown(KeyCode.I)) {
            UIManager.singleton.CreateItemPanel();
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                
            } else {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {

                    // If we select an agent
                    WorldObject selectedObject = hit.transform.GetComponent<WorldObject>();
                    if (selectedObject != null) {
                        UIManager.singleton.CreateInteractionPanel(selectedObject.GetObjectInformation());
                    }
                }
            }
        }
    }
}
