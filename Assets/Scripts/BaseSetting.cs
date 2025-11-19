using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSetting : MonoBehaviour
{
	private void Awake()
	{
		// FPS‚ð60‚ÉŒÅ’è
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
}
