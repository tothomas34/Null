using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Detector : MonoBehaviour
{

    [SerializeField] private ControlerTopDown player;
    private Vector3 P;
    private Vector3 G;
    private Vector3 flatToTarget;
    private Vector3 flatForward;
    private Vector3 toTarget;

    private StateMachine stateMachine;
    public GuardPatrolState patrolState;
    public GuardAlertState alertState;
    public GuardShootingState shootingState;
    public GuardSusState susState;

    public Transform[] points;
    private int destPoint = 0;
    public NavMeshAgent agent;

    [SerializeField] private LayerMask layerMask;
    private int actualSpeed;
    [SerializeField] private int walkSpeed;
    [SerializeField] private int runSpeed;




    [SerializeField] private float distance = 8f;

    private float cosHalfFov;
    [SerializeField] private float fieldOfViewDegrees = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public string CurrentStateName => stateMachine != null && stateMachine.CurrentState != null
    ? stateMachine.CurrentState.GetType().Name
    : "Uninitialized";


    private void Awake()
    {
        stateMachine = new StateMachine();
        patrolState = new GuardPatrolState(this);
        alertState = new GuardAlertState(this);
        shootingState = new GuardShootingState(this);
        susState = new GuardSusState(this);

        cosHalfFov = Mathf.Cos(fieldOfViewDegrees * 0.5f * Mathf.Deg2Rad);
    }
    void Start()
    {
        stateMachine.ChangeState(patrolState);




    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();

    }

    public void StaterChanger(GuardBaseState newState)
    {
        stateMachine.ChangeState(newState); 
    }

    public void NextStep()
    {

        Debug.Log("NEXTSTEP");
        // Renvoi si aucun point n'est rentré
        if (points.Length == 0)
            return;

        // Set the agent to   to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    //ALERTE FONCTION
    public void Chase()
    {
        agent.destination = player.transform.position;

    }

    public void StopAgent()
    {
        agent.destination = transform.position;

    }

    public void ChaseMod()
    {
        fieldOfViewDegrees = 360;
        cosHalfFov = Mathf.Cos(fieldOfViewDegrees * 0.5f * Mathf.Deg2Rad);
        distance = 20;
        agent.speed = runSpeed;

    }

    public void PatrolMod()
    {
        fieldOfViewDegrees = 90;
        cosHalfFov = Mathf.Cos(fieldOfViewDegrees * 0.5f * Mathf.Deg2Rad);
        distance = 8;
        agent.speed = walkSpeed;

    }

    public bool Detection()
    {
        P = player.transform.position;
        G = transform.position;
        toTarget = P - G;



        Debug.Log("HERE");

        if (toTarget.sqrMagnitude < distance * distance)
        {

            Vector3 forward = transform.forward;
            Vector3 delta = toTarget;

            flatToTarget = new Vector3(delta.x, 0f, delta.z);
            flatForward = new Vector3(forward.x, 0f, forward.z);

            flatToTarget.Normalize();
            flatForward.Normalize();

            float dotProduct = flatToTarget.x * flatForward.x + flatToTarget.y * flatForward.y + flatToTarget.z * flatForward.z;
            //Debug.Log(dotProduct);
            if (dotProduct > cosHalfFov)
            {
                if (Vector3.Dot(toTarget.normalized, transform.forward) > cosHalfFov)
                {
                    RaycastHit hit;

                    // Lance un rayon en direction du joueur


                    if (Physics.Raycast(transform.position, toTarget.normalized, out hit, distance, layerMask))
                    {
                        Debug.DrawRay(transform.position, toTarget.normalized * hit.distance, Color.yellow);

                        if (hit.collider.CompareTag("Player"))
                        {

                            Debug.Log(true);
                            return true;
                        }
                        else
                        {
                            Debug.Log("Obstacle between enemy and player!");

                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, toTarget.normalized * distance, Color.white);
                        Debug.Log("Raycast did not hit anything");
                    }
                }
            }
            else
            {
                Debug.Log(false);


            }


        }
        else
        {
            Debug.Log(false);
        }
        return false;

    }

    public Vector3 WhereIsPlayer()
    {
        return player.transform.position;
    }

    public float GetDistanceToPlayer()
    {
        return toTarget.sqrMagnitude;
    }

    public void isSuspicious(Vector3 playerPosition) {
        agent.destination = playerPosition;

    }

    private void OnDrawGizmos()
    {


        // 1. Dessiner le rayon d'action maximum (Cercle de distance)
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f); // Rouge très transparent pour le fond
        Gizmos.DrawSphere(transform.position, distance);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, distance);

        // 2. Calculer les directions des bords du FOV avec les Quaternions
        // On prend la direction avant de l'objet aplatie sur l'axe XZ (Top-down)
        Vector3 forwardXZ = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        if (forwardXZ == Vector3.zero) forwardXZ = Vector3.forward;

        // On calcule la moitié de l'angle pour l'appliquer à gauche et à droite
        float halfAngle = fieldOfViewDegrees * 0.5f;

        // Création des quaternions de rotation sur l'axe Y (vertical)
        Quaternion leftRotation = Quaternion.Euler(0, -halfAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, halfAngle, 0);

        // Multiplication du Quaternion par le vecteur pour obtenir les directions orientées
        Vector3 leftRayDirection = leftRotation * forwardXZ;
        Vector3 rightRayDirection = rightRotation * forwardXZ;

        // 3. Dessiner les lignes du FOV
        Gizmos.color = Color.yellow; // Couleur jaune pour les limites du FOV

        // Ligne gauche : Part du centre de l'objet et va jusqu'à la distance max
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * distance);

        // Ligne droite : Part du centre de l'objet et va jusqu'à la distance max
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * distance);

        // Ligne centrale (Optionnelle) : Pour bien voir où regarde l'objet
        /*
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + forwardXZ * distance);*/
    }
}
