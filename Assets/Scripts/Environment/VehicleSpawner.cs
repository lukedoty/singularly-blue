using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class VehicleSpawner : MonoBehaviour
{
	[Header("Path")]
	[Tooltip("Spline that vehicles will follow (one-way).")]
	public SplineContainer spline;

	[Header("Vehicle")]
	[Tooltip("Prefab that will be spawned and moved along the spline.")]
	public GameObject vehiclePrefab;

	[Tooltip("Movement speed in world units per second along the spline.")]
	public float vehicleSpeed = 10f;

	[Header("Spawn Timing")]
	[Tooltip("Base time between bursts (seconds).")]
	public float interval = 3f;

	[Tooltip("Extra random time added to the interval (0–intervalRandom).")]
	public float intervalRandom = 0f;

	[Tooltip("How many vehicles to spawn per burst (1 for cars, 4+ for trains).")]
	public int burstCount = 1;

	[Tooltip("Time between spawns inside a single burst (seconds).")]
	public float burstInterval = 0.2f;

	Coroutine _loop;

	void OnEnable()
	{
		if (_loop == null)
			_loop = StartCoroutine(SpawnLoop());
	}

	void OnDisable()
	{
		if (_loop != null)
		{
			StopCoroutine(_loop);
			_loop = null;
		}
	}

	IEnumerator SpawnLoop()
	{
		// simple endless loop – you can add your own stop condition if needed
		while (true)
		{
			// wait for next burst (base interval + random [0, intervalRandom])
			float wait = Mathf.Max(0f, interval);
			if (intervalRandom > 0f)
				wait += Random.Range(0f, intervalRandom);

			yield return new WaitForSeconds(wait);

			// spawn burst
			for (int i = 0; i < Mathf.Max(1, burstCount); i++)
			{
				SpawnOne();

				// delay between vehicles *inside* the burst
				if (i < burstCount - 1 && burstCount > 1 && burstInterval > 0f)
					yield return new WaitForSeconds(burstInterval);
			}
		}
	}

	void SpawnOne()
	{
		if (vehiclePrefab == null || spline == null)
			return;

		// spawn at the spline start; prefab orientation is assumed correct
		GameObject instance = Instantiate(vehiclePrefab);

		// ensure there is a SplineVehicle on the instance
		SplineVehicle mover = instance.GetComponent<SplineVehicle>();
		if (mover == null)
			mover = instance.AddComponent<SplineVehicle>();

		mover.Init(spline, vehicleSpeed);
	}
}
