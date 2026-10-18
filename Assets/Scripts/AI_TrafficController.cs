using UnityEngine;

public class AI_TrafficController : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 10f;
    public float turnSpeed = 8f;
    private int currentWP = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWP];
        Vector3 dir = (target.position - transform.position).normalized;

        // Move
        transform.position += dir * speed * Time.deltaTime;

        // Rotate Smoothly
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);

        // Next waypoint
        if (Vector3.Distance(transform.position, target.position) < 2f)
        {
            currentWP = (currentWP + 1) % waypoints.Length;
        }
    }
}
