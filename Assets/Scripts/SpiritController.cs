using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiritController : MonoBehaviour
{
    public GameObject player;
    public Animator ani;
    public PlayerController pcont;
    public Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        pcont = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("CanDash", pcont.canDash);
        Vector3 _offset = new Vector3(offset.x * pcont.facingDirection, offset.y, offset.z);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + _offset, speed * Time.deltaTime);
    }
}
