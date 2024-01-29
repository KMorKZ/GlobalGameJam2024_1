using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npcController : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private Animator anim;

    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] private float searchRange;
    [SerializeField] private float distanceToGet;


    enum STATE { IDLE, WANDER };
    STATE state = STATE.IDLE;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {

        switch (state)
        {
            case STATE.IDLE:
                Debug.Log("idle");
                anim.SetBool("isRunning", false);
                if (Random.Range(0, 1000) < 5)
                    state = STATE.WANDER;
                break;
            case STATE.WANDER:
                Patrol();
                Debug.Log("run");
                anim.SetBool("isRunning", true);
                if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.IDLE;
                    agent.ResetPath();
                }
                break;
        }
    }
    void Patrol()
    {
        if (!walkpointSet) SearchForDestination();
        if (walkpointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < distanceToGet) walkpointSet = false;
    }
    void SearchForDestination()
    {
        float x = Random.Range(-searchRange, searchRange);
        float z = Random.Range(-searchRange, searchRange);

        destPoint = new Vector3(
            transform.position.x + x,
            transform.position.y,
            transform.position.z + z);
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(destPoint.x, destPoint.y + 5f, destPoint.z), Vector3.down);
        if (Physics.Raycast(ray, out hit, groundLayer))
        {
            walkpointSet = true;
            destPoint.y = hit.point.y + 0.1f;
        }
    }
}
