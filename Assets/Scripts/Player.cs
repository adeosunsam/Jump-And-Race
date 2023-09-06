using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audio;

    [SerializeField]
    private ParticleSystem explosionParticle;

    [SerializeField]
    private ParticleSystem dirtParticle;

    [SerializeField]
    private AudioClip crashSound;

    [SerializeField]
    private AudioClip jumpSound;

    [SerializeField]
    private float gravityModifier;

    [SerializeField]
    private LayerMask counterLayerMask;

    private float maxDistance = 1f;

    private bool isOnGround;
    public static bool gameOver;

    private Animator animator;
    public static Player Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            rb.AddForce(Vector3.up * 500f, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            audio.PlayOneShot(jumpSound, 1f);
        }
        
        var obstructed = Physics.Raycast(transform.position, Vector3.right, out RaycastHit rayCastHit, maxDistance, counterLayerMask);
        if (obstructed)
        {
            if (rayCastHit.transform.TryGetComponent(out ICollider obstacle))
            {
                if(obstacle is Obstacle)
                {
                    if (!gameOver)
                    {
                        //this should only play/stop once.
                        dirtParticle.Stop();
                        explosionParticle.Play();
                        audio.PlayOneShot(crashSound, 1f);
                    }
                    gameOver = true;
                    animator.SetBool("Death_b", true);
                    animator.SetInteger("DeathType_int", 1);//player falls backward
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            if (!gameOver)
            {
                dirtParticle.Play();
            }
        }
    }
}
