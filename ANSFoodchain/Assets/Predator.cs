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

    [SerializeField] float maxOffsetDistance =2.5f;
    [SerializeField] float minDuration =1.5f;
    [SerializeField] float maxDuration =3.5f;

    public float verticalRange = 0.05f; // The distance the object moves left and right
    public float movementSpeed = 1.0f; // The speed of the up and down movement
    public float verticalFrequency = 5.0f;
    public float phaseShift = 1.0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        moveSpeed = moveSpeed + Random.Range(-10.0f,10.0f);
        StartCoroutine(RandomizePath());

        movementSpeed = 1.0f; // The speed of the up and down movement
        verticalFrequency += Random.Range(-2.0f,2.0f);
        phaseShift += Random.Range(-2.0f,2.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>nextIncrease){
            nextIncrease +=5f;
            moveSpeed += 5f;
            Debug.Log(moveSpeed);
        }
        
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float verticalMovement = Mathf.Sin(Time.time * movementSpeed * verticalFrequency + phaseShift) * verticalRange;
            transform.Translate(direction * moveSpeed * Time.deltaTime + new Vector3(0,verticalMovement,0));
            
        }

        if (target.transform.position.x <= gameObject.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        // Vector2 adjustedTarget = new Vector2(target.position.x + Random.Range(-10.0f, 10.0f), target.position.y + Random.Range(-10.0f, 10.0f));
        // transform.position = Vector2.MoveTowards(transform.position,adjustedTarget, moveSpeed*Time.deltaTime);
        
    }

    private IEnumerator RandomizePath()

    {
        while (true)
        {
            Vector3 randomOffset = Random.insideUnitSphere * maxOffsetDistance;
            Vector3 newDirection = (target.position - transform.position + randomOffset).normalized;
            float randomDuration = Random.Range(minDuration, maxDuration);

            yield return new WaitForSeconds(randomDuration);

            transform.Translate(newDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("predatorPrefab") && collider.gameObject != gameObject)
            {
                Vector2 separationDirection = (transform.position - collider.transform.position).normalized;
                float separationForce = 1.0f;
                rb.AddForce(separationDirection * separationForce, ForceMode2D.Impulse);
            }
        }
    }

    

    // void OnTriggerEnter2D(Collider2D other)
    // {


    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("predatorPrefab");

    //     foreach(GameObject enemy in enemies){
    //         Vector2 adjustedPosition = (Vector2)transform.position + new Vector2(0f, 0.35f);

    //         // Move the instantiated prefab to the adjusted position
    //         transform.position = adjustedPosition;
    //     }
        
    // }

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
