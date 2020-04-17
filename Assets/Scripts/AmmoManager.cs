using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public enum AmmoType { bullets, plasmaUnits, shotgunShells }
    public int[] count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Remove(AmmoType type, int numRequested, bool allOrNone = true)
    {
        int numReturned = 0;

        if ( numRequested > count[(int)type])
        {
            if (allOrNone == false)
            {
                numReturned = count[(int)type];
                count[(int)type] = 0;
            }
        }
        else if ( numRequested > 0 )
        {
            numReturned = numRequested;
            count[(int)type] -= numRequested;
        }
        return numReturned;
    }

    public int Add(AmmoType type, int num)
    {
        if (num > 0)
        {
            count[(int)type] += num;
        }
        return count[(int)type];
    }

}
