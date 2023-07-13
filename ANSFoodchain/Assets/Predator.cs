using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    
    [SerializeField] float moveSpeed =5f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    GameObject player;

    private float nextIncrease = 0.0f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>nextIncrease){
            nextIncrease +=5f;
            moveSpeed += 2f;
            Debug.Log(moveSpeed);
        }
        

        transform.position = Vector2.MoveTowards(transform.position,target.position, moveSpeed*Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("grrrahhhh");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("predatorPrefab");

        foreach(GameObject enemy in enemies){
            Vector2 adjustedPosition = (Vector2)transform.position + new Vector2(0f, 0.35f);

            // Move the instantiated prefab to the adjusted position
            transform.position = adjustedPosition;
        }
        
    }

    // private bool IsValidPosition(Vector3 position)
    // {
    //     GameObject[] existingPrefabs = GameObject.FindGameObjectsWithTag("predatorPrefab");

    //     foreach (GameObject existingPrefab in existingPrefabs)
    //     {
    //         float distance = Vector3.Distance(existingPrefab.transform.position, position);
    //         if (distance < minDistance)
    //         {
    //             return false; // Minimum distance constraint violated
    //         }
    //     }

    //     return true; // Position satisfies the minimum distance constraint
    // }
   



}
