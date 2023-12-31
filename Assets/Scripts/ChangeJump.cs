using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJump : MonoBehaviour
{
    public float newHeight;
    public ParticleSystem playerParticles;
    public bool playParticles = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.jumpHeight =  newHeight;
            if (playParticles)
            {
                playerParticles.Play();
            }
        }
    }
}
