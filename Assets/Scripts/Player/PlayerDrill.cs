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

    public double InitialHealth => BaseHealth + UpgradeBuffs.instance.DrillHealthAddition;
    public double DrillSpeed => BaseDrillSpeed + UpgradeBuffs.instance.DrillSpeedAddition;
    public double DrillDamage => BaseDrillDamage * UpgradeBuffs.instance.DrillDamageMultiplier;
    public double WeaponDamage => BaseWeaponDamage * UpgradeBuffs.instance.WeaponDamageMultiplier;
    public double WeaponRadius => BaseWeaponRadius + UpgradeBuffs.instance.WeaponRadiusAddition;
    public double WeaponCooldown => BaseWeaponCooldown + UpgradeBuffs.instance.WeaponCooldownAddition;

    public float DrillDepth;

    public PlayerStats BaseDrillStats;
    public PlayerStats DrillStats => UpgradeManager.instance.CalculateEffects(BaseDrillStats);

    [Header("Base Stats")]
    public double BaseHealth;
    public double BaseDrillSpeed;
    public double BaseDrillDamage;
    public double BaseWeaponDamage;
    public double BaseWeaponRadius;
    public double BaseWeaponCooldown;

    [Header("Levels")]
    public int drillHealthLevel = 0;
    public int drillSpeedLevel = 0;
    public int drillDamageLevel = 0;
    public int weaponDamageLevel = 0;
    public int WeaponCooldownLevel = 0;
    public int weaponRadiusLevel = 0;

    [Header("Ability Button Variables")]
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject[] abilityButtons;
    [SerializeField] private double[] abilityCooldownTimers; //stores cooldown timer of button
    private bool[] isAbilityReady = new bool[6];
    Dictionary<string, double> cooldownDict = new Dictionary<string, double>() {
        //stores cooldown times for abilities, uses ability name as key
        //timer values are placeholders for now. add ability upgrading later?
        {"Drill Speed", 10.0 },
        {"Drill Damage", 10.0 },
        {"Weapon Damage", 10.0 },
        {"Recharge Time", 10.0 },
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
        health.SetMaxHealth((float)InitialHealth);

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
    }

    void Update()
    {
        List<Mineral> remainingMinerals = new List<Mineral>();
        foreach (Mineral mineral in collidingMinerals)
        {
            Health mineralHealth = mineral.GetComponent<Health>();
            mineralHealth.TakeDamage((float)DrillDamage * Time.deltaTime);

            if (mineralHealth.CurrentHealth > 0)
                remainingMinerals.Add(mineral);
        }
        collidingMinerals = remainingMinerals;
        IsMoving = collidingMinerals.Count == 0;

        if (IsMoving && GameManager.instance.inRun)
        {
            DrillDepth += (float)DrillSpeed * Time.deltaTime / 10;
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
                }
                else
                {
                    abilityButtons[i].GetComponent<Button>().interactable = true;
                }
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
        if (DrillDamage / 10 >= mineralHealth.CurrentHealth)
            mineralHealth.TakeDamage((float)DrillDamage);
        if (mineralHealth.CurrentHealth > 0)
            collidingMinerals.Add(mineral);
    }

}
