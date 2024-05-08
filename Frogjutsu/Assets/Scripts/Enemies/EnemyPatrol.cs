// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyPatrol : MonoBehaviour
// {

//     public GameObject pointA;
//     public GameObject pointB;
//     private Rigidbody2D rb;
//     private Animator anim;
//     private Transform currentPoint;
//     public float speed;

    
//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         currentPoint = pointB.transform;
//         anim.SetBool("isRunning", true);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         sideMovement();
//     }

//     private void sideMovement(){
        
//         Vector2 point = currentPoint.position - transform.position;
//         if(currentPoint == pointB.transform){
//             rb.velocity = new Vector2(-speed, 0);
//         }else{
//             rb.velocity = new Vector2(speed, 0);
//         }

//         if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform){
//             flip();
//             currentPoint = pointA.transform;
//         }

//         if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform){
//             flip();
//             currentPoint = pointB.transform;
//         } 
//     }
    
//     private void flip(){
//         Vector3 localScale = transform.localScale;
//         localScale.x *= -1;
//         transform.localScale = localScale;
//     }

//     private void OnDrawGizmos(){
//         Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
//         Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
//         Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
//     }
    
// }   

using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("isRunning", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("isRunning", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("isRunning", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(initScale.x * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}


