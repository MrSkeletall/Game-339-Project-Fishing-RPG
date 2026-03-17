using UnityEngine;

public class FishingHook : MonoBehaviour
{
    public ParticleSystem hookParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hookParticles = gameObject.GetComponentInChildren< ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerParticles()
    {
        if (hookParticles != null)
        {
            hookParticles.Play();
        }
    }
}
