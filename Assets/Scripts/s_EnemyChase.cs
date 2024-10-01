using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyChase : MonoBehaviour
{

    [SerializeField]private Animator overworldAnimator;
    public GameObject closestTarget;
    private NavMeshAgent agent;
    public float distance;
    public float range;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();


        

    }

    void UpdateTarget()
    {
        //Array for every player on the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Character");

        //Variable for the shortest distance to a player, it starts of as infinite
        float shortestDistanceToPlayer = Mathf.Infinity;
        GameObject nearestPlayer = null;


        foreach (GameObject player in players)
        {
            //Check distance to every player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            //Choose closest player
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                nearestPlayer = player;
                distance = distanceToPlayer;
            }


        }

        if(shortestDistanceToPlayer <= range)
        {
            if (overworldAnimator)
            {
                overworldAnimator.SetBool("withinRange", true);

            }
            closestTarget = nearestPlayer;
            agent.SetDestination(closestTarget.transform.position);
        }

        else
        {
            if (overworldAnimator)
            {
                overworldAnimator.SetBool("withinRange", false);

            }
            closestTarget = null;
            agent.SetDestination(transform.position);
        }
        
        
    }

    //GameObject FindClosestTarget()
    //{
    //    targets = GameObject.FindGameObjectsWithTag("Character");
    //    closestTarget = null;
    //    distance = Mathf.Infinity;
    //    Vector3 position = transform.position;
    //    foreach (GameObject target in targets)
    //    {
    //        Vector3 diff = target.transform.position - position;
    //        float currentDistance = diff.sqrMagnitude;
    //        if (currentDistance < distance)
    //        {
    //            closestTarget = target;
    //            distance = currentDistance;
    //        }
    //    }
    //    return closestTarget;

    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}