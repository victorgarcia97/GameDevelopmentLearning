using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Baker : MonoBehaviour
{
    [SerializeField]
    public NavMeshSurface[] baker;
    void Start()
    {
       for(int i = 0; i < baker.Length; i++)
        {
            baker[i].BuildNavMesh();
        }  
    }
}
