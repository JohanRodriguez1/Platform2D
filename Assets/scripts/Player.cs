using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    //
    public LayerMask maskFloor;
    public Transform testFloor;
    public float forceJump = 10f;
    private bool isWalking;
    private bool isFloor = true;
    private float radio = 0.07f;
    private bool jump2 = false;
    private float speed = 3f;
    public float factor = 1f;

    private Vector2 posINI;
    public Text txtScore;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        posINI = transform.position;
        txtScore.text = "SCORE: " + score.ToString();
    }

    //FixedUpdate called
    void FixedUpdate()
    {
        isFloor = Physics2D.OverlapCircle(testFloor.position, radio, maskFloor);
        animator.SetBool("isJump", !isFloor);

        if (isFloor)
        {
            jump2 = false;
        }

        if (transform.position.y < -10)
        {
            transform.position = posINI;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // W key
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isFloor || !jump2)
            {
                rb.velocity = new Vector2(rb.velocity.x, forceJump);
                rb.AddForce(new Vector2(0, forceJump));
                if (!jump2 && !isFloor)
                {
                    jump2 = true;
                }
            }
        }

        // D key
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isWalk", true);
            isWalking = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            speed = 3;
            factor = 1;
        }
        if (isWalking)
        {
            rb.velocity = new Vector2(speed * factor,rb.velocity.y);
        }

        // A key
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("isWalk", true);
            isWalking = true;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            speed = -3;
            factor = 1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isWalk", false);
            isWalking = false;
        }

        //Speed-Turbo-Run
        if (Input.GetKeyDown(KeyCode.LeftShift) && isWalking)
        {
            factor = 3;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && isWalking)
        {
            factor = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.transform.tag == "move")
        {
            transform.parent = obj.transform;
        }

        if (obj.transform.tag == "gema")
        {
            Destroy(obj.transform.gameObject);
            score += 1;
            txtScore.text = "SCORE: " + score.ToString();
            return;
        }

        if (obj.transform.name == "top")
        {
            Destroy(obj.transform.parent.gameObject);
            return;
        }

        if (obj.transform.name == "body")
        {
            Destroy(this.gameObject);
            print("LOSE");
            SceneManager.LoadScene("Perdiste");
        }

        if (obj.transform.tag == "door")
        {
            print("WIN");
            SceneManager.LoadScene("Ganaste");
            return;
        }
    }

    void OnCollisionExit2D(Collision2D obj)
    {
        if (obj.transform.tag == "move")
        {
            transform.parent = null;
        }
    }
}
