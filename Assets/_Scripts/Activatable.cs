using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour {

    [HideInInspector]
    public List<PowerSources> powerSources = new List<PowerSources>();
    public enum logicStates { AND, OR };
    public logicStates logicState;
    public bool isActive = false;
    public Sprite onSprite, offSprite;
    protected SpriteRenderer rend;
    protected bool hasBeenInitialized = false;
    protected bool autoSetSprite = true;

	// Use this for initialization
	void Start () {
        Initialize();
	}

    protected virtual void Initialize() {
        rend = GetComponent<SpriteRenderer>();
        SetupSources();
        hasBeenInitialized = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void RenameToParent() {
        gameObject.name = transform.parent.name;
    }

    protected virtual void ChangeState(bool state) {
        isActive = state;
        if (autoSetSprite) {
            ChangeSprite(state);
        }
    }
    protected virtual void ChangeSprite(bool state) {
        if (rend == null) {
            rend = GetComponent<SpriteRenderer>();
        }
        if (state) {
            rend.sprite = onSprite;
        } else  {
            rend.sprite = offSprite;
        }
    }

    public void DetermineState() {
        bool pendingState = false;
        
        switch(logicState) {
            case logicStates.AND:
                pendingState = true;
                foreach(PowerSources source in powerSources) {
                    if (pendingState == true) {
                        if (source.interactable.IsActive()) {
                            if (source.isInverse) {
                                pendingState = false;
                            }
                        } else {
                            if (!source.isInverse) {
                                pendingState = false;
                            }
                        }
                    }
                }
                break;
            case logicStates.OR:
                foreach(PowerSources source in powerSources) {
                    if (pendingState == false) {
                        if (source.interactable.IsActive()) {
                            if (!source.isInverse) {
                                pendingState = true;
                            }
                        } else {
                            if (source.isInverse) {
                                pendingState = true;
                            }
                        }
                    }
                }
                break;
        }
        foreach(PowerSources source in powerSources) {
            if (source.interactable.IsActive()) {
                if (source.isInverse) {
                    source.output = "FALSE";
                } else {
                    source.output = "TRUE";
                }
            } else {
                if (source.isInverse) {
                    source.output = "TRUE";
                } else {
                    source.output = "FALSE";
                }
            }
        }
        ChangeState(pendingState);
    }

    public void SetupSources() {
        foreach (PowerSources source in powerSources) {
            if (!source.interactable.activatableObjects.Contains(this)) {
                source.interactable.activatableObjects.Add(this);
            }
        }
    }

    void OnValidate() {
        RenameToParent();
        //SetupSources();
    }

    void OnDrawGizmosSelected() {
        foreach (PowerSources source in powerSources) {
            if (source.interactable != null) {
                if (source.isInverse) {
                    Gizmos.color = Color.red;
                } else {
                    Gizmos.color = Color.blue;
                }
                Gizmos.DrawLine(transform.position, source.interactable.transform.position);
                Gizmos.DrawWireSphere(source.interactable.transform.position, 0.5f);
            }
        }
    }
}

public class Scaleable : Activatable {

    [HideInInspector]
    public Vector2 dimensions;
    protected BoxCollider2D hitBox;

    void Start() {
        Initialize();
    }

    protected override void Initialize() {
        hitBox = GetComponent<BoxCollider2D>();
        base.Initialize();
    }


    public virtual void UpdateDimensions(Vector2 newLength) {
        Vector2 intDimensions = new Vector2(Mathf.Ceil(newLength.x), Mathf.Ceil(newLength.y));
        //if (!hasBeenInitialized) {
            Initialize();
        //}
        bool isVisible = (!ExtraMath.IsEqual(newLength.x, 0) && !ExtraMath.IsEqual(newLength.y, 0));
        hitBox.enabled = isVisible;
        rend.enabled = isVisible;
        if (isVisible) {
            rend.size = intDimensions;
            hitBox.offset = new Vector2(intDimensions.x / 2,dimensions.y / 2);
            hitBox.size = newLength;
        }
        //dimensions = intDimensions;
    }

}

[System.Serializable]
public class PowerSources {
    public Interactable interactable;
    public bool isInverse;
    public string output = "FALSE";

    public PowerSources(Interactable source, bool is_inverse) {
        interactable = source;
        isInverse = is_inverse;
    }
}
