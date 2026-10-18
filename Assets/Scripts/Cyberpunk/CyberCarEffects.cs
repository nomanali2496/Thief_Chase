using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CyberCarEffects : MonoBehaviour
{
    private RCC_CarControllerV3 rcc;
    
    [Header("Neon Underglow")]
    public Light underglowLight;
    public Color neonColor = Color.cyan;
    
    [Header("Nitro Effects")]
    public TrailRenderer[] lightTrails;
    public ParticleSystem nitroParticles;
    
    [Header("Camera FX (Requires Volume in Scene)")]
    public Camera mainCamera;
    public float normalFOV = 60f;
    public float nitroFOV = 75f;
    public float fovSpeed = 5f;
    
    private Volume globalVolume;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        rcc = GetComponent<RCC_CarControllerV3>();
        if(mainCamera == null) mainCamera = Camera.main;
        
        // Setup underglow dynamically if not assigned
        if(underglowLight == null) {
            GameObject lightObj = new GameObject("Underglow");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            underglowLight = lightObj.AddComponent<Light>();
            underglowLight.type = LightType.Point;
            underglowLight.range = 5f;
            underglowLight.intensity = 3f;
        }
        underglowLight.color = neonColor;
        
        // Find URP Volume in the scene to modify Chromatic Aberration
        globalVolume = FindObjectOfType<Volume>();
        if(globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out chromaticAberration);
        }
    }

    void Update()
    {
        if(rcc == null) return;
        
        bool isNitro = rcc.useNOS;
        
        // Activate Light Trails
        if (lightTrails != null)
        {
            foreach(var trail in lightTrails) {
                if (trail != null) trail.emitting = isNitro;
            }
        }
        
        // Activate Nitro Particles
        if(nitroParticles != null) {
            if(isNitro && !nitroParticles.isPlaying) nitroParticles.Play();
            else if(!isNitro && nitroParticles.isPlaying) nitroParticles.Stop();
        }
        
        // Camera FOV Expansion on Nitro
        if(mainCamera != null) {
            float targetFOV = isNitro ? nitroFOV : normalFOV;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovSpeed);
        }
        
        // Distortion (Chromatic Aberration) on Nitro
        if(chromaticAberration != null) {
            float targetIntensity = isNitro ? 1f : 0.1f;
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, targetIntensity, Time.deltaTime * fovSpeed);
        }
    }
}
