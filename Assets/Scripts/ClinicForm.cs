/*
* Author: Tan Ting Yu Gwyneth
* Date: 24/07/2024
* Description: This file is for storing, validating and confirm the input for clinic form for their preferred profiles
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
    /// This will store all the text object that will be stored on screen once the player is rejected MAY NOT USE
    /// </summary>
    public GameObject clinicForm;

    /// <summary>
    /// List of all profiles
    /// </summary>
    private List<Profile> profiles;

    /// <summary>
    /// Define the data of the profile chosen
    /// </summary>
    public Profile profileChosen;

    /// <summary>
    /// Defines how the profile is going to be formatted
    /// </summary>
    [System.Serializable]
    public class Profile
    {
        public string profileName; // Name of robot
        public int aura; // Aura required to be accepted
        public bool lovesCriminals; // Check if profile loves criminals 
        public bool lovesRich; // Check if profile loves rich people
        public bool lovesSmokers; // Check if profile loves people who smokes
        public bool lovesIntellects; // Check if profile loves intellects
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializeProfiles();
        DisplayProfiles();
    }

    /// <summary>
    /// Put all profile data into the list of profiles 
    /// </summary>
    private void InitializeProfiles()
    {
        Profile Gwyneth = new Profile
        {
            profileName = "Gwyneth",
            aura = 1000,
            lovesCriminals = true,
            lovesRich = false,
            lovesSmokers = false,
            lovesIntellects = true,
        };

        Profile Verlaine = new Profile
        {
            profileName = "Verlaine",
            aura = 3156,
            lovesCriminals = false,
            lovesRich = true,
            lovesSmokers = true,
            lovesIntellects = false,
        };

        Profile Joseph = new Profile
        {
            profileName = "Joseph",
            aura = 4513,
            lovesCriminals = true,
            lovesRich = false,
            lovesSmokers = true,
            lovesIntellects = false,
        };

        Profile Shine = new Profile
        {
            profileName = "Shine",
            aura = 6708,
            lovesCriminals = false,
            lovesRich = true,
            lovesSmokers = false,
            lovesIntellects = true,
        };

        // Add all profile datas in into the entire list of profiles
        profiles.Add(Gwyneth);
        profiles.Add(Verlaine);
        profiles.Add(Joseph);
        profiles.Add(Shine);
    }

    /// <summary>
    /// Prints out the profile details
    /// </summary>
    private void DisplayProfiles()
    {
        foreach (Profile profile in profiles)
        {
            Debug.Log($"Name: {profile.profileName}");
        }
    }

    /// <summary>
    /// Check if format of name and name for the input is existent
    /// </summary>
    public void ValidateClinicForm()
    {
        string input = inputClinicForm.text; // input for name 

        if (int.TryParse(input, out int number)) // Convert input into a integer
        {
            if (number < 5 && number>0) // Ensure integer is within profile index range
            {
                feedbackClinicForm.text = $"Programming profile {input} into robot..."; // Confirms that name can be programmed into robot
                feedbackClinicForm.color = Color.green;
                profileChosen = profiles[number-1]; // Minus one as indexes starts with 0
                clinicForm.SetActive(false);
            }

            else //Integer is out of range
            {
                feedbackClinicForm.text = "Profile number is not available";
                feedbackClinicForm.color = Color.red;
            }

        }
        else // Inform players that their input is not a number 
        {
            feedbackClinicForm.text = "Input is not a number";
            feedbackClinicForm.color = Color.red;
        }
    }
}
