using Asteroids.Managers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Asteroids.asteroid
{
    public class AsteroidController : MonoBehaviour
    {
        public GameManager gameManager;
        public bool largeAsteroid = true;
        
        public GameObject DestroyAsteroid()
        {
            if(gameManager != null) gameManager.OnDeath(DeathType.Asteroid);

            if (largeAsteroid)
            {
                for (int i = 0; i < 2; i++)
                {
                    var resourceObj = Resources.Load<GameObject>("Asteroid");
                    var spawnedObj = Instantiate(resourceObj, transform.position, Quaternion.identity);
                    var newRotation = Random.rotation;
                    newRotation.x = newRotation.y = 0;
                    
                    spawnedObj.transform.rotation = newRotation;
                    if (gameManager != null)
                    {
                        spawnedObj.GetComponent<AsteroidController>().gameManager = this.gameManager;
                        spawnedObj.transform.SetParent(gameManager.transform);
                    }
                    spawnedObj.transform.localScale = transform.localScale / 2;
                    
                    spawnedObj.GetComponent<AsteroidController>().largeAsteroid = false;
                    spawnedObj.GetComponent<Rigidbody2D>().AddForce(spawnedObj.transform.up * 250);
                }
            }

            var effect = DeathEffects();
            return effect;
        }

        private GameObject DeathEffects()
        {
            var effect = Resources.Load<GameObject>("AsteroidExplosionEffect");
            Instantiate(effect, transform.position, Quaternion.identity);
            
            Invoke(nameof(Disable), .05f);
            return effect;
        }

        private void Disable() => gameObject.SetActive(false);
        private void OnDisable() => Destroy(this.gameObject);
    }
}