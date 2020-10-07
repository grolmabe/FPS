using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public enum WeaponType { pistol, shotgun, musket, revoRifle, bigGun };
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
        int i;

        // If we're not in the process of switching to a new weapon, and we have
        // weapons, check if there is a request to change weapons.
        if ((switchingWeapons != true) && (weapons != null))
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
            else if (Input.GetButtonDown("Weapon3"))
            {
                SelectWeapon(2);
            }
            else if (Input.GetButtonDown("Weapon4"))
            {
                SelectWeapon(3);
            }
			else if (Input.GetButtonDown("Weapon5"))
			{
				SelectWeapon(4);
			}
            if (Input.mouseScrollDelta.y > 0)
            {
                i = 1;
                while ((i < weapons.Length) && (SelectWeapon((weaponIndex + i) % weapons.Length) == false))
                {
                    i++;
                }
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                i = 1;
                // We add weapons.Length here to avoid negative result from modulus operator.
                while ((i < weapons.Length) && (SelectWeapon((weapons.Length + weaponIndex - i) % weapons.Length) == false))
                {
                    i++;
                }
            }
        }
    }

    public bool SelectWeapon(int index)
    {
        Weapon newWeapon;

        // Check that we have weapons and that the requested weapon exists.
        if ((weapons != null) && (index >= 0) && (index < weapons.Length) && (weapons[index] != null))
        {
            newWeapon = weapons[index].GetComponent<Weapon>();
            // Only proceed if there is a valid weapon at the requested index in the array 
            // and the requested weapon has been activated (is in the player's weapon inventory).
            if ((newWeapon != null) && (newWeapon.isActivated))
            {
                //Debug.Log("Switching from weapon index " + weaponIndex + " to weapon index " + index);
                StartCoroutine(SwitchWeapon(index));
                return true;
            }
        }
        return false;
    }

    IEnumerator SwitchWeapon(int index)
    {
        float newY;
        Weapon newWeapon;
        Weapon currentWeapon;

        // Switch from the current weapon to the weapon specified by the given index, including animating it.
        //Debug.Log("In SwitchWeapon().");
        newWeapon = weapons[index].GetComponent<Weapon>();
        switchingWeapons = true;
        // If we're switching to the weapon that is already selected, don't need to lower the current weapon.
        if (index != weaponIndex)
        {
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
        }
        // Raise the new weapon.
        //Debug.Log("Raising new weapon.");
        weapons[index].SetActive(true);
        while (weapons[index].transform.localPosition.y < newWeapon.activeHeight)
        {
            newY = weapons[index].transform.localPosition.y + switchSpeed * Time.deltaTime;
            weapons[index].transform.localPosition = new Vector3(weapons[index].transform.localPosition.x, newY, weapons[index].transform.localPosition.z);
            //Debug.Log("Height: " + weapons[index].transform.localPosition.y + "  Target: " + newWeapon.activeHeight);
            yield return null;
        }
        switchingWeapons = false;
        // Turn the safety off on the selected weapon.
        newWeapon.SafetyOff();
        // Switch to the ammo used by this weapon.
        newWeapon.ammo.SelectAmmoType(newWeapon.ammoType);
        // Update the index of the currently selected weapon.
        weaponIndex = index;
        //Debug.Log("Leaving SwitchWeapon().");
        yield return null;
    }
}
