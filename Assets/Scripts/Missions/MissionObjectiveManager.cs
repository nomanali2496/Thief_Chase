using UnityEngine;
using TMPro;
using System.Collections;

public enum ObjectiveType
{
    SurviveTime,        // Survive for X seconds without getting busted
    ReachDistance,      // Get X meters away from the closest police car
    ReachWaypoint       // Reach a specific safehouse/waypoint
}

public class MissionObjectiveManager : MonoBehaviour
{
    public static MissionObjectiveManager Instance;

    [Header("Mission Setup")]
    public ObjectiveType currentObjective;
    public float targetValue; // Seconds, Meters, or Distance to Waypoint
    public Transform safehouseWaypoint; // Optional, only used for ReachWaypoint
    
    [Header("References")]
    public Transform thiefCar;
    public Transform[] policeCars;
    public TextMeshProUGUI objectiveTextHUD; // Display to player (Optional)
    
    [Header("State")]
    public bool isMissionActive = false;
    private float timer = 0f;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartMission(ObjectiveType type, float value, Transform waypoint = null)
    {
        currentObjective = type;
        targetValue = value;
        safehouseWaypoint = waypoint;
        timer = 0f;
        isMissionActive = true;
        
        Debug.Log($"[MISSION] Started: {type}. Target: {value}");
        
        if(AI_Dispatcher.Instance != null) AI_Dispatcher.Instance.PlayVoiceLine("Pursuit");
    }

    void Update()
    {
        if(!isMissionActive) return;
        
        timer += Time.deltaTime;

        switch(currentObjective)
        {
            case ObjectiveType.SurviveTime:
                CheckSurviveTime();
                break;
            case ObjectiveType.ReachDistance:
                CheckReachDistance();
                break;
            case ObjectiveType.ReachWaypoint:
                CheckReachWaypoint();
                break;
        }
    }

    void CheckSurviveTime()
    {
        float remaining = targetValue - timer;
        if(objectiveTextHUD) objectiveTextHUD.text = $"SURVIVE: {Mathf.Ceil(remaining)}s";

        if(remaining <= 0)
        {
            MissionSuccess();
        }
    }

    void CheckReachDistance()
    {
        if(thiefCar == null || policeCars == null || policeCars.Length == 0) return;
        
        // Find closest police
        float closestDist = float.MaxValue;
        foreach(var cop in policeCars)
        {
            if(cop == null) continue;
            float d = Vector3.Distance(thiefCar.position, cop.position);
            if(d < closestDist) closestDist = d;
        }

        if(objectiveTextHUD) objectiveTextHUD.text = $"ESCAPE DISTANCE: {Mathf.Floor(closestDist)}m / {targetValue}m";

        if(closestDist >= targetValue)
        {
            MissionSuccess();
        }
    }

    void CheckReachWaypoint()
    {
        if(thiefCar == null || safehouseWaypoint == null) return;
        
        float dist = Vector3.Distance(thiefCar.position, safehouseWaypoint.position);
        if(objectiveTextHUD) objectiveTextHUD.text = $"SAFEHOUSE: {Mathf.Floor(dist)}m";

        if(dist <= targetValue) // usually targetValue = 10f (within 10 meters)
        {
            MissionSuccess();
        }
    }

    public void MissionSuccess()
    {
        isMissionActive = false;
        if(objectiveTextHUD) objectiveTextHUD.text = "OBJECTIVE COMPLETE";
        
        // Police AI says "Target Lost" when you escape
        if(AI_Dispatcher.Instance != null) AI_Dispatcher.Instance.PlayVoiceLine("Lost"); 
        
        // Call their existing Mission Complete UI
        if(MissionCompleteManager.Instance != null)
        {
            MissionCompleteManager.Instance.ShowResults(timer, 1500, 250);
        }
        else
        {
            Debug.Log("Mission Complete! (MissionCompleteManager not found in scene)");
        }
    }
    
    // Call this if the police catch the player
    public void MissionFailed_Busted()
    {
        if(!isMissionActive) return;
        isMissionActive = false;
        
        MissionFailedManager failMgr = FindObjectOfType<MissionFailedManager>(true);
        if(failMgr != null) failMgr.ShowBusted(timer);
    }
    
    // Call this if the player's car is destroyed
    public void MissionFailed_Wasted()
    {
        if(!isMissionActive) return;
        isMissionActive = false;
        
        MissionFailedManager failMgr = FindObjectOfType<MissionFailedManager>(true);
        if(failMgr != null) failMgr.ShowWasted(timer);
    }
}
