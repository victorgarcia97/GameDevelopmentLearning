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
            enemy = GameObject.Find(collision.gameObject.name);
            isFigthing = true;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name.Equals(enemy.name))
        {
            isFigthing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Equals(enemy.name)){
            isFigthing = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (isFigthing && Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log("Ataca");
            enemy.GetComponent<EnemyConfig>().LoseLife(damage);
        }
    }
}
