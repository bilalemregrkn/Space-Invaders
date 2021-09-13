using System;
using Unity.Mathematics;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public bool isLead;

	public bool IsAlive { get; set; } = true;
	public SpriteRenderer MySpriteRenderer => mySpriteRenderer;
	[SerializeField] private SpriteRenderer mySpriteRenderer;
	[SerializeField] private EnemyBulletController enemyBulletPrefabs;

	[SerializeField] private AudioClip clipDead;
	[SerializeField] private AudioClip clipShoot;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag($"Wall") && isLead)
		{
			EnemyManager.Instance.ChangeDirection();
		}
	}

	public void Dead()
	{
		gameObject.SetActive(false);
		IsAlive = false;
		EnemyManager.Instance.CurrentEnemyAmount--;
		EnemyManager.Instance.SpawnParticle(transform.position, mySpriteRenderer.color);
		AudioManager.Instance.Play(clipDead);
	}

	public void Shoot(float speed)
	{
		var bullet = Instantiate(enemyBulletPrefabs, transform.position, quaternion.identity);
		bullet.GetComponent<SpriteRenderer>().color = mySpriteRenderer.color;
		bullet.MyStart(speed);
		AudioManager.Instance.Play(clipShoot);
	}
}