using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject playerObj;
    public Player playerScript;
    public bool playerAlive;
    void PauseGame ()
    {
        Time.timeScale = 0;
    }

    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Hello!");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("FoodChainGame");
          
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Starting Over...");
        playerObj = GameObject.FindWithTag("Player");
        playerScript = (Player)playerObj.GetComponent(typeof(Player));
        playerAlive = playerScript.playerAlive;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAlive){
            
            Debug.Log("boom boom");
            //StartCoroutine(WaitForFunction());
        }
    }
}
