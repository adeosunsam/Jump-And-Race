using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Rigidbody rb;
	private AudioSource audioSource;

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
	private bool isSliding;
	public static bool gameOver;

	private Animator animator;
	private BoxCollider boxCollider;

	private Vector3 InitialSize;
	private Vector3 InitialCenter;
	public static Player Instance { get; private set; }
	

	private void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		Physics.gravity *= gravityModifier; //this help in controlling how fast the player land after jumping
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		boxCollider = GetComponent<BoxCollider>();
		InitialSize = boxCollider.size;
		InitialCenter = boxCollider.center;
	}

	void LateUpdate()
	{
		if (isSliding)
		{
			var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			if (stateInfo.IsName("Slide"))
			{
				if (stateInfo.normalizedTime >= 1.0f) //Ensures the animation has fully played.
				{
					isSliding = false;

					// reset size
					boxCollider.size = InitialSize;

					// reset center
					boxCollider.center = InitialCenter;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
		{
			rb.AddForce(Vector3.up * 500f, ForceMode.Impulse);
			isOnGround = false;
			animator.SetTrigger("Jump_trig");
			dirtParticle.Stop();
			audioSource.PlayOneShot(jumpSound, 1f);
		}

		if (isOnGround && !gameOver)
		{
			if (MobileInput.Instance.SwipeUp)
			{
				//Jump
				rb.AddForce(Vector3.up * 500f, ForceMode.Impulse);
				isOnGround = false;
				animator.SetTrigger("Jump_trig");
				dirtParticle.Stop();
				audioSource.PlayOneShot(jumpSound, 1f);
			}
			else if (MobileInput.Instance.SwipeDown)
			{
				// Adjust size
				boxCollider.size = new Vector3(InitialSize.x, 2f, InitialSize.z);

				// Adjust center
				boxCollider.center = new Vector3(InitialCenter.x, 1f, InitialCenter.z);
				//rb.AddForce(Vector3.up * 500f, ForceMode.Impulse);
				//isOnGround = false;
				animator.SetTrigger("Slide_trig");
				isSliding = true;
				//dirtParticle.Stop();
				//audioSource.PlayOneShot(jumpSound, 1f);
			}
		}
		else if(!isOnGround && !gameOver)
		{
			if (MobileInput.Instance.SwipeDown)
			{
				rb.AddForce(Vector3.down * 500f, ForceMode.Impulse);
			}
		}

			var obstructed = Physics.Raycast(transform.position, Vector3.right, out RaycastHit rayCastHit, maxDistance, counterLayerMask);
		if (obstructed)
		{
			if (rayCastHit.transform.TryGetComponent(out ICollider obstacle))
			{
				if (obstacle is Obstacle)
				{
					if (!gameOver)
					{
						//this should only play/stop once.
						//dirtParticle.Stop();
						//explosionParticle.Play();
						//audioSource.PlayOneShot(crashSound, 1f);
					}
					//gameOver = true;
					//animator.SetBool("Death_b", true);
					//animator.SetInteger("DeathType_int", 1);//player falls backward
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
