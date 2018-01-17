using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public float speed = 100f;
	public int damage = 1;

	public Transform muzzle;
	public LineRenderer leaserRay;
	private float rayRange = 30000f;


	// -------------------------------------------------------
	private void LateUpdate()
	{
		leaserRay.SetPosition(0, muzzle.position);

		Ray ray = new Ray (muzzle.position, muzzle.forward);
		RaycastHit hit;
		Vector3 nearPoint;

		if (Physics.Raycast(ray, out hit))
		{
			nearPoint = hit.point;
		}
		else
		{
			nearPoint = ray.origin + ray.direction * rayRange;
		}
		leaserRay.SetPosition(1, nearPoint);
	}

	public void Shot()
	{
		Ray ray = new Ray (muzzle.position, muzzle.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider != null)
			{
				var enemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (enemy == null)
					return;
				enemy.HitDamage(damage);
			}
		}
	}
}
