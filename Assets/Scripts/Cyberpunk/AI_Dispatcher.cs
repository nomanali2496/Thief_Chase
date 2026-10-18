using UnityEngine;

public class AI_Dispatcher : MonoBehaviour
{
    public static AI_Dispatcher Instance;
    
    [Header("Audio Source")]
    public AudioSource radioSource;
    
    [Header("Robotic AI Voice Clips")]
    public AudioClip targetLockedClip;
    public AudioClip pursuitActiveClip;
    public AudioClip targetLostClip;
    public AudioClip neutralizedClip;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if(radioSource == null) 
        {
            radioSource = gameObject.AddComponent<AudioSource>();
            radioSource.spatialBlend = 0f; // 2D Sound for HUD
            radioSource.volume = 0.85f;
        }
    }

    public void PlayVoiceLine(string eventName)
    {
        AudioClip clipToPlay = null;
        switch(eventName)
        {
            case "Locked": clipToPlay = targetLockedClip; break;
            case "Pursuit": clipToPlay = pursuitActiveClip; break;
            case "Lost": clipToPlay = targetLostClip; break;
            case "Neutralized": clipToPlay = neutralizedClip; break;
        }
        
        if(clipToPlay != null)
        {
            // Apply slight pitch distortion for robotic glitchy effect
            radioSource.pitch = Random.Range(0.92f, 1.05f);
            radioSource.PlayOneShot(clipToPlay);
            
            Debug.Log($"[AI DISPATCH]: Playing '{eventName}' voice line.");
        }
    }
}
