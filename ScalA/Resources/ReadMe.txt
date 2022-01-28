**Welcome to ScalA**

more info can be found on resurgence discord
https://discord.com/channels/741168805966905374/818886441185837077

To show the shortcut creation submenu hold ctrl when opening quicklaunch or it's subfolders.



**ScalA 0.7.6**
Added icons to quicklaunch menu.
Settings now has options to specify astonia exe names and windowclass as pipe(|) separated lists.

**ScalA 0.7.8**
Fixed critical bug if selected folder for quicklaunch didn't exist anymore.
Fixed bug introduced in 0.7.7 where alt selection combobox wouldn't update properly.
Made shortcuts, executables, jars and urls fully sort by name in quicklaunch.

**ScalA 0.8.0**
Drastically improved responsiveness of 'cursor' so misclicks should be less frequent.
Added logic to prevent misclicks when quicklaunch or comboboxes are open.
Added shortcut creation submenu to quicklaunch accessed by holding ctrl when opening it or it's submenus.
     note: shortcuts created in this manner will bypass the astonia updater.
Added runasinvoker shim to items called from quicklaunch.
Added a systray icon.

**ScalA 0.8.1**
Made right click properties of quicklaunch items be topmost.
Added filesystem watchers to invalidate cached quicklaunch icons.
Sorting in quicklaunch now correctly sorts items starting with numbers higher than 9.
Fixed bug in quicklaunch shortcut creation when name is same as password.
Set default quicklaunch path to a folder in MyDocuments.
   (This will stop ScalA hanging when trying to open quicklaunch in a folder with hundreds of files in it.)
   (Note: if you manually set quicklaunch folder to a directory with a lot of files it will still hang. Be patient if you do so.) 
Added .txt files to quicklaunch (current filetypes scanned are .lnk .url .exe .jar and .txt)

**ScalA 0.8.2**
Added deferred parallelized loading of icons in quicklaunch if initial loading takes more than 50 ms.
Added 3 second timeout to parsing files and folders in quicklaunch.
Sorting in quicklaunch now correctly sorts numbers before letters. ([0-9#a-z] instead of [#0a-z1-9]) 	
Fixed access denied unhandled exception in quicklaunch.
Made form moving and doubleclick maximizing only work on titlebar.
Removed ScalA from windows taskbar when an alt is selected. This hides it from alt-tab and alt-esc. To access the sysmenu shift right click on the systray icon.

**ScalA 0.8.3**
Fixed titlebar malfunctioning when restoring by doubleclick on systray.
Fixed overview not displaying multiple alts with same name properly.
Added icons to overview and made systrayicon reflect currently selected astonia icon. 
Added hotkey ctrl-space to quickly cycle between open astonia windows.

**ScalA 0.8.4**
Improved accuracy of cursor a tiny bit.
Added bigger healthbars to overview. 
Fixed minimizing with an alt selected minimizes to small window on the bottom left of main monitor.

**ScalA 0.8.5**
Fixed a race condition in getting icons for quicklaunch.
Fixed healthbar displaying garbage when questlog/help is open.
Fixed healthbar displaying too many lines in the dark.
Made moving ScalA more responsive and reactivate selected client when done moving.
Handeled exception when Astonia isn't running in windowed mode.
   (note: running Astonia in fullscreen is unsupported and causes weird stuff to happen)
Added upgrading of settings when running a new version.
Removed ability to maximize settings form.
Made settings form center to ScalA instead of screen.
Fixed alignment function in settings working when no alt is selected.
Added a 50ms animation when selecting an alt from overview screen.

**ScalA 0.8.6**
Probably fixed an exception on button image get_width() and get_height().
When creating shortcuts ScalA will prompt to add '-w' to target.
Selecting an alt that isn't windowed will close and relaunch Astonia client as windowed.
   (note: switching to windowed mode can take a while, no more than a few seconds tho)
ScalA will display the UAC shield when running as admin.

**ScalA 0.8.7**
Fixed thumbnails sometimes not being culled correctly after selecting an alt.
Fixed selected client glitching position when opening alt selection combobox dropdown.
Partially replaced 0.8.5 smooth window moving with threadsafe delegate method.
   (this should finally fix exception on button image draw)
Used above delegate to also handle quicklaunch icons, resulting in smoother operation.
Improved sorting in quicklaunch for shortcuts starting with identical numbers.
Made alt cycle hotkey (ctrl-space) immediately stop on releasing keys.
Increased maximum number of alts on overview to 16.

**ScalA 0.8.8**
Fixed reselecting already selected alt would glitch restore position. 
Fixed reselecting 'Someone' while on overview would blank button text and images for a split second.
Moved quicklaunch folder selection dialog to settings.
Implemented Cancellationtoken for quicklaunch deferred icon loading.
Added Rounded Corners setting for them Windows 11 fanboys.
Moved default quicklaunch location to %ProgramData%/ScalA.
   (this is to prevent exceptions when Windows Defender Ransomware Protection is on)

**ScalA 0.8.9**
Added option to unelevate self by right clicking on the admin shield.
   (note: this will only show when ScalA is running as Administrator)

**ScalA 0.9.0**
Improved responsiveness of quicklaunch by refactoring deferred icon loading and always using it. 
Fixed using thumb to scroll stats/inv/depot no longer shifts cursor towards center after releasing mousebutton.
Fixed using thumb to scroll stats/inv/depot no longer causes it to jump down.
   (note there is an astonia client bug which will still cause this to happen intermittently)
Fixed exception when quicklaunch folder doesn't exist.

**ScalA 0.9.1**
Added setting to have thumbnails on overview function as game.
Fixed releasing scroll thumb when dragged with Astonia client initially inactive would set mouse to wrong location.
Fixed upgrading version would not load saved custom resolutions until program restart.
Fixed bug in overview when switching to a different count of buttons it would sometimes mess up button positions.

**ScalA 0.9.2**
Reduced CPU utilization of overview.
Fixed race condition exception on closing ScalA.
Fixed releasing scroll thumb when dragged with ScalA maximized would set mouse to wrong location.
Fixed bug on overview when alt selection combobox is open and hovering over alts would stop overview from updating.
Fixed Astonia window position glitch on active overview when quickly moving between diffrent clients.

**ScalA 0.9.3**
Fixed dragging scroll thumb on active overview.
Made XMouseButton1 and XMouseButton2 on overview buttons activate the relevant Astonia client.
Improved smooth window moving. 
Fixed bug in alignment settings that made it unusable.
Hidden Astonia client better when moving form and restoring from maximized state.

**ScalA 0.9.4**
Enabled clickguard for active overview when quicklaunch, alt and resolution comboboxes are open.
Fixed unelevate sets wrong working directory for ScalA.
Added right click contextmenu for quicklaunch items.
Minor performance improvements.

**ScalA 0.9.5**
Fixed bug where clickguard would sometimes not disable.
Improved clickguard and enabled it when system menu is open.
Fixed creating new folder in root of quicklaunch would create it in parent directory with wrong name.
Fixed overview not displaying enough columns or rows when division of height/width resulted in an off by one error.
