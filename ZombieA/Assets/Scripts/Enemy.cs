using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	private static int count = 0;
	public static int Count
	{
		get { return count; }
	}

	public int hp = 3;


	private void Awake()
	{
		count++;
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
