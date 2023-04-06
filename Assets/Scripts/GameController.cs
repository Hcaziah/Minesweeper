using UnityEngine;
using TMPro;
using System;

public class GameController : MonoBehaviour {
	[SerializeField] private GameObject board;
	[SerializeField] private Canvas endScreen;
	[SerializeField] private Canvas mainStats;
	private decimal seconds, minutes;
	private BoardController boardController;
	public float time; // in seconds
	private bool gameRunning = false;
	// Start is called before the first frame update
	void Start() {
		boardController = board.GetComponent<BoardController>();
		gameRunning = true;
	}

	// Update is called once per frame
	void Update() {
		// Increment the timer and format the time display
		if (gameRunning) time += Time.deltaTime;

		seconds = Mathf.FloorToInt(time % 60);
		minutes = Mathf.FloorToInt(time / 60);
		mainStats.GetComponentInChildren<TMP_Text>().SetText(String.Format("{0:00}:{1:00}", minutes, seconds));

		// Update the number of remaining mines
		mainStats.GetComponentsInChildren<TMP_Text>()[1].SetText(String.Format(":{0:00}", (boardController.numberMines - boardController.numFlags).ToString()));
		// If the player has won, stop the timer and disable the tiles
		if (boardController.CheckWin()) {
			if (gameRunning) winState();
			stopGame();
		}
		// If the player has lost, stop the timer and disable the tiles
		if (boardController.CheckLose()) {
			if (gameRunning) loseState();
			stopGame();
		}
	}
	// Stop the game
	void stopGame() {
		gameRunning = false;
		for (int x = 0; x < boardController.numTiles.x; x++) {
			for (int y = 0; y < boardController.numTiles.y; y++) {
				boardController.tiles[x, y].GetComponent<TileController>().isClickable = false;
			}
		}
	}
	void winState() {
		endScreen.GetComponentInChildren<TMP_Text>().SetText("You Win!");
		endScreen.GetComponentsInChildren<TMP_Text>()[1].SetText(String.Format("{0:00}:{1:00}", minutes, seconds));
		endScreen.GetComponentsInChildren<TMP_Text>()[2].SetText(String.Format("[{0:0}, {1:0}] with {2:00} mines", boardController.numTiles.x, boardController.numTiles.y, boardController.numberMines));
		endScreen.gameObject.SetActive(true);
	}
	void loseState() {
		endScreen.GetComponentInChildren<TMP_Text>().SetText("You Lose!");
		endScreen.GetComponentsInChildren<TMP_Text>()[1].SetText(String.Format("{0:00}:{1:00}", minutes, seconds));
		endScreen.GetComponentsInChildren<TMP_Text>()[2].SetText(String.Format("[{0:0}, {1:0}] with {2:00} mines", boardController.numTiles.x, boardController.numTiles.y, boardController.numberMines));
		endScreen.gameObject.SetActive(true);
	}
}

