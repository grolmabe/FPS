using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    protected MeshRenderer meshRenderer;
    protected Color startColor;
    protected float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            startColor = meshRenderer.material.color;
        }
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HitByWeapon(float damage)
    {
        health -= damage;
        // Check to see if the enemy has run out of health.
        if (health < 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color = Color.Lerp(startColor, Color.red, 1.0f - (health / maxHealth));
            }
        }
    }

}
