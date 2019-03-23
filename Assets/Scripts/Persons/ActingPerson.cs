using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActingPerson : MonoBehaviour
{
    public PersonAct currentAction;
    public bool noAction = false;
    //int curActNum = 0;
    public float distance = 100;//начальное значение должно быть больше дистанции первой задачи, чтобы не сработал переход к следующей задаче пока навмешагент в первый раз просчитывает путь
    public float timeOfStart = 0; //Время начала выполнения задачи

    ActingPerson interlocutor; //Человек с которым мы общаемся
    public bool inDialog = false; //находится ли личность в диалоге
    public bool isDialogSingle = false; //проходит ли этот диалог сам с собой
    public GameObject textBackground; //фон текста
    public Text bubbleText; //текстбокс
    public int currentPhraseNum; //номер текущей фразы
    public float textMaxTime = 3; //время в течении которого отображается фраза
    public float sayTime = 0; //время начала отображения фразы

    protected Animator anim;
    NavMeshAgent navAgent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 smoothWorld2dDelta = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    public float linearSpeed;
    public float angularSpeed;
    //У персоны есть его целевая позиция куда он хочет идти
    //Есть базовый цикл из задач которые он делает пока ничего не происходит
    //У каждой отдельной персоны будет свой цикл.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        if (anim != null)
        {
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        NavAgentUpdate();

        navAgent.SetDestination(currentAction.target.position);
        if (!navAgent.pathPending)
            distance = navAgent.remainingDistance;
        
        //Здесь может установиться новое состояние, которое сразу же можно будет использовать для обновления диалога
        if (!inDialog)
        {
            if (currentAction.byTimer && Time.fixedTime - timeOfStart > currentAction.targetTimer)
            {
                goToNextAction();
            }
            else if (!currentAction.byTimer && distance < currentAction.targetDistance)
            {
                if (!navAgent.pathPending)
                    goToNextAction();
            }
        }
        
        textBackground.gameObject.transform.rotation = Quaternion.identity; //new Quaternion(0,0,0,1);
        bubbleText.gameObject.transform.rotation = Quaternion.identity;

        if (Time.fixedTime - sayTime > textMaxTime)
        {
            if (currentPhraseNum >= currentAction.phrases.Length)
            {
                textBackground.SetActive(false);
                bubbleText.text = "";
            } else
            {
                //Здесь нужно начать произносить все фразы
                if (interlocutor != null) //значит будет диалог с кем-то
                {
                    if (currentPhraseNum % 2 == 1)
                        interlocutor.say(currentAction.phrases[currentPhraseNum]);
                    else
                        say(currentAction.phrases[currentPhraseNum]);
                }
                else
                {
                    //произносим фразы в монологе
                    say(currentAction.phrases[currentPhraseNum]);
                }
                currentPhraseNum += 1;
            }
        }
    }
    protected virtual void goToNextAction()
    {
        Debug.Log("Acting person next Action");
    }
    public float clamp;
    void NavAgentUpdate()
    {
        //Debug.Log(navAgent.updatePosition);
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;
        //Debug.Log("transform.position = " + transform.position);
        //Debug.Log("navAgent.nextPosition = " + navAgent.nextPosition);
        //Debug.Log("navAgent.desiredVelocity = " + navAgent.desiredVelocity); //полезная штука, потом может пригодиться
        //Debug.Log("worldDeltaPosition = " + worldDeltaPosition.x + " " + worldDeltaPosition.y + " " +worldDeltaPosition.z);
        Vector2 world2dDelta = new Vector2(worldDeltaPosition.x, worldDeltaPosition.z);
        //Debug.Log("world2dDelta = " + world2dDelta.x + " " + world2dDelta.y);
        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        //Debug.Log("smooth = " + smooth);
        smoothWorld2dDelta = Vector2.Lerp(smoothWorld2dDelta, world2dDelta, smooth);
        //Debug.Log("smoothWorld2dDelta = " + smoothWorld2dDelta.x + " " + smoothWorld2dDelta.y);
        //float result = Vector3.SignedAngle(worldDeltaPosition, transform.forward, Vector3.up);
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        //Debug.Log("forward = " + forward.x + " " + forward.y);
        float resultAngle = Vector2.SignedAngle(world2dDelta, forward);
        //Debug.Log("resultAngle = " + resultAngle);
        //Debug.Log("world2dDelta = " + world2dDelta.magnitude);

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
        if (navAgent.remainingDistance < currentAction.targetDistance - 0.1)
        {
            //Debug.Log("target reached");
            linearSpeed = 0;
            angularSpeed = 0;
        }
        else if (Math.Abs(resultAngle) > 90)
        {
            //Debug.Log("zero speed, turning");
            linearSpeed = 0;
        }
        else if (navAgent.remainingDistance < 1)
        {
            //Debug.Log("half speed");
            linearSpeed = 1f;
        }
        else
            linearSpeed = 2;
        //linearSpeed = world2dDelta.magnitude;


        clamp = Math.Abs(angularSpeed) > 1 ? 0.7f : 1;
        if (Math.Abs(angularSpeed) > 0.8f && Math.Abs(angularSpeed) < 1.5f)
            angularSpeed *= 1.5f;
        //Debug.Log("Linear = " + linearSpeed + " angular " + angularSpeed);
        anim.SetFloat("LinearSpeed", linearSpeed, 0.3f, Time.deltaTime);
        anim.SetFloat("AngularSpeed", angularSpeed, clamp, Time.deltaTime);

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
    public void setAction(PersonAct newAct)
    {
        currentPhraseNum = 0;
        if (currentAction.phrasesStorage.Length != 0)
        {
            currentAction.phraseStageNum = (currentAction.phraseStageNum + 1) % currentAction.phrasesStorage.Length;
        }
        else
            currentAction.phraseStageNum = 0;

        if (newAct.phrasesStorage.Length != 0)
            newAct.phrases = newAct.phrasesStorage[newAct.phraseStageNum].phrases;

        //Debug.Log(newAct.name);
        currentAction = newAct;
        if (currentAction.byTimer)
            timeOfStart = Time.fixedTime;

        interlocutor = newAct.target.GetComponent<ActingPerson>();
        
        //say(newAct.phrases[UnityEngine.Random.Range(0, newAct.phrases.Length)]);

    }

    public void say(string text)
    {
        textBackground.SetActive(true);
        bubbleText.text = text;
        sayTime = Time.fixedTime;
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

public enum PersonState
{
    Idle,
    Acting
}