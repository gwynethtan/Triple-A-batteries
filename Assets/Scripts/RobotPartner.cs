/*
* Author: Tan Ting Yu Gwyneth
* Date: 24/07/2024
* Description: This file is for storing the data to perform the partner actions 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using static ClinicForm; // To access profile class 

public class RobotPartner : MonoBehaviour
{
    /// <summary>
    /// Get data for the chosen profile
    /// </summary>
    private ClinicForm clinicForm;

    /// <summary>
    /// Define the data of the profile chosen into RobotPartner script 
    /// </summary>
    public Profile profileChosen;

    /// <summary>
    /// Define the data of the profile chosen into RobotPartner script 
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// Name of the robot
    /// </summary>
    public string profileName;

    /// <summary>
    /// Aura required to be accepted
    /// </summary>
    public int aura;

    /// <summary>
    /// Check if profile loves criminals 
    /// </summary>
    public bool lovesCriminals;

    /// <summary>
    /// Check if profile loves rich people
    /// </summary>
    public bool lovesRich;

    /// <summary>
    /// Check if profile loves people who smokes
    /// </summary>
    public bool lovesSmokers;

    /// <summary>
    /// Check if profile loves intellects
    /// </summary>
    public bool lovesIntellects;

    /// <summary>
    /// Defines player
    /// </summary>
    public GameObject player; 

    /// <summary>
    /// Defines enemy group
    /// </summary>
    public GameObject enemyGroup; 

    /// <summary>
    /// Defines the current AI used for this script 
    /// </summary>
    private NavMeshAgent robotAgent;

    /// <summary>
    /// Distance at which the robot retreats from the enemy
    /// </summary>
    public float retreatDistance = 5f; 

    /// <summary>
    /// Points needed for robot to want to fight for player 
    /// </summary>
    public float fightPoints = 10f; 

    /// <summary>
    /// Current state of robbot 
    /// </summary>
    private string currentState; 

    /// <summary>
    /// Location of player 
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// Location of enemy group
    /// </summary>
    private Transform enemyGroupTransform;

    /// <summary>
    /// Defines current running coroutine
    /// </summary>
    private Coroutine currentCoroutine;

    /// <summary>
    /// List of conversations for different situations 
    /// </summary>
    private Dictionary<string, string> dialogueResponses = new Dictionary<string, string>
    {
        { "PlayPiano", "Nice playing!" },
        { "KillSomeone", "Oh my god!" },
    };

    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = player.transform; // Get the player's transform
        enemyGroupTransform = enemyGroup.transform; // Get the enemy's transform
        robotAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component

        // Ensure the NavMeshAgent is enabled and placed on a valid NavMesh
        if (robotAgent != null && robotAgent.isOnNavMesh)
        {
            robotAgent.enabled = true;
            Debug.Log("NavMeshAgent is properly set up and placed on a NavMesh.");
        }
        else
        {
            Debug.LogError("NavMeshAgent is not placed on a NavMesh.");
        }

        currentState = "Idle"; // Start with idle state 

        currentCoroutine = StartCoroutine(currentState); // 
    }

    /// <summary>
    /// Changes the state and stops the current coroutine when next state is updated
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    public void ChangeState(string newState)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Stop the current coroutine
        }
        currentState = newState; // Change new state 
        currentCoroutine = StartCoroutine(currentState); // Start the coroutine for the new state
        Debug.Log($"Switching to state: {currentState}");
    }

    /// <summary>
    /// Coroutine for the robot to stay still before getting programmed 
    /// </summary>
    /// <returns>Returns an IEnumerator to be used by Unity to control the coroutine</returns>
    private IEnumerator Idle()
    {
        Debug.Log("Entering Idle state.");
        while (currentState == "Idle")
        {
            yield return new WaitForEndOfFrame();

            if (profileChosen != null) // Check if any profile chosen yet 
            {
                profileChosen = clinicForm.profileChosen; // Store data of the profile chosen from the clinic form script

                // Access the profile details
                name = profileChosen.profileName;
                aura = profileChosen.aura;
                lovesCriminals = profileChosen.lovesCriminals;
                lovesRich = profileChosen.lovesRich;
                lovesSmokers = profileChosen.lovesSmokers;
                lovesIntellects = profileChosen.lovesIntellects;

                // Print profile details
                Debug.Log($"Profile Name: {name}");
                Debug.Log($"Aura: {aura}");
                Debug.Log($"Loves Criminals: {lovesCriminals}");
                Debug.Log($"Loves Rich: {lovesRich}");
                Debug.Log($"Loves Smokers: {lovesSmokers}");
                Debug.Log($"Loves Intellects: {lovesIntellects}");
                ChangeState("Follow");
            }
            Debug.Log("Exiting Idle state.");
        }
    }

    /// <summary>
    /// Coroutine for the robot to follow player
    /// </summary>
    /// <returns>Returns an IEnumerator to be used by Unity to control the coroutine</returns>
    private IEnumerator Follow()
    {
        Debug.Log("Entering Follow state.");
        robotAgent.speed = 3f; // Follows the player faster

        while (currentState == "Follow")
        {
            // Check if the robot is near enemy
            if (IsEnemyNear())
            {
                if (gameManager.auraTotal < fightPoints) // Check if robot is willing to fight for player
                {
                    ChangeState("Retreat");
                }
                else
                {
                    ChangeState("Fight");
                }
            }

            // Follow the player
            if (robotAgent != null && robotAgent.isOnNavMesh && playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); //Calculate distance between player and robot
                if (distanceToPlayer > retreatDistance) // Lose aura for being so far away from partner
                {
                    GameManager.instance.loseAura(10);
                }
                robotAgent.SetDestination(playerTransform.position);
            }
            else
            {
                Debug.LogError("Cannot set destination. NavMeshAgent is not properly set up.");
            }

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Exiting Follow state.");
    }

    /// <summary>
    /// Coroutine for robot to leave the player 
    /// </summary>
    /// <returns>Returns an IEnumerator to be used by Unity to control the coroutine</returns>
    private IEnumerator Retreat()
    {
        Debug.Log("Entering Retreat state.");
        while (currentState == "Retreat")
        {
            // Fight with enemy once aura is high 
            if (aura > gameManager.auraTotal)
            {
                ChangeState("Fight");
            }
            if (robotAgent != null && robotAgent.isOnNavMesh && enemyGroupTransform != null)
            {
                //Move away from enemy
                Vector3 directionAwayFromEnemy = transform.position - enemyGroupTransform.position;
                Vector3 targetPosition = transform.position + directionAwayFromEnemy.normalized * retreatDistance;
                robotAgent.SetDestination(targetPosition);
            }
            else
            {
                Debug.LogError("Cannot retreat. NavMeshAgent or enemy transform is not properly set up.");
            }

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Exiting Retreat state.");
    }

    /// <summary>
    /// Coroutine for robot to fight the player. Script created to ensure speed is faster while following and hurting enemy
    /// </summary>
    /// <returns>Returns an IEnumerator to be used by Unity to control the coroutine</returns>
    private IEnumerator Fight()
    {
        while (currentState == "Fight")
        {
            robotAgent.speed = 10f; // Move faster

            // Follows the player faster
            if (robotAgent != null && robotAgent.isOnNavMesh && playerTransform != null)
            {
                robotAgent.SetDestination(playerTransform.position);
            }
            else
            {
                Debug.LogError("Cannot set destination. NavMeshAgent is not properly set up.");
            }

            if (IsEnemyGroupNear()) // Check if robot near enough to attack enemy
            {
                //shoot 
                //Block the Player from stealing money using probability if trigger == true for 5 sec ? stop power
            }

            if (enemyGroupTransform == null) //Check if all enemy from group is dead
            {
                ChangeState("Follow");
            }
        }
    }

    /// <summary>
    /// Coroutine for robot to talk to player depending on situations 
    /// </summary>
    /// <returns>Returns an IEnumerator to be used by Unity to control the coroutine</returns>
    private IEnumerator Talk() 
    {
        while (currentState == "Talk")
        {
            if (Player.action != null)
            {
                if (dialogueResponses.ContainsKey(action))
                {
                    string response = dialogueResponses[action];
                    // UI display 
                    ChangeState("Follow");

                    Player.action == null;
                }
            }
        }
    }

    /// <summary>
    /// Determines if the enemy is near the robot
    /// </summary>
    /// <returns>Boolean for whether they are near to enemy</returns>
    private bool IsEnemyGroupNear()
    {
        if (enemyGroupTransform == null) // For cases when enemy is killed already 
            return false;
        float distanceToGroupEnemy = Vector3.Distance(transform.position, enemyGroupTransform.position); //Calculate distance between enemy and robot
        return distanceToGroupEnemy < retreatDistance; // Returns boolean 
    }
} 
