using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class SaveStateController : MonoBehaviour
{
    public PlayerController player;

    // Update is called once per frame
    void Update()
    {
        if (player.save.triggered)
        {
            transform.position = player.transform.position;
        }
        if (player.load.triggered)
        {
            player.transform.position = transform.position;
        }
    }
}
