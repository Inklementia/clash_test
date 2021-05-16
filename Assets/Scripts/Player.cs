using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Transform _target;
    private Rigidbody _rb;
    private bool _isDestroyed;

    private void Awake()
    {
        _target = FindTarget();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isDestroyed)
        {
            _target = FindTarget();
        }
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
            //other.collider.gameObject.tag = "Untagged"; 
            other.gameObject.SetActive(false);
        }
        _isDestroyed = true;
    }

    private Transform FindTarget()
    {
        var candidates = GameObject.FindGameObjectsWithTag("Enemy");
        var minDistance = Mathf.Infinity;
        Transform closest;

        if (candidates.Length == 0)
            return null;

        closest = candidates[0].transform;
        for (var i = 1; i < candidates.Length; i++)
        {
            var distance = (candidates[i].transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                closest = candidates[i].transform;
                minDistance = distance;
            }
        }

        return closest;
    }
}