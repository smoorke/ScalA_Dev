Public NotInheritable Class SysMenu

    Private ReadOnly _form As Form
    Public Sub New(frm As Form)
        _form = frm
        Handle = GetSystemMenu(_form.Handle, False)
    End Sub

    Public ReadOnly Handle As IntPtr = IntPtr.Zero

    Public Shared Operator IsFalse(ByVal [Me] As SysMenu) As Boolean
        Return [Me].Handle = IntPtr.Zero
    End Operator
    Public Shared Operator IsTrue(ByVal [Me] As SysMenu) As Boolean
        Return [Me].Handle <> IntPtr.Zero
    End Operator

    'Public Shared Operator =(ByVal left As IntPtr, [Me] As SysMenu)
    '    Return [Me].Handle = left
    'End Operator

    'Public Shared Operator <>(left As IntPtr, [Me] As SysMenu)
    '    Return [Me].Handle <> left
    'End Operator


    Public Visible As Boolean

    Public Function Show(pos As Point) As Integer
        'Me.Visible = True 'handled in wndproc
        dBug.print($"SysMenu.Show {pos}")

        Dim cmd As Integer = TrackPopupMenuEx(Me.Handle, TPM_RIGHTBUTTON Or TPM_RETURNCMD, pos.X, pos.Y, _form.Handle, Nothing)
        'Me.Visible = False 'handled in wndproc
        If cmd > 0 Then
            dBug.print("SendMessage " & cmd)
            Select Case cmd
                Case SC_MOVE
                    'FrmMain.Detach(False)
                    SendMessage(_form.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
                Case SC_SIZE
                    'FrmMain.Detach(False)
                    SendMessage(FrmSizeBorder.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
                Case Else
                    SendMessage(_form.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
            End Select
        End If
        Return cmd
    End Function

    Private Shared mii As New MENUITEMINFO With {.cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(MENUITEMINFO))}

    Public Function Disable(item As Integer, Optional byPos As Boolean = False) As Boolean
        mii.fMask = MIIM.STATE
        mii.fState = MFS.DISABLED
        Return SetMenuItemInfo(Me.Handle, item, byPos, mii)
    End Function
    Public Function Enable(item As Integer, Optional byPos As Boolean = False) As Boolean
        mii.fMask = MIIM.STATE
        mii.fState = MFS.ENABLED
        Return SetMenuItemInfo(Me.Handle, item, byPos, mii)
    End Function
    Public Function SetDefault(item As Integer, Optional byPos As Boolean = False) As Boolean
        mii.fMask = MIIM.STATE
        mii.fState = MFS.DEFAULT
        Return SetMenuItemInfo(Me.Handle, item, byPos, mii)
    End Function
    Public Function Rename(item As Integer, newName As String, Optional byPos As Boolean = False) As Boolean
        mii.fMask = MIIM.STRING
        mii.dwTypeData = newName
        Return SetMenuItemInfo(Me.Handle, item, byPos, mii)
    End Function
    'todo: convert these to also use mii
    Public Function SetBitmaps(item As Integer, bmUnchecked As Bitmap, bmChecked As Bitmap, Optional byPos As Boolean = False) As Boolean
        Return SetMenuItemBitmaps(Me.Handle, item,
                                  If(byPos, MF_BYPOSITION, MF_BYCOMMAND),
                                  If(bmUnchecked IsNot Nothing, bmUnchecked.GetHbitmap(Color.Red), Nothing),
                                  If(bmChecked IsNot Nothing, bmChecked.GetHbitmap(Color.Red), Nothing))
    End Function
    Public Function InsertSeperator(pos As Integer) As Boolean
        Return InsertMenuA(Me.Handle, pos, MF_SEPARATOR Or MF_BYPOSITION, 0, String.Empty)
    End Function
    Public Function InsertItem(pos As Integer, cmdID As Integer, item As String) As Boolean
        Return InsertMenuA(Me.Handle, pos, MF_BYPOSITION, cmdID, item)
    End Function

    Public Function Contains(pt As Point) As Boolean
        If Not Me.Visible Then Return False
        Dim rc As RECT
        GetWindowRect(FindWindow("#32768", Nothing), rc)
        Return rc.ToRectangle.Contains(pt)
    End Function

End Class

Partial NotInheritable Class FrmMain
    Dim SysMenu As SysMenu
    Const MC_SETTINGS As Integer = &H8000 + 1337
    Public Sub InitSysMenu()
        SysMenu = New SysMenu(Me)
        If SysMenu Then
            With SysMenu
                'remove alt-F4 from close item
                .Rename(SC_CLOSE, "&Close ScalA")
                .SetDefault(SC_CLOSE)
                'add settings
                .InsertSeperator(0)
                .InsertItem(0, MC_SETTINGS, "Se&ttings")
                .SetBitmaps(MC_SETTINGS, My.Resources.gear_wheel, Nothing)
            End With
        End If
    End Sub
    Public Async Sub ShowSysMenu(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseUp, lblTitle.MouseUp, btnMin.MouseUp, btnMax.MouseUp
        CloseOtherDropDowns(cmsQuickLaunch.Items, Nothing)
        cmsQuickLaunch.Close()
        UntrapMouse(e.Button) ' fix mousebutton stuck bug
        If e.Button = MouseButtons.Right Then

            pbZoom.Visible = False
            AButton.ActiveOverview = False

            Dim ret = SysMenu.Show(sender.PointToScreen(e.Location))

            If Not Await RestoreClicking() Then Exit Sub

            dBug.Print($"ShowSysMenu awaited")

            If ret = SC_MINIMIZE AndAlso My.Settings.MinMin AndAlso
               My.Settings.gameOnOverview AndAlso pnlOverview.Visible OrElse
                caption_Mousedown Then
                dBug.Print("sysmenu no attach")
            ElseIf ret <> SC_MINIMIZE OrElse (My.Settings.MinMin AndAlso AltPP?.isSDL) Then
                dBug.print("sysmenu attach")
                Attach(AltPP)
            End If
            'If GetActiveProcessID() = scalaPID AndAlso ret <> MC_SETTINGS Then
            '    dBug.print($"ShowSysMenu activating {AltPP?.Name}")
            '    AltPP?.Activate() 'why no attach here?
            'End If
        End If
    End Sub

End Class