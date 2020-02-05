using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDies : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    // Update is called once per frame
    IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            if (player == null)
            {
                yield return new WaitForSeconds(2);
                Debug.Log("ESPERADO");
                SceneManager.LoadScene("DieScene", LoadSceneMode.Single);
            }
        }

    }
}
