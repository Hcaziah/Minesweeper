using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControllerV2 : MonoBehaviour {
	[SerializeField] private GameObject tilePrefab;
	public TileController[,] tiles;
	private Vector2 tileSize, boardSize;
	public Vector2Int tileArraySize;
	private Vector2Int[] mineArray;
	[SerializeField] public int numTotalMines = 0, numFlags = 0;
	private Vector2Int[] neighborPoints = new Vector2Int[] {
		new Vector2Int(-1, -1),
		new Vector2Int(-1,  0),
		new Vector2Int(-1,  1),
		new Vector2Int(0, -1),
		new Vector2Int(0,  1),
		new Vector2Int(1, -1),
		new Vector2Int(1,  0),
		new Vector2Int(1,  1)
	};
	// Start is called before the first frame update
	void Start() {
		tileArraySize.x = PlayerPrefs.GetInt("numTilesX");
		tileArraySize.y = PlayerPrefs.GetInt("numTilesY");
		numTotalMines = PlayerPrefs.GetInt("numMines");

		SetupBoard();
	}

	// Update is called once per frame
	void Update() {
		// Itterate through each tile and preform a series of checks
		int flagsThisFrame = 0;
		for (int x = 0; x < tileArraySize.x; x++) {
			for (int y = 0; y < tileArraySize.y; y++) {
				ref TileController currentTile = ref tiles[x, y];

				if (currentTile.isFlagged) flagsThisFrame++;
				ClearTiles(x, y, ref currentTile);
			}
		}
		numFlags = flagsThisFrame;
	}

	void ClearTiles(int x, int y, ref TileController currentTile) {
		if (currentTile.isRevealed && !currentTile.isMine && currentTile.numAdjacentMines == 0) {
			// Loop through the neighboring points of the current cell
			for (int i = 0; i < neighborPoints.Length; i++) {
				Vector2Int point = neighborPoints[i];

				// Calculate the position of the neighboring cell
				int neighborX = x + point.x;
				int neighborY = y + point.y;

				// Check if the neighboring cell is within the bounds of the grid
				if (neighborX >= 0 && neighborX < tileArraySize.x && neighborY >= 0 && neighborY < tileArraySize.y) {
					// Get the TileController component of the neighboring cell
					TileController neighborCell = tiles[neighborX, neighborY];

					// Check if the neighboring cell is not a mine and has at least one adjacent mine
					if (!neighborCell.isMine && neighborCell.numAdjacentMines >= 0) {
						// Set the neighboring cell as revealed
						neighborCell.isRevealed = true;
					}
				}
			}
		}
	}

	void SetupBoard() {
		boardSize = transform.GetComponent<Renderer>().bounds.size;
		tileSize = new Vector2(boardSize.x / (tileArraySize.x), boardSize.y / (tileArraySize.y));

		// Initiate cellArray
		tiles = new TileController[tileArraySize.x, tileArraySize.y];

		Vector2 startingPosition = new Vector2((boardSize.x - tileSize.x) / 2, (boardSize.y - tileSize.y) / 2);

		// Create tile grid of tile Gameobjects
		for (int x = 0; x < tileArraySize.x; x++) {
			for (int y = 0; y < tileArraySize.y; y++) {
				// Instantiate a new tile at the current position
				GameObject tile = Instantiate(tilePrefab, new Vector2((x * tileSize.x) - startingPosition.x, (y * tileSize.y) - startingPosition.y), Quaternion.identity);
				tile.transform.parent = transform;
				// Scale the tile so that it takes up the entire board
				tile.transform.localScale = new Vector2(1f / tileArraySize.x, 1f / tileArraySize.y);
				// Inform tile of its position
				tile.GetComponent<TileController>().tilePos = new Vector2Int(x, y);
				// Store the tile in the cells array
				tiles[x, y] = tile.GetComponent<TileController>();
			}
		}
		PlaceMines();
	}
	void PlaceMines() {
		// Place mines
		int numMines = numTotalMines;
		// Initiate mineArray
		mineArray = new Vector2Int[numTotalMines];

		while (numMines > 0) {
			// Choose a random position on the board
			int x = Random.Range(0, tileArraySize.x);
			int y = Random.Range(0, tileArraySize.y);
			AddMine(ref numMines, x, y);
		}
	}

	void AddMine(ref int numMines, int x, int y) {
		if (!tiles[x, y].isMine && !tiles[x, y].isRevealed) {
			// Place a mine on the tile at the chosen position
			tiles[x, y].isMine = true;
			// Add mines location to mineArray, reverse order because why not.
			mineArray[numMines - 1] = new Vector2Int(x, y);

			for (int i = 0; i < neighborPoints.Length; i++) {
				Vector2Int point = neighborPoints[i];
				// Increment the number of adjacent mines for each neighboring tile
				if (point.x + x >= 0 && point.x + x < tileArraySize.x && point.y + y >= 0 && point.y + y < tileArraySize.y)
					tiles[point.x + x, point.y + y].numAdjacentMines++;
			}
			numMines--;
		}
	}
	public bool CheckWin() {
		// Loop through all the cells in the grid
		for (int x = 0; x < tileArraySize.x; x++) {
			for (int y = 0; y < tileArraySize.y; y++) {
				// Get the TileController component of the current cell
				TileController currentTile = tiles[x, y];

				// Check if the current cell is not a mine and is not revealed
				if (!currentTile.isMine && !currentTile.isRevealed) {
					// The player has not won yet
					return false;
				}
			}
		}
		// The player has won
		return true;
	}
	public bool CheckLose() {
		// Loop through all the cells in the grid
		for (int x = 0; x < tileArraySize.x; x++) {
			for (int y = 0; y < tileArraySize.y; y++) {
				// Get the TileController component of the current cell
				TileController currentTile = tiles[x, y].GetComponent<TileController>();

				// Check if the current cell is a mine and is revealed
				if (currentTile.isMine && currentTile.isRevealed) {
					// The player has lost
					return true;
				}
			}
		}
		// The player has not lost
		return false;
	}
}
