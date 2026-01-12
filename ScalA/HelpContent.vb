''' <summary>
''' Contains all help documentation content for the Help window
''' </summary>
Public Module HelpContent

    ''' <summary>
    ''' Returns the help content for a given topic key
    ''' </summary>
    Public Function GetContent(key As String) As String
        Select Case key
            Case "welcome"
                Return GetWelcome()
            Case "quick_start"
                Return GetQuickStart()
            Case "client_management"
                Return GetClientManagement()
            Case "quick_launch"
                Return GetQuickLaunch()
            Case "launcher_setup"
                Return GetLauncherSetup()
            Case "hotkeys"
                Return GetHotkeys()
            Case "settings"
                Return GetSettings()
            Case "troubleshooting"
                Return GetTroubleshooting()
            Case "credits"
                Return GetCredits()
            Case Else
                Return "Select a topic from the menu on the left."
        End Select
    End Function

#Region "Welcome"

    Private Function GetWelcome() As String
        Return "WELCOME TO SCALA" & vbCrLf & vbCrLf &
"ScalA (Scala Astonia) is a window manager and launcher utility for managing multiple Astonia game clients." & vbCrLf & vbCrLf &
"KEY FEATURES:" & vbCrLf &
"  - Manage multiple game clients from one window with live thumbnails" & vbCrLf &
"  - Quick Launch menu for fast character login via shortcuts" & vbCrLf &
"  - Launcher Setup tool for creating and managing login shortcuts" & vbCrLf &
"  - Custom resolution management with scaling options" & vbCrLf &
"  - Global hotkeys for switching between clients" & vbCrLf &
"  - Equipment Lock (EQ Lock) to prevent accidental unequips" & vbCrLf &
"  - Sorting and filtering clients with whitelist/blacklist" & vbCrLf &
"  - Theme support (System/Light/Dark)" & vbCrLf & vbCrLf &
"UI OVERVIEW:" & vbCrLf &
"  Title Bar: Version, warning icons, EQ Lock checkbox, Help (?), window controls" & vbCrLf &
"  Control Bar: Start/Overview button, Alt dropdown, Resolution dropdown" & vbCrLf &
"  Main Area: Client thumbnails in Overview mode" & vbCrLf & vbCrLf &
"RIGHT-CLICK MENUS:" & vbCrLf &
"  - Title bar: Opens Settings and Help & FAQ" & vbCrLf &
"  - Start button: Quick Launch menu (shortcuts)" & vbCrLf &
"  - Close button: Close options (ScalA, clients, or both)" & vbCrLf &
"  - Client thumbnails: Per-client options (sort, move, close)"
    End Function

#End Region

#Region "Quick Start"

    Private Function GetQuickStart() As String
        Return "QUICK START GUIDE" & vbCrLf & vbCrLf &
"BASIC WORKFLOW:" & vbCrLf &
"  1. Launch your game client(s) normally or via Quick Launch" & vbCrLf &
"  2. ScalA auto-detects running clients and shows them in the Alt dropdown" & vbCrLf &
"  3. Select a client from the dropdown to make it active" & vbCrLf &
"  4. Use the Resolution dropdown to resize the game window" & vbCrLf &
"  5. Click Start to enter Overview mode and see all clients as thumbnails" & vbCrLf &
"  6. Click any thumbnail to switch to that client" & vbCrLf & vbCrLf &
"OVERVIEW MODE:" & vbCrLf &
"  - Click Start button OR press Switch to Overview hotkey (default: Ctrl+Tab)" & vbCrLf &
"  - Thumbnails update in real-time showing each client's screen" & vbCrLf &
"  - Left-click a thumbnail to switch to that client" & vbCrLf &
"  - Right-click a thumbnail for options (sort, move, close, TopMost)" & vbCrLf & vbCrLf &
"QUICK LAUNCH (Launching Characters):" & vbCrLf &
"  1. Right-click the Start button to open Quick Launch menu" & vbCrLf &
"  2. Click any shortcut to launch that character" & vbCrLf &
"  3. Subfolders appear as submenus for organization" & vbCrLf & vbCrLf &
"EQUIPMENT LOCK:" & vbCrLf &
"  - Check the lock checkbox in title bar to enable" & vbCrLf &
"  - Blocks left-clicks on worn equipment to prevent accidental unequips" & vbCrLf &
"  - Hold Alt or Shift while clicking to temporarily bypass" & vbCrLf & vbCrLf &
"ACCESSING SETTINGS:" & vbCrLf &
"  - Right-click the title bar and select 'Settings', OR" & vbCrLf &
"  - Press the gear icon if visible"
    End Function

#End Region

#Region "Client Management"

    Private Function GetClientManagement() As String
        Return "CLIENT MANAGEMENT" & vbCrLf & vbCrLf &
"ALT DROPDOWN:" & vbCrLf &
"  - Located in the control bar (top-left area)" & vbCrLf &
"  - Shows all detected game clients by character name" & vbCrLf &
"  - Select a client to make it active" & vbCrLf &
"  - Right-click for Quick Launch menu" & vbCrLf & vbCrLf &
"CLIENT THUMBNAIL CONTEXT MENU (right-click thumbnail):" & vbCrLf &
"  - Select: Make this client active" & vbCrLf &
"  - ReLaunch: Close and restart this client" & vbCrLf &
"  - Sort: Change position (Top First/Last, None, Bot First/Last)" & vbCrLf &
"  - Move To: Move to another ScalA instance (Sidebar Mode)" & vbCrLf &
"  - TopMost: Keep this client window always on top" & vbCrLf &
"  - Active Overview (Beta): Allow clicks to pass through to game" & vbCrLf &
"  - Sidebar Mode: Enable multi-ScalA coordination" & vbCrLf &
"  - Close: Close this client" & vbCrLf &
"  - Close All: Close all clients" & vbCrLf & vbCrLf &
"SORTING CLIENTS:" & vbCrLf &
"  Right-click a thumbnail and select Sort:" & vbCrLf &
"  - Top First: Move to beginning of list" & vbCrLf &
"  - Top Last: Move to end of 'top' group" & vbCrLf &
"  - None: Default/middle position" & vbCrLf &
"  - Bot First: Move to start of 'bottom' group" & vbCrLf &
"  - Bot Last: Move to end of list" & vbCrLf &
"  Sort order affects both Overview thumbnails and Alt dropdown." & vbCrLf &
"  Save/load sort profiles in Settings > Sort & BL tab." & vbCrLf & vbCrLf &
"WHITELIST / BLACKLIST:" & vbCrLf &
"  Configure in Settings > Sort & BL tab:" & vbCrLf &
"  - Whitelist mode: ONLY show clients matching the list" & vbCrLf &
"  - Blacklist mode: HIDE clients matching the list" & vbCrLf &
"  Enter character names, one per line." & vbCrLf & vbCrLf &
"SIDEBAR MODE:" & vbCrLf &
"  Run multiple ScalA instances to manage different groups of clients:" & vbCrLf &
"  - Right-click a thumbnail and select 'Sidebar Mode'" & vbCrLf &
"  - Use 'Move To' to transfer clients between instances" & vbCrLf & vbCrLf &
"ACTIVE OVERVIEW (Beta):" & vbCrLf &
"  Allows mouse clicks to pass through thumbnails to the actual game window:" & vbCrLf &
"  - Right-click a thumbnail and select 'Active Overview'" & vbCrLf &
"  - Useful for quick interactions without leaving Overview" & vbCrLf &
"  - EQ Lock is respected in Active Overview mode" & vbCrLf & vbCrLf &
"CLOSE BUTTON CONTEXT MENU (right-click X button):" & vbCrLf &
"  - Close ScalA: Close only ScalA, leave clients running" & vbCrLf &
"  - Close Astonia: Close only the selected client" & vbCrLf &
"  - Close All Someone: Close idle/unnamed clients" & vbCrLf &
"  - Close Both: Close selected client and ScalA" & vbCrLf &
"  - Close All on Overview: Close all visible clients" & vbCrLf &
"  - Close All but [Name]: Close all except selected" & vbCrLf &
"  - Close All & ScalA: Close everything"
    End Function

#End Region

#Region "Quick Launch"

    Private Function GetQuickLaunch() As String
        Return "QUICK LAUNCH & FILE OPERATIONS" & vbCrLf & vbCrLf &
"Quick Launch provides fast access to game shortcuts directly from ScalA." & vbCrLf & vbCrLf &
"ACCESSING QUICK LAUNCH:" & vbCrLf &
"  - Right-click the Start button, OR" & vbCrLf &
"  - Right-click the Alt dropdown" & vbCrLf & vbCrLf &
"LAUNCHING CHARACTERS:" & vbCrLf &
"  - Shortcuts (.lnk files) appear as menu items" & vbCrLf &
"  - Subfolders appear as submenus" & vbCrLf &
"  - Left-click any shortcut to launch that character" & vbCrLf & vbCrLf &
"FILE OPERATIONS (right-click items in Quick Launch menu):" & vbCrLf &
"  - Copy: Copy selected shortcut(s) to clipboard" & vbCrLf &
"  - Paste: Paste shortcuts from clipboard" & vbCrLf &
"  - Paste Shortcut: Create shortcut to clipboard contents" & vbCrLf &
"  - Rename (F2): Rename the selected shortcut" & vbCrLf &
"  - Delete (Del): Delete the selected shortcut" & vbCrLf &
"  - Create Folder: Create a new subfolder for organization" & vbCrLf & vbCrLf &
"ADDING SHORTCUTS MANUALLY:" & vbCrLf &
"  1. Navigate to your Quick Launch folder (configured in Settings > QL)" & vbCrLf &
"  2. Create .lnk shortcuts with the game executable" & vbCrLf &
"  3. Add command-line arguments: -u username -p password" & vbCrLf &
"  4. Optionally add: -r WxH (resolution), -s server" & vbCrLf &
"  Example: astonia.exe -u mychar -p mypass -r 800x600" & vbCrLf & vbCrLf &
"REMOVING SHORTCUTS:" & vbCrLf &
"  - Right-click the shortcut in Quick Launch menu and select Delete, OR" & vbCrLf &
"  - Delete the .lnk file directly from the Quick Launch folder" & vbCrLf & vbCrLf &
"EDITING SHORTCUTS:" & vbCrLf &
"  - Use the Edit tab in Launcher Setup to rename shortcuts" & vbCrLf &
"  - Or right-click in Quick Launch menu and select Rename" & vbCrLf &
"  - To edit arguments, right-click the .lnk file in Windows Explorer" & vbCrLf &
"    and modify the Target field" & vbCrLf & vbCrLf &
"ORGANIZING WITH FOLDERS:" & vbCrLf &
"  - Create subfolders in your QL folder for organization" & vbCrLf &
"  - Examples: 'Mages', 'Warriors', 'Bots', 'Alts'" & vbCrLf &
"  - Folders appear as submenus in Quick Launch" & vbCrLf & vbCrLf &
"QUICK LAUNCH SETTINGS (Settings > QL tab):" & vbCrLf &
"  - Folder Path: Set the directory containing your shortcuts" & vbCrLf &
"  - Show Hidden Files: Include hidden items in menu" & vbCrLf &
"  - Resolve Links: Show actual target paths" & vbCrLf &
"  - Filter Extensions: Control which file types appear" & vbCrLf &
"  - Launcher Setup button: Open the shortcut creation wizard"
    End Function

#End Region

#Region "Launcher Setup"

    Private Function GetLauncherSetup() As String
        Return "LAUNCHER SETUP" & vbCrLf & vbCrLf &
"Access: Settings > QL tab > Launcher Setup button" & vbCrLf & vbCrLf &
"Launcher Setup has three tabs for creating and managing shortcuts:" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"SINGLE TAB - Create individual shortcuts" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf & vbCrLf &
"TEMPLATES:" & vbCrLf &
"  Templates store the game executable path, default resolution, and options." & vbCrLf &
"  - Add Template: Drag & drop a game .exe onto the template list" & vbCrLf &
"  - Edit Template: Select template and click Edit" & vbCrLf &
"  - Delete Template: Select template and click Delete" & vbCrLf & vbCrLf &
"CREATING A SHORTCUT:" & vbCrLf &
"  1. Select a template from the list" & vbCrLf &
"  2. Enter the character username" & vbCrLf &
"  3. Enter the password" & vbCrLf &
"  4. (Optional) Set a nickname for the shortcut filename" & vbCrLf &
"  5. (Optional) Change resolution or server" & vbCrLf &
"  6. Select target folder" & vbCrLf &
"  7. Click 'Create Shortcut'" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"BULK ADD TAB - Create multiple shortcuts at once" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf & vbCrLf &
"Use this when creating shortcuts for multiple characters with the same password." & vbCrLf & vbCrLf &
"STEP-BY-STEP:" & vbCrLf &
"  1. Click 'Browse' to select a source shortcut" & vbCrLf &
"     This copies the exe path, resolution, and options from an existing .lnk" & vbCrLf &
"  2. Enter usernames in the 'Username' column of the grid" & vbCrLf &
"     - Enter as many usernames as needed" & vbCrLf &
"  3. (Optional) Enter nicknames in the 'Nickname' column" & vbCrLf &
"     - Nickname is used for the shortcut filename" & vbCrLf &
"     - Username is always used for the -u login argument" & vbCrLf &
"     - Leave blank to use username as filename" & vbCrLf &
"  4. Enter the shared password (used for all shortcuts)" & vbCrLf &
"  5. Select target folder from dropdown" & vbCrLf &
"  6. Check 'Overwrite existing' if you want to replace existing shortcuts" & vbCrLf &
"  7. Click 'Create Shortcuts'" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"EDIT TAB - Rename existing shortcuts" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf & vbCrLf &
"RENAMING SHORTCUTS:" & vbCrLf &
"  1. Select a folder from the dropdown" & vbCrLf &
"     - Shows QL root folder and all subfolders" & vbCrLf &
"  2. The grid shows Current Name and New Name columns" & vbCrLf &
"  3. Edit the 'New Name' column for shortcuts you want to rename" & vbCrLf &
"  4. Click 'Apply Renames'" & vbCrLf & vbCrLf &
"CREATING FOLDERS:" & vbCrLf &
"  1. Enter a folder name in the text field" & vbCrLf &
"  2. Click 'Create Folder'" & vbCrLf &
"  The new folder appears in the folder dropdown and in Quick Launch menu."
    End Function

#End Region

#Region "Hotkeys"

    Private Function GetHotkeys() As String
        Return "HOTKEYS & KEY BINDINGS" & vbCrLf & vbCrLf &
"Configure hotkeys in Settings > Hotkeys tab." & vbCrLf & vbCrLf &
"DEFAULT HOTKEY BINDINGS:" & vbCrLf &
"  Switch to Overview (STO):    Ctrl + Tab" & vbCrLf &
"  Cycle Alts Forward:          Ctrl + Space" & vbCrLf &
"  Cycle Alts Backward:         Ctrl + Shift + Space" & vbCrLf &
"  Close All:                   Ctrl + Shift + F11" & vbCrLf &
"  Toggle TopMost:              Alt + Ctrl + T" & vbCrLf & vbCrLf &
"HOTKEY DESCRIPTIONS:" & vbCrLf & vbCrLf &
"  SWITCH TO OVERVIEW (STO):" & vbCrLf &
"    - If not in Overview: Enter Overview mode" & vbCrLf &
"    - If in Overview: Return to the selected client" & vbCrLf &
"    - Most commonly used hotkey for multi-client management" & vbCrLf & vbCrLf &
"  CYCLE ALTS FORWARD/BACKWARD:" & vbCrLf &
"    - Quickly switch between clients without entering Overview" & vbCrLf &
"    - Order follows your sort settings" & vbCrLf & vbCrLf &
"  CLOSE ALL:" & vbCrLf &
"    - Closes ALL detected game clients immediately" & vbCrLf &
"    - Use with caution!" & vbCrLf & vbCrLf &
"  TOGGLE TOPMOST:" & vbCrLf &
"    - Toggles the ScalA window's always-on-top state" & vbCrLf & vbCrLf &
"MODIFIER KEYS:" & vbCrLf &
"  Each hotkey can use any combination of:" & vbCrLf &
"  - Alt" & vbCrLf &
"  - Ctrl" & vbCrLf &
"  - Shift" & vbCrLf &
"  - Win (Windows key)" & vbCrLf & vbCrLf &
"CHANGING HOTKEYS:" & vbCrLf &
"  1. Open Settings > Hotkeys tab" & vbCrLf &
"  2. Click on the hotkey field you want to change" & vbCrLf &
"  3. Press your desired key combination" & vbCrLf &
"  4. Check/uncheck modifier boxes as needed" & vbCrLf &
"  5. Use the Enable checkbox to enable/disable each hotkey" & vbCrLf & vbCrLf &
"KEY BLOCKING OPTIONS:" & vbCrLf &
"  - Block Windows Key: Prevents Windows key from opening Start menu" & vbCrLf &
"  - Block Alt-Tab: Prevents Alt-Tab from switching windows" & vbCrLf &
"  These help prevent accidental interruptions during gameplay."
    End Function

#End Region

#Region "Settings"

    Private Function GetSettings() As String
        Return "SETTINGS REFERENCE" & vbCrLf & vbCrLf &
"Access Settings: Right-click the title bar > Settings" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"RESOLUTIONS TAB" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Custom Resolutions: Add/remove resolutions for the dropdown" & vbCrLf &
"  Generate Resolutions: Auto-create list based on screen size/aspect ratio" & vbCrLf &
"  Scaling Mode: How the game window is resized" & vbCrLf &
"  Reserve Space: Pixels to reserve on screen edges" & vbCrLf &
"  Size Borders: Include window borders in size calculations" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"HOTKEYS TAB" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Configure all hotkeys (see Hotkeys section for details)" & vbCrLf &
"  Block Windows Key: Prevent Start menu from opening" & vbCrLf &
"  Block Alt-Tab: Prevent window switching" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"QL TAB (Quick Launch)" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Folder Path: Directory containing your shortcuts" & vbCrLf &
"  Show Hidden Files: Include hidden items" & vbCrLf &
"  Resolve Links: Display actual target paths" & vbCrLf &
"  Filter Extensions: Control visible file types" & vbCrLf &
"  Launcher Setup: Opens the shortcut creation wizard" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"SORT & BL TAB" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Sort Profiles: Save/load client sort configurations" & vbCrLf &
"  Whitelist/Blacklist:" & vbCrLf &
"    - Enter character names (one per line)" & vbCrLf &
"    - Whitelist: Only show matching clients" & vbCrLf &
"    - Blacklist: Hide matching clients" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"MISC TAB" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Client Class Name: Window class to detect (default: 'Astonia')" & vbCrLf &
"  Client Exe Name: Executable name for detection" & vbCrLf &
"  Theme: System / Light / Dark" & vbCrLf &
"  Rounded Corners: Enable/disable rounded window corners" & vbCrLf &
"  DPI Override: Fix blurry/scaling issues on high-DPI displays" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"MAXIMIZED TAB" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  TopMost: Keep ScalA window always on top" & vbCrLf &
"  Max on Normal Overview: Auto-maximize when entering Overview" & vbCrLf &
"  Min on Min: Minimize ScalA when client is minimized" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"OTHER SETTINGS TOOLS" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  Batch Shortcut Manager: Accessed from QL tab" & vbCrLf &
"    - Modify multiple shortcuts at once" & vbCrLf &
"    - Change password across all shortcuts" & vbCrLf &
"    - Update resolution settings in bulk"
    End Function

#End Region

#Region "Troubleshooting"

    Private Function GetTroubleshooting() As String
        Return "TROUBLESHOOTING" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"CLIENT NOT DETECTED" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  1. Check Client Class Name (Settings > Misc)" & vbCrLf &
"     Default is 'Astonia' - must match your game's window class" & vbCrLf &
"  2. Check Client Exe Name (Settings > Misc)" & vbCrLf &
"     Must match your game executable name" & vbCrLf &
"  3. Check Whitelist/Blacklist (Settings > Sort & BL)" & vbCrLf &
"     Your client may be filtered out" & vbCrLf &
"  4. Restart ScalA after the game is running" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"RESOLUTION NOT CHANGING" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  1. Enable DPI Override (click warning icon in title bar)" & vbCrLf &
"  2. If using SDL2 client, update the SDL2 Wrapper" & vbCrLf &
"  3. Make sure game is in windowed mode (not fullscreen)" & vbCrLf &
"  4. Try a different Scaling Mode in Settings > Resolutions" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"DPI / BLURRY ISSUES" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  1. Click the DPI warning icon in the title bar to enable DPI Override" & vbCrLf &
"  2. Check Windows Display Settings > Scale (try 100% if possible)" & vbCrLf &
"  3. Multi-monitor setups with different DPI may cause issues" & vbCrLf &
"     - Client may behave differently on each monitor" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"HOTKEYS NOT WORKING" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  1. Check hotkey configuration (Settings > Hotkeys)" & vbCrLf &
"     Make sure 'Enable' is checked for each hotkey" & vbCrLf &
"  2. Check for conflicts with other programs" & vbCrLf &
"     Another app may be using the same key combination" & vbCrLf &
"  3. Run ScalA as Administrator" & vbCrLf &
"     Some hotkeys require elevated privileges" & vbCrLf &
"  4. If key blocking is enabled, it may affect other hotkeys" & vbCrLf & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"SDL2 WRAPPER ISSUES" & vbCrLf &
"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" & vbCrLf &
"  If you see a wrapper warning icon in the title bar:" & vbCrLf &
"  - Click the icon for SDL2 Wrapper installation options" & vbCrLf &
"  - The wrapper allows ScalA to manage SDL2-based game clients" & vbCrLf &
"  - Update to the latest wrapper version if prompted"
    End Function

#End Region

#Region "Credits"

    Private Function GetCredits() As String
        Return "CREDITS & ABOUT" & vbCrLf & vbCrLf &
"ScalA - Scala Astonia Window Manager" & vbCrLf & vbCrLf &
"DEVELOPERS:" & vbCrLf &
"  - bool (Lead Developer)" & vbCrLf &
"  - Community Contributors" & vbCrLf & vbCrLf &
"SPECIAL THANKS:" & vbCrLf &
"  - The Astonia community for feedback and testing" & vbCrLf &
"  - All users who report bugs and suggest features" & vbCrLf & vbCrLf &
"TECHNOLOGIES:" & vbCrLf &
"  - Built with Visual Basic .NET" & vbCrLf &
"  - Windows Forms UI" & vbCrLf &
"  - Win32 API for window management" & vbCrLf & vbCrLf &
"SOURCE CODE:" & vbCrLf &
"  - GitHub: github.com/smoorke/ScalA_Dev" & vbCrLf & vbCrLf &
"LICENSE:" & vbCrLf &
"  - Open source project" & vbCrLf & vbCrLf &
"Thank you for using ScalA!"
    End Function

#End Region

End Module
