﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject bulletPrefab;

	public GameObject Shot(Vector3 rot)
	{
		var obj = Instantiate(bulletPrefab);
		obj.transform.SetParent(this.transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		var bullet = obj.GetComponent<Bullet>();
		bullet.vec = rot;
		Destroy(obj, 10f);
		return obj;
	}
}