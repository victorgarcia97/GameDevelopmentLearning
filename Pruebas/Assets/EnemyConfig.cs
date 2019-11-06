using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyConfig : MonoBehaviour
{
    // Start is called before the first frame update
    public static float health = 100f;
    private float currentHealth;
    public Image healthBar;
    void Start()
    {
        currentHealth = 100f;
    }
    
    internal void LoseLife(float lifeLost)
    {
        float pctLost;
        currentHealth -= lifeLost;
        pctLost = lifeLost/health;
        Debug.Log(pctLost);
        healthBar.fillAmount -= pctLost;
    }
    // Update is called once per frame
    void Update()
    {
        if(currentHealth == 0.0)
        {
            Debug.Log("Muere");
            Destroy(gameObject,2f);
        }
       
        
    }
}
