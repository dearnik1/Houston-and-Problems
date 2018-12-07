using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 3f;
    const float TAU = Mathf.PI * 2;
    [Range(0, 1)] [SerializeField] float objectDelay;
    [Range(0, 1)] [SerializeField] float movementFactor;
    Vector3 startingPos;
    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        if (gameObject.tag == "Delayed")
        {
            cycles += 0.25f;    //objectDelay; FIX THIS SOMEONE PLS
        }
        float rawSinWave = Mathf.Sin(cycles * TAU);
        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
