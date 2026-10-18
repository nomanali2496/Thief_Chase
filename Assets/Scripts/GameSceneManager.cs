using UnityEngine;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    public GameObject thiefCharacter;         // Thief (walking/sprinting)
    public GameObject thiefCar;               // Thief car with RCC
    public GameObject policeCar;              // Police car with RCC
    public Camera cinematicCamera;            // Optional for cutscene

    public float cutsceneDuration = 4.5f;
    [Header("Cinematic Settings")]
    public float startingFOV = 60f;
    public float targetFOV = 40f;

    void Start()
    {
        // Start in cinematic mode
        if (thiefCharacter != null) thiefCharacter.SetActive(true);

        if (thiefCar != null)
        {
            var rcc = thiefCar.GetComponent<RCC_CarControllerV3>();
            if (rcc) rcc.enabled = false;
        }

        if (policeCar != null)
        {
            var rcc = policeCar.GetComponent<RCC_CarControllerV3>();
            if (rcc) rcc.enabled = false;
        }

        if (cinematicCamera != null)
            cinematicCamera.enabled = true;

        StartCoroutine(StartChaseSequence());
    }

    IEnumerator StartChaseSequence()
    {
        float timer = 0f;
        Vector3 startCamPos = (cinematicCamera != null) ? cinematicCamera.transform.position : Vector3.zero;

        // Smooth camera panning, tracking, and FOV zoom during Robbery cutscene
        if (cinematicCamera != null) cinematicCamera.fieldOfView = startingFOV;

        while (timer < cutsceneDuration)
        {
            timer += Time.deltaTime;
            if (cinematicCamera != null && thiefCar != null)
            {
                // Smoothly orbit/pan camera towards the getaway vehicle
                Vector3 targetCamPos = thiefCar.transform.position + thiefCar.transform.forward * -6f + Vector3.up * 2.5f;
                cinematicCamera.transform.position = Vector3.Lerp(startCamPos, targetCamPos, timer / cutsceneDuration);
                cinematicCamera.transform.LookAt(thiefCar.transform.position + Vector3.up * 1.2f);
                
                // Cinematic Zoom Effect (Jolt)
                cinematicCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, (timer / cutsceneDuration) * (timer / cutsceneDuration)); // Quadratic easing for a snap effect

            }
            yield return null;
        }

        // Disable cinematic camera and enable active chase
        if (cinematicCamera != null)
            cinematicCamera.enabled = false;

        if (thiefCharacter != null)
            thiefCharacter.SetActive(false);

        if (thiefCar != null)
        {
            var rcc = thiefCar.GetComponent<RCC_CarControllerV3>();
            if (rcc) rcc.enabled = true;
        }

        if (policeCar != null)
        {
            var rcc = policeCar.GetComponent<RCC_CarControllerV3>();
            if (rcc) rcc.enabled = true;
        }

        Debug.Log("🎬 Robbery Cutscene Complete! Police Chase Started.");
    }
}
