Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
	Private Const SPI_SETDESKWALLPAPER As UInteger = &H14UI
	Private Const SPIF_UPDATEINIFILE As UInteger = &H1UI
	Private Const SPIF_SENDCHANGE As UInteger = &H2UI

	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
	Private Shared Function SystemParametersInfo(
		uiAction As UInteger,
		uiParam As UInteger,
		pvParam As String,
		fWinIni As UInteger
	) As Boolean
	End Function

	Private ReadOnly wallpapersList As New ListBox()
	Private ReadOnly previewBox As New PictureBox()
	Private ReadOnly loadButton As New Button()
	Private ReadOnly loadFolderButton As New Button()
	Private ReadOnly applyButton As New Button()
	Private ReadOnly supportedExtensions As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
		".bmp", ".jpg", ".jpeg", ".png"
	}

	Public Sub New()
		InitializeComponent()
		BuildLayout()
	End Sub

	Private Sub BuildLayout()
		Text = "Wallpaper Switcher"
		MinimumSize = New Size(760, 460)

		Dim toolbar As New FlowLayoutPanel() With {
			.Dock = DockStyle.Top,
			.Padding = New Padding(8),
			.Height = 54
		}

		loadButton.Text = "Load Files"
		loadButton.AutoSize = True
		AddHandler loadButton.Click, AddressOf LoadButton_Click

		loadFolderButton.Text = "Load Folder (Recursive)"
		loadFolderButton.AutoSize = True
		AddHandler loadFolderButton.Click, AddressOf LoadFolderButton_Click

		applyButton.Text = "Set Selected Wallpaper"
		applyButton.AutoSize = True
		AddHandler applyButton.Click, AddressOf ApplyButton_Click

		toolbar.Controls.Add(loadButton)
		toolbar.Controls.Add(loadFolderButton)
		toolbar.Controls.Add(applyButton)

		wallpapersList.Dock = DockStyle.Fill
		AddHandler wallpapersList.SelectedIndexChanged, AddressOf WallpapersList_SelectedIndexChanged
		AddHandler wallpapersList.DoubleClick, AddressOf WallpapersList_DoubleClick

		previewBox.Dock = DockStyle.Fill
		previewBox.SizeMode = PictureBoxSizeMode.Zoom
		previewBox.BackColor = Color.Black

		Dim split As New SplitContainer() With {
			.Dock = DockStyle.Fill,
			.SplitterDistance = 320
		}

		split.Panel1.Padding = New Padding(8)
		split.Panel2.Padding = New Padding(8)
		split.Panel1.Controls.Add(wallpapersList)
		split.Panel2.Controls.Add(previewBox)

		Controls.Add(split)
		Controls.Add(toolbar)
	End Sub

	Private Sub LoadButton_Click(sender As Object, e As EventArgs)
		Using dialog As New OpenFileDialog()
			dialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png"
			dialog.Multiselect = True

			If dialog.ShowDialog(Me) <> DialogResult.OK Then
				Return
			End If

			AddWallpapers(dialog.FileNames)

			If wallpapersList.SelectedIndex = -1 AndAlso wallpapersList.Items.Count > 0 Then
				wallpapersList.SelectedIndex = 0
			End If
		End Using
	End Sub

	Private Sub LoadFolderButton_Click(sender As Object, e As EventArgs)
		Using dialog As New FolderBrowserDialog()
			dialog.Description = "Choose a wallpaper folder. Subfolders are included."
			dialog.UseDescriptionForTitle = True

			If dialog.ShowDialog(Me) <> DialogResult.OK Then
				Return
			End If

			Dim files = GetSupportedFilesRecursive(dialog.SelectedPath)
			If files.Count = 0 Then
				MessageBox.Show(Me, "No supported images were found in the selected folder.", "No Images Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return
			End If

			' Folder load replaces the current in-memory list for this app session.
			wallpapersList.Items.Clear()
			AddWallpapers(files)

			If wallpapersList.SelectedIndex = -1 AndAlso wallpapersList.Items.Count > 0 Then
				wallpapersList.SelectedIndex = 0
			End If
		End Using
	End Sub

	Private Function GetSupportedFilesRecursive(rootFolder As String) As List(Of String)
		Dim results As New List(Of String)()
		Dim pending As New Stack(Of String)()
		pending.Push(rootFolder)

		While pending.Count > 0
			Dim current = pending.Pop()

			Try
				For Each subDir In Directory.EnumerateDirectories(current)
					pending.Push(subDir)
				Next
			Catch
				' Skip folders that cannot be enumerated.
			End Try

			Try
				For Each filePath In Directory.EnumerateFiles(current)
					If supportedExtensions.Contains(Path.GetExtension(filePath)) Then
						results.Add(filePath)
					End If
				Next
			Catch
				' Skip files in folders that cannot be read.
			End Try
		End While

		Return results
	End Function

	Private Sub AddWallpapers(filePaths As IEnumerable(Of String))
		For Each filePath In filePaths
			If wallpapersList.Items.Contains(filePath) Then
				Continue For
			End If

			wallpapersList.Items.Add(filePath)
		Next
	End Sub

	Private Sub ApplyButton_Click(sender As Object, e As EventArgs)
		ApplySelectedWallpaper()
	End Sub

	Private Sub WallpapersList_DoubleClick(sender As Object, e As EventArgs)
		ApplySelectedWallpaper()
	End Sub

	Private Sub WallpapersList_SelectedIndexChanged(sender As Object, e As EventArgs)
		Dim selectedPath = TryCast(wallpapersList.SelectedItem, String)
		If String.IsNullOrWhiteSpace(selectedPath) OrElse Not File.Exists(selectedPath) Then
			SetPreviewImage(Nothing)
			Return
		End If

		Try
			SetPreviewImage(LoadImageCopy(selectedPath))
		Catch
			SetPreviewImage(Nothing)
			MessageBox.Show(Me, "Couldn't load preview for the selected image.", "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
		End Try
	End Sub

	Private Sub ApplySelectedWallpaper()
		Dim selectedPath = TryCast(wallpapersList.SelectedItem, String)
		If String.IsNullOrWhiteSpace(selectedPath) Then
			MessageBox.Show(Me, "Select a wallpaper first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		If Not File.Exists(selectedPath) Then
			MessageBox.Show(Me, "The selected file no longer exists.", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Warning)
			Return
		End If

		Dim success = SystemParametersInfo(
			SPI_SETDESKWALLPAPER,
			0UI,
			selectedPath,
			SPIF_UPDATEINIFILE Or SPIF_SENDCHANGE
		)

		If success Then
			MessageBox.Show(Me, "Wallpaper changed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		Dim win32Error = Marshal.GetLastWin32Error()
		MessageBox.Show(Me, $"Wallpaper change failed. Win32 error: {win32Error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
	End Sub

	Private Function LoadImageCopy(filePath As String) As Image
		Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
			Using source = Image.FromStream(fs)
				Return CType(source.Clone(), Image)
			End Using
		End Using
	End Function

	Private Sub SetPreviewImage(newImage As Image)
		Dim oldImage = previewBox.Image
		previewBox.Image = newImage
		oldImage?.Dispose()
	End Sub

	Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
		' Everything stays in memory only and is discarded when the app closes.
		wallpapersList.Items.Clear()
		SetPreviewImage(Nothing)
		MyBase.OnFormClosed(e)
	End Sub
End Class
