using UnityEngine;







public class PaintCan : MonoBehaviour
{
	public Camera cam;
	public float maxDistance = 10f;
	public LayerMask paintMask = ~0;
	[Header("Spray shape")]
	public float baseRadiusUV = 0.01f;
	public float radiusPerMeter = 0.002f;
	[Range(0f, 1f)] public float hardness = 0.5f;
	[Header("Spray Behaviour")]
	public float sprayAdditiveLevel = 2f;
	public float distanceFalloffPower = 1.2f;
	public KeyCode sprayKey = KeyCode.X;






	[Header("VFX")]
	public ParticleSystem sprayVFX;

	void Reset()
	{
		if (cam == null) cam = Camera.main;
	}




	void Update()
	{
		bool spraying = Input.GetKey(sprayKey);

		if (sprayVFX != null)
		{
			if (spraying && !sprayVFX.isPlaying)
				sprayVFX.Play();
			else if (!spraying && sprayVFX.isPlaying)
				sprayVFX.Stop();
		}
		if(!spraying) return;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, paintMask))
			return;
		Paintable p = hit.collider.GetComponent<Paintable>();
		if (p == null) p = hit.collider.GetComponentInParent<Paintable>();
		if (p == null) return;
		Vector2 uv = hit.textureCoord;
		float dist = hit.distance;
		float strokeUVsize = Mathf.Max(0f, baseRadiusUV + dist * radiusPerMeter);
		float t = Mathf.Clamp01(dist / maxDistance);
		float distanceFactor = 1f - Mathf.Pow(t, distanceFalloffPower);
		float additiveSpray = sprayAdditiveLevel * distanceFactor * Time.deltaTime;
		if (additiveSpray <= 0f)
			return;
		p.AirbrushOnMeshUV(uv, strokeUVsize, additiveSpray, hardness);
	}

	void OnDisable()
	{
		if (sprayVFX != null && sprayVFX.isPlaying)
			sprayVFX.Stop();
	}
}
