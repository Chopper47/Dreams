using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_GameController : MonoBehaviour
{
    private bool stopped;
    [SerializeField] private s_CharacterMovement characterMovement;
    public GameObject pauseMenuCanvas;
    public int bossesDefeated = 0;
    [SerializeField] private bool endGame = false;
    [SerializeField] GameObject turnManager;
    [SerializeField] private GameObject bossBody; 
    [SerializeField] private GameObject bossLeftHand;
    [SerializeField] private GameObject bossRightHand;
    [SerializeField] private Transform allyCenterPosition;
    [SerializeField] private Transform allyLeftPosition;
    [SerializeField] private Transform allyRightPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void PauseMenu()
    {
        if (stopped)
        {
            characterMovement.enabled = true;
            pauseMenuCanvas.SetActive(false);
            stopped = false;
        }
        else
        {
            characterMovement.enabled = false;
            pauseMenuCanvas.SetActive(true);
            stopped = true;

        }
    }

    //starts boss battle when called
    public void StartBossBattle()
    {
        s_TurnManager turnManagerCode = turnManager.GetComponent<s_TurnManager>();
        turnManagerCode.isBoss = true;
        turnManagerCode.enemyCenterPosition = bossBody.transform;
        turnManagerCode.enemyLeftPosition = bossRightHand.transform;
        turnManagerCode.enemyRightPosition = bossLeftHand.transform;

        

        bossBody.SetActive(true);
        bossLeftHand.SetActive(true);
        bossRightHand.SetActive(true);

        Debug.Log("Boss Battle Starting");
        turnManagerCode.enemyLeftPrefab = bossRightHand;
        turnManagerCode.enemyCenterPrefab = bossBody;
        turnManagerCode.enemyRightPrefab = bossLeftHand;
        turnManager.GetComponent<s_TurnManager>().enabled = true;
    }
}
