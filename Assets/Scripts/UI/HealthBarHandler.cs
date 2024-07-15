using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{

    
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] public TextMeshProUGUI healthText;
    
    public void SetMaxHealth(int health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
        healthText.text = health + "/" + health;
    }

    public void SetHealth(int health)
    {
        healthBarSlider.value = health;
        healthText.text = health + "/" + healthBarSlider.maxValue;
    }
}

