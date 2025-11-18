using UnityEngine;

[RequireComponent(typeof(SceneManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private SceneManager m_sceneManager;
    public static SceneManager SceneManager => Instance.m_sceneManager;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        m_sceneManager = GetComponent<SceneManager>();
    }
}