using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitApplication()
    {
        GameManager.SceneManager.QuitApplication();
    }
}
