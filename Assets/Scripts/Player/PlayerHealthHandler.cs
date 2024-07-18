using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    public static int maxHealth = 150;
    static public int currentHealth;
    [SerializeField] private HealthBarHandler healthBar;
    private MeshRenderer meshRenderer;
    private MeshRenderer []childrensMeshRenderers;
    private Color originalColor;
    private Color []childrensOriginalColors;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }

        childrensMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        childrensOriginalColors = new Color[childrensMeshRenderers.Length];
        for (int i = 0; i < childrensMeshRenderers.Length; i++)
        {
            childrensOriginalColors[i] = childrensMeshRenderers[i].material.color;
        }
    }

    // Function to handle taking damage
    public void TakeDamage(int damage)
    {
        //return if player is not active
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        StartCoroutine(FlashRed());

        // Check if the player's health is less than or equal to 0 if so, respawn the player
        if (currentHealth <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void ResetHealthAndColor()
    {
        currentHealth = maxHealth;
        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }
    }

    // Function to flash the player red when taking damage
    private IEnumerator FlashRed()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.red;
        }

        foreach (MeshRenderer childMeshRenderer in childrensMeshRenderers)
        {
            childMeshRenderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }

        for (int i = 0; i < childrensMeshRenderers.Length; i++)
        {
            childrensMeshRenderers[i].material.color = childrensOriginalColors[i];
        }
    }
}
