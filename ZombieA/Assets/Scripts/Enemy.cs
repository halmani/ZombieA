using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	private const int ItemLayer = 8;

	// Enemy全体の数
	private static int count = 0;
	public static int Count
	{
		get { return count; }
	}

	// ローカル
	public int hp = 3;
	public float walkSpeed = 0.5f;
	public GameObject itemObject;

	new private Rigidbody rigidbody;
	private Transform trans;

	private Vector3 vec = Vector3.zero;
	private float time = 0f;
	private float walkTime = 0f;
	private bool nowWalking = false;


	// ---------------------------------------------
	private void Awake()
	{
		count++;
		rigidbody = GetComponent<Rigidbody>();
		trans = transform;
	}

	private void Update()
	{
		RandomWalk();
	}

	private Vector3 newAngle;
	private void RandomWalk()
	{
		if (time <= walkTime)
		{
			time += Time.deltaTime;
			rigidbody.velocity = vec;
			if (Mathf.DeltaAngle(trans.eulerAngles.y, newAngle.y) < -0.1f)
			{
				transform.Rotate (new Vector3 (0f, -5f, 0f));
			}
		}
		else
		{
			nowWalking = false;
			vec = Vector3.zero;
		}

		if (nowWalking)
			return;

		// 方向決め
		var randY = Random.Range(0, 360f);
		var rot = new Vector3(0, randY, 0);
		//trans.Rotate(rot);
		newAngle = rot;

		// 移動
		var forward = trans.forward.normalized;
		vec = forward * walkSpeed;
		// 立ち止まっている状態
		if (Random.Range(0, 1f) < 0.5f)
			vec = Vector3.zero;

		time = 0f;
		walkTime = Random.Range(0.5f, 5f);
		nowWalking = true;
	}

	private void Dead()
	{
		Destroy(this.gameObject);
		hp = 0;
		count--;
	}

	private void Drop()
	{
		if (itemObject == null)
			return;
		if (Random.Range(0f, 1f) < 0.5f)
			return;
		var obj = Instantiate(itemObject);
		obj.transform.position = transform.position;
		obj.transform.localRotation = Quaternion.identity;
		obj.layer = ItemLayer;
		var item = obj.GetComponent<Item>();
		item.quantity = Random.Range(2, 5);
	}

	public void HitDamage(int damage)
	{
		if ((hp - damage) <= 0)
		{
			Dead();
			Drop();
		}
		else
		{
			hp -= damage;
		}
	}
}
