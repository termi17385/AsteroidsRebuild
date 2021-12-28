using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.asteroid
{
	/// <summary> Spawns in a sets up the asteroids assigning their
	/// scripts as well as the direction they are going </summary>
	public class AsteroidSpawner : MonoBehaviour
	{
		[Range(1, 100)] public float radius;
		public static AsteroidSpawner ast_Spawner;
		private GameObject AsteroidObj => Resources.Load<GameObject>("Asteroid");
		
		private void Setup()
		{
			ast_Spawner = this;
		}
		private void Awake() => Setup();

		/// <summary> Spawns and asteroid and sets is up with
		/// the necessary scripts and components </summary>
		/// <param name="_speed">How much speed to apply to the asteroid</param>
		public GameObject SpawnAsteroidAndSetup(float _speed = 200)
		{
			// spawn and rotation
			var spawnPos = Random.insideUnitCircle.normalized * radius;
			var ast = Instantiate(AsteroidObj, spawnPos, Quaternion.identity);
			var rot = Random.rotation; rot.x = rot.y = 0;
			ast.transform.rotation = rot;
			
			// Setting up components
			var rb = ast.GetComponent<Rigidbody2D>();
			var controller = ast.GetComponent<AsteroidController>();
			controller.gameManager = GetComponent<GameManager>();
			
			// movement and returning
			var dir = ast.transform.up;
			rb.AddForce(dir * _speed);
			return ast;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}
}