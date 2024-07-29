using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _moveSpeed = 4f;

    private int _currentWaypoint = 0;

    private void Update()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
        {
            _currentWaypoint = ++_currentWaypoint % _waypoints.Length;
        }

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _moveSpeed * Time.deltaTime);        
    }
}
