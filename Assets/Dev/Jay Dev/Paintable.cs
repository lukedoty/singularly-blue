using UnityEngine;










[RequireComponent(typeof(Renderer))]
public class Paintable : MonoBehaviour
{


	public int textureSize = 2048;
	public Color clearColor = Color.black;

	public string paintPropertyName = "_paint";

	[HideInInspector] public Texture2D paintTexture;
	Renderer rend;
	MaterialPropertyBlock mpb;



	void Awake()
	{
		rend = GetComponent<Renderer>();
		mpb = new MaterialPropertyBlock();
		paintTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
		paintTexture.wrapMode = TextureWrapMode.Clamp;
		ClearTexture();
		ApplyToMaterial();
	}





	public void ClearTexture()
	{
		var cols = new Color[textureSize * textureSize];
		for (int i = 0; i < cols.Length; i++)
			cols[i] = clearColor;

		paintTexture.SetPixels(cols);
		paintTexture.Apply();
	}




	void ApplyToMaterial()
	{
		rend.GetPropertyBlock(mpb);
		mpb.SetTexture(paintPropertyName, paintTexture);
		rend.SetPropertyBlock(mpb);
	}





	public void AirbrushOnMeshUV(Vector2 uv, float radiusUV, float strength, float hardness = 0.3f)
	{
		int cx = Mathf.RoundToInt(uv.x * textureSize);
		int cy = Mathf.RoundToInt(uv.y * textureSize);
		int r = Mathf.RoundToInt(radiusUV * textureSize);
		int x0 = Mathf.Clamp(cx - r, 0, textureSize - 1);
		int x1 = Mathf.Clamp(cx + r, 0, textureSize - 1);
		int y0 = Mathf.Clamp(cy - r, 0, textureSize - 1);
		int y1 = Mathf.Clamp(cy + r, 0, textureSize - 1);
		float maxR = r > 0 ? r : 1;
		for (int y = y0; y <= y1; y++)
		{
			for (int x = x0; x <= x1; x++)
			{
				float dx = x - cx;
				float dy = y - cy;
				float dist = Mathf.Sqrt(dx * dx + dy * dy);
				if (dist > maxR)
					continue;
				float t = 1f - (dist / maxR);
				float exp = Mathf.Lerp(0.5f, 4f, Mathf.Clamp01(hardness));
				float falloff = Mathf.Pow(t, exp); 
				float delta = strength * falloff;
				Color existing = paintTexture.GetPixel(x, y);
				float v = existing.r;
				v = Mathf.Clamp01(v + delta);   
				existing.r = v;
				existing.g = v;
				existing.b = v;
				existing.a = v;
				paintTexture.SetPixel(x, y, existing);
			}
		}


		paintTexture.Apply();


	}






}
