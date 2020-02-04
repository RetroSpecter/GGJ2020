using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {
    Controller2D controller;

    [HideInInspector] public bool canJump;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 2;
    float gravity = -20;
    public bool gravityOn = true;
    public float timeToJumpApex = 0.4f;
    float maxJump = 8;
    float minJump = 4;

    public bool canMove = true;
    public float moveSpeed = 6;
    public float holdingSpeed = 3;
    float velocityXSmoothing;
    Vector3 velocity;

    public float accelerationTimeAirborn = 0.2f;
    public float acclerationTimeGrounded = 0.1f;

    [Header("picking up and throwing objects")]
    public GameObject heldObject;
    public Vector2 heldObjectOffset;
    public Vector2 throwDirection;
    public float throwStrength;

    [Header("repair")]
    public float shipRepairTime = 2;
    public float turretRepairTime = 2;

    private PlayerAnimation anim;
    private Healthbar healtbar;
    public delegate void repairDelegate();

    public ParticleSystem dustParticleSystem;

    void Start() {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJump = Mathf.Abs(gravity) * timeToJumpApex;
        minJump = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        anim = GetComponentInChildren<PlayerAnimation>();
        healtbar = GetComponentInChildren<Healthbar>();
        healtbar?.HideSlider();
    }

    void Update() {
        checkIfGrounded();
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (canMove)
        {
            jumpAndHover();
            movement(input);
           
            if (heldObject == null && Input.GetButtonDown("Fire2")) {
                // todo improve direction to ignore gravity and input for a sec
                if (Mathf.Abs(input.x) > 0.1f) {
                    burstForce(Vector2.right * input.normalized.x * 20, false);
                } else {
                    burstForce(Vector2.right * 20, false);
                }
            }          
            anim?.setAnimation(velocity);
        }

        ParticleSystem.EmissionModule em = dustParticleSystem.emission;
        em.rateOverDistanceMultiplier = Mathf.Abs(input.x) > 0.5f && controller.collisions.below ? 1: 0;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (heldObject != null && (Input.GetButtonDown("Fire1") || heldObject.GetComponent<Machine>().health <= 0))
        {
            dropObject(input);
        }

    }

    void checkIfGrounded() {
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        canJump = controller.collisions.below;

    }

    void jumpAndHover() {
        if (Input.GetButtonDown("Jump") && canJump) {
            anim?.setTrigger("jump");
            velocity.y = heldObject != null ? minJump : maxJump;
            canJump = false;
        }

        if (Input.GetButtonUp("Jump")) {
            if (velocity.y > minJump) {
                velocity.y = minJump;
            }
        }
    }

    public void burstForce(Vector3 velocity, bool additive) {
        if (additive)
        {
            this.velocity += velocity;
        } else {
            this.velocity = velocity;
        }
    }

    void movement(Vector2 input) {
        /*
        float dir = 0;
        if (input.magnitude > 0.1f)
        {
            float joystickAngle = Vector2.SignedAngle(input.normalized, Vector2.up);
            float playerAngle = Vector2.SignedAngle(transform.position.normalized, Vector2.up);
            // print(joystickAngle + " vs " + playerAngle);
            if (joystickAngle < 0 && playerAngle < 0)
            {
                joystickAngle += 360;
                playerAngle += 360;
            }
            dir = playerAngle > joystickAngle ? -1 : 1;
        }

        float targetVelocityX = dir * (heldObject != null ? holdingSpeed : moveSpeed);     
        */
        float targetVelocityX = input.x * (heldObject != null ? holdingSpeed : moveSpeed);
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acclerationTimeGrounded : accelerationTimeAirborn);

    }

    private void OnTrigger(Transform other)
    {
        if (other.GetComponent<Asteroid>() != null) {
            anim?.setTrigger("punch");
        }
        if (canMove && heldObject == null) {
            if (other.GetComponent<Machine>() != null && other.GetComponent<Machine>().health <= 0  && Input.GetButtonDown("Fire1") && currentRepair == null) {
                repairDelegate repairPart = (() =>
                {
                    other.GetComponent<Machine>().repair();
                });
                currentRepair = repairTimeEnum(2, repairPart, "Fire1");
                StartCoroutine(currentRepair);
            } else if (other.GetComponent<PlanetSlice>() != null && Input.GetButtonDown("Fire1") && currentRepair == null) {
                repairDelegate repairShip = (() =>
                {
                    other.GetComponent<PlanetSlice>().RepairLayer();
                    transform.position += transform.up * 2;
                });
                currentRepair = repairTimeEnum(2, repairShip, "Fire1");
                StartCoroutine(currentRepair);
            } else if (other.GetComponent<Machine>() != null && Input.GetButtonDown("Fire1")) {
                pickUpObject(other);
            }
        }
    }

    IEnumerator currentRepair;
    IEnumerator repairTimeEnum(float repairTime, repairDelegate repairAction, string actionButton) {
        velocity = Vector3.zero;
        healtbar?.ShowSlider();
        float t = 0;
        canMove = false;
        anim?.setBoolean("repair", true);
        float soundtime = 0;
        while (t < repairTime && Input.GetButton(actionButton)) {
            healtbar?.UpdateSlider(t/repairTime);

            if (t > soundtime)
            {
                AudioManager.instance.Play("repair");
                soundtime += repairTime / 3;
            }

            t += Time.deltaTime;
            yield return null;
        }
        healtbar?.HideSlider();
        if (t >= repairTime)
        {
            repairAction.Invoke();
        }
        anim?.setBoolean("repair", false);
        currentRepair = null;
        canMove = true;
    }

    public void pickUpObject(Transform other) {
        Sequence s = DOTween.Sequence();
        canMove = false;
        s.OnStart(() => {
            other.GetComponent<Machine>().pickUp();
            other.transform.parent = transform;
            other.transform.localPosition = heldObjectOffset;
            other.transform.localEulerAngles = Vector3.zero;
        });
        s.AppendInterval(0.1f);
        s.AppendCallback(() => {
            canMove = true;
            heldObject = other.gameObject;
        });
    }

    public void dropObject(Vector2 velocity) {
        heldObject.GetComponent<Machine>().putDown();
        heldObject.transform.parent = null;

        if (Mathf.Abs(velocity.x) > 0.1f)
        {
            Vector2 dir = throwDirection * throwStrength;
            dir.x *= Mathf.Sign(velocity.x);
            heldObject.GetComponent<Machine>().burstForce(dir, false);         
        }
        canMove = false;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(0.25f);
        s.AppendCallback(() => {
            canMove = true;
            heldObject = null;
        });
    }

    public bool grounded() {
        return canJump;
    }
}
