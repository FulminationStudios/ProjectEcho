using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity {

    // Use this for initialization
    void Start() {
        Initialize();
    }

    protected override void Initialize() {
        isImmortal = true;
        base.Initialize();
    }

    // Update is called once per frame
    void Update() {
        SmoothPushing();
    }

    void OnValidate() {
        base.EditorUpkeep();
    }

    void SmoothPushing() {
        if (ExtraMath.IsEqual(rb.velocity.x,0f,0.5f)) {
            if (ExtraMath.IsEqual(rb.velocity.y, 0f,0.5f)) {
                base.LockSprites("xyz");
            } else {
                base.LockSprites("xz");
            }
        } else {
            if (ExtraMath.IsEqual(rb.velocity.y, 0f,0.5f)) {
                base.LockSprites("yz");
            } else {
                base.LockSprites("z");
            }
        }
    }
}
