WEB_APP_URL = "https://www.google.com/search?q=";

function SceneScene1() { };

SceneScene1.prototype.initialize = function () {
	alert("SceneScene1.initialize()");
	// this function will be called only once when the scene manager show this scene first time
	// initialize the scene controls and styles, and initialize your variables here
	// scene HTML and CSS will be loaded before this function is called
	tvID = this.getid();
	alert("Current Widget id:" + curWidget.id);
	$('#id-label').html(tvID);
};

var pluginAPI = new Common.API.Plugin();

SceneScene1.prototype.handleKeyDown = function (keyCode) {
	switch (keyCode) {
		case sf.key.ENTER:
			document.location = 'http://solvbergetapp.cloudapp.net/infoscreen/#/'+tvID;
			pluginAPI.setOffScreenSaver();
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

SceneScene1.prototype.handleShow = function (data) { };
SceneScene1.prototype.handleHide = function () { };
SceneScene1.prototype.handleFocus = function () { };
SceneScene1.prototype.handleBlur = function () { };