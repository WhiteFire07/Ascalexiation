using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;

    [Header("Settings")]
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // Camera follows player with specified offset and lag
        transform.position = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, offset.z);
    }
}
