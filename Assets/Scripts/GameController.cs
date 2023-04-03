using UnityEngine;
using TMPro;
using System;

public class GameController : MonoBehaviour {
	[SerializeField] private TMP_Text mines;
	[SerializeField] private TMP_Text timer;
	[SerializeField] private GameObject board;
	private BoardController boardController;
	public float time; // in seconds

	// Start is called before the first frame update
	void Start() {
		boardController = board.GetComponent<BoardController>();
	}

	// Update is called once per frame
	void Update() {
		// Increment the timer and format the time display
		time += Time.deltaTime;
		int seconds = Mathf.FloorToInt(time % 60);
		int minutes = Mathf.FloorToInt(time / 60);
		timer.SetText(String.Format("{0:00}:{1:00}", minutes, seconds));

		// Update the number of remaining mines
		mines.SetText(String.Format(":{0:00}", (boardController.numberMines - boardController.numFlags).ToString()));
	}
}

