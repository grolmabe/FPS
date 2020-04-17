using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject[] weapons = null;
    public float switchSpeed = 0.0f;

    private int weaponIndex = 0;
    private bool switchingWeapons = false;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        // If we're in the process of switching to a new weapon, ignore any of these inputs.
        if (switchingWeapons != true)
        {
            // Check to see if the user has requested to switch weapons.
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
    }

    void SelectWeapon(int index)
    {
        //Debug.Log("Switching from weapon index " + weaponIndex + " to weapon index " + index);
        StartCoroutine(SwitchWeapon(index));
    }

    IEnumerator SwitchWeapon(int index)
    {
        float newY;
        Weapon newWeapon;
        Weapon currentWeapon;

        // Switch from the current weapon to the weapon specified by the given index, including animating it.
        //Debug.Log("In SwitchWeapon().");
        // Check to make sure we have weapons and that the requested weapon exists.
        if ((weapons != null) && (index >= 0) && (index < weapons.Length) && (weapons[index] != null))
        {
            newWeapon = weapons[index].GetComponent<Weapon>();
            // Only need to switch weapons if the requested weapon isn't the currently selected one.
            if (index != weaponIndex)
            {
                switchingWeapons = true;
                // Lower the current weapon.
                if (weapons[weaponIndex] != null)
                {
                    //Debug.Log("Lowering current weapon.");
                    currentWeapon = weapons[weaponIndex].GetComponent<Weapon>();
                    if (currentWeapon != null)
                    {
                        currentWeapon.SafetyOn();
                        while (weapons[weaponIndex].transform.localPosition.y > currentWeapon.inactiveHeight)
                        {
                            newY = weapons[weaponIndex].transform.localPosition.y - switchSpeed * Time.deltaTime;
                            weapons[weaponIndex].transform.localPosition = new Vector3(weapons[weaponIndex].transform.localPosition.x, newY, weapons[weaponIndex].transform.localPosition.z);
                            //Debug.Log("Height: " + weapons[weaponIndex].transform.localPosition.y + "  Target: " + currentWeapon.inactiveHeight);
                            yield return null;
                        }
                    }
                    weapons[weaponIndex].SetActive(false);
                }
                // Raise the new weapon.
                //Debug.Log("Raising new weapon.");
                weapons[index].SetActive(true);
                if (newWeapon != null)
                {
                    while (weapons[index].transform.localPosition.y < newWeapon.activeHeight)
                    {
                        newY = weapons[index].transform.localPosition.y + switchSpeed * Time.deltaTime;
                        weapons[index].transform.localPosition = new Vector3(weapons[index].transform.localPosition.x, newY, weapons[index].transform.localPosition.z);
                        //Debug.Log("Height: " + weapons[index].transform.localPosition.y + "  Target: " + newWeapon.activeHeight);
                        yield return null;
                    }
                }
                switchingWeapons = false;
            }
            if (newWeapon != null)
            {
                // Turn the safety off on the selected weapon.
                newWeapon.SafetyOff();
                // Switch to the ammo used by this weapon.
                newWeapon.ammo.SelectAmmoType(newWeapon.ammoType);
            }
            // Update the index of the currently selected weapon.
            weaponIndex = index;
        }
        //Debug.Log("Leaving SwitchWeapon().");
        yield return null;
    }
}
