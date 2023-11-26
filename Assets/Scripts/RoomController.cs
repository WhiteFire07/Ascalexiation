using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public GameObject vcam;

    public bool affectPlayer;

    public dashType setDash;

    public enum dashType
    {
        normal,
        noUp,
        disabled
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            vcam.SetActive(true);
            if (affectPlayer)
            {
                if (setDash == dashType.normal)
                {
                    controller.setDash();
                    controller.setUpDash();
                }
                if (setDash == dashType.noUp)
                {
                    controller.setDash();
                    controller.setUpDash(false);
                }
                if (setDash == dashType.disabled)
                {
                    controller.setDash(false);
                }
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            vcam.SetActive(false);
        }
    }
}
