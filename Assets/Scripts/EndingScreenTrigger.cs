using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EndingScreenTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public GameObject endingScreen;
    public GameObject cart;
    public Camera mainCam;
    public bool active = false;
    private float vel;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            vcam.gameObject.SetActive(true);
            endingScreen.SetActive(true);
            cart.SetActive(true);
            vcam.m_Lens.OrthographicSize = 10.8f;
            active = true;
        }
    }
    public void Update()
    {
        if(active)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(10.8f, 100, vel/8);
            vel += Time.deltaTime;
        }
    }
}
