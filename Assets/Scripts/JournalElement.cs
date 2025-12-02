using UnityEngine;
using UnityEngine.UI;

public class JournalElement : MonoBehaviour
{
    [SerializeField]
    private Artifact m_artifact;
    public Artifact Artifact => m_artifact;

    [SerializeField]
    private Sprite m_taskPhoto;

    public Texture2D CapturedPhoto;

    [SerializeField]
    private Image m_taskPhotoUI;
    [SerializeField]
    private RawImage m_capturedPhotoUI;

    private void Start()
    {
        m_taskPhotoUI.sprite = m_taskPhoto;
        m_capturedPhotoUI.texture = CapturedPhoto;
    }

    private void Update()
    {
        Debug.Log(CapturedPhoto);
        if (CapturedPhoto == null) m_capturedPhotoUI.enabled = false;
        else if (m_capturedPhotoUI.enabled == false)
        {
            m_capturedPhotoUI.enabled = true;
            m_capturedPhotoUI.texture = CapturedPhoto;
        }
    }
}
