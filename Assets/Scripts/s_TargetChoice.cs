using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_TargetChoice : MonoBehaviour
{
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private GameObject hoverTargetGameObject;
    [SerializeField] private GameObject targetGameObject;

    [SerializeField] public s_CharacterStats currentCharacter;
    public int abilityToActivate;
    // Update is called once per frame

    private void Start()
    {
        
    }
    void Update()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Makes a red outline when hovering enemies and a green one when hovering allies
        if (Physics.Raycast(ray, out hit, 100f, TargetLayer))
        {
            if(hit.transform.gameObject != null)
            {
                hoverTargetGameObject = hit.transform.gameObject;
                if(hoverTargetGameObject.GetComponent<Outline>() == null)
                {
                    var outline = hoverTargetGameObject.AddComponent<Outline>();
                    if(hoverTargetGameObject.layer == 8)
                    {
                        outline.OutlineColor = Color.red;

                    }
                    if (hoverTargetGameObject.layer == 9)
                    {
                        outline.OutlineColor = Color.green;

                    }

                    outline.OutlineWidth = 10f;

                }
                hoverTargetGameObject.GetComponent<Outline>().enabled = true;
                
            }


        }

        else
        {
            if(hoverTargetGameObject != null)
            {
                if(hoverTargetGameObject.GetComponent<Outline>() != null)
                {
                    hoverTargetGameObject.GetComponent<Outline>().enabled = false;

                }

            }
        }


        //Chooses target for attack when clicking on enemy
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickHit;

            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            
            if (Physics.Raycast(clickRay, out clickHit, 100f, TargetLayer))
            {
                
                targetGameObject = clickHit.transform.gameObject;
                if (targetGameObject.layer == 8 || targetGameObject.layer == 9)
                {
                    
                    currentCharacter.GetComponent<s_CharacterStats>().target = targetGameObject;
                    switch(abilityToActivate)
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
                        case 5:
                            if(targetGameObject.GetComponent<s_CharacterStats>().isEnemy == false && targetGameObject != currentCharacter)
                            {

                                currentCharacter.TalkWithAlly();
                            }

                            
                            else if (targetGameObject == currentCharacter)
                            {
                                Debug.Log("YOU CAN'T TALK WITH YOURSELF");
                            }

                            else if(targetGameObject.GetComponent<s_CharacterStats>().isEnemy == true)
                            {
                                Debug.Log("YOU CAN'T TALK WITH ENEMIES");
                            }
                            break;
                        default:
                            Debug.Log("NOT A VALID ABILITY");
                            break;
                    }
                    hoverTargetGameObject.GetComponent<Outline>().enabled = false;
                    gameObject.GetComponent<s_TargetChoice>().enabled = false;
                }
            }
        }

    }

    
}
