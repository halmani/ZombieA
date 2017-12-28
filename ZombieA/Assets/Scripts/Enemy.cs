using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	// Enemy全体の数
	private static int count = 0;
	public static int Count
	{
		get { return count; }
	}

	// ローカル
	public int hp = 3;
	
	new private Rigidbody rigidbody;
	private float time = 0f;
	private float walkTime = 0f;
	private bool nowWalking = false;
	public float walkSpeed = 0.5f;

	// ---------------------------------------------
	private void Awake()
	{
		count++;
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		RandomWalk();
	}

	private void RandomWalk()
	{
		if (time <= walkTime)
		{
			time += Time.deltaTime;
		}
		else
		{
			nowWalking = false;
		}

		if (nowWalking)
			return;
		var forward = transform.forward.normalized;
		var randX = Random.Range(-1f, 1f);
		var randZ = Random.Range(-1f, 1f);
		var shift = new Vector3(randX, 0, randZ);
		var dir = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * shift.normalized;
		var vec = dir.normalized * walkSpeed;
		rigidbody.velocity = vec;

		time = 0f;
		walkTime = Random.Range(2f, 10f);
		nowWalking = true;
	}

	private void HitDamage(int damage)
	{
		if ((hp - damage) <= 0)
		{
			Destroy(this.gameObject);
			hp = 0;
			count--;
			return;
		}
		hp -= damage;
	}

	private void OnCollisionEnter(Collision collision)
	{
		var bullet = collision.gameObject.GetComponent<Bullet>();
		if (bullet == null)
			return;
		HitDamage(bullet.damage);
	}
}
