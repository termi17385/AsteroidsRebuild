using Assert = UnityEngine.Assertions.Assert;
using UnityEngine.TestTools;
using System.Collections;
using Asteroids.asteroid;
using Asteroids.Managers;
using Asteroids.Player;
using NUnit.Framework;
using UnityEngine;

public class AsteroidsTest
{
    private GameObject game;
    private GameManager gameManager;
    
    [SetUp] public void Setup()
    { 
        // Used to set up the game and game manager for testing
        game = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"), Vector3.zero, Quaternion.identity);
        gameManager = game.GetComponentInChildren<GameManager>();
        gameManager.testInProgress = true;
    }
    [UnityTest] public IEnumerator AsteroidSpawned()
    {
        // spawns a set amount of asteroids and checks to see if
        // any return null if 1 asteroid returns null we will be notified
        for (int i = 0; i < 5; i++)
        {
            var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
            #region Parenting Object
            ast.transform.SetParent(game.transform);
            var transformPosition = ast.transform.position;
            transformPosition.z = 0; ast.transform.position = transformPosition;
            #endregion // purely used for handling parenting between the objects
            UnityEngine.Assertions.Assert.IsNotNull(ast);
        }
        yield return new WaitForSeconds(4);
        yield return null;
    }
    [UnityTest] public IEnumerator AsteroidMovement()
    {
        // Spawns an asteroid
        var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
        // logs the initial position when it spawns in
        var initialPos = ast.transform.position;
        #region Parenting Object
        ast.transform.SetParent(game.transform);
        var transformPosition = ast.transform.position;
        transformPosition.z = 0; ast.transform.position = transformPosition;
        #endregion

        yield return new WaitForSeconds(5);
        // checks if the asteroid has moved
        UnityEngine.Assertions.Assert.AreNotEqual(initialPos, ast.transform.position);
    }
    [UnityTest] public IEnumerator AsteroidDeath()
    {
        // spawns the needed objects for testing
        var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
        var player = Resources.Load<GameObject>("Prefabs/Player");
        var playerOBJ = MonoBehaviour.Instantiate(player, game.transform);
        
        // sets the players position
        playerOBJ.transform.position = Vector3.zero;
        
        #region Setting up objects
        // makes sure nothing is moving and sets parents 
        ast.transform.SetParent(game.transform);
        playerOBJ.transform.SetParent(game.transform);

        yield return new WaitForSeconds(1);
        // resets all positions
        var transformPosition = ast.transform.position;
        
        transformPosition.x = 0;
        transformPosition.y = 10;
        transformPosition.z = 0;
        
        ast.transform.position = transformPosition;
        // freezes asteroid
        ast.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);

        //resets all positions
        var transformPlayer = playerOBJ.transform.position;
        
        transformPlayer.x = 0;
        transformPlayer.y = -2;
        transformPlayer.z = 0;
        
        playerOBJ.transform.position = transformPlayer;
        #endregion
        
        playerOBJ.GetComponent<PlayerController>().ShootWeapon();
        yield return new WaitForSeconds(5);
        Assert.IsNull(ast); // checks if the asteroid was destroyed
        yield return null;
    }
    [UnityTest] public IEnumerator RoundsTesting()
    {
        gameManager.testInProgress = false;
        // spawns the needed objects
        var ast = gameManager.StartRound(gameManager.spawnAmount);
        var player = Resources.Load<GameObject>("Prefabs/Player");
        var playerOBJ = MonoBehaviour.Instantiate(player, game.transform);
        playerOBJ.GetComponent<PlayerController>().godMode = true;
        
        playerOBJ.transform.position = Vector3.zero;
        
        #region Setting up objects
        ast.transform.SetParent(game.transform);
        playerOBJ.transform.SetParent(game.transform);

        yield return new WaitForSeconds(1);
        var transformPosition = ast.transform.position;
        
        transformPosition.x = 0;
        transformPosition.y = 10;
        transformPosition.z = 0;
        
        ast.transform.position = transformPosition;
        ast.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);

        var transformPlayer = playerOBJ.transform.position;
        
        transformPlayer.x = 0;
        transformPlayer.y = -2;
        transformPlayer.z = 0;
        
        playerOBJ.transform.position = transformPlayer;
        #endregion
        
        playerOBJ.GetComponent<PlayerController>().ShootWeapon();
        yield return new WaitForSeconds(2);
        var remainingAsteroids = GameObject.FindObjectsOfType<AsteroidController>();
        
        var index = 10;
        // any remaining asteroids from the large ones
        // destruction are repositioned so the testing can continue
        foreach (var t in remainingAsteroids)
        {
            var transform = t.transform;
            var pos = transform.position;
            t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            pos.x = 0;
            pos.y = index;
            pos.z = 0;
            
            transform.position = pos;
            index += 5;
        }
        
        yield return new WaitForSeconds(1);
        playerOBJ.GetComponent<PlayerController>().ShootWeapon();
        yield return new WaitForSeconds(2);

        playerOBJ.GetComponent<PlayerController>().ShootWeapon();
        yield return new WaitForSeconds(10);
        var newAsteroids = GameObject.FindObjectsOfType<AsteroidController>();
        
        // checks if the round successful changed
        Assert.AreEqual(6, gameManager.asteroidCount);
        Assert.AreEqual((gameManager.spawnAmount - 1), newAsteroids.Length); // minus 1 because the spawn amount already predicts the next wave and we want the current wave
        yield return null;
    }
    [UnityTest] public IEnumerator PlayerDeath()
    {
        // same as the asteroid death with one difference
        
        var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
        var player = Resources.Load<GameObject>("Prefabs/Player");
        var playerOBJ = MonoBehaviour.Instantiate(player, game.transform);
        playerOBJ.transform.position = Vector3.zero;
        #region Setting up objects
        ast.transform.SetParent(game.transform);
        playerOBJ.transform.SetParent(game.transform);
        playerOBJ.GetComponent<PlayerController>().gameManager = this.gameManager;

        yield return new WaitForSeconds(1);
        var transformPosition = ast.transform.position;
        var rotation = ast.transform.rotation;
        
        transformPosition.x = 0;
        transformPosition.y = 10;
        transformPosition.z = 0;
        
        ast.transform.position = transformPosition;
        ast.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);

        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0;

        ast.transform.rotation = rotation;
        
        var transformPlayer = playerOBJ.transform.position;
        
        transformPlayer.x = 0;
        transformPlayer.y = -2;
        transformPlayer.z = 0;
        
        playerOBJ.transform.position = transformPlayer;
        #endregion
        
        // the player doesnt shoot and instead the asteroid moves down towards the player
        ast.GetComponent<Rigidbody2D>().AddForce(ast.transform.up * -250);
        yield return new WaitForSeconds(10);
        Assert.IsTrue(!playerOBJ.activeSelf);
        Assert.IsTrue(gameManager.gameover);
        Assert.IsNull(ast);
        yield return null;
    }
    [UnityTest] public IEnumerator PlayerMovement()
    {
        #region Setting Up
        var player = Resources.Load<GameObject>("Prefabs/Player");
        var playerOBJ = MonoBehaviour.Instantiate(player, game.transform);
        playerOBJ.transform.position = Vector3.zero;
        
        var right = -90;
        var left = 90;
        
        var testRotLeft = false;
        var testRotRight = false;
        #endregion
        while (playerOBJ.transform.position.y < 18)
        {
            if (playerOBJ.transform.position.y >= 18)
            {
                Assert.AreEqual(playerOBJ.transform.position.y, 18);
                break;
            }
            
            playerOBJ.GetComponent<PlayerController>().Thrust(50);
            yield return null;
        }
        
        #region misc
        playerOBJ.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerOBJ.transform.position = Vector3.zero;
        yield return new WaitForSeconds(2);
        #endregion
        
        while (playerOBJ.GetComponent<Rigidbody2D>().rotation > -95)
        {
            if (playerOBJ.GetComponent<Rigidbody2D>().rotation <= right)
            {
                testRotRight = true;
                Debug.Log($"rotationCheck1: {right}");
                break;
            }
            
            Debug.Log(playerOBJ.GetComponent<Rigidbody2D>().rotation);
            playerOBJ.GetComponent<PlayerController>().RotatePlayer((5 * 10) * Time.deltaTime);
            yield return null;
        }
        Assert.IsTrue(testRotRight);

        while (playerOBJ.GetComponent<Rigidbody2D>().rotation < 95)
        {
            if (playerOBJ.GetComponent<Rigidbody2D>().rotation >= left)
            {
                testRotLeft = true;
                Debug.Log($"rotationCheck2: {left}");
                break;
            }
            
            Debug.Log(playerOBJ.GetComponent<Rigidbody2D>().rotation);
            playerOBJ.GetComponent<PlayerController>().RotatePlayer(-(5 * 10) * Time.deltaTime);
            yield return null;
        }
        Assert.IsTrue(testRotLeft);
    }
    [TearDown] public void TearDown() => Object.Destroy(game);
}
