using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public bool isFigthing;
    public GameObject enemy;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("Ataca");
            enemy = GameObject.Find(collision.gameObject.name);
            enemy.GetComponent<EnemyConfig>().LoseLife(damage);
            isFigthing = true;
        }

    }



    // Update is called once per frame
    void Update()
    {
        /**if (isFigthing)
        {
           enemy.GetComponent<EnemyConfig>().LoseLife(damage);
           isFigthing = false;
        }
        **/
    }
}
