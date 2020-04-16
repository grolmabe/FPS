using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject[] weapons = null;

    private int weaponIndex = 0;
    private GunBase currentWeapon = null;
    private float nextTimeToFire;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon != null)
        {
            if (currentWeapon.fullAuto == true)
            {
                if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
                    currentWeapon.Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    currentWeapon.Shoot();
                }
            }
        }
        if (Input.GetButtonDown("Weapon1"))
        {
            SelectWeapon(0);
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            SelectWeapon(1);
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            SelectWeapon((weaponIndex + 1) % weapons.Length);
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            SelectWeapon(weaponIndex == 0 ? weapons.Length - 1 : weaponIndex - 1);
        }

    }

    void SelectWeapon(int index)
    {
        //Debug.Log("Asked to select weapon " + index);
        if ((weapons != null) && (index >=0) && (index < weapons.Length))
        {
            if (weapons[weaponIndex] != null)
            {
                weapons[weaponIndex].SetActive(false);
            }
            if (weapons[index] != null)
            {
                weaponIndex = index;
                currentWeapon = weapons[index].GetComponent<GunBase>();
                weapons[weaponIndex].SetActive(true);
            }
        }
    }
}
