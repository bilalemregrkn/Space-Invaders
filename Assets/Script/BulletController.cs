using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
	[SerializeField] private Rigidbody2D myRigidbody2D;

	public void MyStart(float speed)
	{
		myRigidbody2D.AddForce(Vector2.up * speed);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag($"Enemy"))
		{
			var enemy = other.transform.GetComponent<EnemyController>();
			enemy.Dead();
			Dead();
			GameManager.Instance.Score++;
		}
	}

	private void Dead()
	{
		gameObject.SetActive(false);
	}
}