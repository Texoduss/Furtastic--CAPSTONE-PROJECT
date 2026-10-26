using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;

public class PetShopManager : MonoBehaviour
{
    AudioManager audioManager;
    

    private void Awake()
    {
        audioManager = AudioManager.instance;
    }
    public void BackButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        SceneManager.LoadScene("PlayMenuScene");
    }

    public void AddCoinButton()
    {
        PetCoinManager.Instance.AddCoins(100);
        Debug.Log("Added 100 coins.");
    }

    public void RemoveCoinButton()
    {
        PetCoinManager.Instance.AddCoins(-100);
        Debug.Log("Removed 100 coins.");
    }

    public void ResetCoinsButton()
    {
        PetCoinManager.Instance.ResetCoin();
        Debug.Log("All coins removed.");
    }
}
