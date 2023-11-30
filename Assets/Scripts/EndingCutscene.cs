using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EndingCutscene : MonoBehaviour
{
    public bool active = false;
    public PlayerController player;
    private Rigidbody2D playerRB;
    private Vector2 smoothVelocity;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            player = other.GetComponent<PlayerController>();
            playerRB = player.GetComponent<Rigidbody2D>();
            playerRB.isKinematic = true;
            player.Freeze();
            player.jump.Disable();
            playerRB.velocity = Vector2.up * 5;
            active = true;
        }
    }
    public void Update()
    {
        if (active)
        {
            playerRB.velocity = Vector2.SmoothDamp(playerRB.velocity, Vector2.zero, ref smoothVelocity, 5);
        }
    }
}
