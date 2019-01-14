using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoJar : Interactable {

    void Start() {
        Initialize();
    }

    protected override void Initialize() {
        rend = GetComponentInChildren<SpriteRenderer>();
        baseParent = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        baseType = rb.bodyType;
        ChangeState(curState);
    }

    public override void ChangeState(States state) {
        curState = state;
        Debug.Log("SWITCHING TO " + curState);
        switch (curState) {
            case States.off:
                break;
            case States.on:
                break;
            case States.disabled:
                break;
        }
    }

}
