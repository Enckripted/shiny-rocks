using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class PlayerDrill : MonoBehaviour
{
    public static PlayerDrill instance;

    public float DrillDepth;
    public bool ShouldDoubleOreYield => abilityActiveTimers[5] > 0;

    public PlayerStats BaseDrillStats;
    public PlayerStats DrillStats => ApplyAbilites(UpgradeManager.instance.CalculateEffects(BaseDrillStats));

    [Header("Ability Button Variables")]
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject[] abilityButtons;
    [SerializeField] private double[] abilityCooldownTimers; //stores cooldown timer of button
    [SerializeField] private double[] abilityActiveTimers = new double[6]; //stores timer for ability
    [SerializeField] private bool[] isAbilityReady = new bool[6];
    Dictionary<string, double> cooldownDict = new Dictionary<string, double>() {
        //stores cooldown times for abilities, uses ability name as key
        //timer values are placeholders for now. add ability upgrading later?
        {"Drill Speed", 10.0 },
        {"Drill Damage", 10.0 },
        {"Weapon Speed", 10.0 },
        {"Weapon Damage", 10.0 },
        {"Weapon Overclock", 20.0},
        {"Ore Doubler", 20.0 }
    };

    [SerializeField] private GameObject healthBarObj;

    public bool IsMoving;
    public Health DrillHealth => health;

    private AudioSource source;
    private List<Mineral> collidingMinerals = new List<Mineral>();
    private ParticleSystem miningParticles;
    private Health health;

    [SerializeField] private AudioClip clip;

    private void StopRun()
    {
        GameManager.instance.StopRun();
    }

    private void OnRunBegin()
    {
        health.SetMaxHealth((float)DrillStats.InitialHealth);

        DrillDepth = 0;

        //set ability cooldowns
        for (int i = 0; i < 6; i++)
        {
            //fetches cooldown using abilityButton text
            abilityCooldownTimers[i] = cooldownDict[abilityButtons[i].transform.Find("AbilityText").GetComponent<TMP_Text>().text];
            abilityButtons[i].transform.Find("CooldownText").GetComponent<TMP_Text>().text = abilityCooldownTimers[i].ToString();
        }
        Array.Fill(isAbilityReady, false);

    }

    void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        health = GetComponent<Health>();

        source.clip = clip;
        abilityActiveTimers = new double[6];
    }

    void Start()
    {
        GameManager.instance.runStartEvent.AddListener(OnRunBegin);
        health.OnDeath += StopRun;

        miningParticles = transform.Find("MiningParticles").gameObject.GetComponent<ParticleSystem>();

        for (int i = 0; i < 6; i++)
        {
            abilityButtons[i] = buttonPanel.transform.GetChild(i).gameObject;
        }

    }


    public void AbilityButtonPress(string buttonName)
    {
        //set the cooldown timer of the pressed button to the associated cooldown in the cooldown dictionary
        GameObject buttonPressed = buttonPanel.transform.Find(buttonName).gameObject;
        string abilityText = buttonPressed.transform.Find("AbilityText").GetComponent<TMP_Text>().text;
        int buttonNum = buttonName[^1] - '0'; //apparently the - '0' part is necessary?? gets the number of the button from the name

        switch (buttonName)
        {
            case "AbilitySubPanel1":
                abilityCooldownTimers[0] = cooldownDict[abilityText];
                break;
            case "AbilitySubPanel2":
                abilityCooldownTimers[1] = cooldownDict[abilityText];
                break;
            case "AbilitySubPanel3":
                abilityCooldownTimers[2] = cooldownDict[abilityText];
                break;
            case "AbilitySubPanel4":
                abilityCooldownTimers[3] = cooldownDict[abilityText];
                break;
            case "AbilitySubPanel5":
                abilityCooldownTimers[4] = cooldownDict[abilityText];
                break;
            case "AbilitySubPanel6":
                abilityCooldownTimers[5] = cooldownDict[abilityText];
                break;
        }

        abilityActiveTimers[buttonNum] = GetActiveTime(abilityText);
    }

    private double GetActiveTime(string abilityName)
    {
        switch (abilityName)
        {
            case "Drill Speed":
                return 2.5;
            case "Drill Damage":
                return 2.5;
            case "Weapon Speed":
                return 2.5;
            case "Weapon Damage":
                return 5.0;
            case "Weapon Overclock":
                return 2.0;
            case "Ore Doubler":
                return 10.0;
            default:
                return 0;
        }
    }

    private PlayerStats ApplyAbilites(PlayerStats curStats)
    {
        if (abilityActiveTimers[0] > 0)
        {
            curStats.DrillSpeed *= 2;
        }
        else if (abilityActiveTimers[1] > 0)
        {
            curStats.DrillDamage *= 2;
        }
        else if (abilityActiveTimers[2] > 0)
        {
            curStats.WeaponCooldown /= 2;
        }
        else if (abilityActiveTimers[3] > 0)
        {
            curStats.WeaponDamage *= 2;
        }
        else if (abilityActiveTimers[4] > 0)
        {
            curStats.WeaponCooldown /= 4;
        }
        return curStats;
    }

    void Update()
    {
        List<Mineral> remainingMinerals = new List<Mineral>();
        foreach (Mineral mineral in collidingMinerals)
        {
            Health mineralHealth = mineral.GetComponent<Health>();
            mineralHealth.TakeDamage((float)DrillStats.DrillDamage * Time.deltaTime);

            if (mineralHealth.CurrentHealth > 0)
                remainingMinerals.Add(mineral);
        }
        collidingMinerals = remainingMinerals;
        IsMoving = collidingMinerals.Count == 0;

        if (IsMoving && GameManager.instance.inRun)
        {
            DrillDepth += (float)DrillStats.DrillSpeed * Time.deltaTime / 10;
            if (miningParticles.isPlaying == false)
            {
                miningParticles.Play();
            }

        }
        else
        {
            miningParticles.Stop();
        }

        source.volume = IsMoving ? 0.25f : 0;

        if (GameManager.instance.inRun)
        {
            for (int i = 0; i < 6; i++)
            {
                if (abilityCooldownTimers[i] > 0)
                {
                    abilityCooldownTimers[i] -= Time.deltaTime;
                    abilityButtons[i].GetComponent<Button>().interactable = false;
                    isAbilityReady[i] = false;
                }
                else
                {
                    abilityButtons[i].GetComponent<Button>().interactable = true;
                    isAbilityReady[i] = true;
                }

                if (abilityActiveTimers[i] > 0)
                    abilityActiveTimers[i] -= Time.deltaTime;

            }
        }

        healthBarObj.SetActive(GameManager.instance.inRun);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Mineral mineral = collision.collider.gameObject.GetComponent<Mineral>();
        if (mineral == null)
            return;

        Health mineralHealth = mineral.GetComponent<Health>();
        if (DrillStats.DrillDamage / 10 >= mineralHealth.CurrentHealth)
            mineralHealth.TakeDamage((float)DrillStats.DrillDamage);
        if (mineralHealth.CurrentHealth > 0)
            collidingMinerals.Add(mineral);
    }

}
