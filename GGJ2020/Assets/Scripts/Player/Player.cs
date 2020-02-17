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
    public float shootingSpeed = 6;
    float velocityXSmoothing;
    Vector3 velocity;

    public float accelerationTimeAirborn = 0.2f;
    public float acclerationTimeGrounded = 0.1f;


    [Header("weapons")]
    public PlayerTurret turret;

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
                turret.startShot("Fire2");
            }

            anim?.setAnimation(velocity);
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (heldObject != null && (Input.GetButtonDown("Fire1") || heldObject.GetComponent<Machine>().health <= 0))
        {
            dropObject(input);
        }

    }

    void checkIfGrounded() {
        if (controller.collisions.below) {
            velocity.y = Mathf.Max(0, velocity.y);
        }

        canJump = controller.collisions.below;

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

        float targetSpeed = (heldObject != null ? holdingSpeed : moveSpeed);
        targetSpeed = Input.GetButton("Fire2") ? shootingSpeed : targetSpeed;

        float targetVelocityX = input.x * targetSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acclerationTimeGrounded : accelerationTimeAirborn);

    }

    private void OnTrigger(Transform other)
    {
        if (other.GetComponent<Asteroid>() != null) {
            anim?.setTrigger("punch");
            burstForce(Vector2.up * 15, false);
        }
        if (canMove && heldObject == null) {
            if (other.GetComponent<Machine>() != null && other.GetComponent<Machine>().health <= 0  && Input.GetButtonDown("Fire1") && currentRepair == null) {
                repairDelegate repairPart = (() =>
                {
                    other.GetComponent<Machine>().repair();
                });
                currentRepair = repairTimeEnum(turretRepairTime, repairPart, "Fire1");
                StartCoroutine(currentRepair);
            } else if (other.GetComponent<ShieldLayer>() != null && Input.GetButtonDown("Fire1") && currentRepair == null) {
                repairDelegate repairShip = (() =>
                {
                    other.GetComponent<ShieldLayer>().RepairLayer();
                    burstForce(Vector2.up * 3, false);
                });
                currentRepair = repairTimeEnum(shipRepairTime, repairShip, "Fire1");
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
       
        heldObject.transform.parent = null;

        //TODO: refine this throw direction to get last direction instead of defaulting to a direction
        //TODO: bug in direction thrown with move velocity
        if (Mathf.Abs(velocity.x) > 0.01f)
        {          
            Vector2 dir = throwDirection * throwStrength;
            dir.x *= Mathf.Sign(velocity.x);
            heldObject.GetComponent<Machine>().putDown(dir);
            heldObject.GetComponent<Machine>().burstForce(dir, false);
        } else {
            Vector2 dir = throwDirection * throwStrength;
            dir.x *= Mathf.Sign(Random.Range(-100, 100));
            heldObject.GetComponent<Machine>().putDown(dir);
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

    void jumpAndHover()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            anim?.setTrigger("jump");
            velocity.y = heldObject != null ? minJump : maxJump;
            canJump = false;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > minJump)
            {
                velocity.y = minJump;
            }
        }
    }

    public bool grounded() {
        return canJump;
    }
}
