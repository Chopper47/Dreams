using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_EnemyTransfer : MonoBehaviour
{
    [SerializeField] s_TurnManager turnManager;
    
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;

    private void Awake()
    {
        turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<s_TurnManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TransferEnemiesToBattle()
    {
        turnManager.isBoss = false;
        turnManager.enemyOverworld = gameObject;
        turnManager.enemyLeftPrefab = enemy1;
        turnManager.enemyCenterPrefab = enemy2;
        turnManager.enemyRightPrefab = enemy3;
        turnManager.enabled = true;

    }

}
