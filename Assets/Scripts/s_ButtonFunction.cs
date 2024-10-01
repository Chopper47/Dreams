using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;
using TMPro;

public class s_ButtonFunction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private s_CharacterStats currentCharacter;
    public int buttonNumber;
    [SerializeField]private GameObject TurnManager;


    [SerializeField]
    private float scaleFactor = 1.1f;
    GameObject panel;
    TextMeshProUGUI descriptionText;
    [SerializeField]
    private float animationDuration = 0.4f;
    [SerializeField]
    private Vector3 originalScale;
    [SerializeField]
    private Vector3 originalPosition;
    [SerializeField]
    private Material material;



    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale*scaleFactor, animationDuration).SetEase(Ease.OutBounce);
        currentCharacter = TurnManager.GetComponent<s_TurnManager>().currentCharacter;
        panel = transform.GetChild(1).gameObject;
        panel.SetActive(true);
        descriptionText = panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        switch(buttonNumber)
        {
            case 1:

                descriptionText.text = "Damage: " + currentCharacter.ability1.damage + "\n"
                    + "Comfort Cost: "  + currentCharacter.ability1.comfortCost + "\n"
                    + "Description: " + currentCharacter.ability1.description;
                break;
            case 2:

                descriptionText.text = "Damage: " + currentCharacter.ability2.damage + "\n"
                    + "Comfort Cost: " + currentCharacter.ability2.comfortCost + "\n"
                    + "Description: " + currentCharacter.ability2.description;
                break;
            case 3:

                descriptionText.text = "Damage: " + currentCharacter.ability3.damage + "\n"
                    + "Comfort Cost: " + currentCharacter.ability3.comfortCost + "\n"
                    + "Description: " + currentCharacter.ability3.description;
                break;
            case 4:

                descriptionText.text = "Damage: " + currentCharacter.ability4.damage + "\n"
                    + "Comfort Cost: " + currentCharacter.ability4.comfortCost + "\n"
                    + "Description: " + currentCharacter.ability4.description;
                break;
            case 5:
                descriptionText.text = "Has a conversation with an ally, making them both relax and recovering their Comfort";
                break;
            default:
                Debug.Log("ERROR: ABILITY DESCRIPTION DOES NOT EXISTS");
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, animationDuration).SetEase(Ease.OutBounce);
        panel = transform.GetChild(1).gameObject;
        panel.SetActive(false);
    }

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        //img = GetComponent<Image>();


    }
    public void ActivateAbility1()
    {
        if(currentCharacter.hasChosen == false)
        {
            s_TargetChoice targetChoice = TurnManager.GetComponent<s_TargetChoice>();
            targetChoice.enabled = true;
            targetChoice.currentCharacter = currentCharacter;
            targetChoice.abilityToActivate = 1;

        }
        
        
    }
    public void ActivateAbility2()
    {
        if(currentCharacter.hasChosen == false)
        {
            s_TargetChoice targetChoice = TurnManager.GetComponent<s_TargetChoice>();
            targetChoice.enabled = true;
            targetChoice.currentCharacter = currentCharacter;
            targetChoice.abilityToActivate = 2;

        }
    }
    public void ActivateAbility3()
    {
        if (currentCharacter.hasChosen == false)
        {
            s_TargetChoice targetChoice = TurnManager.GetComponent<s_TargetChoice>();
            targetChoice.enabled = true;
            targetChoice.currentCharacter = currentCharacter;
            targetChoice.abilityToActivate = 3;

        }
        
    }
    public void ActivateAbility4()
    {
        if (currentCharacter.hasChosen == false)
        {
            s_TargetChoice targetChoice = TurnManager.GetComponent<s_TargetChoice>();
            targetChoice.enabled = true;
            targetChoice.currentCharacter = currentCharacter;
            targetChoice.abilityToActivate = 4;

        }
    }
    public void ActivateTalkWithAlly()
    {
        if (currentCharacter.hasChosen == false)
        {
            s_TargetChoice targetChoice = TurnManager.GetComponent<s_TargetChoice>();
            targetChoice.enabled = true;
            targetChoice.currentCharacter = currentCharacter;
            targetChoice.abilityToActivate = 5;

        }
    }
    public void GetCurrentCharacter(s_CharacterStats characterToCopy)
    {
        currentCharacter = characterToCopy;
    }
}
