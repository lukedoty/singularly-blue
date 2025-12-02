using UnityEngine;

public class SceneButton : MonoBehaviour
{
    [SerializeField]
    private string newScene;

    void Start()
    {
        GameManager.SceneManager.LoadScene(newScene);    
    }

    public void SwapScene()
    {
        GameManager.SceneManager.SwapScene();
    }
}
