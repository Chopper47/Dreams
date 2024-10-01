using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class s_CharacterStats : MonoBehaviour
{
    [SerializeField] private UnityEvent TakeDamageEvent;
    public s_Unit unit;

    [Header("Stats")]
    public int level;
    public string unitName;
    public int speed;
    public int originalAttack;
    public int originalDefense;
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

    [Header("Effects")]
    public List<bool> effects;
    public bool burn;
    private int burnDamage;
    public GameObject burnParticlesPrefab;
    public bool bleed;
    private int bleedDamage;
    public GameObject bleedParticlesPrefab;
    public bool berserk;
    public bool regeneration;
    private int regeneationHeal;
    public GameObject regenParticlesPrefab;


    [Header("EffectImages")]
    public GameObject burnImage;
    public GameObject bleedImage;
    public GameObject attackDownImage;
    public GameObject attackUpImage;
    public GameObject regenImage;

    [Header("Other")]
    public bool isEnemy;
    public GameObject target;
    public bool hasChosen;
    public bool hasFinishedEffects;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private s_CameraShake virtualCameraShake;
    [SerializeField] private GameObject gameController;
    [SerializeField] public s_TurnManager turnManager;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject comfortBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI comfortText;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Assign Stats
        level = unit.level;
        unitName = unit.unitName;
        speed = unit.speed;
        originalAttack = unit.attack;   
        originalDefense = unit.defense;
        hp = unit.hp;
        maxHp = unit.maxHp;
        comfort = unit.comfort;
        maxComfort = unit.maxComfort;
        isEnemy = unit.isEnemy;

        hpText.text = hp.ToString() + "/" + maxHp.ToString();
        comfortText.text = comfort.ToString() + "/" + maxComfort.ToString();
        //Assign Abilities
        ability1 = unit.ability1;
        ability2 = unit.ability2;
        ability3 = unit.ability3;
        ability4 = unit.ability4;

        //Assign original stats
        attack = originalAttack;
        defense = originalDefense;

        //Assign effect stats
        
        burnDamage = Mathf.RoundToInt(maxHp * 0.1f);
        bleedDamage = Mathf.RoundToInt(maxHp * 0.05f);
        regeneationHeal = -Mathf.RoundToInt(maxHp * 0.05f);

        //Assign Camera
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");
        virtualCameraShake = virtualCamera.GetComponent<s_CameraShake>();
        Debug.Log(comfortText.enabled);

        gameController = GameObject.FindGameObjectWithTag("GameController");
        

    }
    private void Update()
    {
        if(comfortText == null)
        {
            Debug.Log("comfort text gone");
        }
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        hpBar.GetComponent<s_UpdateUIBars>().UpdateBar(hp, maxHp);
        hpText.text = hp.ToString() + "/" + maxHp.ToString();

        //Creates damage text if damage is above 0 (red), and healing text if damage is below zero(green)
        if (dmg > 0)
        {
            DynamicTextManager.CreateText(new Vector3(transform.position.x, transform.position.y+2, transform.position.z), dmg.ToString(), DynamicTextManager.defaultData);

        }

        if(dmg < 0)
        {
            DynamicTextManager.CreateText(new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), (-dmg).ToString(), DynamicTextManager.healData);
        }

        //Camera shake when hit by enemies
        if(!isEnemy)
        {
            if(dmg > 0)
            {
                virtualCameraShake.enabled = true;

            }
        }

        if(hp > maxHp)
        {
            hp = maxHp;
            hpText.text = hp.ToString() + "/" + maxHp.ToString();
        }
        if (hp <= 0)
        {
            hp = 1;
            hasFinishedEffects = true;
            turnManager.characterTurnList.Remove(gameObject);
            turnManager.numberOfCharacters--;
            hpText.text = hp.ToString() + "/" + maxHp.ToString();
            if (isEnemy)
            {
                Destroy(gameObject);

            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ChangeComfort(int comfortDmg)
    {
        comfort -= comfortDmg;

        if(comfort >= maxComfort)
        {
            comfort = maxComfort;
        }
        if(comfort < 0)
        {
            comfort = 0;
        }
        comfortText.text = comfort.ToString() + "/" + maxComfort.ToString();

        hpBar.GetComponent<s_UpdateUIBars>().UpdateBar(hp, maxHp);
        comfortBar.GetComponent<s_UpdateUIBars>().UpdateBar(comfort, maxComfort);
        
    }
    public void ActivateAbility(s_Ability abilityToActivate)
    {
        hasChosen = true;
        if(abilityToActivate.abilityAudio)
        {
            Debug.Log(gameController);
            gameController.GetComponent<AudioSource>().clip = abilityToActivate.abilityAudio;
            gameController.GetComponent<AudioSource>().Play();
        }
        ActivateAbilityAnimation();
        DynamicTextManager.CreateText(new Vector3(0, 6, 0), unitName + " used " + abilityToActivate.abilityName, DynamicTextManager.textData);
        if(abilityToActivate.ignoreStats)
        {
            target.GetComponent<s_CharacterStats>().TakeDamage(abilityToActivate.damage);
        }
        else
        {
            target.GetComponent<s_CharacterStats>().TakeDamage(Mathf.RoundToInt(abilityToActivate.damage * attack / target.GetComponent<s_CharacterStats>().defense));

        }

        
        ChangeComfort(abilityToActivate.comfortCost);
        ExtraEffect(abilityToActivate);
        
    }

    public void TalkWithAlly()
    {
        if(target != gameObject)
        {
            DynamicTextManager.CreateText(new Vector3(0, 6, 0), unitName + "talked with his companion" , DynamicTextManager.textData);
            hasChosen = true;
            target.GetComponent<s_CharacterStats>().ChangeComfort(-50);
            ChangeComfort(-50);

        }
        else
        {
            DynamicTextManager.CreateText(new Vector3(0, 6, 0), "You need someone to talk to", DynamicTextManager.textData);
        }
    }
    
    //Applies extra effects at the end of the turn
    public IEnumerator EndTurnEffects()
    {
        
        if(regeneration)
        {
            regenImage.SetActive(true);
            TakeDamage(regeneationHeal);
            GameObject regenParticles = Instantiate(regenParticlesPrefab, transform);
            yield return new WaitForSeconds(0.5f);
        }
        if (this)
        {
            if (burn)
            {
                TakeDamage(burnDamage);
                GameObject burnParticles = Instantiate(burnParticlesPrefab, transform);
                yield return new WaitForSeconds(0.5f);

            }

        }
        if (this)
        {
            if (bleed)
            {
                TakeDamage(bleedDamage);
                ChangeComfort(5);
                GameObject bleedParticles = Instantiate(bleedParticlesPrefab, transform);
                yield return new WaitForSeconds(0.5f);
            }

        }
        yield return new WaitForSeconds(1f);
        if (this)
        {
            hasFinishedEffects = true;
            StopCoroutine(EndTurnEffects());


        }
    }

    /// <summary>
    /// Checks if the ability applies any extra effects
    /// </summary>
    /// <param name="abilityToAdd"></param>
    public void ExtraEffect(s_Ability abilityToAdd)
    {
        s_Ability abilityChecks = abilityToAdd;
        s_CharacterStats targetStats = target.GetComponent<s_CharacterStats>();
        if (abilityChecks.burn)
        {
            targetStats.burn = true;
            //Burned
            targetStats.burnImage.SetActive(true);
            
        }
        if(abilityChecks.absorb)
        {
            TakeDamage(-abilityChecks.damage);
        }

        if (abilityChecks.decreaseComfort)
        {
            targetStats.ChangeComfort(abilityChecks.comfortDamage);

        }
        if(abilityChecks.berserk)
        {
            targetStats.berserk = true; 
        }

        if (abilityChecks.bleed)
        {
            targetStats.bleed = true;
            targetStats.bleedImage.SetActive(true);
        }
        if(abilityChecks.regeneration)
        {
            regeneration = true;
            regenImage.SetActive(true);
        }
        if(abilityChecks.healTargetEffects)
        {
            targetStats.HealAllEffects();
            
        }
        if(abilityChecks.healSelfEffects)
        {
            HealAllEffects();
        }
        if(abilityChecks.HealEnemiesComfort)
        {
            GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Character");
            foreach (GameObject enemy in enemyArray)
            {
                if(enemy.GetComponent<s_CharacterStats>().isEnemy)
                {
                    enemy.GetComponent<s_CharacterStats>().ChangeComfort(-25);
                }
            }
        }
    }

    public void FullHeal()
    {
        

        hp = maxHp;
        comfort = maxComfort;
        hpBar.GetComponent<s_UpdateUIBars>().UpdateBar(hp, maxHp);
        comfortBar.GetComponent<s_UpdateUIBars>().UpdateBar(comfort, maxComfort);
        comfortText.text = comfort.ToString() + "/" + maxComfort.ToString();
        hpText.text = hp.ToString() + "/" + maxHp.ToString();


    }
    public void HealAllEffects()
    {
        //Heal effects
        burn = false;
        bleed = false;
        berserk = false;
        
        burnImage.SetActive(false); 
        bleedImage.SetActive(false);
    }

    public void DisableEffectImages()
    {
        attackDownImage.SetActive(false);
        attackUpImage.SetActive(false);
        burnImage.SetActive(false);
        bleedImage.SetActive(false);
        regenImage.SetActive(false);
    }

    private void ActivateAbilityAnimation()
    {
        if(animator)
        {
            Debug.Log("Animation");
            animator.SetTrigger("Attack");
        }

        else
        {
            return;
        }
    }

    private void DeactivateAbilityAnimation()
    {
        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        else
        {
            return;
        }
    }
}
