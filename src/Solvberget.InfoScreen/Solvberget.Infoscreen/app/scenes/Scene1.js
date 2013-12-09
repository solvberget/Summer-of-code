alert('SceneScene1.js loaded');

WEB_APP_URL = "https://www.google.com/search?q=";

function SceneScene1() {

};

SceneScene1.prototype.initialize = function () {
	alert("SceneScene1.initialize()");
	// this function will be called only once when the scene manager show this scene first time
	// initialize the scene controls and styles, and initialize your variables here
	// scene HTML and CSS will be loaded before this function is called
	//window.location='http://80.203.160.221:3535/webapp/#/';
	tvID = this.getid();
	alert("Current Widget id:" + curWidget.id);
	$('#id-label').html(tvID);
};

SceneScene1.prototype.handleShow = function (data) {
	alert("SceneScene1.handleShow()");
	// this function will be called when the scene manager show this scene
};

SceneScene1.prototype.handleHide = function () {
	alert("SceneScene1.handleHide()");
	// this function will be called when the scene manager hide this scene
};

SceneScene1.prototype.handleFocus = function () {
	alert("SceneScene1.handleFocus()");
	// this function will be called when the scene manager focus this scene
};

SceneScene1.prototype.handleBlur = function () {
	alert("SceneScene1.handleBlur()");
	// this function will be called when the scene manager move focus to another scene from this scene
};

SceneScene1.prototype.handleKeyDown = function (keyCode) {
	alert("SceneScene1.handleKeyDown(" + keyCode + ")");
	// TODO : write an key event handler when this scene get focused
	switch (keyCode) {
		case sf.key.LEFT:
			break;
		case sf.key.RIGHT:
			break;
		case sf.key.UP:
			break;
		case sf.key.DOWN:
			break;
		case sf.key.ENTER:
			document.location=WEB_APP_URL + tvID;
			break;
		default:
			break;
	}
};

SceneScene1.prototype.getid = function() {
	var tvID = sf.core.localData("tv-id");
	
	if (!tvID) {
		tvID = this.makeid();
		sf.core.localData("tv-id", tvID);
		alert("Generated new id: " + tvID);
	}
	
	return tvID;
};

SceneScene1.prototype.makeid = function()
{
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    for( var i=0; i < 6; i++ )
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
};