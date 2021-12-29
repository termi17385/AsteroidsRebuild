using Asteroids.Managers;
using UnityEngine;

public class UselessScript : MonoBehaviour
{
    [SerializeField] private GameManager _manager; 
    
    private void Start()
    {
        _manager.Play();
        Destroy(gameObject);
    }
}
