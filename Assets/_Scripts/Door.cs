using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable {

    Collider2D doorCollider;
	// Use this for initialization
	void Start () {
        Initialize();
	}

    protected override void Initialize() {
        doorCollider = GetComponent<Collider2D>();
        base.Initialize();
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected override void ChangeState(bool state) {
        OpenDoor(state);
        base.ChangeState(state);
    }

    void OpenDoor(bool state) {
        if(!hasBeenInitialized) {
            Initialize();
        }
        doorCollider.enabled = !state;
    }
}
