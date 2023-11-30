using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadSign : MonoBehaviour
{
    public GameObject textPanel;
    public PlayerController player;
    private bool active;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (player.move.ReadValue<Vector2>()[1] > 0)
            {
                textPanel.SetActive(true);
                active = true;
                player.Freeze();
            }
        }
    }

    private void Update()
    {
        if (active)
        {
            if (player.jump.triggered)
            {
                textPanel.SetActive(false);
                active = false;
                player.unFreeze();
            }
        }
    }
}
