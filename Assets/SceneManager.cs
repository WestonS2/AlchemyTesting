using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
	public static SceneManager instance;
		
	public enum PLAYERSTATE {FreeRoam, WorkMode, Interact, Paused};
	public PLAYERSTATE PlayerState = PLAYERSTATE.FreeRoam;
	
	[HideInInspector] public GameObject workCamera;
	
	public GameObject playerObject;
	public Transform playerCamera;
	public Transform playerBody;
	
	public bool dayComplete;
	
	[Header("Key Game Objects")]
	public GameObject GUI;
	[SerializeField] GameObject crosshairUI;
	[SerializeField] GameObject starterTips;
	[SerializeField] TextMeshProUGUI dayCount;
	[SerializeField] TextMeshProUGUI coinCounterText;
	
	bool pauseBuffer;
	
	void Awake()
	{
		if(instance == null || instance == this) instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		GameManager.instance.Load();
		
		ResumeGame();
		
		GameManager.instance.GameState = GameManager.GAMESTATE.InGame;
		
		dayComplete = false;
		
		if(GameManager.instance.dayIndex <= 1) StartCoroutine(PlayerTips());
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
				
			case PLAYERSTATE.Paused:
				Paused();
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
		PlayerState = PLAYERSTATE.FreeRoam;
		transform.GetChild(0).gameObject.SetActive(false);
		Time.timeScale = 1;
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
		//Day count
		dayCount.SetText($"Day {GameManager.instance.dayIndex.ToString()}");
		//Coin Count
		coinCounterText.SetText($"{GameManager.instance.playerCoins}");
	}
	
	#region Player States
	void FreeRoam()
	{
		if(Input.GetKeyDown(Controls.exitKey) && !pauseBuffer) PlayerState = PLAYERSTATE.Paused;
		
		if(!playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = true;
		if(!playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = true;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(!crosshairUI.activeSelf) crosshairUI.SetActive(true);
		if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
		if(Cursor.visible) Cursor.visible = false;
		if(workCamera != null && workCamera.activeSelf) workCamera.SetActive(false);
	}
	
	void WorkMode()
	{
		if(workCamera != null && !workCamera.activeSelf) workCamera.SetActive(true);
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
	}
	
	void Paused()
	{
		if(playerObject.GetComponent<PlayerMovement>().enabled) playerObject.GetComponent<PlayerMovement>().enabled = false;
		if(playerObject.GetComponent<PlayerCamera>().enabled) playerObject.GetComponent<PlayerCamera>().enabled = false;
		if(!playerCamera.gameObject.activeSelf) playerCamera.gameObject.SetActive(true);
		if(!playerBody.gameObject.activeSelf) playerBody.gameObject.SetActive(true);
		if(!GUI.activeSelf) GUI.SetActive(true);
		if(crosshairUI.activeSelf) crosshairUI.SetActive(false);
		if(Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None;
		if(!Cursor.visible) Cursor.visible = true;
		
		if(!transform.GetChild(0).gameObject.activeSelf) transform.GetChild(0).gameObject.SetActive(true);
		if(Time.timeScale != 0) Time.timeScale = 0;
	}
	#endregion
	
	public IEnumerator PauseBuffer()
	{
		pauseBuffer = true;
		yield return new WaitForSeconds(0.1f);
		pauseBuffer = false;
	}
	
	IEnumerator PlayerTips()
	{
		starterTips.SetActive(true);
		yield return new WaitForSeconds(60);
		starterTips.SetActive(true);
	}
	
	#if !UNITY_EDITOR
	void OnApplicationFocus(bool focus)
	{
		if(!focus) PauseGame();
	}
	#endif
}
