using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TheFirstPerson;

public class UIToggle : MonoBehaviour
{
    private GameObject journalMenu;
    private GameObject cameraMenu;
    private GameObject pauseMenu;

    [SerializeField]
    private KeyCode journalToggleKey = KeyCode.Tab;
    [SerializeField]
    private KeyCode cameraToggleKey = KeyCode.C;
    [SerializeField]
    private KeyCode pauseToggleKey = KeyCode.Escape;
    [SerializeField]
    private GameObject paintCan;

    // Start is called on the first frame
    private void Start()
    {
        journalMenu = GameObject.FindWithTag("Journal");
        journalMenu.SetActive(false);
        cameraMenu = GameObject.FindWithTag("CameraUI");
        cameraMenu.SetActive(false);
        pauseMenu = GameObject.FindWithTag("PauseMenu");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if you press the journal key, it will enable/disable the journal menu appropriately
        if (Input.GetKeyDown(journalToggleKey))
        {
            if (journalMenu.activeSelf)
            {
                journalMenu.SetActive(false);
            }
            else
            {
                journalMenu.SetActive(true);
            }
        }

        if (Input.GetKeyDown(cameraToggleKey) && !journalMenu.activeSelf)
        {
            if (cameraMenu.activeSelf)
            {
                cameraMenu.SetActive(false);
                paintCan.SetActive(true);
            }
            else
            {
                cameraMenu.SetActive(true);
                paintCan.SetActive(false);
            }
        }

        if (Input.GetKeyDown(pauseToggleKey))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                GetComponent<FPSController>().mouseLookEnabled = true;
                GetComponent<FPSController>().mouseLocked = true;
                toggleInputs();
            } else
            {
                pauseMenu.SetActive(true);
                GetComponent<FPSController>().mouseLookEnabled = false;
                GetComponent<FPSController>().mouseLocked = false;
                toggleInputs();
            }
        }
    }

    public void toggleInputs()
    {
        if (journalToggleKey == KeyCode.Tab)
        {
            journalToggleKey = KeyCode.None;
            cameraToggleKey = KeyCode.None;
            pauseToggleKey = KeyCode.None;
            paintCan.GetComponent<PaintCan>().SprayKey = KeyCode.None;
        } else
        {
            journalToggleKey = KeyCode.Tab;
            cameraToggleKey = KeyCode.C;
            pauseToggleKey = KeyCode.Escape;
            paintCan.GetComponent<PaintCan>().SprayKey = KeyCode.Mouse0;
        }
    }
}
