using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField] private TextMeshPro textMeshPro;

	[SerializeField] private Canvas canvas;
	[SerializeField] private TextMeshProUGUI textStart;
	[SerializeField] private TextMeshProUGUI textTry;

	public int Score
	{
		get => _score;
		set
		{
			_score = value;
			textMeshPro.text = _score.ToString();
		}
	}

	private int _score;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		textMeshPro.text = "";
	}

	public void OnStartGame()
	{
		canvas.enabled = false;
		Score = 0;
		EnemyManager.Instance.MyStart();
		Time.timeScale = 1;
	}

	public void OnGameOver()
	{
		textStart.enabled = false;
		textTry.enabled = true;
		canvas.enabled = true;
		Time.timeScale = 0;
	}
}