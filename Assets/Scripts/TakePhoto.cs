using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    private bool photoTaken = false;
    private GameObject cameraMenu;

    [SerializeField]
    private RenderTexture cameraRenderTexture;
    [SerializeField]
    private RenderTexture photoOneTexture;
    [SerializeField]
    private GameObject photoOneImage;
    void Start()
    {
        photoOneImage.SetActive(false);
        cameraMenu = GameObject.FindWithTag("CameraUI");
    }
    void Update()
    {
        if (photoTaken)
        {
            this.GetComponent<Camera>().targetTexture = cameraRenderTexture;
        }
        if (Input.GetKeyDown(KeyCode.G) && cameraMenu.activeSelf)
        {
            this.GetComponent<Camera>().targetTexture = photoOneTexture;
            photoOneImage.SetActive(true);
            Debug.Log("photo");
            photoTaken = true;
        }

    }
}
