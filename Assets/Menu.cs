using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class Menu : MonoBehaviour
{
    public UnityEvent onMenuOn;
    public UnityEvent onMenuOff;
    public GameObject startingPanel;
    public GameObject hud;
    public GameObject startButton;
    public GameObject reloadButton;
    public TextMeshProUGUI collectedText;
    public static bool menuOn;
    private void Awake()
    {
        menuOn = false;
        ToggleOptions();
    }
    // Update is called once per frame
    void Update()
    {
        if (startingPanel.activeSelf)
        {
            //if (Input.anyKeyDown)
            //{
            //    DeactivateIntro();

            //}
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            menuOn = !menuOn;
            ToggleOptions();
        }
    }

    public void DeactivateIntro()
    {
        startingPanel.SetActive(false);
        menuOn = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hud.SetActive(true);
    }

    private void ToggleOptions()
    {
        if (menuOn)
        {
            onMenuOn.Invoke();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            onMenuOff.Invoke();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
    public void IntroText(bool died = false)
    {
        startingPanel.SetActive(true);
        menuOn = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        hud.SetActive(false);
        if (died)
        {
            collectedText.gameObject.SetActive(true);
            collectedText.text = GameManager.instance.count.ToString() + "\n Collected";
            reloadButton.SetActive(true);
            startButton.SetActive(false);
        }
        else
        {
            collectedText.gameObject.SetActive(false);
            reloadButton.SetActive(false);
            startButton.SetActive(true);
        }
    }
}
