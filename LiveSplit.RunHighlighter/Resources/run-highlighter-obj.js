console.log("Run Highlighter: code injected");

var RunHighlighter = RunHighlighter || {
	 _xhr: new XMLHttpRequest(),

	 highlight: function () {
	 	 console.log("Run Highlighter: highlighting...");

	 	 var urlVars = this._getUrlVars();
	 	 this.start_time = parseInt(urlVars.start_time);
	 	 this.end_time = parseInt(urlVars.end_time);
	 	 this.automate = parseInt(urlVars.automate) === 1;
	 	 this.title = null;

	 	 if (urlVars.title !== undefined && urlVars.title.length > 0) {
	 	 	 try {
	 	 	 	 this.title = window.atob(decodeURIComponent(urlVars.title));
	 	 	 } catch (e) { this.title = null; }
	 	 }

	 	 if (!isNaN(this.start_time) || !isNaN(this.end_time))
	 	 	 this._markerLoop();
	 	 if (!isNaN(this.start_time))
	 	 	 this._seekToStart();
	 },

	 _getUrlVars: function () {
	 	 var vars = {};
	 	 var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
	 	 	 vars[key] = value;
	 	 });
	 	 return vars;
	 },

	 isPlayerReady: function () {
	 	 try {
	 	 	 if (this.getPlayer().getVideoTime() >= 0)
	 	 	 	 return true;
	 	 } catch (e) { }
	 	 return false;
	 },

	 getPlayer: function () {
	 	 return $("div#player").find("object")[0];
	 },

	 _format_time: function (seconds) {
	 	 var hours = Math.floor(seconds / 3600);
	 	 if (hours > 0) {
	 	 	 seconds -= hours * 3600;
	 	 	 hours = hours + ":";
	 	 }
	 	 else
	 	 	 hours = "";

	 	 var minutes = Math.floor(seconds / 60);
	 	 seconds -= minutes * 60;
	 	 if (hours !== "" && minutes < 10)
	 	 	 minutes = "0" + minutes;

	 	 if (seconds < 10)
	 	 	 seconds = "0" + seconds;
	 	 return hours + minutes + ":" + seconds;
	 },

	 _setStartValue: function (seconds) {
	 	 if (isNaN(seconds))
	 	 	 return;
	 	 var elem = document.querySelector("input.start-time.string");
	 	 elem.value = this._format_time(seconds);
	 	 $("input[name=start_time]").val(seconds);
	 	 this._globalTrigger(elem, "change"); //move markers
	 },

	 _setEndValue: function (seconds) {
	 	 if (isNaN(seconds))
	 	 	 return;
	 	 var elem = document.querySelector("input.end-time.string");
	 	 elem.value = this._format_time(seconds);
	 	 $("input[name=end_time]").val(seconds);
	 	 this._globalTrigger(elem, "change"); //move markers
	 },

	 _globalTrigger: function (elem, evnt) {
	 	 var e = document.createEvent("HTMLEvents");
	 	 e.initEvent(evnt, true, true);
	 	 elem.dispatchEvent(e);
	 },

	 _fillForm: function () {
	 	 console.log("Run Highlighter: updating time ranges");
	 	 this._setStartValue(this.start_time);
	 	 this._setEndValue(this.end_time);

	 	 $("input[name=title]").val(this.title);
	 	 $("textarea[name=description]").text(this.description);
	 	 $("select[name=language]").val(this.lang);
	 	 $("input[name=tag_list]").val("speedrun, speedrunning");
	 	 if (!isNaN(this.start_time) && !isNaN(this.end_time)) {
	 	 	 //click "Describe Highlight"
	 	 	 this._globalTrigger($("div .highlight-content button")[0], "click");
	 	 	 //click "Create Highlight"
	 	 	 if (this.automate === true)
	 	 	 	 this._globalTrigger($("div .highlight-content button")[1], "click");
	 	 }
	 },

	 //waits for the player to load and seeks to the highlight start
	 _seekToStart: function () {
	 	 if (this.start_time <= 1)
	 	 	 return;
	 	 var self = this;
	 	 var player = this.getPlayer();
	 	 setTimeout(function () {
	 	 	 if (player !== undefined && player.getVideoTime !== undefined
				 && player.getVideoTime() > 0) {
	 	 	 	 player.videoSeek(self.start_time);
	 	 	 	 console.log("Run Highlighter: seeked to " + self.start_time);
	 	 	 } else
	 	 	 	 self._seekToStart();
	 	 }, 250);
	 },

	 _markerLoop: function () {
	 	 var self = this;
	 	 setTimeout(function () {
	 	 	 if (self.isPlayerReady()) {
	 	 	 	 console.log("Run Highlighter: player ready");
	 	 	 	 self._fillForm();
	 	 	 } else
	 	 	 	 self._markerLoop();
	 	 }, 250);
	 },

	 _twitchApiCall: function (str, callback) {
	 	 var self = this;
	 	 var listener = function () {
	 	 	 self._xhr.removeEventListener("load", listener);
	 	 	 callback();
	 	 };
	 	 this._xhr.addEventListener("load", listener);

	 	 //prevent caching
	 	 if (str.indexOf("?") >= 0)
	 	 	 str += "&random=" + new Date().getTime();
	 	 else
	 	 	 str += "?random=" + new Date().getTime();

	 	 var url = str.indexOf("http") !== 0
			 ? "https://api.twitch.tv/kraken/" + str
			 : str;
	 	 this._xhr.open("GET", url);
	 	 this._xhr.send();
	 }
};
