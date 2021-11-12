using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class ModularWorldGenerator : MonoBehaviour
{
    public Room[] RoomsModules;

    private List<RoomConector> pendingConnections;

    public Room StartRoom;
    public Room EndRoom;

    public float MAX_CHESTS;

    public GameObject player;

    public GameObject EnemyPrefab;
    public GameObject ChestPrefab;

    public NavMeshSurface baker;

    public int Iterations = 5;

    private Vector3 playerSpawnPoint;

    public LayerMask roomLayerMask;


    void Start()
    {
        StartCoroutine("GenerateDungeon");
    }



    IEnumerator GenerateDungeon()
    {
        WaitForSecondsRealtime startup = new WaitForSecondsRealtime(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        //place start room 
        PlaceStartRoom();
        yield return interval;

        var r = 20;
        //places rooms
        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            List<RoomConector> newPendingConnections = new List<RoomConector>();

            yield return interval;
            //Iterate over all connections that are possible
            foreach (RoomConector pendingConnections in pendingConnections)
            {
                PlaceRoom(r, pendingConnections, newPendingConnections);
                yield return interval;
                r += 10;
            }
            pendingConnections = newPendingConnections;
            yield return interval;
        }

        //place ends
        PlaceEndRooms();
        yield return interval;

        //spawn player
        player.transform.position = playerSpawnPoint;
        player.SetActive(true);

        //bake navigation mesh for AI
        baker.BuildNavMesh();
        //Spawn enemies in all the spawnpoints in the map
        List<EnemySpawnPoint> spawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>().ToList();
        SpawnEnemies(spawnPoints);

        List<ChestSpawnPoint> chestSpawnPoints = GameObject.FindObjectsOfType<ChestSpawnPoint>().ToList();
        SpawnChest(chestSpawnPoints);
        yield return new WaitForSeconds(3);
        yield return 0;

    }


    private void PlaceStartRoom()
    {
        Room startRoom = Instantiate(StartRoom, transform.position, transform.rotation);
        startRoom.name = "Room0";
        startRoom.GetComponentsInChildren<EnemySpawnPoint>().ToList().ForEach(ep => Destroy(ep));
        playerSpawnPoint = startRoom.GetSpawnPoint().gameObject.transform.position;
        pendingConnections = new List<RoomConector>(startRoom.GetRoomConnections());
    }

    private void PlaceRoom(int r, RoomConector pendingConnection, List<RoomConector> newPendingconnections)
    {
        //Random choose a possible connection from the list of the connector
        var newTag = GetRandom(pendingConnection.Connections);

        //Random choose a room that have the choosed tag
        Room newRoomPrefab = GetRandomWithTag(RoomsModules, newTag);

        //Create the room
        Room newRoom = Instantiate(newRoomPrefab, new Vector3(r, r, r), new Quaternion());
        newRoom.gameObject.name = "Room" + r + "";

        RoomConector[] newModuleExits = newRoom.GetRoomConnections();
        var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
        MatchExits(pendingConnection, exitToMatch);

        if (CheckRoomOverlap(newRoom) == true)
        {
            ResetGenerate();
        }
        else
        {

        }

        newPendingconnections.AddRange(newModuleExits.Where(e => e != exitToMatch));

    }

    public bool CheckOverlap(Room room)
    {

        return false;
    }


    public bool CheckRoomOverlap(Room room)
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
                    Debug.Log(extents / 2);
                    Debug.Log(c.transform.parent.gameObject.name);
                    Debug.Log(room.gameObject.name);
                    return true;
                }
            }
        }
        return false;
    }

    private void PlaceEndRooms()
    {
        foreach (var pendingExit in pendingConnections)
        {
            var newModule = Instantiate(EndRoom);
            var newModuleExits = newModule.GetRoomConnections();
            var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);
        }
    }

    private void ResetGenerate()
    {
        StopCoroutine("GenerateDungeon");

        //destroy
        GameObject[] gs = GameObject.FindGameObjectsWithTag("Generation");

        foreach (GameObject g in gs)
        {
            Destroy(g);
        }
        pendingConnections.Clear();

        StartCoroutine("GenerateDungeon");
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

    private void MatchExits(RoomConector oldExit, RoomConector newExit)
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

    private static Room GetRandomWithTag(IEnumerable<Room> modules, string tagToMatch)
    {
        var matchingModules = modules.Where(m => m.RoomTags.Contains(tagToMatch)).ToArray();
        return GetRandom(matchingModules);
    }

    private static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }

    private void SpawnEnemies(List<EnemySpawnPoint> esps)
    {
        var i = 0;
        foreach (EnemySpawnPoint esp in esps)
        {
            GameObject newEnemy = Instantiate(EnemyPrefab);
            newEnemy.gameObject.name = "Enemy" + i.ToString();
            newEnemy.transform.parent = esp.transform;
            newEnemy.transform.position = esp.transform.position;
            newEnemy.transform.localScale = new Vector3(1, 1, 1); 
            newEnemy.SetActive(true);
            i++;
        }
    }

    private void SpawnChest(List<ChestSpawnPoint> csps)
    {
        for(int i = 0; i < MAX_CHESTS; i++)
        {
            int rand = Random.Range(0, csps.Count);
            ChestSpawnPoint csp = csps[rand];
            GameObject newChest = Instantiate(ChestPrefab, csp.transform.position, csp.transform.rotation);
            newChest.gameObject.name = "Chest" + i.ToString();
            newChest.transform.parent = csp.transform;
            newChest.SetActive(true);
            csps.Remove(csp);

        }
    }
}
