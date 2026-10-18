using UnityEngine;

public class DistanceUI : MonoBehaviour
{
    public Transform policeCar;
    public Transform thiefCar;
    public TextMesh distanceText;
    public TMPro.TextMeshProUGUI distanceTextTMP;
    
    [Header("Thrilling Tension Settings")]
    public float tensionDistanceThreshold = 30f;
    public float maxCameraShake = 0.3f;
    public Color normalColor = Color.white;
    public Color tensionColor = Color.red;
    
    [Header("Gemini AI Integration")]
    public GeminiManager geminiManager;
    public float geminiUpdateInterval = 20f; // Request dispatch every 20 seconds
    private float geminiTimer = 0f;

    private Vector3 originalCamPos;

    void Update()
    {
        if (policeCar == null)
        {
            if (RCC_SceneManager.Instance != null && RCC_SceneManager.Instance.activePlayerVehicle != null)
            {
                policeCar = RCC_SceneManager.Instance.activePlayerVehicle.transform;
            }
            else
            {
                GameObject p = GameObject.FindWithTag("Player") ?? GameObject.Find("Police");
                if (p != null) policeCar = p.transform;
            }
        }

        if (thiefCar == null)
        {
            GameObject t = GameObject.FindWithTag("Thief") ?? GameObject.Find("Thief");
            if (t == null)
            {
                CatchOnImpact cop = FindFirstObjectByType<CatchOnImpact>();
                RCC_AICarController ai = FindFirstObjectByType<RCC_AICarController>();
                if (ai != null && (cop == null || ai.gameObject != cop.gameObject)) t = ai.gameObject;
            }
            if (t != null) thiefCar = t.transform;
        }

        if (policeCar == null || thiefCar == null)
            return;

        float distance = Vector3.Distance(policeCar.position, thiefCar.position);
        string distStr = distance.ToString("F0") + "m";

        // Tension System - UI Pulse
        Color currentColor = distance < tensionDistanceThreshold ? Color.Lerp(normalColor, tensionColor, Mathf.PingPong(Time.time * 2f, 1f)) : normalColor;

        if (distanceText != null)
        {
            distanceText.text = distStr;
            distanceText.color = currentColor;
        }
        if (distanceTextTMP != null)
        {
            distanceTextTMP.text = distStr;
            distanceTextTMP.color = currentColor;
        }

        // Always face camera if it exists
        Camera cam = Camera.main ?? FindFirstObjectByType<Camera>();
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
            
            // Tension System - Camera Shake
            if (distance < tensionDistanceThreshold)
            {
                float shakeIntensity = (1f - (distance / tensionDistanceThreshold)) * maxCameraShake;
                cam.transform.localPosition += Random.insideUnitSphere * shakeIntensity * Time.deltaTime;
            }
        }
        
        // Gemini AI Hook
        if (geminiManager != null)
        {
            geminiTimer += Time.deltaTime;
            if (geminiTimer >= geminiUpdateInterval)
            {
                geminiTimer = 0f;
                // Get approx speed of police car (assuming rigidBody is attached)
                float speed = 0f;
                Rigidbody rb = policeCar.GetComponent<Rigidbody>();
                if (rb != null) speed = rb.linearVelocity.magnitude * 2.23694f; // m/s to mph
                
                string locationContext = (distance < tensionDistanceThreshold) ? "Close Intercept" : "Hot Pursuit";
                geminiManager.RequestDynamicChatter(speed, locationContext, 4);
            }
        }
    }
}
