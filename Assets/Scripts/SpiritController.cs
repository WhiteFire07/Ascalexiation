using UnityEngine;

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
        transform.position = player.transform.position + new Vector3(offset.x * pcont.facingDirection, offset.y, offset.z);
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetBool("CanDash", pcont.canDash);
        Vector3 _offset = new Vector3(offset.x * pcont.facingDirection, offset.y, offset.z);
        transform.position = Vector3.Lerp(transform.position, player.transform.position + _offset, speed * Time.deltaTime);
    }
}
