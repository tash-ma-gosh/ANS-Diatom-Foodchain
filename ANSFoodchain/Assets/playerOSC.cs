using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // void OnReceiveZ(OscMessage message) {
    //     float z = message.GetFloat(0);

    //     Vector3 position = transform.position;

    //     position.z = z;

    //     transform.position = position;
    // }

}
