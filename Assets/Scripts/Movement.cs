using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem particleMainBooster;
    [SerializeField] ParticleSystem particleLeftBooster;
    [SerializeField] ParticleSystem particleRightBooster;
    
    Rigidbody rb;
    AudioSource rocketSound;

    // Start is called before the first frame update
    void Start()
    {
        rocketSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            StartThrusting();
        } else
        {
            StopThrusting();
        }
    }
    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        } else
        {
            StopRotation();
        }

    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if(!rocketSound.isPlaying) 
            {
                rocketSound.PlayOneShot(mainEngine);
            }
            if(!particleMainBooster.isPlaying) 
            {
                particleMainBooster.Play();
            }
    }

    private void StopThrusting()
    {
        if (rocketSound.isPlaying)
        {
            rocketSound.Stop();
        }
        if (particleMainBooster.isPlaying)
        {
            particleMainBooster.Stop();
        }
    }


    private void RotateLeft()
    {
        if (!particleRightBooster.isPlaying)
        {
            particleRightBooster.Play();
        }
        ApplyRotation(rotationThrust);
    }
    private void RotateRight()
    {
        if (!particleLeftBooster.isPlaying)
        {
            particleLeftBooster.Play();
        }
        ApplyRotation(-rotationThrust);
    }

    private void StopRotation()
    {
        if (particleRightBooster.isPlaying || particleLeftBooster.isPlaying)
        {
            particleLeftBooster.Stop();
            particleRightBooster.Stop();
        }
    }

    

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //Unfreezing rotation so physics system can take over
    }
}
