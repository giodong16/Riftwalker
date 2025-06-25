using UnityEngine;

public class BasicPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public bool loop = true; // Nếu false, enemy sẽ quay đầu ngược lại

    private int currentIndex = 0;
    private bool forward = true;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform targetPoint = waypoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            GetNextIndex();
            FlipTowards(waypoints[currentIndex].position);
        }
    }

    void GetNextIndex()
    {
        if (loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else
        {
            if (forward)
            {
                currentIndex++;
                if (currentIndex >= waypoints.Length)
                {
                    currentIndex = waypoints.Length - 2;
                    forward = false;
                }
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = 1;
                    forward = true;
                }
            }
        }
    }

    void FlipTowards(Vector3 target)
    {
        Vector3 scale = transform.localScale;
        scale.x = target.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (loop)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
}
