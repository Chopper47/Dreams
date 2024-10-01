using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUnit", menuName = "Units")]
public class s_Unit : ScriptableObject
{
    [Header("Stats")]
    public int level;
    public string unitName;
    public int speed;
    public int attack;
    public int defense;
    public int hp;
    public int maxHp;
    public int comfort;
    public int maxComfort;

    [Header("Abilities")]
    public s_Ability ability1;
    public s_Ability ability2;
    public s_Ability ability3;
    public s_Ability ability4;

    [Header("Other")]
    public bool isEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
