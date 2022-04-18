using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentHealthText;
    [SerializeField]
    private TextMeshProUGUI maxHealthText;
    private HealthComponent healthComponent;

    private void OnEnable()
    {
        PlayerEvents.OnHealthInitialized += OnHealthInitialized;
    }

    private void OnDisable()
    {
        PlayerEvents.OnHealthInitialized -= OnHealthInitialized;
    }

    private void OnHealthInitialized(HealthComponent healthComponent)
    {
        this.healthComponent = healthComponent;
    }

    private void Update()
    {
        if (!healthComponent)
            return;

        currentHealthText.text = healthComponent.CurrentHealth.ToString();
        maxHealthText.text = healthComponent.MaxHealth.ToString();
    }
}
