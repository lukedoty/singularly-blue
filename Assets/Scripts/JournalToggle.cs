using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    private GameObject inventoryMenu;
    private GameObject cameraMenu;

    [SerializeField]
    private KeyCode inventoryToggleKey = KeyCode.Tab;
    [SerializeField]
    private KeyCode cameraToggleKey = KeyCode.C;
    [SerializeField]
    private GameObject paintCan;

    private bool inventoryOpen;
    private bool cameraOpen;

    // Start is called on the first frame
    private void Start()
    {
        inventoryMenu = GameObject.FindWithTag("Journal");
        inventoryMenu.SetActive(false);
        inventoryOpen = inventoryMenu.activeSelf;
        cameraMenu = GameObject.FindWithTag("CameraUI");
        cameraMenu.SetActive(false);
        cameraOpen = cameraMenu.activeSelf;

    }

    // Update is called once per frame
    void Update()
    {
        //if you press the inventory key, it will enable/disable the inventory menu appropriately
        if (Input.GetKeyDown(inventoryToggleKey))
        {
            if (inventoryOpen)
            {
                inventoryOpen = false;
                inventoryMenu.SetActive(false);
            }
            else
            {
                inventoryOpen = true;
                inventoryMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(cameraToggleKey) && !inventoryOpen)
        {
            if (cameraOpen)
            {
                cameraOpen = false;
                cameraMenu.SetActive(false);
                paintCan.SetActive(true);
            }
            else
            {
                cameraOpen = true;
                cameraMenu.SetActive(true);
                paintCan.SetActive(false);
            }
        }
    }
}
