using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 20f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

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
            RespondToThrustInput();
            RespondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;


        }
    }

    private void StartDeathSequence()
    {
        state = State.DEAD;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("ReloadLevel", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.TRANSCENDING;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private  void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        print(scene.name);
        SceneManager.LoadScene(scene.name); 
        
        state = State.ALIVE;
    }

    private void LoadNextLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        print(scene.name);
        int currentSceneIndex = scene.buildIndex;
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            print("Good job");
            currentSceneIndex = 0;
        } else
        {
            currentSceneIndex++;
        }
        SceneManager.LoadScene(currentSceneIndex);
        state = State.ALIVE;
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();

    }
}
