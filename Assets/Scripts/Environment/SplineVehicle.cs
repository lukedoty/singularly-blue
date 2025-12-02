using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineVehicle : MonoBehaviour
{
	[HideInInspector] public SplineContainer spline;
	[HideInInspector] public float speedWorldUnits = 10f;

	float _t;
	float _normalizedSpeed;
	bool _initialized;

	public void Init(SplineContainer splineContainer, float speed)
	{
		spline = splineContainer;
		speedWorldUnits = Mathf.Max(0f, speed);

		if (spline == null)
		{
			Debug.LogWarning("SplineVehicle.Init called with null spline.");
			return;
		}

		var path = spline.Spline;

		// get spline total world-space length
		float length = SplineUtility.CalculateLength(path, spline.transform.localToWorldMatrix);

		_normalizedSpeed = (length <= 0.0001f)
			? 0f
			: speedWorldUnits / length;

		_t = 0f;
		_initialized = true;

		UpdatePosition();
	}

	void Update()
	{
		if (!_initialized || spline == null || _normalizedSpeed <= 0f)
			return;

		_t += _normalizedSpeed * Time.deltaTime;

		if (_t >= 1f)
		{
			Destroy(gameObject);
			return;
		}

		UpdatePosition();
	}

	void UpdatePosition()
	{
		var path = spline.Spline;
		float tClamped = Mathf.Clamp01(_t);

		// Local-space position and tangent
		float3 localPos = SplineUtility.EvaluatePosition(path, tClamped);
		float3 localTan = SplineUtility.EvaluateTangent(path, tClamped);

		// Convert to world space using the spline container transform
		Vector3 worldPos = spline.transform.TransformPoint((Vector3)localPos);
		Vector3 worldForward = spline.transform.TransformDirection((Vector3)localTan);

		if (worldForward.sqrMagnitude < 0.0001f)
			worldForward = transform.forward;   // fallback if tangent is zero

		Quaternion worldRot = Quaternion.LookRotation(worldForward.normalized, Vector3.up);

		transform.SetPositionAndRotation(worldPos, worldRot);
	}

}
