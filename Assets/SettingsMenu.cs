using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] GameObject newGameButton;
	
	void Start()
	{
		if(SaveSystem.LoadPlayerData() != null)
			newGameButton.SetActive(true);
		else newGameButton.SetActive(false);
	}
	
	public AudioMixer mixer;
	public void SetVolume(float volume)
	{
		mixer.SetFloat("volume", Mathf.Log10(volume) * 20);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}
	
	public void NewGame()
	{
		SaveSystem.DeleteSaveFile();
		StartGame();
	}
	
	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}
	
	public void QuitGame()
	{
		print("Pretend the game is quitting");
		Application.Quit();
	}
}

