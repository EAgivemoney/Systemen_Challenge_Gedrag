using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalsController : MonoBehaviour
{
    public enum VitalType { Health, Hunger, Thirst }

    [Header("[Health Settings]")]
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthImage;
    public Text healthTextQty;
    public Color fullHealthColor;
    public Color emptyHealthColor;

    [Header("[Hunger Settings]")]
    public int maxHunger = 100;
    public int currentHunger;
    public Image hungerImage;
    public Text hungerTextQty;
    public Color fullHungerColor;
    public Color emptyHungerColor;

    [Header("[Thirst Settings]")]
    public int maxThirst = 100;
    public int currentThirst;
    public Image thirstImage;
    public Text thirstTextQty;
    public Color fullThirstColor;
    public Color emptyThirstColor;

    private const float FillAmountFactor = 100f; // Factor for fill amount calculations

    private void Start()
    {
        InitializeVitals();
    }

    private void Update()
    {
        ClampVitals();
    }

    private void InitializeVitals()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        currentHunger = maxHunger;
        UpdateHungerUI();

        currentThirst = maxThirst;
        UpdateThirstUI();
    }

    private void ClampVitals()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        currentThirst = Mathf.Clamp(currentThirst, 0, maxThirst);
    }

    public void Increase(int value, VitalType type)
    {
        switch (type)
        {
            case VitalType.Health:
                currentHealth += value;
                UpdateHealthUI();
                break;

            case VitalType.Hunger:
                currentHunger += value;
                UpdateHungerUI();
                break;

            case VitalType.Thirst:
                currentThirst += value;
                UpdateThirstUI();
                break;
        }
    }

    public void Decrease(int value, VitalType type)
    {
        switch (type)
        {
            case VitalType.Health:
                currentHealth -= value;
                UpdateHealthUI();
                break;

            case VitalType.Hunger:
                currentHunger -= value;
                UpdateHungerUI();
                break;

            case VitalType.Thirst:
                currentThirst -= value;
                UpdateThirstUI();
                break;
        }
    }

    private void UpdateHealthUI()
    {
        healthImage.fillAmount = (float)currentHealth / maxHealth;
        healthImage.color = Color.Lerp(emptyHealthColor, fullHealthColor, healthImage.fillAmount);
        healthTextQty.text = currentHealth.ToString();
    }

    private void UpdateHungerUI()
    {
        hungerImage.fillAmount = (float)currentHunger / maxHunger;
        hungerImage.color = Color.Lerp(emptyHungerColor, fullHungerColor, hungerImage.fillAmount);
        hungerTextQty.text = currentHunger.ToString();
    }

    private void UpdateThirstUI()
    {
        thirstImage.fillAmount = (float)currentThirst / maxThirst;
        thirstImage.color = Color.Lerp(emptyThirstColor, fullThirstColor, thirstImage.fillAmount);
        thirstTextQty.text = currentThirst.ToString();
    }
}
