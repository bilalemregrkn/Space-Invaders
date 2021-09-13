using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance { get; set; }

	[SerializeField] private Vector2 gridSize;
	[SerializeField] private Vector2 offset;

	[SerializeField] private EnemyController[] enemyPrefab;
	[SerializeField] private Color[] enemyColor;

	[SerializeField] private Vector2 speedRange;
	[SerializeField] private float downValue;

	[SerializeField] private float shootSpeed;
	[SerializeField] private Vector2 shootDelayRange;

	[SerializeField] private ParticleSystem deadParticle;

	private List<EnemyController> _listEnemy = new List<EnemyController>();
	private int _direction = 1;

	private float CurrentSpeed { get; set; }

	private int TotalEnemyAmount { get; set; }

	public int CurrentEnemyAmount
	{
		get => _currentEnemyAmount;
		set
		{
			_currentEnemyAmount = value;
			CurrentSpeed = Mathf.Lerp(speedRange.y, speedRange.x, value / (float) TotalEnemyAmount);
		}
	}

	private int _currentEnemyAmount;

	private void Awake()
	{
		Instance = this;
	}

	public void MyStart()
	{
		CreateEnemies();

		StartShoot();

		TotalEnemyAmount = _listEnemy.Count;
		CurrentEnemyAmount = TotalEnemyAmount;
	}

	private void Update()
	{
		Move();
	}


	public void SpawnParticle(Vector3 position, Color color)
	{
		var particleSystem = Instantiate(deadParticle, position, quaternion.identity);
		var main = particleSystem.main;
		main.startColor = color;
	}

	private void StartShoot()
	{
		IEnumerator Do()
		{
			yield return new WaitForSeconds(2);
			while (true)
			{
				bool hasAlive = _listEnemy.Exists(x => x.IsAlive);
				if (!hasAlive) break;

				EnemyController enemyController;
				do
				{
					enemyController = _listEnemy[Random.Range(0, _listEnemy.Count)];
				} while (!enemyController.IsAlive);

				enemyController.Shoot(shootSpeed);
				yield return new WaitForSeconds(Random.Range(shootDelayRange.x, shootDelayRange.y));
			}
		}

		StartCoroutine(Do());
	}

	public void ChangeDirection()
	{
		_direction *= -1;

		var position = transform.position;
		position.y -= downValue;
		transform.position = position;
	}

	private void Move()
	{
		var position = transform.position;

		//new position
		position.x += CurrentSpeed * Time.deltaTime * _direction;

		transform.position = position;
	}


	[ContextMenu(nameof(CreateEnemies))]
	private void CreateEnemies()
	{
		_listEnemy = new List<EnemyController>();
		transform.position = Vector3.zero;
		var position = Vector2.zero;
		for (int i = 0; i < gridSize.x; i++)
		{
			position.x = i * offset.x;
			for (int j = 0; j < gridSize.y; j++)
			{
				position.y = j * offset.y;
				var index = Mathf.Clamp(j, 0, enemyPrefab.Length - 1);
				var enemy = Instantiate(enemyPrefab[index], position, quaternion.identity);
				_listEnemy.Add(enemy);
				enemy.transform.name = $"Enemy [{i},{j}]";
				enemy.transform.SetParent(transform);
				enemy.isLead = j == gridSize.y - 1;

				var colorIndex = Mathf.Clamp(j, 0, enemyColor.Length - 1);
				enemy.MySpriteRenderer.color = enemyColor[colorIndex];
			}
		}

		SetPositionCenter();
		transform.position = Vector2.up * 2;
	}

	private void SetPositionCenter()
	{
		var listChild = new List<Transform>();
		Vector3 position = Vector3.zero;
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			listChild.Add(child);
			position += child.position;
		}

		position /= transform.childCount;

		foreach (var item in listChild)
			item.SetParent(transform.parent);

		transform.position = position;
		foreach (var item in listChild)
			item.SetParent(transform);
	}
}