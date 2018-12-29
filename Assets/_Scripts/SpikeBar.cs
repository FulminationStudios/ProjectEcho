using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBar : Scaleable {

    public int damage = 1;

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

    public override void UpdateDimensions(Vector2 newLength) {
        newLength = new Vector2(0.625f, newLength.y);
        base.UpdateDimensions(newLength);
    }

    void OnCollisionStay2D(Collision2D col) {
        Entity thisEntity = col.gameObject.GetComponent<Entity>();
        if (thisEntity != null) {
            thisEntity.ChangeHealth(-damage);
        }
    }

    private void OnValidate() {
        RenameToParent();
    }

}
