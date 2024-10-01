using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public enum BattleState {startBattle, battleTurn, endBattle }
public class s_TurnManager : MonoBehaviour
{
    public BattleState state;
    
    public List<GameObject> characterTurnList;
    

    public s_CharacterStats currentCharacter;
    [Header("Buttons")]
    [SerializeField] private Canvas buttonCanvas;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;
    public TextMeshProUGUI button4Text;
    public TextMeshProUGUI button5Text;

    [Header("Allies and positions")]
    public GameObject mainCharacterPrefab;
    public Transform mainCharacterCameraFollow;
    public GameObject allyLeftPrefab;
    public GameObject allyRightPrefab;
    public Transform allyCenterPosition;
    public Transform allyLeftPosition;
    public Transform allyRightPosition;

    [Header("Enemies and positions")]
    public GameObject enemyOverworld;
    public GameObject enemyCenterPrefab;
    public GameObject enemyLeftPrefab;
    public GameObject enemyRightPrefab;
    public Transform enemyCenterPosition;
    public Transform enemyRightPosition;
    public Transform enemyLeftPosition;

    [Header("Other")]
    public int turnCounter = 0;
    public int numberOfCharacters;
    private bool inTurnBattle = false;
    private bool enemiesLeft = true;
    private int enemiesCounter = 0;
    private bool alliesLeft = true;
    private int alliesCounter = 0;
    public GameObject pointer;
    public GameObject nextCharacterPointer;
    public s_CharacterStats nextCharacter;
    private bool showingNextCharacter = false;
    private bool showNextCharacterAvailable = true;
    public GameObject virtualCamera;
    public bool isBoss = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        buttonCanvas.enabled = true;
        state = BattleState.startBattle;
        StartBattle();
        enemiesLeft = true;
        alliesLeft = true;
        showingNextCharacter = false;
        showNextCharacterAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Restarts turns once all of them are over
        if(turnCounter >= numberOfCharacters)
        {
            
            UpdateTurnOrder();
            turnCounter = 0;
            StopAllCoroutines();
            StartCoroutine(turnCoroutine());
        }

        //Starts the battle
        //if(state == BattleState.battleTurn)
        //{
        //    if(!inTurnBattle)
        //    {
        //        inTurnBattle = true;
        //        StartCoroutine(turnCoroutine());
                
        //    }

        //}
        if(Input.GetKeyDown(KeyCode.N) && showNextCharacterAvailable)
        {
            
            ShowNextCharacter(nextCharacter);
        }

        
        
    }

    void StartBattle()
    {
        //Sets every character in the correct position
        mainCharacterPrefab.GetComponent<s_CharacterMovement>().enabled = false;
        mainCharacterPrefab.GetComponent<NavMeshAgent>().enabled = false;
        mainCharacterPrefab.transform.position = allyCenterPosition.position;
        mainCharacterPrefab.transform.rotation = Quaternion.identity;
        allyLeftPrefab.SetActive(true);
        allyLeftPrefab.transform.position = allyLeftPosition.position;
        allyRightPrefab.transform.position = allyRightPosition.position;
        allyRightPrefab.SetActive(true);
        enemyCenterPrefab.SetActive(true);


        if(!isBoss)
        {
            Instantiate(enemyCenterPrefab, enemyCenterPosition);
            Instantiate(enemyLeftPrefab, enemyRightPosition);
            Instantiate(enemyRightPrefab, enemyLeftPosition);

        }

        pointer.SetActive(true);

        //Checks the number of characters in the battle
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        numberOfCharacters = characters.Length;
        characters = null;
        UpdateTurnOrder();
        state = BattleState.battleTurn;
        StartCoroutine(turnCoroutine());

    }

    //Chooses the order of turns depending on the speed of each character
    void UpdateTurnOrder()
    {
        characterTurnList.Clear();
        List<GameObject> characters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Character"));
        for (int i = 0; i < numberOfCharacters; i++)
        {
            int highestSpeed = 0;
            GameObject fastestCharacter = null;
            foreach (GameObject character in characters)
            {
                if(character.GetComponent<s_CharacterStats>().speed >= highestSpeed)
                {
                    fastestCharacter = character;
                    highestSpeed = character.GetComponent<s_CharacterStats>().speed;
                }
            }
            
            characterTurnList.Add(fastestCharacter);
            characters.Remove(fastestCharacter);

        }

        
        
    }

    //Waits until the character has chosen an action to go onto the next. Gets the info for the abilities of the turn character
    IEnumerator turnCoroutine()
    {
        GameObject[] charactersInScene = GameObject.FindGameObjectsWithTag("Character");
        foreach(GameObject character in charactersInScene)
        {
            character.GetComponent<s_CharacterStats>().turnManager = gameObject.GetComponent<s_TurnManager>();
        }
        
        for(int i = 0; i < characterTurnList.Count; i++)
        {
            
            if (characterTurnList[i] == null  || characterTurnList[i].gameObject.activeSelf == false)
            {
                
                characterTurnList.Remove(characterTurnList[i]);
                numberOfCharacters--;
            }
            if (characterTurnList[i] != null && state == BattleState.battleTurn)
            {

                currentCharacter = characterTurnList[i].GetComponent<s_CharacterStats>();
                currentCharacter.hasChosen = false;

                //Gets the next character in line, if the current is the last in line, it gets the first
                if(i+1 <= numberOfCharacters-1)
                {
                    nextCharacter = characterTurnList[i+1].GetComponent<s_CharacterStats>();

                }
                else
                {
                    nextCharacter = characterTurnList[0].GetComponent<s_CharacterStats>();
                }

                if (currentCharacter != null)
                {
                    showNextCharacterAvailable = true;
                    UpdateStatsByComfort(currentCharacter);
                    UpdateHUD(currentCharacter);
                    
                    //Chooses random target if enemy
                    if (currentCharacter.isEnemy)
                    {
                        List<GameObject> alliesInBattle = new List<GameObject>();
                        foreach(GameObject character in characterTurnList)
                        {
                            if(character)
                            {

                                if(!character.GetComponent<s_CharacterStats>().isEnemy)
                                {
                                    if(character.activeSelf)
                                    {
                                        alliesInBattle.Add(character);

                                    }
                                }

                            }
                    
                        }
                        int randomTarget = Random.Range(0, alliesInBattle.Count);
                        currentCharacter.target = alliesInBattle[randomTarget];
                        //Chooses random ability if enemy
                        int randomAbility = Random.Range(1, 4);
                        switch (randomAbility)
                        {
                            case 1:
                                currentCharacter.ActivateAbility(currentCharacter.ability1);
                                break;
                            case 2:
                                currentCharacter.ActivateAbility(currentCharacter.ability2);
                                break;
                            case 3:
                                currentCharacter.ActivateAbility(currentCharacter.ability3);
                                break;
                            case 4:
                                currentCharacter.ActivateAbility(currentCharacter.ability4);
                                break;
                            default:
                                Debug.Log("NOT A VALID ABILITY");
                                break;
                        }

                    }
                    yield return new WaitUntil(() => currentCharacter.hasChosen == true);
                    buttonCanvas.gameObject.SetActive(false);

                    showNextCharacterAvailable = false;
                    //Deactivates the 'show next character' method once an attack starts
                    if (showingNextCharacter)
                    {
                        ShowNextCharacter(nextCharacter);
                    }
                    currentCharacter.hasChosen = false;
                    currentCharacter.hasFinishedEffects = false;
                    yield return new WaitForSeconds(0.5f);
                    StartCoroutine(currentCharacter.EndTurnEffects());
                    yield return new WaitUntil(() => currentCharacter.hasFinishedEffects == true);
                    buttonCanvas.gameObject.SetActive(true);

                }
            }
            //Checks if there's enemies left
            GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
            foreach (GameObject character in characters)
            {
                if(character)
                {
                    if (character.GetComponent<s_CharacterStats>().isEnemy)
                    {
                        enemiesCounter++;
                    }
                    if (!character.GetComponent<s_CharacterStats>().isEnemy)
                    {
                        if(character.activeSelf)
                        {
                            alliesCounter++;
                        }
                    }


                }
            }
            if(enemiesCounter == 0)
            {
                enemiesLeft = false;
            }
            else
            {
                enemiesCounter = 0;
            }
            if (alliesCounter == 0)
            {
                alliesLeft = false;
            }
            else
            {
                alliesCounter = 0;
            }
            enemiesCounter = 0;
            //Checks if there's no allies or enemies left, in either case it ends the battle
            if(!enemiesLeft || !alliesLeft)
            {
                //Checks every character, if it's an enemy it destroys them
                foreach (GameObject character in characters)
                {
                    if(character.GetComponent<s_CharacterStats>().isEnemy)
                    {
                        Destroy(character);

                    }
                   
                }
                EndBattle();
                

            }

            //If the battle is still going on, it triggers everything that happens at te end of a turn and goes into the next
            if (enemiesLeft && alliesLeft)
            {
                if(currentCharacter)
                {
                    UpdateStatsByComfort(currentCharacter);
                    UpdateHUD(currentCharacter);

                }
                
                turnCounter++;

            }
        }
    }

    void EndBattle()
    {
        //If the player wins, the enemy in the overworld is destroyed
        if(!enemiesLeft)
        {
            if(enemyOverworld)
            {
                enemyOverworld.SetActive(true);
                Destroy(enemyOverworld);

            }

        }
        //else, it appears again
        else
        {
            enemyOverworld.SetActive(true);
        }
        //Activates character in overworld after finishing the fight
        mainCharacterPrefab.SetActive(true);
        allyLeftPrefab.SetActive(true);
        allyRightPrefab.SetActive(true);

        mainCharacterPrefab.transform.position = mainCharacterPrefab.GetComponent<s_CharacterMovement>().originalPosition;
        allyLeftPrefab.transform.position = new Vector3(0, 0, 200);
        allyRightPrefab.transform.position = new Vector3(0, 0, 200);

        mainCharacterPrefab.GetComponent<s_CharacterStats>().FullHeal();
        allyLeftPrefab.GetComponent<s_CharacterStats>().FullHeal();
        allyRightPrefab.GetComponent<s_CharacterStats>().FullHeal();




        mainCharacterPrefab.GetComponent<s_CharacterStats>().HealAllEffects();
        allyLeftPrefab.GetComponent<s_CharacterStats>().HealAllEffects();
        allyRightPrefab.GetComponent<s_CharacterStats>().HealAllEffects();
        mainCharacterPrefab.GetComponent<s_CharacterStats>().DisableEffectImages();
        allyLeftPrefab.GetComponent<s_CharacterStats>().DisableEffectImages();
        allyRightPrefab.GetComponent<s_CharacterStats>().DisableEffectImages();

        mainCharacterPrefab.GetComponent<s_CharacterMovement>().enabled = true;
        mainCharacterPrefab.GetComponent<NavMeshAgent>().enabled = true;
        buttonCanvas.enabled = false;
        state = BattleState.endBattle;

        this.enabled = false;
    }

    //Updates hud(buttons, pointer...)
    void UpdateHUD(s_CharacterStats currentCharacter)
    {
        pointer.transform.position = new Vector3(currentCharacter.transform.position.x, 3.5f, currentCharacter.transform.position.z);
        

        //Checks Effect Images;
        //Burn
        if(currentCharacter.burn)
        {
            currentCharacter.burnImage.SetActive(true);
        }
        else
        {
            currentCharacter.burnImage.SetActive(false);
        }

        //Bleed
        if (currentCharacter.bleed)
        {
            currentCharacter.bleedImage.SetActive(true);
        }
        else
        {
            currentCharacter.bleedImage.SetActive(false);
        }

        //Attack
        if(currentCharacter.attack > currentCharacter.originalAttack)
        {
            currentCharacter.attackUpImage.SetActive(true);
            currentCharacter.attackDownImage.SetActive(false);
        }
        if (currentCharacter.attack < currentCharacter.originalAttack)
        {
            currentCharacter.attackDownImage.SetActive(true);
            currentCharacter.attackUpImage.SetActive(false);
        }
        if(currentCharacter.attack == currentCharacter.originalAttack)
        {
            currentCharacter.attackDownImage.SetActive(false);
            currentCharacter.attackUpImage.SetActive(false);
        }



        if (currentCharacter.isEnemy)
        {
            buttonCanvas.gameObject.SetActive(false);
        }
        else
        {
            buttonCanvas.gameObject.SetActive(true);
        }
        button1.GetComponent<s_ButtonFunction>().GetCurrentCharacter(currentCharacter);
        button1Text.text = currentCharacter.ability1.abilityName;

        button2.GetComponent<s_ButtonFunction>().GetCurrentCharacter(currentCharacter);
        button2Text.text = currentCharacter.ability2.abilityName;

        button3.GetComponent<s_ButtonFunction>().GetCurrentCharacter(currentCharacter);
        button3Text.text = currentCharacter.ability3.abilityName;

        button4.GetComponent<s_ButtonFunction>().GetCurrentCharacter(currentCharacter);
        button4Text.text = currentCharacter.ability4.abilityName;

        button5.GetComponent<s_ButtonFunction>().GetCurrentCharacter(currentCharacter);
        button5Text.text = "Talk with Ally";
    }

    //Changes stats according to the character's current comfort amount
    void UpdateStatsByComfort(s_CharacterStats currentCharacter)
    {
        currentCharacter.attack = Mathf.RoundToInt(currentCharacter.originalAttack * (currentCharacter.comfort / (float)currentCharacter.maxComfort + 0.1f));
        if(currentCharacter.attack > currentCharacter.originalAttack )
        {
            currentCharacter.attack = currentCharacter.originalAttack;
        }

        if(currentCharacter.attack == 0)
        {
            currentCharacter.attack = 1;
        }
        currentCharacter.defense = Mathf.RoundToInt(currentCharacter.originalDefense * (currentCharacter.comfort / (float)currentCharacter.maxComfort + 0.1f));
        if (currentCharacter.defense > currentCharacter.originalDefense)
        {
            currentCharacter.defense = currentCharacter.originalDefense;
        }
        if (currentCharacter.defense == 0)
        {
            currentCharacter.defense = 1;
        }
    }

    public void GetNextCharacter()
    {
        ShowNextCharacter(nextCharacter);
    }

    
    //Shows whose character is next turn
    public void ShowNextCharacter(s_CharacterStats character)
    {
        if(!showingNextCharacter)
        {
            buttonCanvas.gameObject.SetActive(false);
            nextCharacterPointer.SetActive(true);
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = nextCharacter.transform;
            nextCharacterPointer.transform.position = new Vector3(character.transform.position.x, 3.5f, character.transform.position.z);
            showingNextCharacter = true;
        }
        else
        {
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = mainCharacterCameraFollow;
            buttonCanvas.gameObject.SetActive(true);
            nextCharacterPointer.SetActive(false);
            showingNextCharacter = false;
        }
        
    }
}
