using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class s_CharacterMovement : MonoBehaviour
{
    [SerializeField]
    LayerMask mask;
    [SerializeField] s_GameController gameController;
    private NavMeshAgent agent;
    public GameObject turnManager;
    public Vector3 originalPosition;
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
        //Moves when clicking on the ground while in the overworld
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, 100f, mask))
            {

                agent.SetDestination(hit.point);
                
            }
        }
    }

    /// <summary>
    /// Checks collision with the overworld enemies
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "OverworldEnemy")
        {
            Debug.Log("touch");
            originalPosition = gameObject.transform.position;
            collision.gameObject.GetComponent<s_EnemyTransfer>().TransferEnemiesToBattle();
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);

        }
        
    }

    /// <summary>
    /// Starts boss battle when within the correct position and after defeated both minibosses
    /// </summary>
    /// <param name="other"></param>

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if (other.gameObject.tag == "BossZoneCollider" && gameController.bossesDefeated >= 2)
        {
            Debug.Log("Starting boss battle");
            gameController.StartBossBattle();
        }
    }
}
