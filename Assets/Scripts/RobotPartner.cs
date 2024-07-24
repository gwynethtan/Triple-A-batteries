/*
* Author: Tan Ting Yu Gwyneth
* Date: 24/07/2024
* Description: This file is for storing the data to perform the partner actions 
*/

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static ClinicForm;

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

    private void Start()
    {
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
                
            // Use the profile details
            Debug.Log($"Profile Name: {name}");
            Debug.Log($"Aura: {aura}");
            Debug.Log($"Loves Criminals: {lovesCriminals}");
            Debug.Log($"Loves Rich: {lovesRich}");
            Debug.Log($"Loves Smokers: {lovesSmokers}");
            Debug.Log($"Loves Intellects: {lovesIntellects}");
        }
    }
}

