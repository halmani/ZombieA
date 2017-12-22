using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	public int hp = 3;


	private void HitDamage(int damage)
	{
		if ((hp - damage) <= 0 )
		{
			Destroy(this.gameObject);
			hp = 0;
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
