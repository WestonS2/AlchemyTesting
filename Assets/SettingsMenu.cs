using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer mixer;
	public void SetVolume(float volume)
	{
		mixer.SetFloat("volume", Mathf.Log10(volume) * 20);
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}
	
	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
	
	public void QuitGame()
	{
		print("Pretend the game is quitting");
		Application.Quit();
	}
}

