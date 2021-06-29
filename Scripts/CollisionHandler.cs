using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    public float levelLoadDelay = 1f;
    public AudioClip success;
    public AudioClip crash;

    AudioSource audioSource;
    bool isTransitioning = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other) {
        if(isTransitioning) {
            return;
        }

        switch(other.gameObject.tag) {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        //TODO add particle effect upon success
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); //SceneManager.LoadScene(0);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        //TODO add particle effect upon success
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

}
