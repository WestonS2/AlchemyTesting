using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
	public static SceneManager instance;
	
	public static SaveData saveData;
	
	public enum PLAYERSTATE {FreeRoam, WorkMode, Interact};
	public PLAYERSTATE PlayerState = PLAYERSTATE.FreeRoam;
	
	[HideInInspector] public GameObject workCamera;
	
	public GameObject playerObject;
	public Transform playerCamera;
	public Transform playerBody;
	
	[Header("Key Game Objects")]
	public GameObject GUI;
	[SerializeField] GameObject crosshairUI;
	[SerializeField] TextMeshProUGUI coinCounterText;
	
	bool gamePaused;
	
	void Awake()
	{
		if(instance == null) instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		GameManager.instance.Load();
		
		ResumeGame();
		
		GameManager.instance.GameState = GameManager.GAMESTATE.InGame;
		
		gamePaused = false;
	}
	
	void Update()
	{
		LocatePlayer();
		
		UpdateGUI();
		
		//Player State Manager
		switch(PlayerState)
		{
			case PLAYERSTATE.FreeRoam:
				FreeRoam();
				break;
				
			case PLAYERSTATE.Interact:
				Interaction();
				break;
				
			case PLAYERSTATE.WorkMode:
				WorkMode();
				break;
				
			default:
				break;
		}
	}
	
	public void QuitToMenu()
	{
		GameManager.instance.Save();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
	
	public void ResumeGame()
	{
		gamePaused = false;
		PlayerState = PLAYERSTATE.FreeRoam;
		Time.timeScale = 1;
		transform.GetChild(0).gameObject.SetActive(false);
	}
	
	public void PauseGame()
	{
		gamePaused = true;
		PlayerState = PLAYERSTATE.Interact;
		Time.timeScale = 0;
		transform.GetChild(0).gameObject.SetActive(true);
	}
	
	void LocatePlayer()
	{
		if(playerObject == null || playerCamera == null || playerBody == null)
		{
			playerObject = GameObject.FindWithTag("Player");
			playerCamera = playerObject.transform.GetChild(0);
			playerBody = playerObject.transform.GetChild(1);
		}
	}
	
	void UpdateGUI()
	{
		//Coin Count
		coinCounterText.SetText($"{GameManager.instance.playerCoins}");
	}
	
	#region Player States
	void FreeRoam()
	{
		if(!playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = true;
		if(!playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = true;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(!crosshairUI.activeSelf) crosshairUI.SetActive(true);
		if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
		if(Cursor.visible) Cursor.visible = false;
		if(workCamera != null && workCamera.activeSelf) workCamera.SetActive(false);
		
		if(Input.GetKeyDown(Controls.exitKey) && !gamePaused) PauseGame();
	}
	
	void WorkMode()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(false);
		if(playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(false);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
	}
	
	void Interaction()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
		
		if(Input.GetKeyDown(Controls.exitKey) && gamePaused) ResumeGame();
	}
	#endregion
	
	void OnApplicationFocus(bool focus)
	{
		if(!focus) PauseGame();
	}
}