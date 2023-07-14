using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerOSC : MonoBehaviour
{
    
   	public OSC osc;


	// Use this for initialization
	void Start () {
       osc.SetAddressHandler( "/PlayerXY" , OnReceiveXY );
       osc.SetAddressHandler("/PlayerX", OnReceiveX);
       osc.SetAddressHandler("/PlayerY", OnReceiveY);
    //    osc.SetAddressHandler("/CubeZ", OnReceiveZ);
    }
	


	// Update is called once per frame
	void Update () {
        Boundary();
	}

	void OnReceiveXY(OscMessage message){
		float x = message.GetFloat(0);
         float y = message.GetFloat(1);
		// float z = message.GetFloat(2);

		transform.position = new Vector3(x,y);
	}

    void OnReceiveX(OscMessage message) {
        float x = message.GetFloat(0);

        Vector3 position = transform.position;

        position.x = x;

        transform.position = position;
    }

    void OnReceiveY(OscMessage message) {
        float y = message.GetFloat(0);

        Vector3 position = transform.position;

        position.y = y;

        transform.position = position;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("predatorPrefab");
        SceneManager.LoadScene("Level1_Transition");
        foreach(GameObject enemy in enemies){
            Debug.Log("bloop");
            GameObject.Destroy(enemy);
        }
        
        
        Debug.Log("You've been eaten :3");
    }

}
