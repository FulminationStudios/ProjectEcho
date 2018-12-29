using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public enum InteractTypes
    {
        button,
        lever
    }
    public InteractTypes interactType;

    public enum States {
        off,
        on,
        disabled
    }
    public States curState;
    public List<Activatable> activatableObjects = new List<Activatable>();
    protected SpriteRenderer rend;
    public Sprite onSprite, offSprite;


	// Use this for initialization
	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        DoState();
    }

    protected void RenameToParent() {
        gameObject.name = transform.parent.name;
    }

    protected virtual void Initialize() {
        rend = GetComponent<SpriteRenderer>();
        ChangeState(curState);
    }
    public virtual bool IsActive() {
        return curState == States.on;
    }
    public virtual void ChangeState(bool state) {
        if (curState != States.disabled) {
            if (state) {
                ChangeState(States.on);
            } else {
                ChangeState(States.off);
            }
        }
    }
    public virtual void ChangeState(States state) {
        curState = state;
        ChangeSprite();
        switch (curState) {
            case States.on:
                break;
            case States.off:
                break;
            default:
                break;
        }
        ActivateObjects();
    }

    protected virtual void DoState() {
        switch (curState) {
            case States.on:
                break;
            case States.off:
                break;
            default:
                break;
        }
    }

    protected virtual void ChangeSprite() {
        switch(curState) {
            case States.on:
                rend.sprite = onSprite;
                break;
            case States.off:
                rend.sprite = offSprite;
                break;
            default:
                break;
        }
    }

    protected virtual void AddActivatable(Activatable activatable) {
        activatableObjects.Add(activatable);
    }

    protected void ActivateObjects() {
        foreach (Activatable act in activatableObjects) {
            act.DetermineState();
        }
    }

    void OnValidate() {
        RenameToParent();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        foreach (Activatable act in activatableObjects) {
            Gizmos.DrawLine(transform.position, act.transform.position);
            Gizmos.DrawWireSphere(act.transform.position, 0.5f);
        }
    }
}
