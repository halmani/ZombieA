using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
//	private Vector2 startPos;
	private Vector2 direction;
	
	public Vector2 DragDirection
	{
		get { return direction; }
	}

	public Vector3 DragDirection2Dto3D
	{
		get { return new Vector3(direction.x, 0, direction.y); }
	}

	// 入力 ----------------------------------------
	private PointerEventData lastEventData;
	public void OnBeginDrag(PointerEventData data)
	{
//		lastEventData = data;
//		startPos = data.pressPosition;
	}

	public void OnDrag(PointerEventData data)
	{
//		lastEventData = data;
		direction += data.delta;
	}

	public void OnEndDrag(PointerEventData data)
	{
//		lastEventData = data;
//		startPos = Vector2.zero;
		direction = Vector2.zero;
	}
}
