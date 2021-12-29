using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.asteroid;
using Asteroids.Scripts;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 2000;
    private Rigidbody2D rb2d;
    
    private void Setup()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroyObj), 5);
    }
    private void Awake() => Setup();
    private void Update() => MovementUtils.Thrust(speed, rb2d, transform);
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Asteroid")) return;
        if (other.collider.TryGetComponent(out AsteroidController _ast))
            _ast.DestroyAsteroid();
         
        CancelInvoke(nameof(DestroyObj));
        Destroy(gameObject);
    }

    private void DestroyObj() => Destroy(gameObject);
}
