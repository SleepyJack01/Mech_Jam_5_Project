using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar3 : MonoBehaviour
{
    //DONT ASK WHY ITS CALLED HELATH BAR 3
    // left all as public for testing purpose
    // need jack to make his code public to intergrate
    public float _maxHealth = 100;
    public float _currentHealth;
    // gets the canvas fill to show as background when health is missing
    //text is place holder unless we wnat to keep it
    [SerializeField] public Image _healthBarFill;
    [SerializeField] public TextMeshProUGUI _healthText;
    void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = "Health: " + _currentHealth;
    }

    public void UpdateHealth(float amount)
    {
        _currentHealth += amount;
        _healthText.text = "Health: " + _currentHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / _maxHealth;
        _healthBarFill.fillAmount = targetFillAmount;
    }
}

