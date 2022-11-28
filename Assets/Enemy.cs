using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private float _speed = 1;
    [SerializeField] GameObject player;
    [SerializeField] float patrolRadius = 100;
    [SerializeField] float changePatrolTimer = 10;
    [SerializeField] float sightRange = 15;
    [SerializeField] float agroRange = 20;
    private float timer;
    private Enums.EnemyState mode;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _speed = agent.speed;
        mode = Enums.EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Enums.EnemyState.Patrol:
                {
                    timer += Time.deltaTime;
                    if (timer >= changePatrolTimer)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, patrolRadius, -1);
                        agent.SetDestination(newPos);
                        timer = 0;
                    }
                    break;
                }
            case Enums.EnemyState.Seek:
                {
                    agent.SetDestination(player.transform.position);
                    Attack();
                    break;
                }
        }
        
        if(Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, sightRange))
        {
            if (hit.transform.gameObject == player)
            {
                if(mode == Enums.EnemyState.Patrol)
                {
                    GameEvents.InvokePlayAudio("alert");
                }
                mode = Enums.EnemyState.Seek;
            }
        }

        if(Vector3.Distance(player.transform.position,transform.position) > agroRange)
        {
            mode = Enums.EnemyState.Patrol;
        }
    }

    private void Attack()
    {
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, sightRange))
        {
            if (hit.transform.gameObject == player)
            {
                player.GetComponent<Player>().damage(10 * Time.deltaTime);
            }
        }
    }

    public void snare(float t)
    {
        agent.speed = 0;
        Invoke("unsnare", t);
    }

    private void unsnare()
    {
        agent.speed = _speed;
    }

    private void blind(float t)
    {

    }
    
    private void unblind()
    {

    }

    // random point on navmesh inside given sphere from https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/ from user Cnc96
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
