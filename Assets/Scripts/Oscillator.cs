using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 3f;
    const float TAU = Mathf.PI * 2;
    [Range(0, 1)] [SerializeField] float objectDelay = 0.25f;
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    private int index;
    private static int count = 0;
    

    static Dictionary<int, float> obstacleDelay = new Dictionary<int, float>();
    // Use this for initialization
    void Start()
    {
        count++;
        print(count);
        index = count;int b = 16000;
        short a = (short)b;
        print(a);
        startingPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;
        cycles += (float)index/(float)count; //FIX THIS SOMEONE PLS
        //print(string.Format( "{0}, {1}", index, cycles));
        float rawSinWave = Mathf.Sin(cycles * TAU);
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }

}
