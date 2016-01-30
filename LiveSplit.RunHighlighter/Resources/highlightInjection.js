$(document).ready(function () {
	 if ($('body').attr("run-highlighter-injected") === "true")
		  return;

	 $('body').attr("run-highlighter-injected", "true");

	 RunHighlighter.description = "{description}";
	 RunHighlighter.lang = "{lang}";
	 RunHighlighter.out_of_vid = {out_of_vid};

	 if (/^\/[^\/]+\/manager\/[^\/]+\/highlight\/?$/.test(window.location.pathname))
		  RunHighlighter.highlight();
});