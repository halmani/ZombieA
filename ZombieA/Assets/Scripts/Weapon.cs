﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
	const int DefaultLayer = 1;

	public int damage = 1;
	public int bulletCount = 10;

	public Transform muzzle;
	public LineRenderer leaserRay;
	public MeshRenderer pointCube;
	public GameObject muzzleFlash;
	private float rayRange = 30000f;
	public Text bulletCountText;


	// -------------------------------------------------------
	private void Start()
	{
		UpdateBulletText();
	}

	private void UpdateBulletText()
	{
		bulletCountText.text = bulletCount.ToString();
	}

	private void Update()
	{
		muzzleFlash.SetActive(false);
	}

	private void LateUpdate()
	{
		leaserRay.SetPosition(0, muzzle.position);

		Ray ray = new Ray(muzzle.position, muzzle.forward);
		RaycastHit hit;
		Vector3 nearPoint;

		if (DoRaycast(ray, out hit))
		{
			nearPoint = hit.point;
			pointCube.enabled = true;
			pointCube.transform.position = hit.point;
		}
		else
		{
			pointCube.enabled = false;
			nearPoint = ray.origin + ray.direction * rayRange;
		}
		leaserRay.SetPosition(1, nearPoint);
	}

	private bool DoRaycast(Ray ray, out RaycastHit hit)
	{
		return Physics.Raycast(ray, out hit, rayRange, DefaultLayer);
	}

	public void Shot()
	{
		// 弾がない
		if (bulletCount <= 0)
			return;

		// 撃つ
		bulletCount--;
		UpdateBulletText();

		Ray ray = new Ray (muzzle.position, muzzle.forward);
		RaycastHit hit;

		if (DoRaycast(ray, out hit))
		{
			if (hit.collider != null)
			{
				muzzleFlash.SetActive(true);
				var enemy = hit.collider.gameObject.GetComponent<Enemy>();
				if (enemy == null)
					return;
				enemy.HitDamage(damage);
			}
		}
	}

	public void Add(int addCount)
	{
		bulletCount += addCount;
		UpdateBulletText();
	}
}
