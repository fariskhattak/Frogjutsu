using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rockhead : Enemy
{
    [SerializeField] public float speed;
    [SerializeField] public float range;
    [SerializeField] public float checkDelay;
    [SerializeField] public LayerMask playerLayer;
    private float checkTimer;
    private Vector3 destination;

    private bool attacking;

    private Vector3[] directions = new Vector3[4];

    private void OnEnable() {
        Stop();
    }    

    private void Update() {
        if (attacking) {
        //   transform.Translate(destination * Time.deltaTime * speed);  
        }
        else {
            // checkTimer += checkTimer.deltaTime;
            if (checkTimer > checkDelay) {
                CheckForPlayer();
            }
        }
    }

    private void CheckForPlayer() {
        CalculateDirections();
        for (int i = 0; i < directions.Length; i++) {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking) {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections() {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop() {
        destination = transform.position;
        attacking = false;
    }

    public override void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        Stop();
    }
}
