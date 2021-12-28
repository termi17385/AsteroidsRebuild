using Random = UnityEngine.Random;
using Asteroids.asteroid;
using UnityEngine;
using System;

namespace Asteroids.Managers
{
    public enum DeathType
    {
        Player,
        Asteroid
    }
    
    [RequireComponent(typeof(AsteroidSpawner))]
    public class GameManager : MonoBehaviour
    {
        public void OnDeath(DeathType _death)
        {
            switch (_death)
            {
                case DeathType.Asteroid: OnAsteroidDeath(); break;
                case DeathType.Player: OnPlayerDeath(); break;
                default: throw new ArgumentOutOfRangeException(nameof(_death), _death, null);
            }
        }

        private void OnPlayerDeath()
        {
            Debug.Log("Player Death");
        }

        private void OnAsteroidDeath()
        {
            Debug.Log("Asteroid Death"); 
        }
    }
}