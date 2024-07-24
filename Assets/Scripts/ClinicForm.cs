/*
* Author:  
* Date: 24/07/2024
* Description: This file is for storing and validating the input for clinic form for their preferred profiles
*/

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClinicForm : MonoBehaviour
{
    /// <summary>
    /// Input for the name of preferred dating profile
    /// </summary>
    [SerializeField]
    private TMP_InputField inputClinicForm; 

    /// <summary>
    /// Text to show players if input has been validated
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI feedbackClinicForm; 

    /// <summary>
    /// List of valid names
    /// </summary>
    private readonly HashSet<string> validNames = new HashSet<string>
    {
        "Gwyneth", "Verlaine", "Joseph", "Shine"
    };

    /// <summary>
    /// Check if format of name and name for the input is existent
    /// </summary>
    public void ValidateClinicForm()
    {
        string input = inputClinicForm.text; // input for name 

        if (validNames.Contains(input))
        {
            feedbackClinicForm.text = $"Programming {input} into robot..."; // Confirms that name can be programmed into robot
            feedbackClinicForm.color = Color.green;
        }
        else // Inform players that their input is wrong 
        {
            feedbackClinicForm.text = "Wrong format of name or non-existent name";
            feedbackClinicForm.color = Color.red;
        }
    }
}
