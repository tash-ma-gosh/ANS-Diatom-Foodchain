using UnityEngine;
using UnityEngine.InputSystem;
using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class PlayerHandMove : MonoBehaviour
{
    public KinectInput kinectInput;
    public bool isLeftHand = true;
    Rigidbody2D rb;
    public JointType leftHandJoint = JointType.HandLeft;
    public JointType rightHandJoint = JointType.HandRight;
    public float moveSpeed = 5f;

    private void Update()
    {
        if (kinectInput != null && kinectInput.bodies != null)
        {
            // Assuming player 1's hand movement controls the game object
            Vector2 handPosition = Vector2.zero;

            if (isLeftHand)
            {
                var leftHandPosition = GetJointPosition(kinectInput.bodies[0], leftHandJoint);
                handPosition = KinectToUnityPosition(leftHandPosition);
            }
            else
            {
                var rightHandPosition = GetJointPosition(kinectInput.bodies[0], rightHandJoint);
                handPosition = KinectToUnityPosition(rightHandPosition);
            }

            // Update the game object's position in 2D
            Vector2 targetPosition = handPosition * moveSpeed;
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime);
        }
    }

    private Vector3 GetJointPosition(Body body, JointType jointType)
    {
        var joint = body.Joints[jointType];
        return new Vector3(joint.Position.X, joint.Position.Y, -joint.Position.Z);
    }

    private Vector2 KinectToUnityPosition(Vector3 kinectPosition)
    {
        // Convert Kinect 3D position to Unity's 2D space
        Vector2 unityPosition = new Vector2(kinectPosition.x, kinectPosition.y);

        return unityPosition;
    }
}
