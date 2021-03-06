﻿using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {
	public static StageManager instance;
	private Stage[] stages;
	private int currentStageIndex = 0;
	private Stage currentStage;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
		stages = GetComponents<Stage>();

	}

	public Stage CurrentStage {
		get {
			return currentStage;
		}
	}


	/// <summary>
	/// Starts the next stage, or call the GameFinished method if no more stages are available.
	/// </summary>
	public void StartNextStage(){
		//TODO: Implement this
		if (currentStageIndex <= stages.Length - 1){
			currentStage = stages[currentStageIndex];
			//Debug.Log(CurrentStage.maxTime.ToString());
			StartStage(currentStage);
			currentStageIndex++;
		}
		else {
			GameManager.instance.GameFinished();
		}
	}

	/// <summary>
	/// Starts the stage.
	/// </summary>
	/// <param name="stage">Stage.</param>
	private void StartStage(Stage stage){
		GameManager.instance.spirit.Trasnformation();
		//TODO: implement this
		//TODO: Show suggestion
		//Enabling dragging of items on gameManager
		GameManager.instance.spirit.ShowSuggestion(stage.suggestion);
		GameManager.instance.isDragEnabled = true;
        //TODO: Dragging Phase 

        //Starts the countdown for this phase
        GameManager.instance.progressBar.StartRound(stage.maxTime);
		Invoke("OnStageTimeout", CurrentStage.maxTime);
	}

	/// <summary>
	/// Called when an item has been dragged into the game zone. This methods checks the solution for this stage and calls the related events.
	/// </summary>
	public void OnItemPlacement(IItem placedItem){
		CancelInvoke ("OnStageTimeout");
		Debug.Log("Item Placed");
		if (placedItem.Equals(CurrentStage.requiredItem)){
			Debug.Log("Right Item!");
			//The player selected the right item for this stage. Succeeded
			placedItem.GameObject().SetActive(false);
			GameManager.instance.StageSucceded();
		} else {
			Debug.Log("Wrong Item!");

			GameManager.instance.DropHealth();
			GameManager.instance.isDragEnabled = true;
			placedItem.ResetPosition();
		}
	}

	public void OnStageTimeout(){
			if (GameManager.instance.currentDraggedItem != null){
            //The player is dragging an item
            Debug.Log("Repositioning Item");
				//TODO: Call the function to reposition the item
				GameManager.instance.currentDraggedItem.ResetPosition();
			} 
		GameManager.instance.OnStageTimeOut(CurrentStage);
	}


}
