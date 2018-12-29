using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

}

public static class GameData {
    public static int maxEchoes = 1;
}

public static class ExtraMath {

    public static bool IsEqual(float a, float b, float range = 0.002f) {
        return Mathf.Abs(a - b) < range ? true : false;
    }
    public static bool IsEqual(Vector3 a, Vector3 b, float range = 0.002f) {
        return Vector3.Distance(a, b) < range ? true : false;
    }
}
