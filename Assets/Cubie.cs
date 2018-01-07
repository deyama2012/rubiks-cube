using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cubie : MonoBehaviour
{
	Dictionary<MeshRenderer, Vector3> dictVectorFromMesh = new Dictionary<MeshRenderer, Vector3>();

	Facelet[] facelets = new Facelet[6];

	public MeshRenderer[] panels;
	public Vector3 axis;


	void Awake()
	{
		Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

		for (int i = 0; i < 6; i++)
			dictVectorFromMesh.Add(panels[i], directions[i]);
	}


	void Start()
	{
		RefreshPanelsColor();
	}


	public void SetAllSides(Color color)
	{
		Vector3[] normals = { Vector3.back, Vector3.forward, Vector3.left, Vector3.right, Vector3.up, Vector3.down };

		for (int i = 0; i < 6; i++)
			facelets[i] = new Facelet(normals[i], color);
	}


	public void SetColor(int side, Color color)
	{
		facelets[side].color = color;

		RefreshPanelsColor();
	}

	[ContextMenu("Rotate")]
	public void Rotate()
	{
		RotateFacelets(axis);
		RefreshPanelsColor();
	}


	private void RotateFacelets(Vector3 axis)
	{
		Quaternion rotation = Quaternion.AngleAxis(90f, axis);

		for (int i = 0; i < 6; i++)
		{
			Vector3 newNormal = rotation * facelets[i].normal;
			facelets[i].normal = new Vector3(Mathf.RoundToInt(newNormal.x), Mathf.RoundToInt(newNormal.y), Mathf.RoundToInt(newNormal.z));
		}
	}


	private void RefreshPanelsColor()
	{
		foreach (var rendVectPair in dictVectorFromMesh)
		{
			Color color = facelets.First(x => x.normal == rendVectPair.Value).color;
			rendVectPair.Key.material.color = color;
		}
	}


	public class Facelet
	{
		public Vector3 normal;
		public Color color;

		public Facelet(Vector3 normal, Color color)
		{
			this.normal = normal;
			this.color = color;
		}
	}
}