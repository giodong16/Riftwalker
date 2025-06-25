using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform[] wayPoints;
    
    int currentIndex = 0;

    private void Update()
    {
        if(wayPoints.Length == 0) return;
        Vector2 direction = wayPoints[currentIndex].position - transform.position;
        RotateTowards(direction);
        Transform target = wayPoints[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed* Time.deltaTime);
        if(Vector2.Distance(transform.position, target.position) < 0.02f)
        {
            
            currentIndex = (currentIndex+1)%wayPoints.Length;
            direction = wayPoints[currentIndex].position - transform.position;
            RotateTowards(direction);
        }
    }

    public void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform point in wayPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.05f);
        }
    }

}
