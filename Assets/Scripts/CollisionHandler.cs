using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    int currentSceneIndex;
    [SerializeField] float delay = 1f;
    [SerializeField] AudioClip audioSuccess;
    [SerializeField] AudioClip audioCollision;
    [SerializeField] ParticleSystem particleSuccess;
    [SerializeField] ParticleSystem particleCollision;
    [SerializeField] bool disableCollision;

    AudioSource audioSource;

    bool isTransitioning;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isTransitioning = false;
        disableCollision = false;
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollision();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if(disableCollision) {
            return;
        }
        switch(other.gameObject.tag){
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish": 
                Debug.Log("Finish");
                StartSuccessSequence();
                break;
            default:
                    Debug.Log("Obstacle");
                    StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        if(!isTransitioning) 
        {
            GetComponent<Movement>().enabled = false;
            audioSource.Stop();
            audioSource.PlayOneShot(audioSuccess);
            particleSuccess.Play();
            toggleTransition(true);
            Invoke("LoadNextLevel", delay);
        }
    }

    void StartCrashSequence()
    {
        if(!isTransitioning) 
        {
            GetComponent<Movement>().enabled = false;
            audioSource.Stop();
            audioSource.PlayOneShot(audioCollision);
            particleCollision.Play();
            toggleTransition(true);
            Invoke("ReloadLevel", delay);
        }
    }
    private void ToggleCollision()
    {
        disableCollision = !disableCollision;
        Debug.Log("Collision disabled: " + disableCollision);
    }
    void ReloadLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex,LoadSceneMode.Single);
        // toggleTransition(false);
    }

    void LoadNextLevel()
    {   
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }
        // toggleTransition(false);
        SceneManager.LoadScene(nextSceneIndex,LoadSceneMode.Single);
    }

    void toggleTransition(bool state)
    {
        isTransitioning = state;
    }
}
