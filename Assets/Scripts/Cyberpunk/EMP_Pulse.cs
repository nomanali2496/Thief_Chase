using UnityEngine;
using System.Collections;

public class EMP_Pulse : MonoBehaviour
{
    [Header("EMP Settings")]
    public float empRadius = 30f;
    public float slowDuration = 3f;
    public float cooldown = 15f;
    public KeyCode empKey = KeyCode.E;
    
    [Header("Visuals")]
    public ParticleSystem empShockwave;
    
    private bool isCooldown = false;

    void Update()
    {
        if(Input.GetKeyDown(empKey) && !isCooldown)
        {
            FireEMP();
        }
    }

    void FireEMP()
    {
        isCooldown = true;
        if(empShockwave) empShockwave.Play();
        
        Collider[] hits = Physics.OverlapSphere(transform.position, empRadius);
        foreach(var hit in hits)
        {
            // Find RCC car within the radius
            RCC_CarControllerV3 targetCar = hit.GetComponentInParent<RCC_CarControllerV3>();
            
            // Apply EMP only to other cars, not our own
            if(targetCar != null && targetCar.gameObject != this.gameObject)
            {
                StartCoroutine(SlowDownCar(targetCar));
            }
        }
        
        StartCoroutine(CooldownRoutine());
    }
    
    IEnumerator SlowDownCar(RCC_CarControllerV3 car)
    {
        // Save original stats
        float originalTorque = car.maxEngineTorque;
        float originalSpeed = car.maxspeed;
        
        // Apply EMP slow (Reduce speed and torque)
        car.maxEngineTorque = originalTorque * 0.3f; // 70% reduction
        car.maxspeed = originalSpeed * 0.5f;       // 50% reduction
        
        Debug.Log($"[EMP] {car.gameObject.name} is slowed down! Systems glitching.");
        
        yield return new WaitForSeconds(slowDuration);
        
        // Restore stats
        car.maxEngineTorque = originalTorque;
        car.maxspeed = originalSpeed;
        
        Debug.Log($"[EMP] {car.gameObject.name} recovered from EMP.");
    }
    
    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
        Debug.Log("[EMP] Ready to fire again.");
    }
}
