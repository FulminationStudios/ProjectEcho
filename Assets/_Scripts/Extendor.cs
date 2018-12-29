using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extendor : Activatable {

    private Animator anim;

    void Start() {
        Initialize();
    }

    protected override void Initialize() {
        anim = GetComponent<Animator>();
        base.Initialize();
    }

    protected override void ChangeState(bool state) {
        if (anim != null) {
            anim.SetBool("IsActivated", state);
        }
        autoSetSprite = false;
        base.ChangeState(state);
    }
}