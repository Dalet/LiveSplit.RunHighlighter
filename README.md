LiveSplit.RunHighlighter
========================
Run Highlighter is a [LiveSplit](http://livesplit.org/) component that helps you highlight your runs.

![Image](http://i.imgur.com/dR4qvgb.png)

Features
--------
* Find when your runs started and ended in your Twitch past broadcasts
* Highlight automatically or just pre-fill the highlight details (title, description, etc)

Installation
------------
1. [Download Run Highlighter](https://github.com/Dalet/LiveSplit.RunHighlighter/releases/latest)
2. Close LiveSplit if it was open and place `LiveSplit.RunHighlighter.dll` in the `Components` **directory** of LiveSplit.
3. Start LiveSplit and add **Run Highlighter** to your LiveSplit **layout**. It is in the "**Other**" category.

**Note:** if the component isn't in the layout during a run, this run won't be highlightable.

How to use
----------
1. Leave the component in your layout. Every completed run will be added to your history, which you will be able to highlight at any time.
2. When you want to highlight runs, right click LiveSplit's window -> Control -> Run Highlighter...
3. Enter the Twitch channel where the run was streamed and click on the run you want to highlight in the list.  
4. If the run is found in one of the VODs, a link to the VOD and the time range to highlight will appear.
5. Click the "Open Highlighter..." button to open the Video Manager. If you have to log in first, you might need to close the window and reopen it before it automatically fills everything.

Settings
--------
You can access the settings in the layout editor.

* **Buffer time**:  
The time added before and after the run. It is essential to correct the few seconds of inaccuracy.

Requirements
------------
* Internet Explorer (version 11+ is recommended)
* .NET 4.5 (Windows Vista or later)
* [LiveSplit](http://livesplit.org/) 1.5 or later

Credits
-------
* [Dalet](https://twitter.com/Dalleth_) [(aka Dalleth)](http://twitch.tv/dalleth_)
* Icons provided by [FatCow.com](http://www.fatcow.com/free-icons)
