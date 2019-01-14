using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour {

    public string entityName = "Entity";
    public float moveSpeed = 10f;
    public bool isImmortal = false;
    public bool isMoving = false;
    public int maxHealth = 1;
    protected int curHealth;
    protected float timeAlive;
    protected Interactable carryingItem;

    public enum EntityStates {
        dead,
        alive,
        inactive
    }
    public enum XDirection {
        right,
        left
    }
    public enum YDirection {
        down,
        up
    }
    public XDirection curXDirection;
    public YDirection curYDirection;
    protected EntityStates curState;
    protected Rigidbody2D rb;
    protected List<SpriteLock> sprites = new List<SpriteLock>();
    protected SpriteRenderer rend;
    public SpriteKey[] spriteKeys;
    protected Transform effectorBox;

	// Use this for initialization
	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        DoState();
        DoTimeAlive();
	}

    protected void DoTimeAlive() {
        timeAlive += Time.deltaTime;
    }

    protected virtual void Initialize() {
        gameObject.name = entityName;
        rend = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        effectorBox = transform.Find("EffectorBox");
        GetSprites();
        ChangeState(EntityStates.alive);
        ChangeDirection(XDirection.right, YDirection.down);
    }

    public virtual void ChangeHealth(int amount) {
        curHealth += amount;
        if (curHealth >= maxHealth) {
            curHealth = maxHealth;
        } else if (curHealth <= 0) {
            ChangeState(EntityStates.dead);
        }
    }

    public float GetTimeAlive() {
        return timeAlive;
    }

    public void ResetTimeAlive(){
        timeAlive = 0;
    }

    public virtual void ChangeState(EntityStates state) {
        curState = state;
        switch (curState) {
            case EntityStates.alive:
                Alive();
                break;
            case EntityStates.dead:
                if (isImmortal) {
                    ChangeState(EntityStates.alive);
                } else {
                    Die();
                }
                break;
            default:
                break;
        }
    }

    protected virtual void DoState() {
        switch (curState) {
            case EntityStates.alive:
                break;
            case EntityStates.dead:
                break;
            default:
                break;
        }
    }

    protected virtual void Die() {
        Debug.Log(entityName + " died.");
        Destroy(gameObject);
    }

    protected virtual void Alive() {
        Debug.Log(entityName + " is alive.");
    }

    protected virtual void EditorUpkeep() {
        gameObject.name = entityName;
    }
    protected virtual void GetSprites() {
        foreach (SpriteLock sl in GetComponents<SpriteLock>()) {
            sprites.Add(sl);
        }
        foreach(SpriteLock sl in GetComponentsInChildren<SpriteLock>()) {
            sprites.Add(sl);
        }
    }
    protected void LockSprites(string direction) {
        foreach (SpriteLock sl in sprites) {
            sl.lockX = direction.Contains("x");
            sl.lockY = direction.Contains("y");
            sl.lockZ = direction.Contains("z");
        }
    }

    protected virtual void DoSpriteLock(Vector2 direction) {
        if (ExtraMath.IsEqual(direction.x,0)) {
            if (ExtraMath.IsEqual(direction.y, 0)) {
                LockSprites("xyz");
            } else {
                LockSprites("xz");
            }
        } else {
            if (ExtraMath.IsEqual(direction.y, 0)) {
                LockSprites("yz");
            } else {
                LockSprites("z");
            }
        }
    }
    protected virtual void ChangeDirection(Vector2 direction) {
        XDirection thisX = XDirection.right;
        YDirection thisY = YDirection.down;

        if (direction.x > 0) { // Going Right
            thisX = XDirection.right;
        } else if (direction.x < 0) { // Going Left
            thisX = XDirection.left;
        } else { // Not Moving Horizontally
            thisX = curXDirection;
        }

        if (direction.y > 0) { // Going Up
            thisY = YDirection.up;
        } else if (direction.y < 0) { // Going Down
            thisY = YDirection.down;
        } else { // Not Moving Vertically
            thisY = curYDirection;
        }
        ChangeEffectorBoxLocation(direction);

        ChangeDirection(thisX, thisY);
    }
    protected virtual void ChangeEffectorBoxLocation(Vector2 direction) {
        if (direction.x > 0) { // Going Right
            effectorBox.localPosition = new Vector2(1, -0.5f);
        } else if (direction.x < 0) { // Going Left
            effectorBox.localPosition = new Vector2(-1, -0.5f);
        }

        if (direction.y > 0) { // Going Up
            effectorBox.transform.localPosition = new Vector2(0, 0.5f);
        } else if (direction.y < 0) { // Going Down
            effectorBox.transform.localPosition = new Vector2(0, -1.5f);
        }
    }
    protected virtual void ChangeDirection(XDirection x, YDirection y) {
        curXDirection = x;
        curYDirection = y;
        rend.sprite = GetSpriteWithKey(y + "_" + x);
    }
    protected Sprite GetSpriteWithKey(string key) {
        foreach (SpriteKey spriteKey in spriteKeys) {
            if (spriteKey.key.Equals(key)) {
                return spriteKey.sprite;
            }
        }
        return rend.sprite;
    }

    protected string GetCurrentSprite() {
        return rend.sprite.name;
    }

    protected virtual void Interact() {
        if (carryingItem != null) {
            DropItem();                                                                           
        } else {
            Interactable thisObject = GetEffectorObject();
            if (thisObject != null) {
                switch (thisObject.interactType) {
                    case Interactable.InteractTypes.lever:
                        thisObject.ChangeState(!thisObject.IsActive());
                        break;
                    case Interactable.InteractTypes.holdable:
                        PickUpItem(thisObject);
                        break;
                }
            }
        }
    }

    protected virtual void PickUpItem(Interactable item) {
        carryingItem = item;
        carryingItem.ChangeState(Interactable.States.on);
        carryingItem.transform.parent = transform;
        carryingItem.transform.localPosition = Vector3.up;
        SpriteLock[] itemSprites = carryingItem.GetComponentsInChildren<SpriteLock>();
        foreach (SpriteLock sprite in itemSprites) {
            sprites.Add(sprite);
        }
        foreach(Collider2D thisCollider in carryingItem.GetComponents<Collider2D>()) {
            thisCollider.enabled = false;
        }
        if (carryingItem.rb != null) {
            carryingItem.rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    protected virtual void DropItem() {
        carryingItem.ChangeState(Interactable.States.off);
        carryingItem.transform.position = effectorBox.transform.position;
        SpriteLock[] itemSprites = carryingItem.GetComponentsInChildren<SpriteLock>();
        foreach (SpriteLock sprite in itemSprites) {
            sprites.Remove(sprite);
        }
        carryingItem.transform.parent = carryingItem.baseParent;
        foreach (Collider2D thisCollider in carryingItem.GetComponents<Collider2D>()) {
            thisCollider.enabled = true;
        }
        if (carryingItem.rb != null) {
            carryingItem.rb.bodyType = carryingItem.baseType;
        }
        carryingItem = null;

    }
    protected virtual void CarryItem() {

    }

    protected virtual Interactable GetEffectorObject() {
        List<Interactable> validObjects = new List<Interactable>();
        Interactable returnObj = null;
        foreach (Collider2D col in Physics2D.OverlapBoxAll(effectorBox.transform.position,effectorBox.transform.localScale,0)) {
            Interactable thisObject = col.GetComponent<Interactable>();
            if (thisObject != null) {
                validObjects.Add(thisObject);
            }
        }
        float proximity = 99999f;
        foreach(Interactable obj in validObjects) {
            float thisDistance = Vector2.Distance(gameObject.transform.position, obj.transform.position + new Vector3(0.5f,0.5f,0));
            if (thisDistance < proximity) {
                proximity = thisDistance;
                returnObj = obj;
            }
        }
        return returnObj;
    }
}
[System.Serializable]
public class SpriteKey {
    public Sprite sprite;
    public string key;
}
