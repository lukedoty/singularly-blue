using UnityEngine;

public class DrawbridgeShipDetector : MonoBehaviour
{
	[Header("Detection")]
	[Tooltip("How far to search for ships.")]
	public float detectionRadius = 50f;

	[Tooltip("How often (seconds) to re-check for ships.")]
	public float checkInterval = 1f;

	[Tooltip("Layer that ships are on.")]
	public LayerMask shipLayer;

	[Header("Rotation")]
	[Tooltip("Base Z angle for the bridge (degrees). Usually 0).")]
	public float baseAngleZ = 0f;

	[Tooltip("Raised Z angle for the bridge (degrees). Negative to tilt upwards on Z, e.g. -70.")]
	public float raisedAngleZ = -70f;

	[Tooltip("Speed (degrees/sec) when raising the bridge.")]
	public float raiseSpeed = 30f;

	[Tooltip("Speed (degrees/sec) when lowering the bridge.")]
	public float lowerSpeed = 40f;

	[Header("Options")]
	[Tooltip("Use local rotation. Leave this ON unless you know you need world rotation.")]
	public bool useLocalRotation = true;

	private bool shipNearby = false;
	private float checkTimer = 0f;

	private void Update()
	{
		// 1. Periodic ship check
		checkTimer += Time.deltaTime;
		if (checkTimer >= checkInterval)
		{
			checkTimer = 0f;
			CheckForShips();
		}

		// 2. Rotate toward target angle
		float targetZ = shipNearby ? raisedAngleZ : baseAngleZ;
		RotateToward(targetZ);
	}

	private void CheckForShips()
	{
		// Overlap sphere around the bridge position (you can offset if needed)
		Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shipLayer);

		shipNearby = hits != null && hits.Length > 0;
	}

	private void RotateToward(float targetZ)
	{
		Vector3 currentEuler = useLocalRotation ? transform.localEulerAngles : transform.eulerAngles;

		// Move Z angle toward target
		float speed = (shipNearby ? raiseSpeed : lowerSpeed) * Time.deltaTime;
		float newZ = Mathf.MoveTowardsAngle(currentEuler.z, targetZ, speed);

		currentEuler.z = newZ;

		if (useLocalRotation)
			transform.localEulerAngles = currentEuler;
		else
			transform.eulerAngles = currentEuler;
	}

	private void OnDrawGizmosSelected()
	{
		// Visualize detection radius
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}
