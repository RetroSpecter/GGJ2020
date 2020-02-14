using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Animator anim;
    Vector3 targetRot;

    // Start is called before the first frame update
    void Start()
    {
        targetRot = new Vector3(0, 90,0);
        anim = GetComponent<Animator>();
    }

    public void setAnimation(Vector2 velocity) {
        if (Mathf.Abs(velocity.x) > 0.1f) {
            anim.SetFloat("x_velocity", Mathf.Abs(velocity.x) / 12f);
            targetRot = new Vector2(0, velocity.x > 0.1f ? 90 : 270);
        }
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRot, Time.deltaTime * 7);
        anim.SetFloat("y_velocity", velocity.y);
    }

    public void setTrigger(string trigger) {
        anim.SetTrigger(trigger);
    }

    public void setBoolean(string boolean, bool state) {
        anim.SetBool(boolean, state);
    }
}
