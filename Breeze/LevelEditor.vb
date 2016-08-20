﻿Imports System.Math
Imports System.Drawing
Imports SFML.Graphics
Imports SFML.System
Imports SFML.Window
Imports NAudio.Wave
Imports System.Threading
Imports System.Windows.Forms
Imports GameShardsCoreSFML

Module LevelEditor
    'GUI
    'Dim BlockBTN As New SFMLButton
    'Dim BGBTN As New SFMLButton

    Friend AuthorTB As New SFMLTextbox
    Friend NameTB As New SFMLTextbox
    Friend WithEvents Panel1 As New SFMLPanel
    Friend WithEvents Grid As New SFMLGrid
    Friend WithEvents GroupTileset As New SFMLGroupbox
    Friend WithEvents GroupBGObjects As New SFMLGroupbox
    Friend WithEvents ForeRB As New SFMLRadioButton
    Friend WithEvents NormalRB As New SFMLRadioButton
    Friend WithEvents BackRB As New SFMLRadioButton
    'Friend WithEvents Button5 As Button
    'Friend WithEvents Button6 As Button
    'Friend WithEvents GroupBox2 As GroupBox
    'Friend WithEvents GroupBox4 As GroupBox
    'Friend WithEvents Button4 As Button
    'Friend WithEvents Button3 As Button
    'Friend WithEvents GroupBox5 As GroupBox
    'Friend WithEvents CheckBox4 As CheckBox
    'Friend WithEvents CheckBox3 As CheckBox
    'Friend WithEvents CheckBox2 As CheckBox
    'Friend WithEvents CheckBox1 As CheckBox
    'Friend WithEvents Label3 As Label
    'Friend WithEvents Label2 As Label
    'Friend WithEvents NumericUpDown2 As NumericUpDown
    'Friend WithEvents NumericUpDown1 As NumericUpDown
    'Friend WithEvents Button1 As Button
    'Friend WithEvents Button2 As Button
    'Friend WithEvents Label1 As Label
    'Friend WithEvents Button7 As Button
    'Friend WithEvents Button8 As Button
    'Friend WithEvents GroupBox8 As GroupBox
    'Friend WithEvents GroupBox7 As GroupBox
    'Friend WithEvents GroupBox6 As GroupBox
    'Friend WithEvents Label5 As Label
    'Friend WithEvents Label4 As Label

    Friend WithEvents FakeCHK As New SFMLCheckbox
    Friend WithEvents RealCHK As New SFMLCheckbox
    Friend KB As New SFMLKeyboard(New SFML.Graphics.Font("arial.ttf"))

    'Windows
    Dim BlocksForm As New BlocksForm
    Dim BGForm As New BGForm

    Sub DoEditor()

        'Draw BackColor
        window.Clear(SFML.Graphics.Color.White)

        'Draw Backrect

        'Draw Background

        'Draw Background objects

        'Draw Blocks

        'Draw Items

        'Draw Player

        'Draw Grid
        'If ShowGrid Then
        '    DrawGrid()
        'End If

        'Draw GUI
        EditorGUI.Draw(window)

    End Sub

    Sub PostInitLevelEditor()
        BlocksForm = New BlocksForm
        BlocksForm.StartPosition = FormStartPosition.CenterScreen

        'Backrect.FillColor = SFML.Graphics.Color.White
        'Backrect.FillColor = SFML.Graphics.Color.Black

        'GridOutlineColor = SFML.Graphics.Color.White
        'GridColor = SFML.Graphics.Color.Transparent

    End Sub

    Public Sub GUILoadLevelEditor()
        With Panel1
            .Location = New Point(12, 12)
            .Size = New Size(800, 608)
            .ContentColor = SFML.Graphics.Color.White
            EditorGUI.Controls.Add(Panel1)
        End With

        With Grid
            .Location = New Point(12, 12)
            .Size = New Size(800, 608)
            .ContentColor = SFML.Graphics.Color.Transparent
            EditorGUI.Controls.Add(Grid)
        End With

        With NameTB
            .TextAlign = HorizontalAlignment.Left
            .Text = "LevelName"
            .ForeColor = Drawing.Color.Black
            .SFMLFont = New SFML.Graphics.Font("arial.ttf")
            .SFMLFontSize = 32
            .Location = New Point(400, 150)
            .Size = New Size(300, 30)
            .IDStr = "BGBTN"
            .AutoSize = False
            .MaxLength = 15
            EditorGUI.Controls.Add(NameTB)
        End With

        'KB = New SFMLKeyboard(New Vector2f(0, 650), New Vector2f(522, 172))
        With KB
            '.KeyPadding = 3
            .SetKeys(New Vector2f(0, 660), New Vector2f(522, 172), 0, .SFMLFont)
            .ForeColor = Drawing.Color.Black
            .Font = New Drawing.Font("arial", 15)
            '.SFMLFont = New SFML.Graphics.Font("arial.ttf")
            '.SizeWH = New Size(522, 172)
            '.Location = New Point(0, 700)
            NameTB.BoundKeyboard = KB
            .BoundToTextbox = True
            EditorGUI.Controls.Add(KB)
        End With

        With FakeCHK
            .CheckSpriteNormal = New Sprite(New Texture("C:\\GameShardsSoftware\Resources\Sprites\Breeze\CheckMark.png"))
            .CheckSpriteUnchecked = New Sprite(New Texture("C:\\GameShardsSoftware\Resources\Sprites\Breeze\CheckMark1.png"))
            .Checked = True
            .Location = New Point(500, 500)
            .ForeColor = Drawing.Color.Purple
            .SFMLFont = KeyFont
            .SFMLFontSize = 24
            .TextAlign = ContentAlignment.MiddleLeft
            .Text = "Is Fake?"
            .Autoscale = True
            .BoxSize = New Size(24, 24)
            '.BorderColornormal = SFML.Graphics.Color.White
            '.BorderColorHover = New SFML.Graphics.Color(0, 0, 128)
            .CycleIndeterminate = True
            EditorGUI.Controls.Add(FakeCHK)
        End With

        With realCHK
            .CheckSpriteNormal = New Sprite(New Texture("C:\\GameShardsSoftware\Resources\Sprites\Breeze\CheckMark.png"))
            .CheckSpriteUnchecked = New Sprite(New Texture("C:\\GameShardsSoftware\Resources\Sprites\Breeze\CheckMark1.png"))
            .Checked = True
            .Location = New Point(500, 550)
            .ForeColor = Drawing.Color.Purple
            .SFMLFont = KeyFont
            .SFMLFontSize = 24
            .TextAlign = ContentAlignment.MiddleLeft
            .Text = "Is Real?"
            .Autoscale = True
            .BoxSize = New Size(24, 24)
            '.BorderColornormal = SFML.Graphics.Color.White
            '.BorderColorHover = New SFML.Graphics.Color(0, 0, 128)
            .CycleIndeterminate = False
            EditorGUI.Controls.Add(RealCHK)
        End With

        With GroupTileset
            .Location = New Point(818, 35)
            .SFMLFont = KeyFont
            .Size = New Size(359, 365)
            .SFMLFontSize = 16
            .Text = "Tileset"
            EditorGUI.Controls.Add(GroupTileset)
        End With

        With GroupBGObjects
            .SFMLFont = KeyFont
            .SFMLFontSize = 16
            .Location = New Point(824, 274)
            .Size = New Size(347, 120)
            .Text = "Backgrounds"
            EditorGUI.Controls.Add(GroupBGObjects)
        End With

        With ForeRB
            .SFMLFont = KeyFont
            .SFMLFontSize = 16
            .Location = New Point(824, 500)
            .Text = "Foreground"
            .BoxSize = New Size(15, 15)
            .Group = "ZBlockEnum"
            EditorGUI.Controls.Add(ForeRB)
        End With

        With NormalRB
            .SFMLFont = KeyFont
            .SFMLFontSize = 16
            .Location = New Point(824, 520)
            .Text = "Normal Z position"
            .BoxSize = New Size(15, 15)
            .Group = "ZBlockEnum"
            EditorGUI.Controls.Add(NormalRB)
        End With

        With BackRB
            .SFMLFont = KeyFont
            .SFMLFontSize = 16
            .Location = New Point(824, 540)
            .Text = "BackGround"
            .BoxSize = New Size(15, 15)
            .Group = "ZBlockEnum"
            EditorGUI.Controls.Add(BackRB)
        End With
    End Sub



#Region "Handles"

    Sub EditorWindowClick(sender As Object, e As MouseButtonEventArgs)

        For x = 0 To EditorGUI.Controls.Count - 1
            EditorGUI.Controls(x).CheckClick(New Point(e.X, e.Y))

            'Additional External Click Checks
            If GGeom.CheckIfRectangleIntersectsPoint(New Rectangle(EditorGUI.Controls(x).location.X, EditorGUI.Controls(x).location.Y, EditorGUI.Controls(x).size.Width, EditorGUI.Controls(x).size.Height), New Point(e.X, e.Y)) Then

                'Textbox
                If TypeOf EditorGUI.Controls(x) Is SFMLTextbox Then
                    DirectCast(EditorGUI.Controls(x), SFMLTextbox).SetActive(New Point(e.X, e.Y))

                    'Keyboard
                ElseIf TypeOf EditorGUI.Controls(x) Is SFMLKeyboard Then
                    If NameTB.IsActive Then
                        DirectCast(EditorGUI.Controls(x), SFMLKeyboard).SetKeyPressed(New Point(e.X, e.Y), NameTB.Text)
                    End If

                    'Radiobutton
                ElseIf TypeOf EditorGUI.Controls(x) Is SFMLRadioButton Then
                    ControlUtils.RadioButtonUtils.CheckRadiobuttons(EditorGUI, DirectCast(EditorGUI.Controls(x), SFMLRadioButton), DirectCast(EditorGUI.Controls(x), SFMLRadioButton).Group, New Point(e.X, e.Y))
                End If
            End If
        Next
    End Sub

    Sub LevelEditorMouseMoved(sender As Object, e As MouseMoveEventArgs)
        For x = 0 To EditorGUI.Controls.Count - 1
            If TypeOf EditorGUI.Controls(x) Is SFMLKeyboard Then
                DirectCast(EditorGUI.Controls(x), SFMLKeyboard).SetKeyToggled(New Point(e.X, e.Y))


            ElseIf TypeOf EditorGUI.Controls(x) Is SFMLCheckbox Then
                DirectCast(EditorGUI.Controls(x), SFMLCheckbox).CheckHover(New Point(e.X, e.Y))
            End If
        Next
    End Sub

    Sub EditorKeyDown(sender As Object, e As SFML.Window.KeyEventArgs)
        For x = 0 To EditorGUI.Controls.Count - 1
            If TypeOf EditorGUI.Controls(x) Is SFMLTextbox Then
                ControlUtils.TextboxUtils.UpdateTextboxes(EditorGUI, e.Code.ToString)
            End If
        Next
    End Sub

    Sub EditorKeyup(sender As Object, e As SFML.Window.KeyEventArgs)

    End Sub
#End Region
End Module
