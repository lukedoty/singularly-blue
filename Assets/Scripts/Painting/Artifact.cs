using UnityEngine;

[RequireComponent(typeof(Paintable))]
public class Artifact : MonoBehaviour
{
    [SerializeField]
    private Texture2D m_artifactTexture;
    [SerializeField]
    private float m_coverageTaget = 0.8f;

    private Renderer m_renderer;

    private readonly string m_artifactPropertyName = "_Artifact";

    private Paintable m_paintable;
    private Texture2D m_paintTexture;
    private MaterialPropertyBlock m_materialPB;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        m_paintable = GetComponent<Paintable>();
        m_paintTexture = m_paintable.PaintTexture;
        m_materialPB = m_paintable.MaterialPB;

        m_renderer.GetPropertyBlock(m_materialPB);
        m_materialPB.SetTexture(m_artifactPropertyName, m_artifactTexture);
        m_renderer.SetPropertyBlock(m_materialPB);
    }

    public float CalculateArtifactCoverage()
    {
        int blue = 0;
        int i = 0;

        for (; i < m_paintTexture.width * m_paintTexture.height; i++)
        {
            if (!m_paintTexture.GetPixel(i % m_paintTexture.width, i / m_paintTexture.width).Equals(m_paintable.m_clearColor)) blue++;
        }

        return (float)blue / i / m_coverageTaget;
    }
}
