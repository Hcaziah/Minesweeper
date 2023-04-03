using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileController : MonoBehaviour {
	// Start is called before the first frame update
	[SerializeField] private Sprite mine;
	[SerializeField] private Sprite flag;
	[SerializeField] private Sprite empty;
	[SerializeField] private Sprite tile;
	public Vector2Int tilePos;
	public bool isMine = false;
	public bool isRevealed = false;
	public bool isFlagged = false;
	public int numAdjacentMines = 0;
	private SpriteRenderer spriteRenderer;
	private SpriteRenderer numberRenderer;
	private bool isMouseOver = false;
	public bool isClickable = true;

	void Start() {
		// Get the sprite renderer and number renderer components
		spriteRenderer = GetComponent<SpriteRenderer>();
		numberRenderer = transform.Find("Number").GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update() {
		// If the tile is revealed
		if (isRevealed) {
			// Set the sprite to mine or empty depending on if it is a mine
			spriteRenderer.sprite = isMine ? mine : empty;

			// If there are adjacent mines and it is not a mine, set the number renderer sprite to the corresponding number
			if (numAdjacentMines != 0 && !isMine) {
				numberRenderer.sprite = Resources.Load<Sprite>(String.Format("Sprites/numMap/{0}", numAdjacentMines));
			}
		}
		// If the tile is flagged, set the sprite to flag
		else if (isFlagged) {
			spriteRenderer.sprite = flag;
		}
		// Otherwise, set the sprite to tile
		else {
			spriteRenderer.sprite = tile;
		}

		// If the mouse is over the tile and it is not revealed
		if (isMouseOver && !isRevealed) {
			// If left mouse button is pressed and the tile is not flagged, reveal the tile
			if (Input.GetMouseButtonDown(0) && !isFlagged) {
				isRevealed = true;
				if (isMine) {
					spriteRenderer.color = new Color(1, 0, 0);
				}
			}
			// If right mouse button is pressed, toggle the flag
			if (Input.GetMouseButtonDown(1)) {
				isFlagged = !isFlagged;
			}
		}
	}

	// When the mouse enters the tile
	void OnMouseEnter() {
		if (isClickable) {
			isMouseOver = true;
			// Change the sprite color to indicate that the tile is being hovered over
			spriteRenderer.color = new Color(0.9f, 0.95f, 1f);
		}
	}

	// When the mouse exits the tile
	void OnMouseExit() {
		if (isClickable) {
			isMouseOver = false;
			// Reset the sprite color to white
			spriteRenderer.color = new Color(1, 1, 1);
		}
	}
}
