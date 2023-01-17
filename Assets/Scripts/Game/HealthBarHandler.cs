using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void Upload(int maxHealth, int currentHealth)
    {
        _bar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}
