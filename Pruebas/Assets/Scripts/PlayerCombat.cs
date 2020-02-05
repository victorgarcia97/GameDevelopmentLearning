using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public bool isFigthing;
    public GameObject enemy;


    public float attackSpeed;
    private float nextAttack = 0.0f;

    public Image playerHealthBar;
    public float playerHealth = 100f;
    private float currentPlayerHealth;
    public bool isPlayerDead;

    public GameObject canv;

    void Start()
    {
        currentPlayerHealth = playerHealth;
        isPlayerDead = false;   
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

    internal void PlayerTakeDamage(float playerLifeLost)
    {
        float pctLost;
        currentPlayerHealth -= playerLifeLost;
        pctLost = playerLifeLost / playerHealth;
        playerHealthBar.fillAmount -= pctLost;
        if(currentPlayerHealth == 0.0f)
        {
            isPlayerDead = true;
        }
        
        
    }



    // Update is called once per frame
    void Update()
    {
        if (isPlayerDead)
        {
            isPlayerDead = false;
            Debug.Log("Muere jugador");
            Destroy(gameObject);
            Debug.Log("ESPERADO");
            //SceneManager.LoadScene("DieScene", LoadSceneMode.Single);
            canv.SetActive(true);
        }

        if (Time.time > nextAttack && Input.GetKey(KeyCode.Mouse0) && isFigthing)
        {
            nextAttack = Time.time + attackSpeed;
            Debug.Log("Ataca a enemigo");
            enemy.GetComponent<EnemyConfig>().LoseLife(damage);
        }
    }
}
