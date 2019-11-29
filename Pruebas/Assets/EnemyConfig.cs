using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyConfig : MonoBehaviour
{
    // Start is called before the first frame update
    public static float health = 100f;
    private float currentHealth;
    public Image healthBar;
    public bool isDead;
    private Rigidbody rb;
    private NavMeshAgent nav;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        nav = gameObject.GetComponent<NavMeshAgent>();
        isDead = false;
        currentHealth = 100f;
    }

    internal void LoseLife(float lifeLost)
    {
        float pctLost;
        currentHealth -= lifeLost;
        pctLost = lifeLost/health;
        Debug.Log(pctLost);
        healthBar.fillAmount -= pctLost;
        if(currentHealth == 0.0f)
        {
            nav.isStopped = true;
            isDead = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            //rb.mass = 1;
            //rb.drag = 1;
            //rb.AddForce(new Vector3(0, -5, 0));
            Debug.Log("Muere");
            Destroy(gameObject,2f);
        }
       
        
    }
}
