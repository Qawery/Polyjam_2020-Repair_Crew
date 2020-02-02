using UnityEngine;

namespace Polyjam2020
{
	public class PlaySoundOnEnable : MonoBehaviour
	{
		[SerializeField] private AudioClip audioClip;
		private void OnEnable()
		{
			GetComponent<AudioSource>().PlayOneShot(audioClip);
		}
	}
}
