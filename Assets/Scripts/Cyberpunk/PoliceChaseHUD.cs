using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PoliceChaseHUD : MonoBehaviour
{
    [Header("Targeting System")]
    public RectTransform targetBracket; // The red corners in the middle of the screen
    public TextMeshProUGUI distanceText; // "SUSPECT 12 m"
    public Transform targetSuspect; // The thief car

    [Header("Top Bars")]
    public Image catchProgressBar;
    public Image suspectProgressBar;
    public TextMeshProUGUI dispatchText;

    [Header("Timer")]
    public TextMeshProUGUI timerText;

    [Header("Dashboard (RCC Integration)")]
    public RCC_CarControllerV3 rccCar;
    
    [Header("Speedometer Gauge")]
    public RectTransform speedNeedle;
    public TextMeshProUGUI speedText;
    public float maxSpeed = 260f;
    
    [Header("RPM Gauge")]
    public RectTransform rpmNeedle;
    public TextMeshProUGUI rpmText;
    public float maxRpm = 8f; // Represented as x1000 RPM

    [Header("NOS Gauge")]
    public RectTransform nosNeedle;
    public TextMeshProUGUI nosText;

    // Angles for the needles (adjust these based on your needle sprite)
    private float needleMinRotation = 135f;  // Starting angle (Bottom Left)
    private float needleMaxRotation = -135f; // Ending angle (Bottom Right)

    void Update()
    {
        if (rccCar == null) return;

        UpdateGauges();
        UpdateTargeting();
    }

    void UpdateGauges()
    {
        // 1. Speedometer
        float speed = rccCar.speed;
        float speedRot = Mathf.Lerp(needleMinRotation, needleMaxRotation, speed / maxSpeed);
        if (speedNeedle) speedNeedle.localEulerAngles = new Vector3(0, 0, speedRot);
        if (speedText) speedText.text = Mathf.FloorToInt(speed).ToString();

        // 2. RPM Gauge
        float rpm = rccCar.engineRPM / 1000f; 
        float rpmRot = Mathf.Lerp(needleMinRotation, needleMaxRotation, rpm / maxRpm);
        if (rpmNeedle) rpmNeedle.localEulerAngles = new Vector3(0, 0, rpmRot);
        if (rpmText) rpmText.text = rpm.ToString("F1");

        // 3. NOS Gauge
        float nos = rccCar.NoS / 100f; // Assuming NOS max is 100 in RCC
        float nosRot = Mathf.Lerp(needleMinRotation, needleMaxRotation, nos);
        if (nosNeedle) nosNeedle.localEulerAngles = new Vector3(0, 0, nosRot);
        if (nosText) nosText.text = Mathf.FloorToInt(nos * 100).ToString() + "%";
    }

    void UpdateTargeting()
    {
        if (targetSuspect != null && Camera.main != null && targetBracket != null)
        {
            // Position the red bracket exactly over the thief car in the 3D world
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetSuspect.position);
            
            if (screenPos.z > 0) // If suspect is in front of the camera
            {
                targetBracket.gameObject.SetActive(true);
                targetBracket.position = screenPos;
                
                // Calculate and update distance
                float dist = Vector3.Distance(Camera.main.transform.position, targetSuspect.position);
                if (distanceText) distanceText.text = "SUSPECT " + Mathf.FloorToInt(dist) + " m";
            }
            else
            {
                targetBracket.gameObject.SetActive(false); // Hide if behind camera
            }
        }
    }

    // Call this from other scripts to update the Dispatch text
    public void UpdateDispatchMessage(string message)
    {
        if (dispatchText) dispatchText.text = "DISPATCH: " + message;
    }
}
