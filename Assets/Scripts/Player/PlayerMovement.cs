using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    public Rigidbody2D rb;
    private Animator anim;
    private void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.position = transform.position + (Vector3.up) + transform.forward * -10;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            //movimiento
            rb.velocity = ((transform.right * _speed) * _speed * Input.GetAxisRaw("Horizontal") + (transform.up * rb.velocity.y));

            //Flip
            if (rb.velocity.x > 0.1f)
            {
                GetComponent<SpriteRenderer>().flipX = false;

            }
            else if (rb.velocity.x < -0.1f)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            //Jump
            if (Input.GetButtonDown("Jump")) //el control para poder saltar tanto con el teclado como con mando
            {
                rb.AddForce(transform.up * _jumpForce);
            }

            //animaciones
            anim.SetFloat("Velocity X", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("Velocity Y", rb.velocity.y);
        }
    }
}
