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
    enum State { ALIVE, DEAD, TRANSCENDING }
    State state = State.ALIVE;
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (state == State.ALIVE) //if dead => stop sound
        {
            Thrust();
            Rotate();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.ALIVE)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Friendly");
                break;
            case "Finish":
                state = State.TRANSCENDING;
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                state = State.DEAD;
                ReloadLevel();
                break;


        }
    }

    private  void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        print(scene.name);
        SceneManager.LoadScene(scene.name); //current
        
        state = State.ALIVE;
    }

    private void LoadNextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        print(scene.name);
        int currentSceneIndex = scene.buildIndex;
        if (currentSceneIndex == SceneManager.sceneCount)
        {
            print("Good job");
            currentSceneIndex = 0;
        } else
        {
            currentSceneIndex++;
        }
        SceneManager.LoadScene(currentSceneIndex); //next
        state = State.ALIVE;
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
