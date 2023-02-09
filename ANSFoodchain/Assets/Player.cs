using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // float inputX;
    // float inputY;
    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    Rigidbody2D rb;

            

    // Start is called before the first frame update
    void Start()
    {
        
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
                
    }

   
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("You've been eaten :3");
    }
}
