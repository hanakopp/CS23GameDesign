using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorVisionPowerUp : MonoBehaviour
{
    public Volume globalVolume; // drag your Global Volume here in Inspector
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Get the ColorAdjustments from the volume
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            // Make sure it starts black & white
            colorAdjustments.saturation.value = -100f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ZombiePlayer player = other.GetComponent<ZombiePlayer>();
        if (player != null && colorAdjustments != null)
        {
            // Restore color
            colorAdjustments.saturation.value = 0f;
            Destroy(gameObject); // remove powerup
        }
    }
}
