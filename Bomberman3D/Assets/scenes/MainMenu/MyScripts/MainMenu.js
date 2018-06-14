#pragma strict
import UnityEngine.UI;

var CameraObject : Animator;
var PanelControls : GameObject;
var PanelGame : GameObject;
var hoverSound : GameObject;
var sfxhoversound : GameObject;
var clickSound : GameObject;
var areYouSure : GameObject;

// campaign button sub menu
var lanModeBtn : GameObject;

// highlights
var lineGame : GameObject;

function Play(){
	areYouSure.gameObject.active = false;
	lanModeBtn.gameObject.active = true;
}

function DisablePlay(){
	lanModeBtn.gameObject.active = false;
}

function Position2(){
	DisablePlay();
	CameraObject.SetFloat("Animate",1);
}

function Position1(){
	CameraObject.SetFloat("Animate",0);
}

function GamePanel(){
	PanelControls.gameObject.active = false;
	PanelGame.gameObject.active = true;
}

function ControlsPanel(){
	PanelControls.gameObject.active = true;
	PanelGame.gameObject.active = false;
}

function PlayHover(){
	hoverSound.GetComponent.<AudioSource>().Play();
}

function PlaySFXHover(){
	sfxhoversound.GetComponent.<AudioSource>().Play();
}

function PlayClick(){
	clickSound.GetComponent.<AudioSource>().Play();
}

function AreYouSure(){
	areYouSure.gameObject.active = true;
	DisablePlay();
}

function No(){
	areYouSure.gameObject.active = false;
}

function Yes(){
     #if UNITY_EDITOR

         UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
}

function StartLanMode(){
    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
}
