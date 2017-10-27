using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float acceleration = 10f;
    public float jumpforce = 300f;
    public float maxspeed = 5f;

    private Rigidbody rb;
    private Collider coll;
    private Renderer rend;
    private Color colorStart;

    /* needed for preventing double triggering (see ontriggerexit) */
    private int i = 1;
    private int q = 1;

    private GameObject cameraFirstPerson;

    private bool grounded;
    private bool playermoving;
    private bool jumping;
    private bool shieldactivated;

    public AudioClip bouncesound;
    public AudioClip collectablessound;
    public AudioClip finishsound;
    public AudioClip impactsound;
    public AudioClip jumpsound;
    public AudioClip wormhole;

    public Material[] materials;
    public Material shieldmaterial;
    private Material startmaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
	      rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
	      cameraFirstPerson = GameObject.Find("CameraFirstPerson");

        startmaterial = rend.material = materials[PlayerPrefs.GetInt("Material")];

        Invoke("SetPlayerMoving", GameManager.DELAY_TIMER);
    }

    private void Update()
    {
        if (playermoving)
        {
            /* here we calculate the rather complicated movement...
            because when the 'firstpersoncam' is activated we only want to allow
            the player to be moved vertically, thats why this is so nested */
            float movehorizontal = 0;
            float movevertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(0, 0, 0); // initialize movement

            if (cameraFirstPerson.activeSelf)
                movement = cameraFirstPerson.transform.forward * movevertical;
            else
            {
                movehorizontal = Input.GetAxis("Horizontal");
                movement = new Vector3(movehorizontal, 0.0f, movevertical);
            }
            rb.AddForce(movement * acceleration); // add the movement to the player
        }

        /* Jumping */
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.numOfJumps > 0)
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && grounded)
            StopMove();

        if (Input.GetKeyDown(KeyCode.X) && GameManager.numOfShields > 0)
        {
            GameManager.instance.powercontroller.DecreasePower(PowerController.Powers.shield);
            shieldactivated = true;
            Invoke("SetShieldFalse", 3);
			rend.material = shieldmaterial;
        }
    }

    private void FixedUpdate()
    {
        NormalizeSpeed();
    }

    private void SpawnAtRandom()
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(1, GameManager.instance.gridcontroller.gridsize), 1, UnityEngine.Random.Range(1, GameManager.instance.gridcontroller.gridsize));
        rb.transform.position = position;
    }

    private void SetPlayerMoving()
    {
        playermoving = true;
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpforce));
        jumping = true;
        GameManager.instance.powercontroller.DecreasePower(PowerController.Powers.jump);
        SoundManager.instance.PlaySoundEffect(jumpsound);
    }

    private void StopMove()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void NormalizeSpeed()
    {
        if (rb.velocity.magnitude > maxspeed)
            rb.velocity = rb.velocity.normalized * maxspeed;
    }

    private void LevelFinish()
    {
        SceneManager.LoadScene(1);
    }

    /* ***************** TRIGGER ENTER ***************** */
    private void OnTriggerEnter(Collider collider)
    {
        /* "Gr" - regular ground tiles */
        if (collider.gameObject.tag.Contains("Gr"))
            grounded = true;

        /* ***************** reaching the end - loading next level ***************** */
        if (collider.gameObject.tag.Contains("GrFi") && q == 1)
        {
            Debug.Log("Finish");
            SoundManager.instance.PlaySoundEffect(finishsound);
            GameManager.instance.LevelFinish();
            Destroy(rb); // destroy rigidbody, to stop movement
            Invoke("LevelFinish", GameManager.DELAY_LEVELRESTART); // start next level with delay
            enabled = false;
            q++;
        }

        /* ***************** collecting a powerup ***************** */
        if (collider.gameObject.tag.Contains("Collect"))
        {
            SoundManager.instance.PlaySoundEffect(collectablessound);

            collider.gameObject.SetActive(false);

            float random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
                GameManager.instance.powercontroller.IncreasePower(PowerController.Powers.shield);
            if (random == 1)
                GameManager.instance.powercontroller.IncreasePower(PowerController.Powers.jump);
        }

        /* ***************** colliding with the directionchanger ***************** */
        if (collider.gameObject.tag.Contains("GrDC") && !shieldactivated)
        {
            float rand = UnityEngine.Random.Range(0, 5);
            Vector3 vec = Vector3.zero;
            if (rand == 1)
                vec = Vector3.forward;

            if (rand == 2)
                vec = Vector3.back;

            if (rand == 3)
                vec = Vector3.left;

            if (rand == 4)
                vec = Vector3.right;

            rb.AddForce(vec * 1000);
        }

        /* ***************** colliding with the directionchanger ***************** */
        if (collider.gameObject.tag.Contains("Meteor") && !shieldactivated)
        {
            Destroy(gameObject);
            /* to prevent double triggering */
            if (i == 1)
                GameManager.instance.GameOver();

            i++;
        }

        /* ***************** colliding with enemyfollower ***************** */
        if (collider.gameObject.tag.Contains("EnemyFollower") && !shieldactivated)
        {
            Debug.Log("EnemyCollison");
            SoundManager.instance.PlaySoundEffect(impactsound);


                Vector3 force = transform.position - collider.gameObject.transform.position;
                force.Normalize();
                rb.AddForce(force * GameManager.instance.enemyPushPower);
        }
    }

    /* ***************** TRIGGER EXIT ***************** */
    private void OnTriggerExit(Collider collider)
    {
        /* ***************** leaving ground ***************** */
        if (collider.gameObject.tag.Contains("Gr") && jumping)
        {
            grounded = false;
            jumping = false; // why false - check later
        }

        /* ***************** dropping in wormhole ***************** */
        if (collider.gameObject.tag.Contains("Wormhole"))
        {
            SoundManager.instance.PlaySoundEffect(wormhole);

            Debug.Log("Wormhole");
            StopMove();
            SpawnAtRandom();
        }

        /* ***************** gameover ***************** */
        if (collider.gameObject.tag.Contains("GameOver"))
        {
            /* to prevent double triggering */
            if (i == 1)
                GameManager.instance.GameOver();

            i++;
        }
    }

    private void SetShieldFalse()
    {
        shieldactivated = false;
	      rend.material = startmaterial;
    }

}
