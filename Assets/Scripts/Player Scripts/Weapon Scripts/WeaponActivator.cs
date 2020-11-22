using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivator : MonoBehaviour
{
    public WeaponSystem.WeaponType weaponType; // The type of weapon this will activate.
    public float rotationTime = 2f;
    public int rounds; // The number of rounds of ammunition included with the activation.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( rotationTime != 0f )
        {
            transform.Rotate(new Vector3(0f, (360f / rotationTime) * Time.deltaTime, 0f));
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        WeaponSystem ws;
        Weapon w;
        AmmoManager am;
        int i;

        if (collider.gameObject.name == "Player")
        {
            ws = collider.gameObject.GetComponentInChildren<WeaponSystem>();
            if (ws != null)
            {
                i = 0;
                while (i < ws.weapons.Length)
                {
                    if ((ws.weapons[i] != null) && ((w = ws.weapons[i].GetComponent<Weapon>()) != null) && (w.weaponType == weaponType))
                    {
                        if ( w.isActivated != true)
                        {
                            w.isActivated = true;
                            ws.SelectWeapon(i);
                        }
                        w.isActivated = true;
                        i = ws.weapons.Length;
                        am = collider.gameObject.GetComponentInChildren<AmmoManager>();
                        if (am != null)
                        {
                            am.Add(rounds, w.ammoType);
                        }
                    }
                    i++;
                }
            }
        }
        Destroy(gameObject);
    }
}
