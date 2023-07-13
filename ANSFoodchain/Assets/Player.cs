using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    // float inputX;
    // float inputY;
    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    Rigidbody2D rb;
    public bool playerAlive;

            

    // Start is called before the first frame update
    void Start()
    {
        playerAlive = true;
    }

    void Boundary()
    {
        if(transform.position.y >= 30)
        {
            //first check for upper right and left corners
            if(transform.position.x >=56)
            {
                transform.position = new Vector3(56,30,0);
            }
            else if(transform.position.x <=-56)
            {
                transform.position = new Vector3(-56, 30,0);
            }
            else{
                transform.position = new Vector3(transform.position.x, 30,0);
            }
        }
        else if (transform.position.y <= -30)
        {
            if(transform.position.x >=56)
            {
                transform.position = new Vector3(56,-30,0);
            }
            else if(transform.position.x <=-56)
            {
                transform.position = new Vector3(-56,-30,0);
            }
            else{
                transform.position = new Vector3(transform.position.x,-30,0);
            }
            
        }

        else{
            if (transform.position.x >=56)
            {
                transform.position = new Vector3(56, transform.position.y,0);
            }
            else if (transform.position.x <=-56)
            {
                transform.position = new Vector3(-56, transform.position.y,0);
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

        //boundary logic
        Boundary();
        IsPlayAlive();       
    }

    

   
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("predatorPrefab");
        SceneManager.LoadScene("Level1_Transition");
        foreach(GameObject enemy in enemies){
            Debug.Log("bloop");
            GameObject.Destroy(enemy);
        }

        
        

        playerAlive = false;

        //PauseGame();
        //StartCoroutine(WaitForFunction());
        
        Debug.Log("You've been eaten :3");
    }

    void IsPlayAlive(){
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(playerAlive);
        }
    }
}
