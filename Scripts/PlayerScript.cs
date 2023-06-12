using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
     // Simple player script for player movement.
    Rigidbody2D rb;
    
    Vector2 movements;
    public float speed;

    public int maxStability;
    public int currentStability = 0; 
    public Slider mentalSlider;

    public string xAxis = "Horizontal";
    public string yAxis = "Vertical";

    public Vector3 rotation;
    public GameObject camera;
    GameObject gdDash;

    GameObject gdMental;
    GameObject meter;
    GameObject[] Enemies;

    public Animator animator;

    SpriteRenderer sr;

    private bool Rotate = false;

    [SerializeField] public TrailRenderer trail;
    private bool isFacingRight = true;

    //Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 15f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.5f;
    public AudioSource dashSound;
    public bool isRotated = false;

    float timer;

    //Audio

    public AudioSource family;
    public AudioSource love;
    public AudioSource friend;
    public AudioSource house;
    public AudioSource names;
    public AudioSource clock;
    public AudioSource self;
    public AudioSource hit;
    public AudioSource death;

    string[] input = { "Clock", "Family", "Friends", "House", "Love", "Names" };

    void Awake()
    {
       
        gdMental = GameObject.FindGameObjectWithTag("MentalStability");
        gdDash = GameObject.FindGameObjectWithTag("DashMeter");
        meter = GameObject.FindGameObjectWithTag("fill");
        camera = GameObject.Find("CM vcam1");
        sr = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
      //family = GameObject.FindGameObjectWithTag("Family");
        rb = GetComponent<Rigidbody2D>();
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");


        if(Enemies.Length <= 3)
        {
            maxStability = 2;
        }
        else
        {
            maxStability = 3;
        }
        

        currentStability = maxStability;
        gdMental.GetComponent<Slider>().value = currentStability;
        gdMental.GetComponent<Slider>().maxValue = currentStability;
        animator.SetBool("walk", false);
        animator.SetBool("walkUp", false);
        animator.SetBool("walkDown", false);

        sr.flipY = false;
    }

   
    void Update()
    {
        if (isDashing)
        {

            return;
        }


        if (movements.x != 0)
        {
            animator.SetBool("walk", true);
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", false);
        }
        else if ((movements.y < 0 && Rotate) || (movements.y > 0 && !Rotate))
        {
            animator.SetBool("walk", false);
            animator.SetBool("walkUp", true);
            animator.SetBool("walkDown", false);
        }
        else if ((movements.y > 0 && Rotate) || (movements.y < 0 && !Rotate))
        {
            animator.SetBool("walk", false);
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", true);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", false);
        }


        if (!Rotate)
        {
            movements.x = Input.GetAxis(xAxis);
            movements.y = Input.GetAxis(yAxis);
        }
        else
        {
            movements.x = -Input.GetAxis(xAxis);
            movements.y = -Input.GetAxis(yAxis);
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && canDash && (SceneManager.GetActiveScene().name != "FinalScene"))
        {
            animator.SetBool("sprint", true);
            StartCoroutine(Dash());
        }

        Flip();

        if(canDash == false)
        {
            timer += Time.deltaTime;
            gdDash.GetComponent<Slider>().value = timer;

        }
        else
        {
            gdDash.GetComponent<Slider>().value = dashingCooldown;
            timer = 0;
        }


    }

    private void FixedUpdate()
    {
        if (isDashing)
        {

            return;
        }

        rb.MovePosition(rb.position + movements * speed * Time.fixedDeltaTime);
    } 

void OnCollisionEnter2D(Collision2D collision){

    if(collision.gameObject.tag == "Enemy"){

        currentStability -= 1;
        gdMental.GetComponent<Slider>().value = currentStability;
        GameObject ob;
        animator.SetBool("hit", true);
        

            if (currentStability==0)
            {
                death.Play();
                camera.transform.Rotate(rotation);
                animator.SetBool("rotate", true);
                for (int x = 0; x <= input.Length - 1; x++)
                {
                    ob = GameObject.FindGameObjectWithTag(input[x]);
                    if (ob != null)
                    {
                        ob.transform.Rotate(rotation);
                    }

                }

                for (int x = 0; x <= Enemies.Length - 1; x++)
                {
                    ob = Enemies[x];
                    if (ob != null)
                    {
                        ob.transform.Rotate(rotation);
                    }

                }

                isRotated = !isRotated;

                currentStability = maxStability;
                gdMental.GetComponent<Slider>().value = currentStability;

                Rotate = !Rotate;
                sr.flipY = !(sr.flipY);

            }
            StartCoroutine(Hit());
            Destroy(collision.gameObject);

    }
        
}


    private void Flip()
    {

        if (isFacingRight && movements.x < 0f || !isFacingRight && movements.x > 0f)
        {

            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {

        canDash = false;
        meter.GetComponent<Image>().color = new Color(0.4170968f, 0.9716981f, 0.9470491f); 
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashSound.Play();
        rb.velocity = new Vector2(movements.x * dashingPower, movements.y * dashingPower);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("sprint", false);
        yield return new WaitForSeconds(dashingCooldown);
        meter.GetComponent<Image>().color = new Color(0.6862745f, 1f, 0.03137255f);
        canDash = true;
    }

    private IEnumerator Hit() {
        hit.Play();
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("hit", false);
        animator.SetBool("rotate", false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Family")
        {
      
            family.Play();
        }

       
        if (collider.gameObject.tag == "Love")
        { 
           love.Play();
       }

       
       if (collider.gameObject.tag == "Friends")
       {

           friend.Play();
       }

       if (collider.gameObject.tag == "House")
       {

           house.Play();
       }

       if (collider.gameObject.tag == "Names")
       {

           names.Play();
       }

       if (collider.gameObject.tag == "Clock")
       {

           clock.Play();
       }

       if (collider.gameObject.tag == "Self")
       {

           self.Play();
           speed = 0;
       }
       

    }


}
