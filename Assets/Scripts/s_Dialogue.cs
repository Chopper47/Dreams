using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class s_Dialogue : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI textComponent;
    private s_CharacterMovement characterMovement;
    public string[] lines;
    public float textSpeed;
    private int index;
    public bool finished;

    private void OnEnable()
    {
        finished = false;
        characterMovement = player.GetComponent<s_CharacterMovement>();
        player.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        characterMovement.enabled = false;
        textComponent.text = string.Empty;
        StartDialogue();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();

            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Length-1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            
            gameObject.SetActive(false);
            characterMovement.enabled = true;
            finished = true;
            index = 0;
        }
    }
}
