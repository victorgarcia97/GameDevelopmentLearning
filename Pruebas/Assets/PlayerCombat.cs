using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public bool isFigthing;
    private GameObject enemy;
    public float attackSpeed;
    private float nextAttack = 0.0f;
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
        if (enemy != null && collision.gameObject.name.Equals(enemy.name))
        {
            isFigthing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (enemy != null && collision.gameObject.name.Equals(enemy.name)){
            isFigthing = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttack && Input.GetKey(KeyCode.Mouse0) && isFigthing)
        {
            nextAttack = Time.time + attackSpeed;
            Debug.Log("Ataca");
            enemy.GetComponent<EnemyConfig>().LoseLife(damage);
        }
    }
}
