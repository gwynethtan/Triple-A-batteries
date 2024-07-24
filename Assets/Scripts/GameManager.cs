/*
* Author: Tan Ting Yu Gwyneth
* Date: 22/7/24 
* Description: This file is for handling functions and game objects that will be shown throughout the entire game
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{/*
    /// <summary>
    /// Define total aura earned
    /// </summary>
    public float auraTotal = 100f;

    /// <summary>
    /// Define total coins earned
    /// </summary>
    public int coinTotal = 0;

    /// <summary>
    /// Define rejected audio sound
    /// </summary>
    [SerializeField]
    private AudioSource rejectAudio;

    /// <summary>
    /// Define audio sound once player earns aura 
    /// </summary>
    [SerializeField]
    private AudioClip gainAuraAudio;

    /// <summary>
    /// Define audio sound once player loses aura
    /// </summary>
    [SerializeField]
    private AudioClip loseAuraAudio;

    /// <summary>
    /// Reference to UIInteractions script 
    /// </summary>
    private UIInteractions uiInteractions;

    /// <summary>
    /// This will store the text object that will be stored on screen for player aura
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// This will store the text object that will be stored on screen for player coins
    /// </summary>
    public TextMeshProUGUI scoreCoinText;

    /// <summary>
    /// Explains to player how to interact with game object
    /// </summary>
    public TextMeshProUGUI interactionText;

    /// <summary>
    /// This will store all the text object that will include player items and aura
    /// </summary>
    public GameObject sidePanel;

    /// <summary>
    /// This will store all the text object that will be stored on screen once the player is accepted MAY NOT USE
    /// </summary>
    public GameObject acceptPanel;

    /// <summary>
    /// This will store all the text object that will be stored on screen once the player is rejected MAY NOT USE
    /// </summary>
    public GameObject rejectPanel;

    /// <summary>
    /// This will store all the text object that will be stored on screen once the player pauses the game
    /// </summary>
    public GameObject pause;

    /// <summary>
    /// List for collectibles user currently posesses
    /// </summary>
    public static List<string> userCollectibles = new List<string>();

    /// <summary>
    /// Defines GameManager throughout all scripts
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// Ensures GameManager is not being produced twice for every scene swap
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Check for anything at the start
    /// </summary>
    private void Start()
    {
        if (PlayerPrefs.HasKey("Volume")) // Ensure volume adjusted in the beginning will be used throughout the game
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            AudioListener.volume = volume;
        }
    }

    /// <summary>
    /// Shows the list of collectibles player collected 
    /// </summary>
    private void Update()
    {
        PrintList(userCollectibles);
    }

    /// <summary>
    /// Formats the print statement for userCollectibles to make it readable
    /// </summary>
    /// <typeparam name="T">Each itme</typeparam>
    /// <param name="list">List of items</param>
    private void PrintList<T>(List<T> list)
    {
        string formattedList = "User Collectibles: ";
        foreach (T item in list)
        {
            formattedList += item.ToString() + ", ";
        }
        // Remove the trailing comma and space
        if (list.Count > 0)
        {
            formattedList = formattedList.Substring(0, formattedList.Length - 2);
        }
        Debug.Log(formattedList);
    }

    /// <summary>
    /// Resets everything the player has done when player chooses to go to start menu
    /// </summary>
    public void ResetGameState()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Update user list once user has collected somthing 
    /// </summary>
    /// <param name="collectibleName"></param>
    public void UpdateUserList(string collectibleName)
    {
        userCollectibles.Add(collectibleName); // adding elements into list
        Debug.Log(collectibleName + "has been added to list");
    }

    /// <summary>
    /// Checks whether required item is still available in user list 
    /// </summary>
    /// <param name="collectibleName"></param>
    /// <returns></returns>
    public bool CheckUserList(string collectibleName)
    {
        return (userCollectibles.Contains(collectibleName)); //Booleon statement
    }

    /// <summary>
    /// Removes item from user list if item can be used once only
    /// </summary>
    /// <param name="collectibleName"></param>
    public void UsedUserList(string collectibleName)
    {
        Debug.Log("Collectible used");
        userCollectibles.Remove(collectibleName);
    }

    /// <summary>
    /// Clears the entire list 
    /// </summary>
    public void ClearUserList()
    {
        userCollectibles.Clear();
    }

    /// <summary>
    /// Decreases player total coins
    /// </summary>
    /// <param name="totalCoins"></param>
    public void LoseCoin(float loseCoinAmount)
    { 
      if ((coinTotal-loseCoinAmount)>=0) // Ensure that the total coins do not hit below 0 when player purhases something
        {
            auraTotal -= loseAuraAmount;
            scoreText.text = auraTotal.ToString();
        }
    }
    
    /// <summary>
    /// Increases player total coins 
    /// </summary>
    /// <param name="coinTotal"></param>
    public void GainCoin(float gainCoinAmount)
    {
        coinTotal += gainCoinAmount;
        scoreCoinText.text = coinTotal.ToString();
    }

    /// <summary>
    /// Increases player aura
    /// </summary>
    /// <param name="healingAmount"></param>
    public void GainAura(float gainAuraAmount)
    {
        AudioSource.PlayClipAtPoint(gainAuraAudio, transform.position, 1); //Plays music and determines volume
        auraTotal += gainAuraAmount;
        scoreText.text = auraTotal.ToString();
    }

    /// <summary>
    /// Decreases player aura
    /// </summary>
    /// <param name="damage"></param>
    public void LoseAura(float loseAuraAmount)
    {
        AudioSource.PlayClipAtPoint(loseAuraAudio, transform.position, 1); //Plays music and determines volume
        auraTotal -= loseAuraAmount;
        scoreText.text = auraTotal.ToString();
        if (auraTotal<=0) // Player gets rejected once aura is below or equal zero
        {
            rejectPanel.gameObject.SetActive(true);
            rejectAudio.Play();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Sets the interactive textbox active when the ray hits a interactCollectible
    /// </summary>
    /// <param name="active">Booleon</param>
    public void ActiveInteractText(bool active)
    {
        //Debug.Log("InteractiveText current settings: "+active);
        interactionText.gameObject.SetActive(active); // Sets active according to given boolean
    }

    /// <summary>
    /// Sets the win panel active as a congratulatory message when player completes the game
    /// </summary>
    public void ActiveRejectPanel()
    {
        //Debug.Log("InteractiveText current settings: "+active);
        rejectPanel.gameObject.SetActive(true); // Sets active according to given boolean
    }
*/
}
