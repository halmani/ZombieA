using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject bulletPrefab;
	public float speed = 100f;

	public GameObject Shot(Vector3 rot)
	{
		var obj = Instantiate(bulletPrefab);
		obj.transform.SetParent(this.transform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one * 0.2f;
		var bullet = obj.GetComponent<Bullet>();
		bullet.vec = rot.normalized * speed;
		obj.transform.SetParent(null);
		Destroy(obj, 10f);
		return obj;
	}
}
