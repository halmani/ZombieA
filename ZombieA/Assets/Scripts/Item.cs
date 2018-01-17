using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public enum Type
	{
		Bullet,
	}
	public Type type = Type.Bullet;
	public int quantity = 0;
	public float speed = 30f;

	private Transform trans;

	private void Start()
	{
		trans = transform;
	}

	private void Update()
	{
		trans.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}
