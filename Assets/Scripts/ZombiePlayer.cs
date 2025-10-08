using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using TMPro;

public class ZombiePlayer : MonoBehaviour
{
    private const float orthosize = 3.265625f;
    private const float orthomax = 6f;
    private const float zoomrate = 5f;

    [Header("Abilities Unlocked")]
    public bool ZoomUnlocked;
    public bool ShootUnlocked;
    public GameObject Syringe;
    private SpriteRenderer SyringeRend;


    [Header("Movement Settings")]
    public float speed = 2f;
    public float SpeedMult;
    public GameObject playerArt;
    public GameObject SpeedUp;



    [Header("Zoom Settings")]

    public bool Zoomed;
    public Camera mainCam;

    // public PixelPerfectCamera pixelcam;




    [Header("TextBoxSettings")]
    public GameObject console;
    public GameObject textbox;
    private TMP_Text consoletext;
    

    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private bool facingRight;

    public Transform FireBase;

    public Vector2 momentum;
    public Vector2 direction;

    private Vector3 ProjDirection;

    public Sprite UpSprite;
    public Sprite DownSprite;
    public Sprite SideSprite;

    public GameObject projpre;

    private Rigidbody2D projmass;

    // --- NEW ---

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = playerArt.GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
        FireBase.position = Vector2.zero;
        SpeedMult = 1.0f;


        //  Finds the console object
        console = GameObject.Find("/RPGtb/OuterConsole");
        textbox = GameObject.Find("/RPGtb/OuterConsole/InnerConsole/ConsoleText");
        //  Sets it immediately to inactive
        console.SetActive(false);
        //  Finds the text mesh object
        consoletext = textbox.GetComponent<TMP_Text>();

        GameObject maincamera = GameObject.Find("/Main Camera");
        mainCam = maincamera.GetComponent<Camera>();

        mainCam.orthographic = true;

        Zoomed = false;
        ZoomUnlocked = false;
        Syringe.SetActive(false);
        ShootUnlocked = false;
        SyringeRend = Syringe.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");


        momentum = (moveDirection).normalized;


        moveDirection = moveDirection.normalized;



        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

            if (moveDirection.sqrMagnitude > 0f)
                lastMoveDirection = moveDirection;

            animator.SetFloat("X", lastMoveDirection.x);
            animator.SetFloat("Y", lastMoveDirection.y);
        }

        if (spriteRenderer != null && moveDirection.y > 0f)
        {
            spriteRenderer.sprite = UpSprite;
            direction = Vector2.up;
        }
        if (spriteRenderer != null && moveDirection.y < 0f)
        {
            spriteRenderer.sprite = DownSprite;
            direction = Vector2.down;
        }
        if (spriteRenderer != null && moveDirection.x != 0f)
        {
            spriteRenderer.sprite = SideSprite;
            spriteRenderer.flipX = moveDirection.x < 0f;
            if (spriteRenderer.flipX) { direction = Vector2.left; }
            else { direction = Vector2.right; }
        }



        if (moveDirection != Vector2.zero) {
            Vector2 halfway = 0.5f * (direction).normalized;
            FireBase.localPosition = halfway;

            //      Kind of a cool animation, but not for idle
            // Vector3 weaponrotate = new Vector3(0f, 0f, momentum.x);
            // Syringe.transform.Rotate(weaponrotate);
        }
        if (Input.GetKeyDown(KeyCode.Space) && ShootUnlocked) {
            PlayerFire();
        }
        if (Input.GetKey(KeyCode.U) && ZoomUnlocked) {
            PlayerZoomOut();
        }
        if (Input.GetKey(KeyCode.I) && ZoomUnlocked) {
            PlayerZoomIn();
        }
        
        // if (momentum != Vector2.zero)
        // Vector3 weaponrotate = new Vector3(momentum.x, momentum.y, )
        // Syringe.transform.Rotate(new Vector3())


    }

    void PlayerZoomOut()
    {
        mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, orthomax, zoomrate * Time.deltaTime);
    }
    void PlayerZoomIn()
    {
        mainCam.orthographicSize = Mathf.MoveTowards(mainCam.orthographicSize, orthosize, zoomrate * Time.deltaTime);
    }



    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;

        // FireBase.localPosition = momentum;

        // if (spriteRenderer.sprite == DownSprite) {
        //     FireBase.localPosition = Vector2.down;
        // }
        // else if (spriteRenderer.sprite == UpSprite) {
        //     FireBase.localPosition = Vector2.up;
        // } else if (spriteRenderer.sprite == SideSprite ) {
        //     if (spriteRenderer.flipX) { FireBase.localPosition = Vector2.left; }
        //     else { FireBase.localPosition = Vector2.right; }
        // }
    }

    // --- NEW: flips the sprite upon moving left/right
    // void Flip() 
    // {
    //     facingRight = !facingRight;
    //     Vector3 thescale = transform.localScale;
    //     theScale.x *= -1;
    //     transform.localScale = theScale;
    // }

    // --- NEW: call this when the player touches a powerup ---

    void PlayerFire()
    {
        StopCoroutine("StowSyringe");
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Syringe.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        SyringeParse();
        StartCoroutine("StowSyringe");

        GameObject projectile = Instantiate(projpre, FireBase, false);
        projectile.transform.parent = null;
        projmass = projectile.GetComponent<Rigidbody2D>();

        Vector2 force = 50 * direction;
        projmass.AddForce(force, ForceMode2D.Impulse);
    }

    //  Upgrades the player's speed
    void OnCollisionEnter2D(Collision2D upgrade) 
    {
        if (upgrade.gameObject.CompareTag("SpeedUp"))
        {
            SpeedMult = 2.0f;
            speed = speed * SpeedMult;
            DisplayText("Speed Upgrade Acquired");
            Destroy(upgrade.gameObject, 0.0f);
        }
        if (upgrade.gameObject.CompareTag("ShootUp"))
        {
            ShootUnlocked = true;
            Syringe.SetActive(true);
            DisplayText("You can now fire energy beams. Press Space to fire");
            Destroy(upgrade.gameObject, 0.0f);
        }
        if (upgrade.gameObject.CompareTag("ZoomAbility"))
        {
            ZoomUnlocked = true;
            DisplayText("You can now look further out. Press U to zoom out and I to zoom in.");
            Destroy(upgrade.gameObject, 0.0f);
        }
    }

    void DisplayText(string text)
    {
        consoletext.text = text;
        StartCoroutine("BoxVisible");
    }

    IEnumerator BoxVisible() 
    {
        console.SetActive(true);
        yield return new WaitForSeconds(5);
        console.SetActive(false);
    }

    IEnumerator StowSyringe()
    {
        yield return new WaitForSeconds(1.5f);
        Syringe.SetActive(false);
    }


    // IEnumerator DisplayText(string text)
    // {
    //     console.SetActive(true);
    //     yield return StartCoroutine("TextBox");
    //     console.SetActive(false);
        

    // }

    // IEnumerator TextBox()
    // {
    //     // suspend execution for 5 seconds
    //     yield return new WaitForSeconds(5);
    //     print("WaitAndPrint " + Time.time);
    // }

    void SyringeParse() 
    {
        if (spriteRenderer.sprite == DownSprite) { 
            SyringeRend.sortingOrder = 0;
        } else {              
            SyringeRend.sortingOrder = -1;
        }
        Syringe.SetActive(true);
    }
}
