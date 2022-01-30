using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public float moveSpeed = 5f;
    public bool moveLegs = true;
    public bool locked = true;
    public bool hanging = false;
    public float jumpPower = 5f;
    public float jumpPowerAlone = 2f;
    public float jumpPowerHang = 10f;
    public Hang isHanging;
    public Animator animBot;
    public Animator animTop;
    public bool endgame = false;
    public AudioClip audioJump;
    public AudioClip audioWalk;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Hang();
        SeperateOrJoin();
        SwtichPart();
    }

    void MovePlayer()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector3 newPos;

            if (moveLegs)
            {
                if (hanging && locked)
                {
                    return;
                }
                newPos = bottom.position;

            }
            else
            {
                if (!hanging && top.GetComponent<Rigidbody2D>().velocity.y == 0)
                {
                    return;
                }
                newPos = top.position;
            }

            Vector3 bottomOffset = newPos - bottom.position;
            Vector3 topPosOffset = newPos - top.position;

            newPos.x += Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;

            if (moveLegs)
            {
                bottom.position = newPos;
                if (locked)
                {
                    top.position = newPos - topPosOffset;
                }
            }
            else
            {
                top.position = newPos;
                if (locked)
                {
                    bottom.position = newPos - bottomOffset;
                }
            }
        }

        if (Input.GetButton("Jump") && bottom.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            if (moveLegs)
            {
                float power = jumpPower;
                if (!locked)
                {
                    power = jumpPowerAlone;
                }
                bottom.GetComponent<Rigidbody2D>().AddForce(Vector2.up * power, ForceMode2D.Impulse);
                transform.GetComponent<AudioSource>().clip = audioJump;
                transform.GetComponent<AudioSource>().Play();
            }
            else
            {
                if (top.GetComponent<Rigidbody2D>().velocity.y == 0 && hanging)
                {
                    locked = false;
                    animBot.SetBool("locked", locked);
                    _Hang(true);
                    transform.GetComponent<AudioSource>().clip = audioJump;
                    transform.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void Hang()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHanging.bar)
        {
            _Hang(false);
        }
    }

    public void SwtichPart()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveLegs = !moveLegs;
            animBot.SetBool("moveLegs", moveLegs);
            animTop.SetBool("moveLegs", moveLegs);
            if (moveLegs)
            {
                Camera.main.gameObject.transform.SetParent(bottom, false);
            }
            else
            {
                Camera.main.gameObject.transform.SetParent(top, false);
            }
        }
    }

    public void _Hang(bool jump)
    {
        hanging = !hanging;
        animTop.SetBool("hanging", hanging);
        if (hanging)
        {
            top.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            if (locked)
            {
                bottom.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            top.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            bottom.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            if (jump)
            {
                top.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPowerHang);
                transform.GetComponent<AudioSource>().clip = audioJump;
                transform.GetComponent<AudioSource>().Play();
            }
            else
            {
                top.GetComponent<Rigidbody2D>().AddForce(Vector2.down);
            }
            bottom.GetComponent<Rigidbody2D>().AddForce(Vector2.down);
        }
    }

    public void SeperateOrJoin()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (locked)
            {
                locked = false;
                animBot.SetBool("locked", locked);
                bottom.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                bottom.GetComponent<Rigidbody2D>().AddForce(Vector2.down);
            }
            else
            {
                if (Vector2.Distance(bottom.position, top.position) <= 1.2f)
                {
                    if (!hanging)
                    {
                        locked = true;
                        animBot.SetBool("locked", locked);
                        top.position = new Vector3(bottom.position.x, bottom.position.y + 1f, 0);
                    }
                    else
                    {
                        locked = true;
                        animBot.SetBool("locked", locked);
                        bottom.position = new Vector3(top.position.x, top.position.y - 1f, 0);
                        bottom.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "endgame" && !endgame && locked)
        {
            if (Input.GetKey(KeyCode.F))
            {
                endgame = true;
                Debug.Log("endgame");
            }
        }
    }
}
