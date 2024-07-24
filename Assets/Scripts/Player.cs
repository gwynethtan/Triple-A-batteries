/*
* Author: Tan Ting Yu Gwyneth 
* Date: 23/07/2024
* Description: This file is for handling player interactions like rays and button input 
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Reference to UIInteractions script 
    /// </summary>
    UIInteractions uiInteractions;

    /// <summary>
    /// Store robot partner preferences
    /// </summary>
    RobotPartner robotPartner;

    /// <summary>
    /// This is to check if the door is locked already
    /// </summary>
    //public bool locked = true;

    /// <summary>
    /// Define audio sound when player collects somthing 
    /// </summary>
    [SerializeField]
    private AudioClip collectAudio;

    /// <summary>
    /// Define audio sound when player sucessfully finished the game
    /// </summary>
    [SerializeField]
    private AudioSource acceptAudio;

    /// <summary>
    /// Define audio sound once player punch
    /// </summary>
    [SerializeField]
    private AudioSource punchAudio;

    /// <summary>
    /// Define audio sound once player play piano
    /// </summary>
    [SerializeField]
    private AudioSource pianoAudio;

    /// <summary>
    /// This will store all the text object that will be stored on screen once the player rejected
    /// </summary>
    public GameObject rejectPanel;

    /// <summary>
    /// This will store all the text object that will be stored on screen once the player pauses the game
    /// </summary>
    public GameObject pause;

    /// <summary>
    /// Store the current door infront of the player
    /// </summary>
   // RegularDoor currentDoor;

    /// <summary>
    /// Store the interactive collectible that the player is touching
    /// </summary>
    InteractCollectible currentCollectible;

    /// <summary>
    /// Reference to SceneChanger script 
    /// </summary>
    //SceneChanger sceneChange;

    /// <summary>
    /// Reference to enemy script 
    /// </summary>
   // Enemy currentEnemy;

    /// <summary>
    /// Defines a field for the player's camera transform
    /// </summary>
    [SerializeField]
    Transform playerCamera;

    /// <summary>
    /// Defines distance at which interaction will occur
    /// </summary>
    [SerializeField]
    float interactionDistance;

    /// <summary>
    /// Flag for whether ray has hit piano
    /// </summary>
    public static bool nearPiano = false;

    /// <summary>
    /// Flag for whether ray has hit art
    /// </summary>
    public static bool nearArt = false;

    /// <summary>
    /// Flag for whether ray has hit ipad
    /// </summary>
    public static bool nearIpad = false;

    /// <summary>
    /// Flag for whether ray has hit tv
    /// </summary>
    public static bool nearTv = false;

    /// <summary>
    /// // Updates the status of current collectible
    /// </summary>
    /// <param name="newCollectible">Details about the current collectible status</param>
    public void UpdateCollectible(InteractCollectible newCollectible)
    {
        currentCollectible = newCollectible;
    }

    /// <summary>
    /// // Updates the status of current collectible
    /// </summary>
    /// <param name="newCollectible">Details about the current collectible status</param>
    public void UpdateEnemy(Enemy newEnemy)
    {
        currentEnemy = newEnemy;
    }

    /// <summary>
    /// // Updates the status of current door
    /// </summary>
    /// <param name="newDoor">Details about current door status</param>
    public void UpdateDoor(RegularDoor newDoor)
    {
        currentDoor = newDoor;
    }

    /// <summary>
    /// Update lock status of the door
    /// </summary>
    /// <param name="lockStatus">Details about the lock status</param>
    public void SetLock(bool lockStatus)
    {
        locked = lockStatus;
    }

    /// <summary>
    /// Function which uses the E button to collect interactive collectibles
    /// </summary>
    private void OnButtonE()
    {
        Debug.Log("Button E is pressed");
        if (currentCollectible != null) // Check if player infront of coins to collect
        {
            AudioSource.PlayClipAtPoint(collectAudio, transform.position, 1); //Plays music and determines volume
            GameManager.instance.GainCoin(100); // Get coins 
            if (currentCollectible.tag == "Stolen")
            {
                if (robotPartner.lovesCriminals) //Check if partner like criminals 
                {
                    GameManager.instance.GainAura(10);
                }
                else
                {
                    GameManager.instance.LoseAura(100);
                }
            }
            Debug.Log("The name is " + currentCollectible.name);
        }
        if (nearPiano) // Check if player near piano
        {
            AudioSource.PlayClipAtPoint(pianoAudio, transform.position, 1); //Plays music and determines volume
            GameManager.instance.GainAura(100); // Increase player score
        }

        if (nearArt) // Check if player near art to criticise 
        {
            if (robotPartner.lovesIntellects) //Check if partner like criminals 
            {
                GameManager.instance.GainAura(10);
            }
            else
            {
                GameManager.instance.LoseAura(5);
            }
        }

        if (nearIpad)
        {
            uiInteractions.ViewClinicForm();
        }

        if (nearTv) 
        { 
        }
    }

    /// <summary>
    /// Function to punch enemies 
    /// </summary>
    private void OnRightMouse()
    {
        Debug.Log("Right mouse button is pressed");
        punchAudio.Play(); //Plays audio
    }

    /// <summary>
    /// Function to pause the game 
    /// </summary>
    private void OnButtonP()
    {
        uiInteractions = GetComponent<UIInteractions>();
        //uiInteractions.PauseGame();
    }

    private void Update()
    {
        //Make the rays visible
        Debug.DrawLine(playerCamera.position, playerCamera.position + (playerCamera.forward * interactionDistance), Color.red);

        /// <summary>
        /// Infomation of the item the rays is in contact with 
        /// </summary>
        RaycastHit hitInfo;

        // Handle the logic when the player is in contact with something and update their status 
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, interactionDistance))
        {
            //print out the name whatever my ray hit 
            Debug.Log(hitInfo.transform.name);

            // Check if the hit object has a interactable component
            if (hitInfo.transform.TryGetComponent<InteractCollectible>(out currentCollectible))
            {
                UpdateCollectible(currentCollectible);
                GameManager.instance.ActiveInteractText(true); // Increase player score
            }
            else
            {
                GameManager.instance.ActiveInteractText(false);
                // Check if the hit object has a enemy component
                if (hitInfo.transform.TryGetComponent<Enemy>(out currentEnemy))
                {
                    Debug.Log("Enemy is nearby");
                    UpdateEnemy(currentEnemy);
                }

                // Check if the hit object has a door component 
                else if (hitInfo.transform.TryGetComponent<RegularDoor>(out currentDoor))
                {
                    UpdateDoor(currentDoor);
                    currentDoor.OpenDoor();
                }

                else if (hitInfo.transform.name == "PianoPlay")
                {
                    nearPiano = true;
                }

                else if (hitInfo.transform.name == "CriticisedArt")
                {
                    nearArt = true;
                }
                else if (hitInfo.transform.name == "Ipad")
                {
                    nearIpad = true;
                }
                else if (hitInfo.transform.name == "Television")
                {
                    nearTv = true;
                }
                else
                {
                    UpdateCollectible(null);
                    UpdateEnemy(null);
                    UpdateDoor(null);
                    nearPiano = false;
                    nearArt = false;
                    nearIpad= false;    
                    nearTv = false;
                    GameManager.instance.ActiveInteractText(false);
                }
            }
        }
        else
        {
            //Remove interaction text
            GameManager.instance.ActiveInteractText(false);
        }
    }
}
