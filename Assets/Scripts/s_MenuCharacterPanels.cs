using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class s_MenuCharacterPanels : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject character;
    private s_CharacterStats characterStats;
    // Start is called before the first frame update
    void Start()
    {
        characterStats = character.GetComponent<s_CharacterStats>();
        textComponent.text = "Name: " + characterStats.unitName + "\n"
            + "Level: " + characterStats.level + "\n"
            + "Hp: " + characterStats.hp + "/" + characterStats.maxHp + "\n"
            + "Comfort: " + characterStats.comfort + "/" + characterStats.maxComfort + "\n"
            + "Attack: " + characterStats.originalAttack + "\n"
            + "Defense: " + characterStats.originalDefense + "\n"
            + "Speed: " + characterStats.speed + "\n";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
