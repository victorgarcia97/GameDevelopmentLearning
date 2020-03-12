using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public LayerMask layerMask;
    /**public void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Debug.Log("NUEVO"); 
        //Debug.Log("###################");
        //Debug.Log(gameObject.transform.parent.gameObject.name);
        Bounds bound = other.bounds;
        //bound.Expand(-5.0f);
        //Debug.Log("###################");
        Collider[] colliders = Physics.OverlapBox(bound.center, bound.size / 2, gameObject.transform.rotation,layerMask);
        foreach (Collider c in colliders)
        {
            if (c.transform.parent.gameObject.Equals(gameObject.transform.parent))
            {
                continue;
            }
            else
            {
                Debug.Log("OVERLAP");
                //Debug.Log(c.transform.parent.gameObject.name);
            }
        }


        //Debug.Log("###################");
    }


    public void OnTriggerStay(UnityEngine.Collider other)
    {
        
    }

}
    **/
}