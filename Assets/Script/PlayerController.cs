using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float horizontalLimit = 2.45f;
	
	[SerializeField] private BulletController bulletPrefab;
	[SerializeField] private float bulletSpeed;
		
	private bool IsPressShootKey => Input.GetKeyDown(KeyCode.C);

	[SerializeField] private AudioClip clipDead;
	[SerializeField] private AudioClip clipShoot;

	private void Update()
	{
		Move();

		if (IsPressShootKey)
			Shoot();
	}

	private void Shoot()
	{
		var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.MyStart(bulletSpeed);
		AudioManager.Instance.Play(clipShoot);
	}

	private void Move()
	{
		var position = transform.position;

		//Set new Position
		var input = Input.GetAxis("Horizontal");
		position.x += input * Time.deltaTime * speed;
		position.x = Mathf.Clamp(position.x, -horizontalLimit, horizontalLimit);

		transform.position = position;
	}

	public void Dead()
	{
		GameManager.Instance.OnGameOver();
		AudioManager.Instance.Play(clipDead);
	}
}