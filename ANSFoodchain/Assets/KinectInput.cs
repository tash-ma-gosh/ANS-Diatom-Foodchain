using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Windows.Kinect;

public class KinectInput : MonoBehaviour
{
    private KinectSensor kinectSensor;
    private BodyFrameReader bodyFrameReader;
    public Body[] bodies;

    private void Start()
    {
        kinectSensor = KinectSensor.GetDefault();

        if (kinectSensor != null)
        {
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            if (!kinectSensor.IsOpen)
                kinectSensor.Open();
        }
    }

    private void Update()
    {
        if (bodyFrameReader != null)
        {
            var frame = bodyFrameReader.AcquireLatestFrame();
            if (frame != null)
            {
                bodies = new Body[kinectSensor.BodyFrameSource.BodyCount];
                frame.GetAndRefreshBodyData(bodies);
                
                frame.Dispose();
                frame = null;
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (bodyFrameReader != null)
        {
            bodyFrameReader.Dispose();
            bodyFrameReader = null;
        }

        if (kinectSensor != null)
        {
            if (kinectSensor.IsOpen)
                kinectSensor.Close();

            kinectSensor = null;
        }
    }

    // Add additional functions to track and retrieve hand positions
}
