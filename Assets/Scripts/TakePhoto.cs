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
    [SerializeField]
    private GameObject cantBorder;
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

        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (photoTaken)
        {
            this.GetComponent<Camera>().targetTexture = cameraRenderTexture;
        }
        if (Input.GetKeyDown(KeyCode.G) && cameraMenu.activeSelf)
        {
            if (Physics.Raycast(transform.position, fwd, out hit))
                if (hit.transform.gameObject.GetComponent<Artifact>())
                {
                    Debug.Log("hit artifact");
                    this.GetComponent<Camera>().targetTexture = photoOneTexture;
                    photoOneImage.SetActive(true);
                    photoTaken = true;
                }
        }
    }
}
