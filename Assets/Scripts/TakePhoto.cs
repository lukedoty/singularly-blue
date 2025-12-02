using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    private bool photoTaken = false;

    [SerializeField]
    private GameObject cameraMenu;

    [SerializeField]
    private RenderTexture cameraRenderTexture;
    [SerializeField]
    private RenderTexture photoOneTexture;
    //[SerializeField]
    //private GameObject photoOneImage;
    [SerializeField]
    private GameObject canBorder;
    void Start()
    {
        //photoOneImage.SetActive(false);
        canBorder.SetActive(false);
    }
    void Update()
    {
        if (photoTaken)
        {
            this.GetComponent<Camera>().targetTexture = cameraRenderTexture;
        }

        canBorder.SetActive(false);

        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (cameraMenu.activeSelf)
        {
            if (Physics.Raycast(transform.position, fwd, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Artifact>())
                {
                    canBorder.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        if (Physics.Raycast(transform.position, fwd, out hit))
                            if (hit.transform.gameObject.GetComponent<Artifact>())
                            {
                                Debug.Log("hit artifact");
                                this.GetComponent<Camera>().targetTexture = photoOneTexture;
                                //photoOneImage.SetActive(true);
                                photoTaken = true;
                            }
                    }
                }
            }
        }
    }
}
