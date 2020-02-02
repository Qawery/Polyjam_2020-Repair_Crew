using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polyjam2020
{
	public class PlaySoundOnClick : MonoBehaviour
	{
		[SerializeField] private AudioClip audioClip;
		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() =>
			{
				GetComponent<AudioSource>().PlayOneShot(audioClip);
			});
		}
	}
}
