using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private int currentHealth;
    private MeshRenderer meshRenderer;
    private MeshRenderer []childrensMeshRenderers;
    private Color originalColor;
    private Color []childrensOriginalColors;

    void Start()
    {
        currentHealth = maxHealth;
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

    public void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage.");
        currentHealth -= damage;

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

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

    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject);
    }
}
