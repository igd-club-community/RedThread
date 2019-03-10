using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActingPerson : MonoBehaviour
{
    public PersonAct[] actions;
    public PersonAct currentAction;
    public bool noAction = false;
    int curActNum = 0;
    public float distance = 0;

    public GameObject textBackground;
    public Text bubbleText;

    protected Animator anim;
    NavMeshAgent navAgent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 smoothWorld2dDelta = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    //У персоны есть его целевая позиция куда он хочет идти
    //Есть базовый цикл из задач которые он делает пока ничего не происходит
    //У каждой отдельной персоны будет свой цикл.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
    }

    public float sayTime = 0;
    // Update is called once per frame
    protected virtual void Update()
    {
        NavAgentUpdate();
        navAgent.SetDestination(currentAction.target.position);
        //if (noAction)
        //{
        //    navAgent.enabled = false;
        //}
        //else
        //{
        //    navAgent.enabled = true;
        //}
        //Если мы долши до нужной точки
        //distance = Vector3.Distance(currentAction.target.position, transform.position);
        //if (distance < 0.1)
        //{
        //    //Тогда запускаем нужную анимацию.
        //}

        //bubbleText.text = textBackground.gameObject.transform.rotation.ToString();
        textBackground.gameObject.transform.rotation = Quaternion.identity; //new Quaternion(0,0,0,1);
        bubbleText.gameObject.transform.rotation = Quaternion.identity;

        if (Time.fixedTime - sayTime > 4)
        {
            textBackground.SetActive(false);
            bubbleText.text = "";
        }
    }
    void NavAgentUpdate()
    {
        //Debug.Log(navAgent.updatePosition);
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;
        //Debug.Log("transform.position = " + transform.position);
        //Debug.Log("navAgent.nextPosition = " + navAgent.nextPosition);
        Debug.Log("worldDeltaPosition = " + worldDeltaPosition.x + " " + worldDeltaPosition.y + " " +worldDeltaPosition.z);
        Vector2 world2dDelta = new Vector2(worldDeltaPosition.x, worldDeltaPosition.z);
        Debug.Log("world2dDelta = " + world2dDelta.x + " " + world2dDelta.y);
        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        Debug.Log("smooth = " + smooth);
        smoothWorld2dDelta = Vector2.Lerp(smoothWorld2dDelta, world2dDelta, smooth);
        Debug.Log("smoothWorld2dDelta = " + smoothWorld2dDelta.x + " " + smoothWorld2dDelta.y);
        //float result = Vector3.SignedAngle(worldDeltaPosition, transform.forward, Vector3.up);
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        Debug.Log("forward = " + forward.x + " " + forward.y);
        float result = Vector2.SignedAngle(smoothWorld2dDelta, forward);
        Debug.Log(result);
        Debug.Log("world2dDelta = " + world2dDelta.magnitude);

        //// Map 'worldDeltaPosition' to local space
        //float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        //float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        //Vector2 deltaPosition = new Vector2(dx, dy);
        //Debug.Log("new deltaPosition = " + deltaPosition.x + " " + deltaPosition.y);


        // Update velocity if delta time is safe
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;

        // Update animation parameters
        float linearSpeed;
        if (navAgent.remainingDistance < 0.01) {
            Debug.Log("zero speed");
            linearSpeed = 0;
        }
        //if (world2dDelta.magnitude < 0.1)
        //    linearSpeed = 0;
        //else if (world2dDelta.magnitude > 0.2)
        //    linearSpeed = 0.7f;
        else
            linearSpeed = 1;
        //linearSpeed = world2dDelta.magnitude;

        float angularSpeed = result/180*((float)Math.PI);
        //if (Math.Abs(angularSpeed) < 0.1)
        //{
        //    angularSpeed = 0;
        //}

        Debug.Log("Linear = " + linearSpeed + " angular " + angularSpeed);
        anim.SetFloat("LinearSpeed", linearSpeed);
        anim.SetFloat("AngularSpeed", -angularSpeed);

        navAgent.nextPosition = transform.position;
        //transform.rotation = navAgent.transform.rotation;

        //anim.SetBool("move", shouldMove);
        //anim.SetFloat("velx", velocity.x);
        //anim.SetFloat("vely", velocity.y);

        //LookAt lookAt = GetComponent<LookAt>();
        //if (lookAt)
        //    lookAt.lookAtTargetPosition = navAgent.steeringTarget + transform.forward;

        ////		// Pull character towards agent
        //if (worldDeltaPosition.magnitude > navAgent.radius)
        //    transform.position = navAgent.nextPosition - 0.9f * worldDeltaPosition;

        ////		// Pull agent towards character
        //if (worldDeltaPosition.magnitude > navAgent.radius)
        //    navAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
    }
    public void setAction(PersonAct newAct)
    {
        currentAction = newAct;
        //say(newAct.phrases[UnityEngine.Random.Range(0, newAct.phrases.Length)]);
    }

    public void say(string text)
    {
        textBackground.SetActive(true);
        bubbleText.text = text;
        sayTime = Time.fixedTime;
    }
    //void OnAnimatorMove()
    //{
    //    // Update postion to agent position
    //    transform.position = navAgent.nextPosition;

    //    // Update position based on animation movement using navigation surface height
    //    Vector3 position = anim.rootPosition;
    //    position.y = navAgent.nextPosition.y;
    //    transform.position = position;
    //}
}

public enum PersonState
{
    Idle,
    Acting
}


//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Animator))]
//public class MotionSimpleAgent : MonoBehaviour
//{
//    Animator anim;
//    NavMeshAgent agent;
//    Vector2 smoothDeltaPosition = Vector2.zero;
//    Vector2 velocity = Vector2.zero;

//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        agent = GetComponent<NavMeshAgent>();
//        agent.updatePosition = false;
//    }

    

//}
