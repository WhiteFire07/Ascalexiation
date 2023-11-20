using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateController : MonoBehaviour
{
    public GameObject ss;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ss.transform.position = transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = ss.transform.position;
        }
    }
}
