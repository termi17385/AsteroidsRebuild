using System;
using Asteroids.asteroid;
using Asteroids.Managers;
using Asteroids.Scripts;
using UnityEngine;

namespace Asteroids.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform gun;
        public GameManager gameManager;
        public bool godMode = false;
        
        private Rigidbody2D rb2d;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.W)) Thrust(100);
            if(Input.GetKeyDown(KeyCode.Space)) ShootWeapon();
            if(Input.GetKey(KeyCode.D)) RotatePlayer((500) * Time.deltaTime);
            if(Input.GetKey(KeyCode.A)) RotatePlayer(-(500) * Time.deltaTime);
        }

        /// <summary> Spawns a bullet shooting in
        /// the direction of the ships up </summary>
        public void ShootWeapon()
        {
            var bullet = Resources.Load<GameObject>("Prefabs/Bullet");
            Instantiate(bullet, gun.position, gun.rotation);
        }

        /// <summary> Handles rotating
        /// the player </summary>
        /// <param name="_dir">can be positive or
        /// negative speed to change directions</param>
        public void RotatePlayer(float _dir)
        {
            var rotation = Vector3.zero;
            rotation.z -= _dir;
            transform.Rotate(rotation);
        }

        /// <summary> Handles the flight
        /// of the ship </summary>
        public void Thrust(float _speed) => MovementUtils.Thrust(_speed, rb2d, transform);
        private void OnCollisionEnter2D(Collision2D other)
        {
            if(godMode) return;
            
            if (!other.gameObject.CompareTag("Asteroid")) return;
            if (other.collider.TryGetComponent(out AsteroidController asteroidController))
                asteroidController.DestroyAsteroid();
            
            if(gameManager != null) gameManager.OnDeath(DeathType.Player);
            gameObject.SetActive(false);
        }
    }
}