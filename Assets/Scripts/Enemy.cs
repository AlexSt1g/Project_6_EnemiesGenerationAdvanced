using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;    
    [SerializeField] private float _lifetime = 8f;

    private Target _target;
    private Vector3 _startPosition;

    public event Action<Enemy> Died;

    public Vector3 StartPosition => _startPosition;    

    public void Initialize(Target target, Vector3 spawnPointPosition)
    {
        _target = target;
        _startPosition = spawnPointPosition;
    }    

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Target>())
        {
            StartCoroutine(WaitLifetime(_lifetime));
        }
    }

    private IEnumerator WaitLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Died?.Invoke(this);
    }
}
