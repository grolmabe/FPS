using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public enum AmmoType { bullets, shotgunShells, plasmaUnits }
    public int[] count;
    public Transform shotText; //Textbox that displays ammo count

    private AmmoType currentType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Remove(int numRequested, bool allOrNone = true)
    {
        int numReturned = 0;

        if ( numRequested > count[(int)currentType])
        {
            if (allOrNone == false)
            {
                numReturned = count[(int)currentType];
                count[(int)currentType] = 0;
            }
        }
        else if ( numRequested > 0 )
        {
            numReturned = numRequested;
            count[(int)currentType] -= numRequested;
        }
        shotText.GetComponent<Text>().text = count[(int)currentType].ToString();
        return numReturned;
    }

    public int Add(int num)
    {
        if (num > 0)
        {
            count[(int)currentType] += num;
            shotText.GetComponent<Text>().text = count[(int)currentType].ToString();
        }
        return count[(int)currentType];
    }

    public void SelectAmmoType(AmmoType type)
    {
        if ( ((int)type >= 0) && ((int)type < count.Length))
        {
            currentType = type;
            shotText.GetComponent<Text>().text = count[(int)currentType].ToString();
        }
    }

}
