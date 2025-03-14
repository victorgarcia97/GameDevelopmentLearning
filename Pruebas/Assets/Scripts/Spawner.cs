﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public int spawners = 10;
    public int maxRange;
    public int minRange;
    public int maxPackSpawn;




    private Rigidbody rb;
    void Start()
    {
       rb = enemy.GetComponent<Rigidbody>();
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
        int spawnPointZ = Random.Range(minRange, maxRange);
        /**
        int packSpawn = Random.Range(5, maxPackSpawn);
        int i = 0;
        while(packSpawn != 0)
        {

            enemy.name = "nuevo " + spawners + " " + packSpawn;
           
            GameObject nuevo = Instantiate(enemy, new Vector3(spawnPointX, 1, spawnPointZ), new Quaternion());
            nuevo.SetActive(true);

            int signo = Random.Range(-1, 1);
            spawnPointX += signo;
            spawnPointZ += -signo;
            packSpawn--;
        }
        **/
        enemy.name = "nuevo" + spawners+"1";
        GameObject nuevo = Instantiate(enemy, new Vector3(spawnPointX, 1, spawnPointZ), new Quaternion());
        enemy.name = "nuevo" + spawners + "2";
        GameObject nuevo1 = Instantiate(enemy, new Vector3(spawnPointX+1, 1, spawnPointZ+1), new Quaternion());
        enemy.name = "nuevo" + spawners + "3";
        GameObject nuevo2 = Instantiate(enemy, new Vector3(spawnPointX-1, 1, spawnPointZ-1), new Quaternion());
        enemy.name = "nuevo" + spawners + "4";
        GameObject nuevo3 = Instantiate(enemy, new Vector3(spawnPointX+1, 1, spawnPointZ-1), new Quaternion());
        enemy.name = "nuevo" + spawners + "5";
        GameObject nuevo4 = Instantiate(enemy, new Vector3(spawnPointX-1, 1, spawnPointZ+1), new Quaternion());
        nuevo.SetActive(true);
        nuevo1.SetActive(true);
        nuevo2.SetActive(true);
        nuevo3.SetActive(true);
        nuevo4.SetActive(true);
        spawners--;
    }

}
