using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 20f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Trascending }
    State state = State.Alive;
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (state == State.Alive) //if dead => stop sound
        {
            Thrust();
            Rotate();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Finish":
                state = State.Trascending;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.Dying;
                ReloadLevel();
                break;


        }
    }

    private  void ReloadLevel()
    {
        SceneManager.LoadScene(0); //current
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //next
        state = State.Alive;
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        };
        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
