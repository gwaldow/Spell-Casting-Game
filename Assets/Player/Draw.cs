using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class Draw : MonoBehaviour
{

	public Transform gestureOnScreenPrefab;
	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	private bool recognized;
	private bool locked;
	private string bestGuess = "";
	private float confidenceThreshhold = 0.8f; // gesture recognition must be at least this confident to accept symbol

	[SerializeField] GameObject _snareSpell;
	[SerializeField] GameObject _teleportInSpell;
	[SerializeField] GameObject _teleportOutSpell;
	[SerializeField] GameObject _lightSpell;
	[SerializeField] RuntimeData _runtimeData;

	void Start()
	{

		locked = true;
		platform = Application.platform;
		drawArea = new Rect(Screen.width / 3, Screen.height / 8, Screen.width / 3, Screen.height / 1.5f);

		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
	}

	void Update()
	{

		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touchCount > 0)
			{
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		}
		else
		{
			if (Input.GetMouseButton(0))
			{
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}

		if (drawArea.Contains(virtualKeyPosition))
		{

			if (Input.GetMouseButtonDown(0))
			{
				locked = false;

				++strokeId;

				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
				gestureLinesRenderer.Add(currentGestureLineRenderer);

				vertexCount = 0;
			}

			if (Input.GetMouseButton(0) && !locked)
			{
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, Vector3.Distance(transform.position, transform.parent.Find("castingframe").position))));
			}
		}

		// if the user has finished a stroke, it's worth checking for a shape
		if (Input.GetMouseButtonUp(0) && !locked)
		{
			recognized = true;

			Gesture candidate = new Gesture(points.ToArray());
			Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

			if (gestureResult.Score > confidenceThreshhold)
			{
				bestGuess = gestureResult.GestureClass + "";
			}
			Debug.Log(bestGuess);
			Debug.Log(gestureResult.Score);
			// if real gesture, spawn spell at location

			// if not, poof the drawing

		}
	}

	// only invoked when the player exits a cast with "esc"
	public void RemoveSuperfluousDrawing()
	{
		// destroy lines if this draw does not have a guess

		GameEvents.InvokePlayAudio("runeforge");
		// there is a valid rune, so we will instantiate a spell
		if (bestGuess == "five point star")
		{
			Instantiate(_snareSpell, transform.parent.Find("castingframe").transform.position, transform.parent.Find("castingframe").rotation * Quaternion.Euler(90, 0, 0));
		}
		else if (bestGuess == "arrowhead")
		{
			GameObject tps = Instantiate(_teleportInSpell, transform.parent.Find("castingframe").transform.position, transform.parent.Find("castingframe").rotation * Quaternion.Euler(90, 0, 0));
			_runtimeData.teleportIn = tps.transform;
		}
		else if (bestGuess == "P")
		{
			GameObject tps = Instantiate(_teleportOutSpell, transform.parent.Find("castingframe").transform.position, transform.parent.Find("castingframe").rotation * Quaternion.Euler(90, 0, 0));
			_runtimeData.teleportOut = tps.transform;
		}
		else if (bestGuess == "half note")
		{
			GameObject tps = Instantiate(_lightSpell, transform.parent.Find("castingframe").transform.position, transform.parent.Find("castingframe").rotation * Quaternion.Euler(90, 0, 0));
		}
		else
		{
			points.Clear();
			foreach (LineRenderer lineRenderer in gestureLinesRenderer)
			{
				lineRenderer.SetVertexCount(0);
				Destroy(lineRenderer.gameObject);
			}
			gestureLinesRenderer.Clear();

		}
	}
}
