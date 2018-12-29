using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoMist : Scaleable {

    void Start() {
        Initialize();
        ChangeState(isActive);
    }

    protected override void ChangeState(bool state) {
        if (state) {
            UpdateDimensions(dimensions);
        } else {
            UpdateDimensions(Vector2.zero);
        }
        autoSetSprite = false;
        base.ChangeState(state);
    }

}
