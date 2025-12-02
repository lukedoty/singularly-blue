using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanSim : MonoBehaviour
{
	[Header("Wave 1")]
	public float amplitude1 = 0.5f;      // Height
	public float wavelength1 = 10f;      // Distance between peaks
	public float speed1 = 1f;            // Scroll speed
	public Vector2 direction1 = new Vector2(1f, 0f);

	[Header("Wave 2")]
	public float amplitude2 = 0.3f;
	public float wavelength2 = 5f;
	public float speed2 = 1.5f;
	public Vector2 direction2 = new Vector2(0.5f, 1f);

	[Header("General")]
	public bool recalculateNormals = true;    // Turn off if too expensive
	public float normalRecalcInterval = 0.1f; // Seconds between recalcs

	private MeshFilter _meshFilter;
	private Mesh _meshInstance;
	private Vector3[] _baseVertices;
	private Vector3[] _deformedVertices;
	private float _normalTimer;

	void Awake()
	{
		_meshFilter = GetComponent<MeshFilter>();

		// Clone the mesh so we don't modify the original asset
		_meshInstance = Instantiate(_meshFilter.sharedMesh);
		_meshInstance.name = _meshFilter.sharedMesh.name + "_OceanInstance";
		_meshFilter.mesh = _meshInstance;

		_baseVertices = _meshInstance.vertices;
		_deformedVertices = new Vector3[_baseVertices.Length];

		// Normalize directions to avoid weird scaling
		if (direction1.sqrMagnitude > 0.0001f) direction1.Normalize();
		if (direction2.sqrMagnitude > 0.0001f) direction2.Normalize();
	}

	void Update()
	{
		AnimateWaves();
	}

	void AnimateWaves()
	{
		if (_meshInstance == null || _baseVertices == null) return;

		float time = Time.time;

		float k1 = (wavelength1 != 0f) ? (2f * Mathf.PI / wavelength1) : 0f; // Wave number
		float k2 = (wavelength2 != 0f) ? (2f * Mathf.PI / wavelength2) : 0f;

		for (int i = 0; i < _baseVertices.Length; i++)
		{
			Vector3 v = _baseVertices[i];

			// Local xz position of the vertex
			float x = v.x;
			float z = v.z;

			// Project onto wave directions
			float d1 = direction1.x * x + direction1.y * z;
			float d2 = direction2.x * x + direction2.y * z;

			float phase1 = d1 * k1 + time * speed1;
			float phase2 = d2 * k2 + time * speed2;

			float y =
				Mathf.Sin(phase1) * amplitude1 +
				Mathf.Sin(phase2) * amplitude2;

			v.y = y;
			_deformedVertices[i] = v;
		}

		_meshInstance.vertices = _deformedVertices;

		// Recalculate normals less often for performance
		if (recalculateNormals)
		{
			_normalTimer -= Time.deltaTime;
			if (_normalTimer <= 0f)
			{
				_meshInstance.RecalculateNormals();
				_normalTimer = normalRecalcInterval;
			}
		}

		_meshInstance.RecalculateBounds();
	}

	// Optional: helper to get water height at a given world position
	public float GetHeightAtWorldPosition(Vector3 worldPos)
	{
		Vector3 localPos = transform.InverseTransformPoint(worldPos);

		float time = Time.time;
		float k1 = (wavelength1 != 0f) ? (2f * Mathf.PI / wavelength1) : 0f;
		float k2 = (wavelength2 != 0f) ? (2f * Mathf.PI / wavelength2) : 0f;

		float x = localPos.x;
		float z = localPos.z;

		float d1 = direction1.x * x + direction1.y * z;
		float d2 = direction2.x * x + direction2.y * z;

		float phase1 = d1 * k1 + time * speed1;
		float phase2 = d2 * k2 + time * speed2;

		float y =
			Mathf.Sin(phase1) * amplitude1 +
			Mathf.Sin(phase2) * amplitude2;

		// Back to world space
		Vector3 worldPoint = transform.TransformPoint(new Vector3(localPos.x, y, localPos.z));
		return worldPoint.y;
	}
}
