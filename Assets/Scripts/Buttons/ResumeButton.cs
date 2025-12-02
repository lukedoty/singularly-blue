using TheFirstPerson;
using Unity.VisualScripting;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    GameObject pauseMenu;
    [SerializeField]
    GameObject player;
    void Start()
    {
        pauseMenu = GameObject.FindWithTag("PauseMenu");

    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        player.GetComponent<FPSController>().mouseLookEnabled = true;
        player.GetComponent<FPSController>().mouseLocked = true;
        player.GetComponent<UIToggle>().toggleInputs();
    }
}
