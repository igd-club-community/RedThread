using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//По идее, навигатору нужно только ставить цель, после чего он каждый фрейм должен пересчитывать необходимые аниматору параметры
//ActingPerson соответственно должен выполняться позже навигатора, брать из него результаты вычислений и передавать аниматору
public class PersonNavigationController : MonoBehaviour
{
    public string name;
    public Animator anim;
    NavMeshAgent navAgent;
    public NavigationState navState;

    public bool startMoving = true;
    public bool onCourse;
    public bool inPlace;

    Transform target;
    public float minDistanceToTarget;
    public float distance = 100;//начальное значение должно быть больше дистанции первой задачи, чтобы не сработал переход к следующей задаче пока навмешагент в первый раз просчитывает путь
    public bool talkingWithPerson;
    public string animationName = "";

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
        name = GetComponent<ActingPerson>().name;
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
        navState = NavigationState.StartMoving;

    }

    public void SetTarget(Transform target, float minDistanceToTarget, bool talkingWithPerson, string animationName)
    {
        this.target = target;
        this.minDistanceToTarget = minDistanceToTarget;
        this.talkingWithPerson = talkingWithPerson;
        
        if (!this.animationName.Equals(animationName))
            anim.SetBool(this.animationName, false);
        this.animationName = animationName;

        navState = NavigationState.StartMoving;
        _targetReached = false;
        targetForward = new Vector2(target.transform.forward.x, target.transform.forward.z);
        //if (debugText3 != null) debugText3.text = targetForward.ToString();
    }
    public Vector3 deciredVelocity;
    public Vector3 nextPosition;
    public Vector3 worldDeltaPosition;
    public float worldDeltaPositionMagn;
    public Vector2 world2dDelta;
    public float smooth;
    public Vector2 forward;
    public float prevAngleToTarget;
    public float angleToTarget;
    public Vector2 targetForward;
    public Vector3 vectorToTargetDelta;
    public Vector2 vectorToTarget;

    public Text debugText1;
    public Text debugText2;
    public Text debugText3;
    public Text debugText4;
    public Vector3 gizmo_nextPosition;
    public Vector3 gizmo_transformPosition;
    public Vector3 gizmo_worldDeltaPosition;
    // Update is called once per frame
    void Update()
    {
        navAgent.SetDestination(target.position);
        if (navAgent.pathPending) //В принципе можно заменить одним вычислением длинны дельты позиции которое уже применяется дальше
        {
            return;
        }
        distance = navAgent.remainingDistance;

        //if (debugText1 != null) debugText1.text = distance.ToString();
        //if (debugText2 != null) debugText2.text = navState.ToString();

        //полезная штука, потом может пригодиться
        deciredVelocity = navAgent.desiredVelocity;
        nextPosition = navAgent.nextPosition;

        worldDeltaPosition = navAgent.nextPosition - transform.position;
        worldDeltaPositionMagn = worldDeltaPosition.magnitude;
        if (worldDeltaPositionMagn < 0.0001f)
            return;

        gizmo_nextPosition = nextPosition;
        gizmo_transformPosition = transform.position;
        gizmo_worldDeltaPosition = worldDeltaPosition;

        world2dDelta = new Vector2(worldDeltaPosition.x, worldDeltaPosition.z);

        // Low-pass filter the deltaMove
        //smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);

        //smoothWorld2dDelta = Vector2.Lerp(smoothWorld2dDelta, world2dDelta, smooth);

        //float result = Vector3.SignedAngle(worldDeltaPosition, transform.forward, Vector3.up);
        forward = new Vector2(transform.forward.x, transform.forward.z);
        angleToTarget = Vector2.SignedAngle(world2dDelta, forward); //Этому углу доверять особо нельзя. Он постоянно пляшет
                                                                    //Debug.Log(resultAngle);

        //if (debugText4 != null) debugText4.text = forward.ToString();

        if (navAgent.remainingDistance < minDistanceToTarget)
        {
            //В состоянии InPlace угловая скорость будет меняться в зависимости от угла к направлению цели, а prevAngle считался от направления К цели
            prevAngleToTarget = Vector2.SignedAngle(targetForward, forward);
            navState = NavigationState.InPlace;
        }

        switch (navState)
        {
            case NavigationState.StartMoving:
                linearSpeed = 0;
                if (angleToTarget * prevAngleToTarget < 0 && Math.Abs(angleToTarget) > 120) //Если по каким то причинам направление угла изменилось и оно гдето сзади, то это глюки поиска пути. игнорим новый угол
                {
                    angularSpeed = prevAngleToTarget / 180 * ((float)Math.PI);
                }
                else
                {
                    angularSpeed = angleToTarget / 180 * ((float)Math.PI);
                }
                angularSpeed *= 2f;
                if (Math.Abs(angleToTarget) < 25)
                    navState = NavigationState.Moving;
                break;

            case NavigationState.Moving:
                if (navAgent.remainingDistance < 0.5f)
                    linearSpeed = maxLinearSpeed / 2;
                else
                    linearSpeed = maxLinearSpeed;
                if (angleToTarget * prevAngleToTarget > 0 && Math.Abs(angleToTarget) > 120) //Если по каким то причинам направление угла изменилось и оно гдето сзади, то это глюки поиска пути. игнорим новый угол
                {
                    angularSpeed = prevAngleToTarget / 180 * ((float)Math.PI);
                }
                else
                {
                    angularSpeed = angleToTarget / 180 * ((float)Math.PI);
                }
                angularSpeed *= 2f;
                break;

            case NavigationState.OnCourse:
                break;

            //Цель достигнута, доворачиваем 
            case NavigationState.InPlace:
                linearSpeed = 0;
                if (talkingWithPerson)
                {
                    //мы берем вектор цели и вычитаем из него вектор нашей позиции, 
                    //после чего берем угол между полученным вектором и нашим форвардом.
                    vectorToTargetDelta = target.position - transform.position;

                    vectorToTarget = new Vector2(vectorToTargetDelta.x, vectorToTargetDelta.z);
                    //angleToTarget = Vector2.SignedAngle(vectorToTarget, forward);
                    //сделать доворот к человеку с которым разговариваем
                    _targetReached = true;
                    angularSpeed = 0;
                }
                else
                {
                    angleToTarget = Vector2.SignedAngle(targetForward, forward);
                    if (angleToTarget * prevAngleToTarget < 0 || Math.Abs(angleToTarget) < 2) //Значит направление угла через ноль не перешло, продолжаем доворачивать
                    {
                        _targetReached = true;
                        angularSpeed = 0; //Возможно стоит закомментировать, чтобы анимация могла идеально точно встать
                        if (!animationName.Equals(""))
                            anim.SetBool(animationName, true);
                    }
                    else
                    {
                        angularSpeed = angleToTarget / 180 * ((float)Math.PI);
                        angularSpeed *= 3f;
                    }

                }
                break;
        }
        prevAngleToTarget = angleToTarget;


        // Update velocity if delta time is safe
        //if (Time.deltaTime > 1e-5f)
        //    velocity = smoothDeltaPosition / Time.deltaTime;

        //bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;



        //clamp = Math.Abs(angularSpeed) >= 1 ? 0.7f : 1;
        //if (Math.Abs(angularSpeed) > 0.05f && Math.Abs(angularSpeed) < 1f)
        //    angularSpeed *= 2f;
        //Debug.Log("Linear = " + linearSpeed + " angular " + angularSpeed);
        //Скорость должна иметь возможность мгновенно изменяться чтобы не промахнуться к цели
        anim.SetFloat("LinearSpeed", linearSpeed, 0.2f, Time.deltaTime);
        anim.SetFloat("AngularSpeed", angularSpeed, 0.2f, Time.deltaTime);
        //anim.SetFloat("LinearSpeed", linearSpeed);
        //anim.SetFloat("AngularSpeed", angularSpeed);

        //navAgent.nextPosition = transform.position;

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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(gizmo_nextPosition, Vector3.one * 0.2f);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(gizmo_transformPosition, Vector3.one * 0.2f);
    //    Gizmos.color = Color.blue;
    //    Vector3 posGizmo = gizmo_transformPosition + gizmo_worldDeltaPosition * 5f;
    //    Gizmos.DrawWireCube(posGizmo, Vector3.one * 0.2f);
    //}

    void OnAnimatorMove()
    {
        // Update postion to agent position
        //transform.position = navAgent.nextPosition;

        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
        position.y = navAgent.nextPosition.y;
        transform.position = position;
        transform.rotation = anim.rootRotation;

        navAgent.nextPosition = transform.position;
    }
}

public enum NavigationState
{
    StartMoving,
    Moving,
    OnCourse,
    InPlace
}