using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAbility", menuName = "Abilities")]
public class s_Ability : ScriptableObject
{
    [Header("Stats")]
    public string abilityName;
    public int damage;
    public int comfortCost;
    public string description;
    [Header("ExtraEffect")]
    public bool healTargetEffects;
    public bool healSelfEffects;
    public bool ignoreStats;
    public bool burn;
    public bool decreaseComfort;
    public int comfortDamage;
    public bool bleed;
    public bool berserk;
    public bool regeneration;
    public bool absorb;
    public bool HealEnemiesComfort;
    public AudioClip abilityAudio;


    
}
