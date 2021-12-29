using System.Collections.Generic;
using UnityEngine.TestTools;
using System.Collections;
using Asteroids.asteroid;
using Asteroids.Managers;
using NUnit.Framework;
using UnityEngine;

public class AsteroidsTest
{
    private GameObject game;
    
    [SetUp]
    public void Setup()
    { 
        game = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Game"), Vector3.zero, Quaternion.identity);
        
    }
  
    [UnityTest]
    public IEnumerator AsteroidSpawned()
    {
        for (int i = 0; i < 5; i++)
        {
            var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
            #region Parenting Object
            ast.transform.SetParent(game.transform);
            var transformPosition = ast.transform.position;
            transformPosition.z = 0; ast.transform.position = transformPosition;
            #endregion
            UnityEngine.Assertions.Assert.IsNotNull(ast);
        }
        yield return new WaitForSeconds(4);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator AsteroidMovement()
    {
        var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
        var initialPos = ast.transform.position;
        #region Parenting Object
        ast.transform.SetParent(game.transform);
        var transformPosition = ast.transform.position;
        transformPosition.z = 0; ast.transform.position = transformPosition;
        #endregion

        yield return new WaitForSeconds(5);
        UnityEngine.Assertions.Assert.AreNotEqual(initialPos, ast.transform.position);
    }

    [UnityTest]
    public IEnumerator AsteroidDeath()
    {
        var ast = AsteroidSpawner.ast_Spawner.SpawnAsteroidAndSetup();
        
        #region Parenting Object
        ast.transform.SetParent(game.transform);
        var transformPosition = ast.transform.position;
        transformPosition.z = 0; ast.transform.position = transformPosition;
        #endregion
        
        yield return new WaitForSeconds(2);
        var effect = ast.GetComponent<AsteroidController>().DestroyAsteroid();

        var check1 = false;
        var check2 = false;

        yield return new WaitForSeconds(15);

        check1 = ast == null;
        check2 = effect != null;

        
        UnityEngine.Assertions.Assert.IsTrue(check1 && check2);
        yield return null;
    }
    
    [TearDown]
    public void TearDown() => Object.Destroy(game);
}
