using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Entity {

    public List<PathPoint> PathPoints = new List<PathPoint>();
    protected int pathIndex;
    protected float curWaitTime;
    protected float waitTimer;
    protected bool doWait;
    protected bool endOfPath;

    public enum movementMethods { travel, snap };
    public movementMethods movementMethod;

    void Start() {
        pathIndex = 0;
        Initialize();
    }

    protected override void Initialize() {
        base.Initialize();
        Pathing();
    }

    protected virtual void Pathing() {
        if (PathPoints.Count > 0) {
            if (pathIndex < PathPoints.Count) {
                //for (int i = 0; i < PathPoints.Count; i++) {
                //    Debug.Log("PATHPOINT " + i + ": " + PathPoints[i].position);
                //}
                StartCoroutine(Movement(PathPoints[0]));
            }
        }
    }

    protected virtual IEnumerator Movement(PathPoint point) {
        switch (movementMethod) {
            case movementMethods.snap:
                rb.MovePosition(point.position);
                break;
            case movementMethods.travel:
                rb.MovePosition(Vector2.MoveTowards(transform.position, point.position, moveSpeed));
                break;
        }
        ChangeDirection(point.localPosition);
        DoSpriteLock(point.position - transform.position);
        while (transform.position != point.position) {
            yield return null;
        }

        pathIndex++;
        endOfPath = pathIndex >= PathPoints.Count;
        pathIndex = Mathf.Clamp(pathIndex, 0, PathPoints.Count - 1);

        if (point.waitTime > 0) {
            yield return new WaitForSeconds(point.waitTime);
        }
        yield return Movement(PathPoints[pathIndex]);
    }

    protected override void ChangeDirection(Vector2 direction) {
        Debug.Log("DIRECTION: " + direction);
        base.ChangeDirection(direction);
    }

}

[System.Serializable]
public class PathPoint {
    public Vector3 position;
    public Vector3 localPosition;
    public float waitTime;

    public PathPoint(Vector2 position_, Vector2 localPosition_, float waitTime_ = 0f) {
        position = position_;
        localPosition = localPosition_;
        waitTime = waitTime_;
    }
}

[System.Serializable]
public class EventPoint {
    public float timeOfEvent;
    public bool interactSuccessful;
    public EventPoint(float time, bool wasSuccessful) {
        timeOfEvent = time;
        interactSuccessful = wasSuccessful;
    }
}
