using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public IntReference _maxHealth;

    [SerializeField]
    private ScriptableInt _currentHealth;

    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private Image _ghostBar;

    void Start()
    {
        _currentHealth?.SubscribeToValueChangedWithHistory(UpdateBars);
    }

    void UpdateBars(int previous, int current)
    {
        _healthBar.fillAmount = current * 1f / _maxHealth.Value;
        _ghostBar.fillAmount = previous * 1f / _maxHealth.Value;
        _ghostBar.color = Color.red;
    }

    void Update()
    {
        _ghostBar.fillAmount = Mathf.Lerp(_ghostBar.fillAmount, _healthBar.fillAmount, 3 * Time.deltaTime);
    }

    void OnDestroy()
    {
        _currentHealth?.UnSubscribeToValueChangedWithHistory(UpdateBars);
    }
}
