using UnityEngine;

public class ShipRocking : MonoBehaviour
{
	[Header("Rock Amount (Degrees)")]
	public float rockX = 2f;
	public float rockZ = 3f;

	[Header("Rock Speed")]
	public float speedX = 1f;
	public float speedZ = 1.3f;

	private Quaternion baseRot;
	private float tX;
	private float tZ;

	void Start()
	{
		baseRot = transform.localRotation;

		// randomize phase offsets
		tX = Random.Range(0f, 1000f);
		tZ = Random.Range(0f, 1000f);
	}

	void Update()
	{
		tX += Time.deltaTime;
		tZ += Time.deltaTime;

		float x = Mathf.Sin(tX * speedX) * rockX;
		float z = Mathf.Cos(tZ * speedZ) * rockZ;

		transform.localRotation = baseRot * Quaternion.Euler(x, 0f, z);
	}
}
