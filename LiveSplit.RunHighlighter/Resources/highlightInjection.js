var count = 0;

function markerLoop() {
    setTimeout(function () {
        $('input.start-time.string').trigger('change');
        if ($('div.left-marker').attr('title') !== "{start_time_str}"
            || $('div.right-marker').attr('title') !== "{end_time_str}") {
            if (!{out_of_vid}) { //no loop if the video is incomplete
                markerLoop();
            }
        }
    }, 100);
}

function playerLoop() {
    var player = $('div#player object')[0];
    setTimeout(function () {
        if (player !== undefined && player.getVideoTime !== undefined && player.getVideoTime() > 0) {
            player.playVideo(); 
            player.videoSeek({start_time});
        } else {
            console.log("Recalling playerLoop()");
            playerLoop();
        }
    }, 100);
}

function formLoop() {
    setTimeout(function () {
        if (count == 8) {
            $("div .highlight-content").find("button")[0].click();
            if ({automated}) {
                $("div .highlight-content").find("button")[1].click();
            }
        } else {
            formLoop();
        }
    }, 100);
}

var timeOut = 400;

$('document').ready(function () {
    waitForKeyElements("input[name=start_time]", function (jNode) {
        setTimeout(function () {
            jNode.prop("value", "{start_time}");
            count++;
        }, timeOut);
    });
    waitForKeyElements("input[name=end_time]", function (jNode) {
        setTimeout(function () {
            jNode.prop("value", "{end_time}");
            count++;
        }, timeOut);
    });
    waitForKeyElements("input.start-time.string", function (jNode) {
        setTimeout(function () {
            jNode.prop("value", "{start_time_str}");
            count++;
        }, timeOut);
    });
    waitForKeyElements("input.end-time.string", function (jNode) {
        setTimeout(function () {
            jNode.prop("value", "{end_time_str}");
            count++;
            markerLoop();
        }, timeOut);
    });
    waitForKeyElements("input[name=title]", function (jNode) {
        jNode.prop("value", "{title}");
        count++;
    });
    waitForKeyElements("input[name=tag_list]", function (jNode) {
        jNode.prop("value", "{tag_list}");
        count++;
    });
    waitForKeyElements("textarea[name=description]", function (jNode) {
        jNode.text("{description}");
        count++;
    });
    waitForKeyElements("select[name=language]", function (jNode) {
        jNode.prop("value", "{lang}");
        count++;
    });
    waitForKeyElements("a#left_close", function (jNode) {
        var col = $('div#left_col')[0];
        if (col !== undefined && col.getAttribute("class").indexOf("open") != -1) {
            jNode.click();
        }
    });

    playerLoop();
    formLoop();
});