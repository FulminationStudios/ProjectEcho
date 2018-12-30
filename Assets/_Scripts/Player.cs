using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity {

    public List<PathPoint> echoPathPoints = new List<PathPoint>();
    public List<EventPoint> eventPoints = new List<EventPoint>();

    public GameObject echoObject;
    private Echo echo;
    public int numEchoesActive;

	void Start () {
        Initialize();
	}

    protected override void Initialize() {
        base.Initialize();
    }

    void Update () {
        base.DoState();
        Interact();
        SummonEcho();
        Movement();
        DoTimeAlive();
    }

    void FixedUpdate() {
    }

    void OnValidate() {
        base.EditorUpkeep();
    }

    void SummonEcho() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (echoPathPoints.Count > 0 && numEchoesActive < GameData.maxEchoes) {
                echo = Instantiate(echoObject, echoPathPoints[0].position, Quaternion.identity).GetComponent<Echo>();
                numEchoesActive++;
            }
        }
    }

    void Movement() {
        Vector2 directionX = Vector2.right * Input.GetAxisRaw("Horizontal");
        Vector2 directionY = Vector2.up * Input.GetAxisRaw("Vertical");

        if (directionX == Vector2.zero) {
            if (directionY == Vector2.zero) {
                base.LockSprites("xyz");
            } else  {
                base.LockSprites("xz");
            }
        } else {
            if (directionY == Vector2.zero) {
                base.LockSprites("yz");
            } else {
                base.LockSprites("z");
            }
        }
        Vector2 direction = (directionX + directionY).normalized;
        if (direction != Vector2.zero) {
            rb.MovePosition(rb.position + (direction * moveSpeed) * Time.deltaTime);
            base.ChangeDirection(direction);
        } else {
            rb.velocity = Vector2.zero;
        }
        MakeEchoPathPoints(direction,transform.position);
    }

    void MakeEchoPathPoints(Vector2 movementVector, Vector2 globalPosition) {
        if (movementVector == Vector2.zero && echoPathPoints.Count > 0) {
            echoPathPoints[echoPathPoints.Count - 1].waitTime += Time.deltaTime;
        } else {
            echoPathPoints.Add(new PathPoint(globalPosition, movementVector));
        }
    }

    protected override void Interact() {
        if (Input.GetKeyDown(KeyCode.E)) {
            eventPoints.Add(new EventPoint(timeAlive,GetEffectorObject() != null));
            base.Interact();
        }
    }

    protected override void Die() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Application.OpenURL("https://res.cloudinary.com/teepublic/image/private/s--NLcTrgMZ--/t_Preview/b_rgb:191919,c_lpad,f_jpg,h_630,q_90,w_1200/v1466191557/production/designs/549487_1.jpg");
    }
}

