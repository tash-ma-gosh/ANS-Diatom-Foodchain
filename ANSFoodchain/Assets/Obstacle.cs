using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float verticalRange = 60.0f; // The distance the object moves up and down
    public float horizontalRange = 15.0f; // The distance the object moves left and right
    public float movementSpeed = 1.0f; // The speed of the up and down movement
    public float verticalFrequency = 1.0f; // The frequency of the vertical sine wave
    public float horizontalFrequency = 5.0f;

    [SerializeField] Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the vertical movement using a sine wave
        float verticalMovement = Mathf.Cos(Time.time * movementSpeed * verticalFrequency) * verticalRange;

        // Calculate the horizontal movement using a sine wave
        float horizontalMovement = Mathf.Sin(Time.time * movementSpeed * horizontalFrequency) * horizontalRange;

        // Set the new position of the object
        transform.position = initialPosition + new Vector3(horizontalMovement, verticalMovement, 0.0f);
    }
}
