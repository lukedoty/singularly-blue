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
    private RenderTexture photoRenderTexture;
    [SerializeField]
    private GameObject canBorder;
    [SerializeField] JournalManager journalManager;

    private RaycastHit hit;

    void Start()
    {
        canBorder.SetActive(false);

        photoRenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
    }
    void Update()
    {
        if (photoTaken)
        {
            Artifact a = hit.transform.gameObject.GetComponent<Artifact>();
            if (journalManager.Elements.ContainsKey(a))
            {
                JournalElement je = journalManager.Elements[a];
                je.CapturedPhoto = ToTexture2D(photoRenderTexture);
            }
            

            this.GetComponent<Camera>().targetTexture = cameraRenderTexture;
            photoTaken = false;
        }
        
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (cameraMenu.activeSelf)
        {
            if (Physics.Raycast(transform.position, fwd, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Artifact>() && hit.transform.gameObject.GetComponent<Artifact>().CalculateArtifactCoverage() >= 1)
                {
                    canBorder.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        Debug.Log("hit artifact " + hit.transform.gameObject.name);
                        this.GetComponent<Camera>().targetTexture = photoRenderTexture;
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

    Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
