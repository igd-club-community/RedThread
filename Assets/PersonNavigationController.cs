using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//По идее, навигатору нужно только ставить цель, после чего он каждый фрейм должен пересчитывать необходимые аниматору параметры
//ActingPerson соответственно должен выполняться позже навигатора, брать из него результаты вычислений и передавать аниматору
public class PersonNavigationController : MonoBehaviour
{
    protected Animator anim;
    NavMeshAgent navAgent;

    public bool startMoving = true;
    public bool onCourse;
    public bool inPlace;

    Transform target;
    public float minDistanceToTarget;
    public float distance = 100;//начальное значение должно быть больше дистанции первой задачи, чтобы не сработал переход к следующей задаче пока навмешагент в первый раз просчитывает путь
    public bool talkingWithPerson;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 smoothWorld2dDelta = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    public float clamp;

    public float maxLinearSpeed;
    public float linearSpeed;
    public float angularSpeed;

    public bool _targetReached;
    public bool targetReached
    {
        get
        {
            //    if (navAgent.pathPending)
            //        return false;
            //    else
            return _targetReached;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;

    }

    public void SetTarget(Transform target, float minDistanceToTarget, bool talkingWithPerson)
    {
        this.target = target;
        this.minDistanceToTarget = minDistanceToTarget;
        this.talkingWithPerson = talkingWithPerson;
         startMoving = true;
        onCourse = false;
        inPlace = false;
        _targetReached = false;
        targetForward = new Vector2(target.transform.forward.x, target.transform.forward.z);
    }
    public Vector3 deciredVelocity;
    public Vector3 nextPosition;
    public Vector3 worldDeltaPosition;
    public Vector2 world2dDelta;
    public float smooth;
    public Vector2 forward;
    public float resultAngle;
    public Vector2 targetForward;
    // Update is called once per frame
    void Update()
    {
        navAgent.SetDestination(target.position);
        if (navAgent.pathPending)
            return;
        distance = navAgent.remainingDistance;

        //полезная штука, потом может пригодиться
        deciredVelocity = navAgent.desiredVelocity;
        nextPosition = navAgent.nextPosition;

        worldDeltaPosition = navAgent.nextPosition - transform.position;

        world2dDelta = new Vector2(worldDeltaPosition.x, worldDeltaPosition.z);

        // Low-pass filter the deltaMove
        //smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);

        //smoothWorld2dDelta = Vector2.Lerp(smoothWorld2dDelta, world2dDelta, smooth);

        //float result = Vector3.SignedAngle(worldDeltaPosition, transform.forward, Vector3.up);
        forward = new Vector2(transform.forward.x, transform.forward.z);
        resultAngle = Vector2.SignedAngle(world2dDelta, forward);
        //Debug.Log(resultAngle);

        if (Math.Abs(resultAngle) < 10)
        {
            startMoving = false;
            onCourse = true;
        }
        else
        {
            onCourse = false;
        }
        //if (Math.Abs(resultAngle) > 110)
        //    startMoving = true;

        //// Map 'worldDeltaPosition' to local space
        //float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        //float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        //Vector2 deltaPosition = new Vector2(dx, dy);
        //Debug.Log("new deltaPosition = " + deltaPosition.x + " " + deltaPosition.y);


        // Update velocity if delta time is safe
        //if (Time.deltaTime > 1e-5f)
        //    velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;

        //Debug.Log("navAgent.remainingDistance = " + navAgent.remainingDistance);
        // Update animation parameters
        angularSpeed = resultAngle / 180 * ((float)Math.PI);
        if (navAgent.remainingDistance < minDistanceToTarget)
        {
            inPlace = true;
        }
        else if (navAgent.remainingDistance < 0.5f)
        {
            //Debug.Log("half speed");
            linearSpeed = maxLinearSpeed / 2;
        }
        else
            linearSpeed = maxLinearSpeed;

        //Цель достигнута, доворачиваем 
        if (inPlace)
        {
            linearSpeed = 0;
            if (talkingWithPerson)
            {
                _targetReached = true;
            } else
            {
                resultAngle = Vector2.SignedAngle(targetForward, forward);
                if (Math.Abs(resultAngle) < 10)
                {
                    _targetReached = true;
                    angularSpeed = 0;
                }
            }
        }

        angularSpeed = resultAngle / 180 * ((float)Math.PI);
        //else if (Math.Abs(resultAngle) > 90)
        //{
        //    //Debug.Log("zero speed, turning");
        //    linearSpeed = 0;
        //}
        //linearSpeed = world2dDelta.magnitude;


        //clamp = Math.Abs(angularSpeed) >= 1 ? 0.7f : 1;
        if (Math.Abs(angularSpeed) > 0.8f && Math.Abs(angularSpeed) < 1.5f)
            angularSpeed *= 1.5f;
        if (startMoving)
            linearSpeed = 0;
        //Debug.Log("Linear = " + linearSpeed + " angular " + angularSpeed);
        anim.SetFloat("LinearSpeed", linearSpeed, 0.3f, Time.deltaTime);
        if (onCourse)
            anim.SetFloat("AngularSpeed", angularSpeed, 0.3f, Time.deltaTime);
        else
            anim.SetFloat("AngularSpeed", angularSpeed, 0.3f, Time.deltaTime);

        navAgent.nextPosition = transform.position;
        //transform.rotation = navAgent.transform.rotation;


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
    void OnAnimatorMove()
    {
        // Update postion to agent position
        transform.position = navAgent.nextPosition;

        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
        position.y = navAgent.nextPosition.y;
        transform.position = position;
        transform.rotation = anim.rootRotation;
    }
}

public enum NavigationState
{
    StartMoving,
    OnCourse,
    InPlace
}