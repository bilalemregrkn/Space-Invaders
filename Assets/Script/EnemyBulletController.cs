using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBulletController : MonoBehaviour
{
	[SerializeField] private Rigidbody2D myRigidbody2D;

	public void MyStart(float speed)
	{
		myRigidbody2D.AddForce(Vector2.down * speed);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.CompareTag($"Player"))
		{
			var player = other.transform.GetComponent<PlayerController>();
			player.Dead();
			Dead();
		}
	}

	private void Dead()
	{
		gameObject.SetActive(false);
	}
}