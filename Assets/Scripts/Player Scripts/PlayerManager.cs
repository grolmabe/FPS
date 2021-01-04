using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float health;
    public Transform healthText; //Textbox that displays health value
    private string fieldLabel = "Health: ";


    // Start is called before the first frame update
    void Start()
    {
        healthText.GetComponent<Text>().text = fieldLabel + health.ToString();
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
            health = 0.0f;
        }
        healthText.GetComponent<Text>().text = fieldLabel + health.ToString();
    }
}
