using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActivator : MonoBehaviour
{
    public GameObject cutscene;
    public GameObject player;
    public Transform playerPos;
    private PlayerController pcont;

    void Start() {
        pcont = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            cutscene.SetActive(true);
            player.transform.position = playerPos.position;
            pcont.Freeze();
            gameObject.SetActive(false);
        }
    }
}
