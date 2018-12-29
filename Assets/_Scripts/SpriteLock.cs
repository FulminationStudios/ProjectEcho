using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLock : MonoBehaviour
{

    private SpriteRenderer sprite;
    private float spriteLockValue;
    public bool lockX = true;
    public bool lockY = true;
    public bool lockZ = true;
    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        spriteLockValue = sprite.sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void Update()
    {
        LockSprite();
    }

    void LockSprite()
    {
        float lockXPos = Mathf.Round(transform.parent.position.x * spriteLockValue) / spriteLockValue;
        float lockYPos = Mathf.Round(transform.parent.position.y * spriteLockValue) / spriteLockValue;
        if (!lockX) {
            lockXPos = transform.parent.position.x;
        }
        if (!lockY) {
            lockYPos = transform.parent.position.y;
        }
        if (lockZ) {
            transform.rotation = Quaternion.identity;
        }
        transform.position = new Vector2(lockXPos,lockYPos);

    }
}
