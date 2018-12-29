using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable {

    //private Entity triggeringMass;
    private int triggeringMasses;

	// Use this for initialization
	void Start () {
        base.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.GetComponent<Entity>() != null) {
            base.ChangeState(States.on);
            triggeringMasses++;
            //triggeringMass = col.GetComponent<Entity>();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<Entity>() != null)
        {
            triggeringMasses--;
            triggeringMasses = Mathf.Clamp(triggeringMasses, 0, 10);
            if (triggeringMasses <= 0) {
                base.ChangeState(States.off);
            }
            //triggeringMass = null;
        }
    }
}
