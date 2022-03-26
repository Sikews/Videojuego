using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SikiMovimiento : MonoBehaviour
{

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;

    private int score;
    public Text scoreString;
    public Text healthString;

    private float Horizontal;
    private bool Grounded;
    private float timeJump;
    private float timeJumpAnimation;
    private bool dobleJump;

    //Atributos Siki
    public float Speed;
    public float JumpForce;
    public int Health;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Speed = 1.5f;
        JumpForce = 3;
        Health = 1;
}

    // Update is called once per frame
    void Update()
    {
        //Gestión movimiento horizontal
        Horizontal = Input.GetAxisRaw("Horizontal") * Speed;
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Animator.SetBool("Running", Horizontal != 0.0f);

        //Gestion salto, doblesalto y caida
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.2f))
        {
            Grounded = true;
            dobleJump = false;
            Animator.SetBool("DoubleJump", false);
        }
        else Grounded = false;

        if (Input.GetKey(KeyCode.Space) && Grounded && !dobleJump)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
            timeJump = Time.time;
        }
        if (Input.GetKey(KeyCode.Space) && !Grounded && !dobleJump && Time.time > timeJump + 0.25f)
        {
            Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, JumpForce);
            dobleJump = true;
            timeJumpAnimation = Time.time;
        }

        //Gestion de animaciones
        if (!dobleJump)
        {
            if (Rigidbody2D.velocity.y > 0.5) //Salto
            {
                Animator.SetBool("Jumping", true);
            }
            else if (Rigidbody2D.velocity.y < -0.5) //Caida
            {
                Animator.SetBool("Falling", true);
                Animator.SetBool("Jumping", false);
            }
            else //Suelo
            {
                Animator.SetBool("Jumping", false);
                Animator.SetBool("Falling", false);
            }
        }
        else
        {
            Animator.SetBool("DoubleJump", true);
            if (Time.time > timeJumpAnimation + 0.5f)
            {
                Animator.SetBool("DoubleJump", false);
                Animator.SetBool("Falling", true);
            }
        }

    }
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }

    public void Hit()
    {
        Health = Health - 1;
        healthString.text = "Health: " + Health;
        if (Health == 0) Destroy(gameObject);
    }
}