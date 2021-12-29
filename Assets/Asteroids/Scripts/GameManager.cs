using Random = UnityEngine.Random;
using Asteroids.asteroid;
using UnityEngine;
using System;
using Asteroids.Player;
using UnityEngine.SceneManagement;

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
        public bool gameover = false;
        public int spawnAmount = 1;
        public int asteroidCount = 0;
        public bool testInProgress = false;

        private void Awake()
        {
            asteroidCount = 0;
            spawnAmount = 1;
        }

        public void Play()
        {
            var player = Resources.Load<GameObject>("Prefabs/Player");
            Instantiate(player, this.transform);
            
            player.transform.position = Vector3.zero;
            player.GetComponent<PlayerController>().gameManager = gameObject.GetComponent<GameManager>();
            StartRound(spawnAmount);
        }

        public GameObject StartRound(int _count)
        {
            GameObject ast = null;
            for (int i = 0; i < _count; i++)
            {
                ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
                ast.GetComponent<AsteroidController>().gameManager = this;
            }
            
            asteroidCount += (_count * 3);
            spawnAmount++;
            return ast;
        }

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
            gameover = true;
            if(!testInProgress)SceneManager.LoadScene("MainMenu");
        }

        private void OnAsteroidDeath()
        {
            Debug.Log("Asteroid Death");
            asteroidCount--;
            if(!testInProgress)if(asteroidCount <= 0) StartRound(spawnAmount);
        }
    }
}