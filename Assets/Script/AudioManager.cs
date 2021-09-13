using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	private AudioSource _audioSource;

	private void Awake()
	{
		Instance = this;

		_audioSource = GetComponent<AudioSource>();
	}


	public void Play(AudioClip clip, float volume = 1)
	{
		_audioSource.PlayOneShot(clip,volume);
	}
}