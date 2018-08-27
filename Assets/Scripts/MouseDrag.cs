using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDrag : MonoBehaviour
{
	public LayerMask layerMask;
	public Cube rubik;
	public bool relativeToCamera = false;

	Vector3 start, normal;
	List<Transform> list = new List<Transform>();
	const float cubieHalfSize = 0.5f;
	enum Axes { X, Y, Z }

	void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 50, layerMask))
		{
			start = hit.point;
			normal = hit.normal;
			list.Add(hit.transform);
		}

		else if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit, 50, layerMask))
		{
			if (!list.Contains(hit.transform))
				list.Add(hit.transform);
		}

		else if (Input.GetMouseButtonUp(0))
		{
			if (list.Count > 1)
				Recreate();

			list.Clear();
		}
	}

	void Recreate()
	{
		Vector3 offset = start - list[0].position;
		Vector3 end = list[1].position + offset;
		Vector3 cross = Vector3.Cross(normal, Vector3.Normalize(list[1].position + offset - start));

		Debug.DrawLine(start, end, Color.blue, 2);
		Debug.DrawRay(start, normal * 0.5f, Color.green, 2);
		Debug.DrawRay(start, cross * 0.5f, Color.red, 2);

		print("<Color=blue>" + start + " " + end + "</Color>");

		Vector3 face;
		if (IsValidRotation(start, end, out face))
		{
			print("Rotate face: " + face);

			// Rotation direction
			float dot = Vector3.Dot(face, cross);
			print("Dot product: " + dot.ToString("F10"));

			rubik.RotateFace(face, dot >= 0 ? 90 : -90, relativeToCamera);
		}
	}


	bool IsValidRotation(Vector3 start, Vector3 end, out Vector3 face)
	{
		face = Vector3.zero;

		Axes maxAxis = MaxAbsoluteAxis(start);
		print("Max axis: " + maxAxis);

		Axes dragAxis = DragDirectionAxis(start, end);
		print("Drag axis: " + dragAxis);

		for (int i = 0; i < 3; i++)
		{
			bool isMaxAxis = i == (int) maxAxis;
			bool isDragDirectionAxis = i == (int) dragAxis;
			bool isMiddleLayer = Mathf.Abs(start[i]) < cubieHalfSize;

			if (isMiddleLayer)
				print("<Color=red>Middle layer</Color>");

			if (!isMaxAxis && !isDragDirectionAxis && !isMiddleLayer)
			{
				print("Rotation axis: " + (Axes) i);
				face = Vector3.zero;
				face[i] = start[i] >= 0 ? 1f : -1f;
				return true;
			}
		}

		return false;
	}


	Axes MaxAbsoluteAxis(Vector3 vector)
	{
		float max = 0;
		int axisIndex = -1;

		for (int i = 0; i < 3; i++)
		{
			float axis = Mathf.Abs(vector[i]);
			if (axis > max)
			{
				max = axis;
				axisIndex = i;
			}
		}

		return (Axes) axisIndex;
	}


	Axes DragDirectionAxis(Vector3 dragStart, Vector3 dragEnd)
	{
		int axisIndex = -1;

		for (int i = 0; i < 3; i++)
		{
			if (Mathf.Abs(dragEnd[i] - dragStart[i]) > cubieHalfSize)
			{
				axisIndex = i;
				break;
			}
		}

		return (Axes) axisIndex;
	}
}