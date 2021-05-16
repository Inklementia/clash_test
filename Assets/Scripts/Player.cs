using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Transform _target;
    private Rigidbody _rb;
  

    private void Start()
    {
        EventManager.Instance.deathEvent.Invoke();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.Instance.deathEvent += OnDestroyEnemy;
    }

    private void OnDisable()
    {
        EventManager.Instance.deathEvent -= OnDestroyEnemy;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            var direction = (_target.position - transform.position).normalized;
            _rb.MovePosition(transform.position + direction * (speed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
        
            EventManager.Instance.deathEvent.Invoke();
        }
    }
        private Transform FindTarget()
    {
        var distanceToClosestEnemy = Mathf.Infinity;
        Transform closestEnemy = null;
        var allEnemies = FindObjectsOfType<Enemy>();

        foreach (var currentEnemy in allEnemies)
        {
            var distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy.transform;
            }
        }

        return closestEnemy;
    }

    void OnDestroyEnemy()
    {
        _target = FindTarget();
    }

}