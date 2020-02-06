using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ModularWorldGenerator : MonoBehaviour
{
	public Module[] Modules;
	public Module StartModule;
    public GameObject player;
    public GameObject enemy;


    private Vector3 playerSpawnPoint;

    public NavMeshSurface baker;

    public int Iterations = 5;


    private List<Vector3> enemySpawnPoints;


	void Start()
	{
        enemySpawnPoints = new List<Vector3>();
        GenerateDungeon();


        //spawn player
        player.transform.position = playerSpawnPoint;
        player.SetActive(true);

        baker.BuildNavMesh();
        EnemySpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>();
        CheckEnemySpawn(spawnPoints);
        SpawnEnemies();

        //bake navigation mesh for AI
          

    }


    private void GenerateDungeon()
    {
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<ModuleConnector>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(Modules, newTag);
                var newModule = Instantiate(newModulePrefab);
                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);
                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
            }

            pendingExits = newExits;
        }
        playerSpawnPoint = startModule.GetSpawnPoint().gameObject.transform.position;
    }


	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}

    private void CheckEnemySpawn(EnemySpawnPoint[] spawnPoints)
    {
        foreach (var p in spawnPoints)
        {
            Debug.Log(p.gameObject.ToString());
                enemySpawnPoints.Add(p.transform.position);
        }
        
    }

    private void SpawnEnemies()
    {
        foreach(var p in enemySpawnPoints)
        {
            GameObject enemigo = Instantiate(enemy);
            enemigo.transform.position = p;
            enemigo.SetActive(true);
        }
    }
}
