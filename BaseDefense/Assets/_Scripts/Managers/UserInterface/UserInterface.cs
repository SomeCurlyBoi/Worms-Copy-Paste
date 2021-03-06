﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    #region ShowInEditor

    [Header("Cursor:")]
    [SerializeField] Texture2D crosshairImage;

    [Header("Weapon:")]
    [SerializeField] string playerWeapon_name;
    [SerializeField] string weaponImage_name;
    [SerializeField] string currentAmmo_name;
    [SerializeField] string extraAmmo_name;

    [Header("Reload:")]
    [SerializeField] string reloadtime_name;
    [SerializeField] string reloadWheelCurrent_name;
    [SerializeField] string reloadwheelBG_name;

    [Header("Health:")]
    [SerializeField] string healthbar_name;
    [SerializeField] string health_name;
    [SerializeField] string maxhealth_name;
         
    [Header("JetPack:")]
    [SerializeField] string fuelbar_name;
    [SerializeField] string fuel_name;
    [SerializeField] string maxfuel_name;

    [Header("Base:")]
    [SerializeField] string baseName;
    [SerializeField] string baseHealthbar_name;
    [SerializeField] string baseHealth_name;
    [SerializeField] string baseMaxHealth_name;

    [Header("Money:")]
    [SerializeField] string money_name;

    [Header("Wave:")]
    [SerializeField] string waveInfo_name;
    [SerializeField] float waveInfoTime;
    [SerializeField] string startWaveText;

    [Header("BuildMode:")]
    public LayerMask clickLayer;
    public string shop_name;

    [Header("Debug:")]
    [SerializeField] public string debug_name;

    #endregion
    #region HideInEditor

    //Camera
    Camera mainCamera;

    //Ammo
    Image weaponImage_Image;
    Text ammo_Text;
    Text extraAmmo_Text;

    //ReloadWheel
    Text reloadtime_Text;
    Image reloadWheelCurrent_Image;
    Image reloadWheelBackGround_Image;

    //Health
    Image healthBar_Image;
    Text health_Text;
    Text maxHealth_Text;

    //Fuel
    Image fuelBar_Image;
    Text fuel_Text;
    Text maxFuel_Text;

    // BaseHealth
    Image baseHealthBar_Image;
    Text baseHealth_Text;
    Text maxBaseHealth_Text;

    //Debug
    Text debugText;

    // Ammo
    private int ammo;
    private int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            AmmoChange(ammo);
        }
    }
    private int extraAmmo;
    private int ExtraAmmo
    {
        get => extraAmmo;
        set
        {
            extraAmmo = value;
            ExtraAmmoChange(extraAmmo);
        }
    }

    // Health
    private int currentHealth;
    int maxHealth;
    float healthPercent;
    private int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            HealthPercent = (float)CurrentHealth / MaxHealth;
        }
    }
    int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            HealthPercent = (float)CurrentHealth / MaxHealth;
        }
    }
    float HealthPercent
    {
        get
        {
            return healthPercent;
        }
        set
        {
            healthPercent = value;
            healthBar_Image.fillAmount = HealthPercent;
        }
    }

    // Fuel
    private int currentFuel;
    private int maxFuel;
    private float fuelPercent;
    private int CurrentFuel
    {
        get
        {
            return currentFuel;
        }
        set
        {
            currentFuel = value;
            FuelPercent = (float)currentFuel / MaxFuel;
        }
    }
    private int MaxFuel
    {
        get
        {
            return maxFuel;
        }
        set
        {
            maxFuel = value;
            FuelPercent = (float)CurrentFuel / maxFuel;
        }
    }
    private float FuelPercent
    {
        get
        {
            return fuelPercent;
        }
        set
        {
            fuelPercent = value;
            fuelBar_Image.fillAmount = fuelPercent;
        }
    }

    // BaseHealth
    private int currentBaseHealth;
    private int maxBaseHealth;
    private float baseHealthPercent;
    private int CurrentBaseHealth
    {
        get
        {
            return currentBaseHealth;
        }
        set
        {
            currentBaseHealth = value;
            BaseHealthPercent = (float)currentBaseHealth / MaxBaseHealth;
        }
    }
    private int MaxBaseHealth
    {
        get
        {
            return maxBaseHealth;
        }
        set
        {
            maxBaseHealth = value;
            BaseHealthPercent = (float)currentBaseHealth / maxBaseHealth;
        }
    }
    private float BaseHealthPercent
    {
        get
        {
            return baseHealthPercent;
        }
        set
        {
            baseHealthPercent = value;
            baseHealthBar_Image.fillAmount = baseHealthPercent;
        }
    }

    // Shop
    private Text money_Text;
    private Shop shop;

    // Wave
    private Text wave_Text;

    public static UserInterface Instance { get; private set; }

    #endregion

    #region UnityFunctions

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    #endregion
    #region CustomFunctions
    
    // Initialize 
    public void InitializeLevel()
    {
        // Get Camera
        mainCamera = Camera.main;

        // Money
        GameManager.Instance.OnMoneyChaned -= HandleMoneyChange;
        GameManager.Instance.OnMoneyChaned += HandleMoneyChange;
        money_Text = GameObject.Find(money_name).GetComponent<Text>();

        // Get and Set Weapon
        Weapon weaponComponent = GameManager.Instance.Player.transform.Find(playerWeapon_name).GetComponent<Weapon>();
        weaponComponent.OnWeaponChanged += RefreshWeaponData;
        weaponComponent.OnReloadStart += ReloadStart;
        weaponComponent.OnReloadChange += Instance.ReloadChange;
        weaponComponent.OnReloadStop += ReloadStop;
        weaponComponent.OnExtraAmmoChange += ExtraAmmoChange;
        weaponComponent.OnMagChange += AmmoChange;

        weaponImage_Image = GameObject.Find(weaponImage_name).GetComponent<Image>();
        ammo_Text = GameObject.Find(currentAmmo_name).GetComponent<Text>();
        extraAmmo_Text = GameObject.Find(extraAmmo_name).GetComponent<Text>();

        RefreshWeaponData(weaponComponent.WeaponData);

        // Get and Set ReloadWheel
        reloadtime_Text = GameObject.Find(reloadtime_name).GetComponent<Text>();
        reloadWheelCurrent_Image = GameObject.Find(reloadWheelCurrent_name).GetComponent<Image>();
        reloadWheelBackGround_Image = GameObject.Find(reloadwheelBG_name).GetComponent<Image>();

        reloadtime_Text.enabled = false;
        reloadWheelCurrent_Image.enabled = false;
        reloadWheelBackGround_Image.enabled = false;

        // Get And Set Health
        healthBar_Image = GameObject.Find(healthbar_name).GetComponent<Image>();
        health_Text = GameObject.Find(health_name).GetComponent<Text>();
        maxHealth_Text = GameObject.Find(maxhealth_name).GetComponent<Text>();

        Health healthComponent = GameManager.Instance.Player.GetComponent<Health>();
        healthComponent.OnHealthCHange += HealthChange;
        //healthComponent.OnMaxHealthCHange += MaxHealthChange;

        CurrentHealth = healthComponent.HealthPoint;
        MaxHealth = healthComponent.MaxHealthPoint;

        // Get and Set Fuel 
        fuelBar_Image = GameObject.Find(fuelbar_name).GetComponent<Image>();
        fuel_Text = GameObject.Find(fuel_name).GetComponent<Text>();
        maxFuel_Text = GameObject.Find(maxfuel_name).GetComponent<Text>();

        JetPack jetPackComponent = GameManager.Instance.Player.GetComponent<JetPack>();
        jetPackComponent.OnFuelChange += FuelChange;
        jetPackComponent.OnMaxFuelCHange += MaxFuelChange;
        CurrentFuel = jetPackComponent.Fuel;
        MaxFuel = jetPackComponent.MaxFuel;

        // Get and Set BaseHealth
        baseHealthBar_Image = GameObject.Find(baseHealthbar_name).GetComponent<Image>();
        baseHealth_Text = GameObject.Find(baseHealth_name).GetComponent<Text>();
        maxBaseHealth_Text = GameObject.Find(baseMaxHealth_name).GetComponent<Text>();

        Health baseHealth = GameObject.Find(baseName).GetComponent<Health>();
        baseHealth.OnHealthCHange += BaseHealthChange;
        //healthComponent.OnMaxHealthCHange += MaxBaseHealthChange;
        CurrentBaseHealth = baseHealth.HealthPoint;
        baseHealth_Text.text = "/ " + baseHealth.HealthPoint.ToString();

        MaxBaseHealth = baseHealth.MaxHealthPoint;
        maxBaseHealth_Text.text = "/ " + baseHealth.MaxHealthPoint.ToString();

        // Get Debug
        debugText = GameObject.Find(debug_name).GetComponent<Text>();

        // Get and Set Shop
        shop = GameObject.Find(shop_name).GetComponent<Shop>();
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
        shop.enabled = false;
        
        // Set Cursor
        ChangeCursor(crosshairImage);

        // Wave
        wave_Text = GameObject.Find(waveInfo_name).GetComponent<Text>();
        wave_Text.text = startWaveText;

        SpawnManager_Szabolcs.Instance.OnWaveStarted -= HandleWaveStart;
        SpawnManager_Szabolcs.Instance.OnWaveStarted += HandleWaveStart;

        SpawnManager_Szabolcs.Instance.OnWaveEnded -= HandleWaveEnd;
        SpawnManager_Szabolcs.Instance.OnWaveEnded += HandleWaveEnd;

    }

    // Cursor
    public void ChangeCursor(Texture2D newCursor = null)
    {
        Cursor.SetCursor(newCursor, new Vector2(crosshairImage.width/2, crosshairImage.height/2), CursorMode.Auto);
    }
    
    // Weapon
    private void RefreshWeaponData(WeaponData weapon)
    {
        weaponImage_Image.sprite = weapon.sprite;

        Ammo = weapon.AmmoInMag;
        extraAmmo_Text.text =  weapon.infiniteAmmo ? "/ ∞" : "/ " + weapon.ExtraAmmo.ToString();
    }
    
    // Ammo
    private void AmmoChange(int ammo)
    {
        ammo_Text.text = ammo.ToString() + " ";
    }
    private void ExtraAmmoChange(int extraAmmo)
    {
        extraAmmo_Text.text = "/ " + extraAmmo.ToString();
    }
    
    // Reload
    private void ReloadStart()
    {
        weaponImage_Image.color = Color.black;
        reloadtime_Text.enabled = true;
        reloadWheelCurrent_Image.enabled = true;
        reloadWheelBackGround_Image.enabled = true;
    }
    private void ReloadStop()
    {
        weaponImage_Image.color = Color.white;
        reloadtime_Text.enabled = false;
        reloadWheelCurrent_Image.enabled = false;
        reloadWheelBackGround_Image.enabled = false;
    }
    private void ReloadChange(float time, float percent)
    {
        reloadtime_Text.text = time.ToString("0.0");
        reloadWheelCurrent_Image.fillAmount = percent;
    }
    
    // Health
    private void HealthChange(int value)
    {
        CurrentHealth = value;
        health_Text.text = value.ToString() + " ";
    }
    private void MaxHealthChange(int value)
    {
        MaxHealth = value;
        maxHealth_Text.text = "/ " + value.ToString();
    }

    // Fuel
    private void FuelChange(int value)
    {
        CurrentFuel = value;
        fuel_Text.text = value.ToString() + " ";
    }
    private void MaxFuelChange(int value)
    {
        MaxFuel = value;
        maxFuel_Text.text = "/ " + value.ToString();
    }

    // BaseHealth
    private void BaseHealthChange(int value)
    {
        CurrentBaseHealth = value;
        baseHealth_Text.text = value.ToString() + " ";
    }
    // Debug
    public void DebugLog(string text)
    {
        debugText.text = text;
    }

    // BuildMode
    private void HandleGameStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.TrapShop)
        {
            shop.enabled = true;
            shop.Open(Shop.ShopType.Trap);
        }
        else if(state == GameManager.GameState.GunShop)
        {
            shop.enabled = true;
            shop.Open(Shop.ShopType.Gun);
        }
        else
        {
            shop.enabled = true;
            shop.Close();
            shop.enabled = false;
        }
    }

    // Money
    private void HandleMoneyChange(int money)
    {
        money_Text.text = money.ToString();
    }

    // Wave
    private void HandleWaveStart()
    {
        wave_Text.CrossFadeAlpha(0, waveInfoTime, false);
    }

    private void HandleWaveEnd()
    {
        wave_Text.text = startWaveText;
        wave_Text.CrossFadeAlpha(1, waveInfoTime, false);
    }

    #endregion
}
