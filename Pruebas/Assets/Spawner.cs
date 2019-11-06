using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public int spawners = 10;
    public int maxRange;
    public int minRange;
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawners != 0)
        {
            Spawn();
        }    
    }

    void Spawn()
    {
        int spawnPointX = Random.Range(minRange, maxRange);
        int spawnPointY = Random.Range(minRange, maxRange);


        enemy.name = "nuevo" + spawners+"1";
        GameObject nuevo = Instantiate(enemy, new Vector3(spawnPointX, 1, spawnPointY), new Quaternion());
        enemy.name = "nuevo" + spawners + "2";
        GameObject nuevo1 = Instantiate(enemy, new Vector3(spawnPointX+1, 1, spawnPointY+1), new Quaternion());
        enemy.name = "nuevo" + spawners + "3";
        GameObject nuevo2 = Instantiate(enemy, new Vector3(spawnPointX-1, 1, spawnPointY-1), new Quaternion());
        enemy.name = "nuevo" + spawners + "4";
        GameObject nuevo3 = Instantiate(enemy, new Vector3(spawnPointX+1, 1, spawnPointY-1), new Quaternion());
        enemy.name = "nuevo" + spawners + "5";
        GameObject nuevo4 = Instantiate(enemy, new Vector3(spawnPointX-1, 1, spawnPointY+1), new Quaternion());
        nuevo.SetActive(true);
        nuevo1.SetActive(true);
        nuevo2.SetActive(true);
        nuevo3.SetActive(true);
        nuevo4.SetActive(true);
        spawners--;
    }

}
