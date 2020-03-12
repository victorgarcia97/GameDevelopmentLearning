using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class ModularWorldGenerator : MonoBehaviour
{
    public Module[] Modules;

    private List<ModuleConnector> pendingExits;
    public Module StartModule;
    public Module endModule;

    public GameObject player;
    public GameObject enemy;


    private Vector3 playerSpawnPoint;

    public NavMeshSurface baker;

    public int Iterations = 5;


    private List<Vector3> enemySpawnPoints;

    public LayerMask roomLayerMask;


    void Start()
    {
        //done = true;
        enemySpawnPoints = new List<Vector3>();
        //GenerateDungeon();

        StartCoroutine("Generate");


        //spawn player
        //player.transform.position = playerSpawnPoint;
        //player.SetActive(true);

        //bake navigation mesh for AI
        //baker.BuildNavMesh();
        //Spawn enemies in all the spawnpoints in the map
        //EnemySpawnPoint[] spawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>();
        //CheckEnemySpawn(spawnPoints);
        //SpawnEnemies();
    }



    IEnumerator Generate()
    {
        WaitForSecondsRealtime startup = new WaitForSecondsRealtime(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        //place start room 
        PlaceStartRoom();
        yield return interval;

        var r = 20;
        //places rooms
        for(int iteration = 0; iteration < Iterations; iteration++)
        {
            List<ModuleConnector> newExits = new List<ModuleConnector>();
            yield return interval;
            foreach (ModuleConnector pendingExit in pendingExits)
            {
                PlaceRoom(r, pendingExit, newExits);
                yield return interval;
                r += 10;
            }
            pendingExits = newExits;
            yield return interval;
        }

        //place ends
        PlaceEndRooms();
        yield return interval;

        yield return new WaitForSeconds(3);
        yield return 0;

    }


    private void PlaceStartRoom()
    {
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        playerSpawnPoint = startModule.GetSpawnPoint().gameObject.transform.position;
        pendingExits = new List<ModuleConnector>(startModule.GetExits());
    }

    private void PlaceRoom(int r, ModuleConnector pendingExit, List<ModuleConnector> newExits)
    {
            GameObject[] gameobjectsMeshes = GameObject.FindGameObjectsWithTag("Generation");
       
            var newTag = GetRandom(pendingExit.Tags);
            var newModulePrefab = GetRandomWithTag(Modules, newTag);

            Module newModule = Instantiate(newModulePrefab,new Vector3(r,r,r),new Quaternion());

            newModule.gameObject.name = ""+ r +"";
            var newModuleExits = newModule.GetExits();
            var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);

        if (CheckRoomOverlap(newModule)==true)
        {
            ResetGenerate(); 
        }
        else
        {
            
        }

            newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));

    }


    public bool CheckRoomOverlap(Module room)
    {
        Vector3 center = room.transform.position;
        Vector3 extents = room.transform.localScale;
        

        Collider[] colliders = Physics.OverlapBox(center, extents / 2, room.transform.rotation, roomLayerMask);

        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                if (c.transform.parent.gameObject.Equals(room.gameObject))
                {
                    continue;
                }
                else
                {
                    Debug.Log("overlap");
                    Debug.Log(center);
                    Debug.Log(extents/2);
                    Debug.Log(c.transform.parent.gameObject.name);
                    Debug.Log(room.gameObject.name);
                    return true;
                }
            }
        }
        return false;
    }

    public bool CollisionIntersect(Module room, GameObject[] gameObjectsMeshes)
    {
        Debug.Log("Nueva Comprobacion");
        Bounds boundsToTest = room.gameObject.GetComponentInChildren<MeshCollider>().bounds;
        boundsToTest.Expand(-0.1f);
        Debug.Log(room.gameObject.name);
        Debug.Log(room.GetComponentInChildren<MeshCollider>().gameObject);
        Debug.Log(room.transform.position);
        Debug.Log(boundsToTest);
        Debug.Log("######################");

        foreach (GameObject g in gameObjectsMeshes)
        {
            Bounds b = g.GetComponentInChildren<MeshCollider>().bounds;
           
            if (boundsToTest.Intersects(b))
            {
                Debug.Log(g.name);
                Debug.Log(b);
                Debug.Log("#####################");
                Debug.Log("Overlap");
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }

    private void PlaceEndRooms()
    {
        foreach (var pendingExit in pendingExits)
        {
            var newModule = Instantiate(endModule);
            var newModuleExits = newModule.GetExits();
            var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);
        }
    }

    private void ResetGenerate()
    {
        StopCoroutine("Generate");

        //destroy
        GameObject[] gs = GameObject.FindGameObjectsWithTag("Generation");
        foreach(GameObject g in gs)
        {
            Destroy(g);
        }
        pendingExits.Clear();
        


        StartCoroutine("Generate");
    }


/**private void GenerateDungeon()
    {
        done = true;
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        playerSpawnPoint = startModule.GetSpawnPoint().gameObject.transform.position;
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());
        var r = 20;

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<ModuleConnector>();


            foreach (var pendingExit in pendingExits)
            {
               //GameObject[] t = GameObject.FindGameObjectsWithTag("Generation");




                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(Modules, newTag);

                var newModule = Instantiate(newModulePrefab, new Vector3(r,r,r), new Quaternion());

                newModule.gameObject.name = "" + r + "";
                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);

                //CheckRoomOverlap(newModule);


                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));


                r += 10;

            }
            pendingExits = newExits;
        }

        foreach (var pendingExit in pendingExits)
        {
            var newModule = Instantiate(endModule);
            var newModuleExits = newModule.GetExits();
            var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);
        }
    }**/

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
            enemySpawnPoints.Add(p.transform.position);
        }

    }

    private void SpawnEnemies()
    {
        foreach (var p in enemySpawnPoints)
        {
            GameObject enemigo = Instantiate(enemy);
            enemigo.transform.position = p;
            enemigo.SetActive(true);
        }
    }
}
