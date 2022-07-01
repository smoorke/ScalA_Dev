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

**ScalA 0.9.6**
Enabled ctrl-space hotkey on overview also implemented ctrl-shift-space to cycle backwards.
Fixed overview sometimes glitching an alt for a brief second when logging one in.
Fixed alt being at wrong restore position when de-elevating ScalA.
Added configuration settings for hotkeys.

**ScalA 0.9.7**
Fixed overview glitch fix of 0.9.6 erroneously excluded alts with custom titles.
Added Cycle on Close to misc settings.

**ScalA 0.9.8**
Added custom sorting and white/blacklisting of alts.
 (white/blacklisting is integrated in the sort tab of settings)
Fixed quicklaunch path selection button in settings not doing anything.
Fixed cycling bug when it tried to cycle to closed clients.
Made maximized overview attempt to maintain aspect ratio.
Increased maximum number of alts.

**ScalA 0.9.9**
Fixed quicklaunch icons sometimes blank when first opening it or its submenus.
Fixed quicklaunch contextmenu submenu edge case where it glitched behind quicklaunch when running multiple ScalAs with same alt selected.
Fixed scroll thumb dragging glitch when multiple ScalAs have same alt selected and they are not on same Y position.
Made New Folder subroutine ask for new name after folder creation.
Improved overview garbage collection and CPU usage by caching process list.
Added SingleInstance setting and enabled it by default. 
  (If you want to run multiple instances you should make a copy of ScalA.exe and rename it to Scala2.exe or something)
  (This will give the additional benefit of having separate settings)
Fixed right click drag opening of contextmenus glitching the zoom and look cursor getting stuck until you right click again.
Made clickguard activate selected client when triggered.

**ScalA 1.0.0**
Fixed exception when closing alts with overview visible.
Added sorting options to overview alt contextmenu.
Improved clickguard when reopening menus or dropdowns.
Made holding ctrl while opening quicklaunch or its submenus show hidden files and folders.
Fixed alignment settings once again.
Fixed numerous bugs pertaining to restoring to/from maximized state.
Extended untrapping of right/middle mousebuttons when interacting with ScalA.
Fixed rename dialog losing focus with an alt selected.

**ScalA 1.0.1**
Implemented EQ Lock.
Minor UI improvements.

**ScalA 1.0.2**
Fixed Quicklaunch and Comboboxes glitching wrong cursor when overlapping EQ lock zone.
Fixed hovering over alt selection dropdown items would show EQ lock zone on overview.
Fixed ScalA popping to front after hovering over alt selection dropdown items.
Fixed cursor glitch when settings form overlaps EQ lock zone.

**ScalA 1.0.3**
Grayed EQ Lock icon to indicate it is not supported on active overview.
Fixed EQ lock stuck on indeterminate state when switching to overview with shift or alt-key pressed.
Fixed client popping in front after restoring from minimized to maximized state when relaunching ScalA.
Implemented a border reservation setting for maximized state.
Implemented settings to handle ultra widescreen monitors.
Fixed Quicklaunch being slow on vanilla clients.
Minor UX improvements.

**ScalA 1.0.4**
Fixed untrap mousebutton sending both rmb and mmb up message to client when interacting with ScalA and not dragged from currently selected alt.
Enabled dragging ScalA by the titlebar when maximized by adding a 1px border between ScalA and taskbar if no border was manually set.
  (adds the border to bottom if there is no taskbar on the monitor)
Fixed resolution being all wonky and EQ Lock checkbox disappearing when dragged from maximized state.
Fixed changing size and/or position of taskbar didn't adjust maximized ScalA correctly.
Fixed Settings form showing at wrong location with ScalA minimized.
Fixed Scala forgetting original Astonia positions when minimized.
Fixed EQ Lock not showing no-cursor when sysMenu is open.
Enabled EQ Lock on active overview.

**ScalA 1.0.5**
Fixed ScalA intermittently popping to front on active overview.
Fixed white pixel when mousedown on close button with rounded corners.
Added method to auto-close error dialog when an Astonia client was improperly closed. 
  (eg. by not using F12, clicking exit or the ScalA context menu options)
Fixed quicklaunch contextmenu staying wide after opening on an item with a long name.
Fixed rename dialog possibly opening off screen.
Increased the size of the EQ Lock zone as it was still possible to click off gear when clicking near the edge.
Fixed cancelling settings could possibly not restore sorting/black/whitelist.
Fixed ScalA not coming to front when one of its alts is the active window on active overview.
Increased cursor accuracy. (note: scaling more than 2x is still very jittery)
Implemented click capture to prevent clicks passing behind ScalA/Client.
Fixed moving other Astonia clients with quicklaunch open would be buggy.