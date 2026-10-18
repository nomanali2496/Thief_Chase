using UnityEngine;
using UnityEngine.UI;

public class MinimapRadarSweep : MonoBehaviour
{
    [Header("Radar Settings")]
    public float sweepSpeed = -150f;
    public Image sweepImage; // Assign a semi-transparent gradient line image here

    void Update()
    {
        if (sweepImage != null)
        {
            // Rotate the radar sweep line continuously
            sweepImage.rectTransform.Rotate(0f, 0f, sweepSpeed * Time.deltaTime);
        }
        else
        {
            // If no image assigned, try to rotate this object itself
            transform.Rotate(0f, 0f, sweepSpeed * Time.deltaTime);
        }
    }
}
