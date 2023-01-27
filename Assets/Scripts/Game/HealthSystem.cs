using UnityEngine.Events;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth { get; set; }
    private HealthBarHandler _healthBar;
    private NumbersEffect _numbersEffect;

    public UnityEvent OnHealthFinished;

    private void Awake()
    {
        // Set max health
        //_maxHealth = _playerData.MaxHealth;
        _healthBar = GetComponentInChildren<HealthBarHandler>();
        _currentHealth = _maxHealth;
        _numbersEffect = FindObjectOfType<NumbersEffect>();
    }

    public void GetHealing(int value)
    {
        _currentHealth += value;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
        _healthBar.Upload(_maxHealth, _currentHealth);
        _numbersEffect.Activate(value, transform.position, false);

        
    }

    public void GetDamage(int value)
    {
        _currentHealth -= value;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnHealthFinished.Invoke();
        }
        _healthBar.Upload(_maxHealth, _currentHealth);
        _numbersEffect.Activate(value, transform.position, true);

    }

}
