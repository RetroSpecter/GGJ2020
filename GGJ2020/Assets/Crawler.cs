using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crawler : MonoBehaviour
{
    protected Controller2D controller;

    public float gravity;

    public float minVel, maxVel;
    private float horizVelocity;
    private SpriteRenderer sprite;
    public delegate void damageDelegate();

    protected Vector3 velocity;
    private Healthbar healtbar;

    IEnumerator curState;
    public float minTimeToDrill, maxTimeToDrill;
    private float timeTillDrill;
    public float timeToDrill = 2;
    public float delayBetweenActions = 1;

    void Start()
    {
        healtbar = GetComponentInChildren<Healthbar>();
        healtbar?.HideSlider();
        controller = GetComponent<Controller2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        timeTillDrill = Random.Range(minTimeToDrill, maxTimeToDrill);
        horizVelocity = Random.Range(minVel, maxVel);
        setCurState(MoveEnum());
    }

    // Update is called once per frame
    void Update()
    {       
        if (controller.collisions.below) {
            velocity.y = Mathf.Max(0, velocity.y);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move((velocity) * Time.deltaTime);
    }

    IEnumerator MoveEnum() {
        float t = 0;

        float randDir = Mathf.Sign(Random.Range(-100, 100));

        while (t < timeTillDrill) {

            if (controller.collisions.right || controller.collisions.left) {
                burstForce(Vector2.up * 1, true);
            }

            velocity.x = randDir * horizVelocity;

            yield return null;

            if(controller.collisions.below)
                t += Time.deltaTime;
        }

        int angle = Mathf.RoundToInt(get360Angle((Vector2)transform.position, Vector2.right));
        ShieldQuadrant curQuadrant = ShipManager.instance.getQuadrant(angle);

        damageDelegate hurtQuadrant = (() => {
            curQuadrant.GetComponent<ShieldQuadrant>().takeDamage();
        });

        setCurState(DamageEnum(timeToDrill, curQuadrant, hurtQuadrant));
    }

    IEnumerator DamageEnum(float time, ShieldQuadrant quadrent, damageDelegate damageAction) {
        velocity.x = 0;
        yield return new WaitForSeconds(delayBetweenActions);
        healtbar?.ShowSlider();
        float t = 0;
        float soundtime = 0;
        while (t < time && quadrent.canTakeDamage()) {
            healtbar?.UpdateSlider(t / time);

            if (t > soundtime) {
                soundtime += time / 3;
            }

            t += Time.deltaTime;
            yield return null;
        }
        healtbar?.HideSlider();

        if (t >= time && quadrent.canTakeDamage())
        {
            damageAction.Invoke();
        }
        yield return new WaitForSeconds(delayBetweenActions);
        setCurState(MoveEnum());
    }

    private void setCurState(IEnumerator state) {
        if (curState != null)
            StopCoroutine(curState);

        curState = state;

        if (state != null) {
            StartCoroutine(curState);
        }
    }

    private void OnTrigger(Transform collision)
    {
        setCurState(null);
        healtbar?.HideSlider();
        //GameObject stuff = Instantiate(coin, transform.position, Quaternion.identity);
        //Destroy(stuff, 1f);
        float thisAngle = get360Angle((Vector2)transform.position, Vector2.right);
        float otherAngle = get360Angle((Vector2)collision.position, Vector2.right);
        bool collideRight = thisAngle > otherAngle;

        //Note that we have a corner case on the crossover between 1 - 359
        GetComponent<Collider2D>().enabled = false;
        burstForce(Vector3.up * 20 + Vector3.right * (collideRight ? 15 : -15), false);
        Destroy(this.gameObject, 0.75f);
        sprite.transform.DORotate(new Vector3(0,0, collideRight ? 360 : -180), 2, RotateMode.FastBeyond360);
    }

    //TODO: abstract this into its own class since this also happen in Healer
    private float get360Angle(Vector2 from, Vector2 to)
    {
        float ret = Vector2.SignedAngle(from, to);
        if (ret < 0)
        {
            ret += 360;
        }
        return ret;
    }

    public void burstForce(Vector3 velocity, bool additive)
    {
        if (additive)
        {
            this.velocity += velocity;
        }
        else
        {
            this.velocity = velocity;
        }
    }
}
