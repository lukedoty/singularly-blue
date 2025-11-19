using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Paintable : MonoBehaviour
{
	public int TextureSize = 2048;

	private readonly Color m_clearColor = new Color(0, 0, 0, 0);
	private readonly string paintPropertyName = "_Paint";

	private Texture2D m_paintTexture;
	public Texture2D PaintTexture => m_paintTexture;

	private Renderer m_renderer;
	private MaterialPropertyBlock m_materialPB;

	void Awake()
	{
		m_renderer = GetComponent<Renderer>();
		m_materialPB = new MaterialPropertyBlock();
		m_paintTexture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
		m_paintTexture.wrapMode = TextureWrapMode.Clamp;

		ClearPaintTexture();
		ApplyPaintTextureToMaterial();
	}

	public void ClearPaintTexture()
	{
		var pixels = new Color[TextureSize * TextureSize];
		for (int i = 0; i < pixels.Length; i++)
			pixels[i] = m_clearColor;

		m_paintTexture.SetPixels(pixels);
		m_paintTexture.Apply();
	}

	void ApplyPaintTextureToMaterial()
	{
		m_renderer.GetPropertyBlock(m_materialPB);
		m_materialPB.SetTexture(paintPropertyName, m_paintTexture);
		m_renderer.SetPropertyBlock(m_materialPB);
	}

	public void AirbrushOnMeshUV(Vector2 uv, float radiusUV, float strength, float hardness = 0.3f)
	{
		int cx = Mathf.RoundToInt(uv.x * TextureSize);
		int cy = Mathf.RoundToInt(uv.y * TextureSize);
		int r = Mathf.RoundToInt(radiusUV * TextureSize);

		int x0 = Mathf.Clamp(cx - r, 0, TextureSize - 1);
		int x1 = Mathf.Clamp(cx + r, 0, TextureSize - 1);
		int y0 = Mathf.Clamp(cy - r, 0, TextureSize - 1);
		int y1 = Mathf.Clamp(cy + r, 0, TextureSize - 1);

		float maxR = r > 0 ? r : 1;

		for (int y = y0; y <= y1; y++)
		{
			for (int x = x0; x <= x1; x++)
			{
				float dx = x - cx;
				float dy = y - cy;

				float dist = Mathf.Sqrt(dx * dx + dy * dy);
				if (dist > maxR) continue;

				float t = 1f - (dist / maxR);
				float exp = Mathf.Lerp(0.5f, 4f, Mathf.Clamp01(hardness));
				float falloff = Mathf.Pow(t, exp); 
				float delta = strength * falloff;

				Color existing = m_paintTexture.GetPixel(x, y);

				float v = existing.r;
				v = Mathf.Clamp01(v + delta);   

				existing.r = v;
				existing.g = v;
				existing.b = v;
				existing.a = v;

				m_paintTexture.SetPixel(x, y, existing);
			}
		}

		m_paintTexture.Apply();
	}
}
