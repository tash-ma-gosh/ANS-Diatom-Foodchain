using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    // public static event Action<Predator> OnEnemyKilled;

    [SerializeField] float moveSpeed =5f;
    Rigidbody2D rb;
    Transform target;
    Vector2 moveDirection;
    [SerializeField] GameObject player;

    private float nextIncrease = 0.0f;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}
