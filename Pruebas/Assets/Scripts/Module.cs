using UnityEngine;

public class Module : MonoBehaviour
{
    public string[] Tags;

    public ModuleConnector[] GetExits()
    {
        return GetComponentsInChildren<ModuleConnector>();
    }

    public SpawnPoint GetSpawnPoint()
    {
        return GetComponentInChildren<SpawnPoint>();
    }

    public EnemySpawnPoint[] GetEnemySpawnPoints()
    {
        return GetComponentsInChildren<EnemySpawnPoint>();
    }
}

  
