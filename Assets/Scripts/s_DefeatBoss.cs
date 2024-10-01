using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_DefeatBoss : MonoBehaviour
{
    [SerializeField] s_GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        gameController.bossesDefeated++;
    }
}
