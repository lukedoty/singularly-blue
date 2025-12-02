using UnityEngine;

public class SprayPaintController : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private ParticleSystem sprayParticlesA;
	[SerializeField] private ParticleSystem sprayParticlesB;

	[Header("Input")]
	[SerializeField] private int mouseButtonIndex = 0;

	private bool isSpraying;

	void Awake()
	{
		StopSprayingSystems();
		isSpraying = false;
	}

	void Update()
	{
		bool buttonHeld = Input.GetMouseButton(mouseButtonIndex);

		if (buttonHeld && !isSpraying)
			StartSprayingSystems();
		else if (!buttonHeld && isSpraying)
			StopSprayingSystems();
	}

	private void StartSprayingSystems()
	{
		isSpraying = true;
		if (sprayParticlesA) sprayParticlesA.Play(true);
		if (sprayParticlesB) sprayParticlesB.Play(true);
	}

	private void StopSprayingSystems()
	{
		isSpraying = false;
		if (sprayParticlesA) sprayParticlesA.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		if (sprayParticlesB) sprayParticlesB.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}
}
