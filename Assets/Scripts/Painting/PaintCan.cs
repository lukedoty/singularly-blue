using UnityEngine;

public class PaintCan : MonoBehaviour
{
	public float MaxDistance = 10f;
	public LayerMask PaintMask = ~0;

	[Header("Spray shape")]
	public float BaseRadiusUV = 0.01f;
	public float RadiusPerMeter = 0.002f;
	[Range(0f, 1f)] public float Hardness = 0.5f;

	[Header("Spray Behaviour")]
	public float SprayAdditiveLevel = 2f;
	public float DistanceFalloffPower = 1.2f;
	public KeyCode SprayKey = KeyCode.X;

	[Header("VFX")]
	public ParticleSystem SprayVFX;

	private Camera m_cam;

	void Reset()
	{
		if (m_cam == null) m_cam = Camera.main;
	}

	void Update()
	{
		bool spraying = Input.GetKey(SprayKey);

		if (SprayVFX != null)
		{
			if (spraying && !SprayVFX.isPlaying)
				SprayVFX.Play();
			else if (!spraying && SprayVFX.isPlaying)
				SprayVFX.Stop();
		}
		if(!spraying) return;
		Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out RaycastHit hit, MaxDistance, PaintMask))
			return;
		Paintable p = hit.collider.GetComponent<Paintable>();
		if (p == null) p = hit.collider.GetComponentInParent<Paintable>();
		if (p == null) return;
		Vector2 uv = hit.textureCoord;
		float dist = hit.distance;
		float strokeUVsize = Mathf.Max(0f, BaseRadiusUV + dist * RadiusPerMeter);
		float t = Mathf.Clamp01(dist / MaxDistance);
		float distanceFactor = 1f - Mathf.Pow(t, DistanceFalloffPower);
		float additiveSpray = SprayAdditiveLevel * distanceFactor * Time.deltaTime;
		if (additiveSpray <= 0f)
			return;
		p.AirbrushOnMeshUV(uv, strokeUVsize, additiveSpray, Hardness);
	}

	void OnDisable()
	{
		if (SprayVFX != null && SprayVFX.isPlaying)
			SprayVFX.Stop();
	}
}
