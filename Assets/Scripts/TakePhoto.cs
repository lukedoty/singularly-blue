using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
                                Debug.Log("hit artifact");
                                this.GetComponent<Camera>().targetTexture = photoOneTexture;
                                //photoOneImage.SetActive(true);
                                photoTaken = true;
                    }
                } else
                {
                    canBorder.SetActive(false);
                }
            } else
            {
                canBorder.SetActive(false);
            }
        }
    }
}
