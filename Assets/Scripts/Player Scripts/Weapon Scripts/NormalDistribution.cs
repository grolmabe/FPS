using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to generate random values that follow a normal (gaussian) distribution.
public class NormalDistribution
{
    private bool nextIsAvailable;
    private float next;

    public float Next()
    {
        float u, v, s, f;

        if (nextIsAvailable)
        {
            nextIsAvailable = false;
            return next;
        }
        do
        {
            u = 2f * Random.value - 1f;
            v = 2f * Random.value - 1f;
            s = u * u + v * v;
        }
        while ((s == 0.0d) || (s >= 1.0));

        f = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        next = v * f;
        nextIsAvailable = true;
        return u * f;
    }

    public float Next(float mean, float sigma = 1f) => mean + sigma * Next();

    public float Next(float mean, float sigma, float min, float max)
    {
        float x = min - 1f; while (x < min || x > max) x = Next(mean, sigma);
        return x;
    }
}
