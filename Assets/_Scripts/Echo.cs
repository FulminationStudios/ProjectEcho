using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : NPC {

    Player thePlayer;
    int eventIndex = 0;

    void Start() {
        Initialize();
        Pathing();
    }

    protected override void Initialize() {
        thePlayer = FindObjectOfType<Player>();
        PathPoints = thePlayer.echoPathPoints;
        transform.position = PathPoints[0].position;
        base.Initialize();
    }

    void Update() {
        if (endOfPath) {
            DespawnEcho();
        }
        EchoInteract();
        DoTimeAlive();
    }

    void DespawnEcho() {
        Debug.Log("Echo Despawned");
        Destroy(gameObject.transform.parent.gameObject);
    }

    void EchoInteract() {
        if (eventIndex < thePlayer.eventPoints.Count) {
            if (timeAlive >= thePlayer.eventPoints[eventIndex]) {
                base.Interact();
                eventIndex++;
                eventIndex = Mathf.Clamp(eventIndex, 0, thePlayer.eventPoints.Count);
            }
        }
    }

}
