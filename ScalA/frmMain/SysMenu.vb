Class SysMenu
    Private Shared _handle As IntPtr
    Private Shared _form As Form
    Public Shared ReadOnly Property Handle(form As Form) As IntPtr
        Get
            If _handle = IntPtr.Zero Then
                _form = form
                _handle = GetSystemMenu(form.Handle, False)
            End If
            Return _handle
        End Get
    End Property
    Public Shared ReadOnly Property Handle() As IntPtr
        Get
            If _handle = IntPtr.Zero Then
                _form = FrmMain
                _handle = GetSystemMenu(FrmMain.Handle, False)
            End If
            Return _handle
        End Get
    End Property

    Public Shared Visible As Boolean

    Public Shared Function Show(pos As Point)
        SysMenu.Visible = True
        Dim cmd As Integer = TrackPopupMenuEx(SysMenu.Handle, TPM_RIGHTBUTTON Or TPM_RETURNCMD, pos.X, pos.Y, _form.Handle, Nothing)
        SysMenu.Visible = False
        If cmd > 0 Then
            Debug.Print("SendMessage " & cmd)
            '_form.WndProc(Message.Create(_form.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)) 'cant call because protected
            SendMessage(_form.Handle, WM_SYSCOMMAND, cmd, IntPtr.Zero)
        End If
    End Function

    Private Shared mii As New MENUITEMINFO With {
               .cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(MENUITEMINFO)),
               .fMask = MIIM.STATE}

    Public Shared Function Disable(item As Integer) As Boolean
        mii.fState = MFS.DISABLED
        Return SetMenuItemInfo(Handle, item, False, mii)
    End Function
    Public Shared Function Enable(item As Integer) As Boolean
        mii.fState = MFS.ENABLED
        Return SetMenuItemInfo(Handle, item, False, mii)
    End Function

End Class

Partial Class FrmMain

    Public Sub MangleSysMenu()
        Const GWL_STYLE As Integer = -16
        Debug.Print("SetWindowLong")
        SetWindowLong(Me.Handle, GWL_STYLE, GetWindowLong(Me.Handle, GWL_STYLE) Or WindowStyles.WS_SYSMENU Or WindowStyles.WS_MINIMIZEBOX) 'Enable SysMenu and MinimizeBox 
        If SysMenu.Handle(Me) Then
            'remove alt-F4 from close item
            Dim mii As New MENUITEMINFO With {
                .cbSize = Runtime.InteropServices.Marshal.SizeOf(GetType(MENUITEMINFO)),
                .fMask = MIIM.STRING Or MIIM.STATE,
                .dwTypeData = "&Close",
                .fState = MFS.DEFAULT}
            SetMenuItemInfo(SysMenu.Handle, SC_CLOSE, False, mii)
            'disable size and restore item
            SysMenu.Disable(SC_SIZE)
            SysMenu.Disable(SC_RESTORE)
            'add settings
            InsertMenuA(SysMenu.Handle, 0, MF_SEPARATOR Or MF_BYPOSITION, 0, String.Empty)
            InsertMenuA(SysMenu.Handle, 0, MF_BYPOSITION, &H8000 + 1337, "Settings")
            SetMenuItemBitmaps(SysMenu.Handle, 0, MF_BYPOSITION, My.Resources.gear_wheel.GetHbitmap(Color.Red), Nothing)
        End If
    End Sub
    Public Async Sub ShowSysMenu(sender As Control, e As MouseEventArgs) Handles pnlTitleBar.MouseUp, lblTitle.MouseUp, btnMin.MouseUp, btnMax.MouseUp
        UntrapMouse(e.Button) ' fix mousebutton stuck bug
        If e.Button = MouseButtons.Right Then
            Debug.Print("ShowSysMenu hSysMenu=" & SysMenu.Handle.ToString)
            pbZoom.Visible = False
            AButton.ActiveOverview = False

            SysMenu.Show(sender.PointToScreen(e.Location))

            Await Task.Delay(200)
            If cboAlt.DroppedDown OrElse cmbResolution.DroppedDown OrElse cmsQuickLaunch.Visible OrElse cmsAlt.Visible OrElse SysMenu.Visible Then Exit Sub
            AltPP?.Activate()
            If Not pnlOverview.Visible Then
                pbZoom.Visible = True
            Else
                AButton.ActiveOverview = My.Settings.gameOnOverview
            End If
        End If
    End Sub

End Class