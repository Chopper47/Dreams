using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class s_CampFire : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private s_CharacterStats ally1;
    [SerializeField] private s_CharacterStats ally2;
    private void OnEnable()
    {
        s_CharacterMovement characterMovement = mainCharacter.GetComponent<s_CharacterMovement>();
        NavMeshAgent mainCharacterAgent = mainCharacter.GetComponent<NavMeshAgent>();
        mainCharacterAgent.SetDestination(mainCharacter.transform.position);
        s_CharacterStats mainCharacterStats = mainCharacter.GetComponent<s_CharacterStats>();
        characterMovement.enabled = false;
        mainCharacterStats.FullHeal();
        ally1.FullHeal();
        ally2.FullHeal();
        characterMovement.enabled = true;
        this.enabled = false;
    }

    private void Awake()
    {
        


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
