using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : NPC {

    Player thePlayer;

    void Start() {
        Initialize();
        Pathing();
    }

    protected override void Initialize() {
        thePlayer = FindObjectOfType<Player>();
        PathPoints = thePlayer.echoPathPoints;
        base.Initialize();
    }

    void DoEchoDespawn() {
        if (transform.position == thePlayer.transform.position) {
            Destroy(gameObject.transform.parent);
        }
    }
}
