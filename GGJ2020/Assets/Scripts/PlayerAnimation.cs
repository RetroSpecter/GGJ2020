using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void setAnimation(Vector2 velocity) {
        anim.SetFloat("x_velocity", Mathf.Abs(velocity.x)/12f);
        anim.SetFloat("y_velocity", velocity.y);
        transform.localEulerAngles = new Vector2(0, velocity.x > 0.1f ? 90 : 270);
    }

    public void setTrigger(string trigger) {
        anim.SetTrigger(trigger);
    }

    public void setBoolean(string boolean, bool state) {
        anim.SetBool(boolean, state);
    }
}
