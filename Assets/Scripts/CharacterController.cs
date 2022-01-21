using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviourPunCallbacks
{
    private Animator animator;
    private int _state;
    private new PhotonView photonView;
    public float rotationSpeed = 180;
    public float speed = 10;
    private Vector3 rotation;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!photonView.IsMine) return;

        if (Input.GetMouseButtonDown(0))
        {
            animator.Play("Attack");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 500);
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                speed = 0;
                rotationSpeed = 0;
            }
            else
            {
                speed = 10;
                rotationSpeed = 180;
            }


            float x = Input.GetAxisRaw("Vertical");
            float y = Input.GetAxisRaw("Horizontal");

            rotation = new Vector3(0, y * rotationSpeed * Time.deltaTime, 0);

            transform.Translate(0, 0, x * Time.deltaTime * speed);
            //transform.Translate(y * Time.deltaTime * speed, 0, 0);
            _state = x == 0f ? 0 : 1;

            if (y != 0f)
            {
                transform.Rotate(rotation);
            }
        }

        animator.SetInteger("state", _state);
    }
}
