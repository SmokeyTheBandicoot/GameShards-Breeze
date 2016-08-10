﻿Imports System.Math
Imports System.Drawing
Imports SFML.Graphics
Imports SFML.System
Imports SFML.Window
Imports NAudio.Wave
Imports System.Threading
Imports System.Windows.Forms
Imports GameShardsCoreSFML
'Imports TGUI

Module MainGame
    'Base
    Dim IsExit As Boolean = False
    Dim IsPaused As Boolean = False

    'Threading
    Dim GameLoopThread As Thread

    'Framerate
    Dim TimeSpan As Date
    Dim FPS As Double

    'Engine
    Dim IsCont As Boolean = False
    Dim Pict As New PictureBox
    Dim angle As Single = 0
    Dim XSpeed As Single = 0
    Dim YSpeed As Single = 0
    Dim XAcceleration As Single = 0
    Dim YAcceleration As Single = 0
    Dim Gravity As Single = 0.2 'Breeze lows gravity
    Dim BreezeSpeed As Single = 0.1
    Public AngleMax As Single = 90 - 0.001
    Public MinY As Integer = 0
    Public MaxY As Integer = 646
    Public XLoc As Integer = 500
    Public Friction As Single = 0.98
    Public Env As FlightEnvironment = FlightEnvironment.Normal
    Public Wind As WeatherWind = WeatherWind.LandBreeze

    Public Enum FlightEnvironment As Byte
        Normal = 20
        Underwater = 10
        Moon = 5
    End Enum

    Public Enum WeatherWind As Integer
        LandBreeze = 35 'Weakest, Arid (Right)
        SeaBreeze = 35 'Weakest, Humid (Left)
        LightBreeze = 50 'Weak, almost non-existant, Clean
        MountainBreeze = 75 'Weak-Normal, Cold, Arid
        ValleyBreeze = 75 'Weak-Normal, Warm, Humid
        ModerateBreeze = 150 'Normal, Humid
        ModerateWind = 200 'Normal-Strong, Clean
        StrongWind = 250 'Strong, Cold, Clean
        TempestWind = 350 'Stronger, Cold, Humid
        WirlWind = 500 'Strongest, Cold
    End Enum

    'Rendering Engine
    Dim BackScroll As Single = 0

    'Level
    Dim level As New Level

    'GamePlay
    Dim Score As Long = 0
    Dim Coins As Byte = 0
    Dim Lives As Byte = 3
    Dim HP As Byte = 100

    'Graphic Elements
    Dim Font As SFML.Graphics.Font
    Dim Player As New Sprite(New Texture("C:\\GameShardsSoftware\Resources\Sprites\Bankruptcy\[Bankruptcy]Bankruptcy.png"))
    Dim HUDTopLeft As New Text
    'Dim Background As New Background(New Sprite(New Texture("C:\Program Files (x86)\SMBX141\GFXPack\NSMB\NSMBWii\Backgrounds\New Super Mario Bros. Wii Custom Backgrounds\background2-19.gif")))

    'GUI elements
    'MainGame
    Dim LevelEditorBTN As New SFMLButton
    Dim CloseBTN As New SFMLButton

    Public Sub DoMainGame()
        If Not IsPaused Then

            'Start calculating FPS
            TimeSpan = Now

            'Execute Physics
            GameLoop()

            'Update Rendering

            'Update HUDs
            HUDTopLeft.DisplayedString = String.Format("Time: {1}{0}Position: X: {2}; Y:{3}{0}Speed: X: {4}; Y: {5}{0}Acceleration: X: {6}; Y: {7}{0}Rotation: {8}{0}Current State: {9}{0}Backscroll: {10}", vbNewLine, Now.ToString, Pict.Left, Pict.Top, XSpeed, YSpeed, XAcceleration, YAcceleration, Player.Rotation.ToString, CurrentState.Name.ToString, BackScroll.ToString)
            HUDTopLeft.CharacterSize = 24
            HUDTopLeft.Color = SFML.Graphics.Color.Black
            HUDTopLeft.Position = New Vector2f(0, 0)

            'Update Player position
            Player.Position = New Vector2f(Pict.Left, Pict.Top)
            Player.Rotation = 48 * CSng(Atan(YSpeed / XSpeed))
            Player.Color = New SFML.Graphics.Color(128, 128, 128)

            'Update CheckPoints

            'Update Items

            'Update Blocks

            'Update Background objects

            'Update Background
            Select Case level.BackGround.HScroll
                Case Background.HorizontalScrollMode.Fixed
                Case Background.HorizontalScrollMode.Repeated
                    level.BackGround.BGImage.Position = New Vector2f(level.BackGround.BGImage.Position.X - (XSpeed * Friction / level.BackGround.ScrollSpeedX), 0)
                    If level.BackGround.BGImage.Position.X + level.BackGround.BGImage.Texture.Size.X < 0 Then
                        level.BackGround.BGImage.Position = New Vector2f(0, level.BackGround.BGImage.Position.Y)
                    End If
                Case Background.HorizontalScrollMode.Stretched

            End Select



        End If

        'Draw everything
        'Draw Background
        Select Case level.BackGround.HScroll
            Case Background.HorizontalScrollMode.Fixed
            Case Background.HorizontalScrollMode.Repeated
                'If Background.Right < window.Size.X Then
                '    For x = 0 To Background.GetHowManyRepeated(window,level)
                '        Background.BGImage.Position = New Vector2f(x * Background.BGImage.Texture.Size.X - BackScroll, Background.BGImage.Position.Y)
                window.Draw(level.BackGround.BGImage)
                level.BackGround.BGImage.Position = New Vector2f(level.BackGround.BGImage.Position.X + level.BackGround.BGImage.Texture.Size.X, level.BackGround.BGImage.Position.Y)
                window.Draw(level.BackGround.BGImage)
                level.BackGround.BGImage.Position = New Vector2f(level.BackGround.BGImage.Position.X + level.BackGround.BGImage.Texture.Size.X, level.BackGround.BGImage.Position.Y)
                window.Draw(level.BackGround.BGImage)
                level.BackGround.BGImage.Position = New Vector2f(level.BackGround.BGImage.Position.X - 2 * level.BackGround.BGImage.Texture.Size.X, level.BackGround.BGImage.Position.Y)
                'Next

                'End If
            Case Background.HorizontalScrollMode.Stretched

        End Select

        'window.Draw(Background.BGImage)

        'Draw Background objects
        'Draw Blocks
        'Draw Items

        'Draw Checkpoints

        'draw the GUI
        MainGameGUI.Draw(window)

        'Draw player
        window.Draw(Player)

        'Draw HUDs
        window.Draw(HUDTopLeft)

        'Finish calculating FPS
        FPS = 1000 / (Now - TimeSpan).TotalMilliseconds
    End Sub

#Region "Loading"
    Public Sub PostInitMainGame()

        'Configure HUD
        HUDTopLeft.Font = New SFML.Graphics.Font("crash-a-like.ttf")
        HUDTopLeft.CharacterSize = 20

        'YLocation will be replaced by level's startpoint
        Player = New Sprite(New Texture("C:\GameShardsSoftware\paperplane.png"))
        Pict.Location = New Point(XLoc, 30)
        Pict.Size = New Size(20, 20)

        'Load the correct level - should go in tandem with MainMenu
        level = LoadLevel("No path yet")

    End Sub

    Sub GUILoadMainGame()
        Console.WriteLine("Loading Main Game GUI...")
        With LevelEditorBTN
            .TextAlign = ContentAlignment.MiddleLeft
            .Text = "LevelEditor"
            .ForeColor = Drawing.Color.Blue
            .SFMLFont = New SFML.Graphics.Font("crash-a-like.ttf")
            .SFMLFontSize = 48
            .Toggleable = True
            .ToggleChangesSprite = False
            .ToggleChangesColor = True
            .Location = New Point(0, CInt(window.Size.Y - 50))
            .Size = New Size(900, 50)
            .AutoPadding = True
            .ColorNormal = New SFML.Graphics.Color(255, 255, 255, 255)
            .ColorToggled = New SFML.Graphics.Color(200, 200, 200, 200)
            .SpriteNormal = New Sprite(New Texture("C:\GameShardsSoftware\Resources\Sprites\Breeze\MainLayout.png"))
            .SpriteToggled = New Sprite(New Texture("C:\GameShardsSoftware\Resources\Sprites\Breeze\MainLayoutToggled.png"))

            MainGameGUI.Controls.Add(LevelEditorBTN)
        End With

        With CloseBTN
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = "X"
            .ForeColor = Drawing.Color.Black
            .SFMLFont = New SFML.Graphics.Font("crash-a-like.ttf")
            .SFMLFontSize = 48
            .Toggleable = True
            .ToggleChangesSprite = False
            .ToggleChangesColor = True
            .Location = New Point(CInt(window.Size.X - 50), 0)
            .Size = New Size(50, 50)
            .AutoPadding = True
            .ColorNormal = New SFML.Graphics.Color(255, 255, 255, 255)
            .ColorToggled = New SFML.Graphics.Color(200, 200, 200, 200)
            .SpriteNormal = New Sprite(New Texture("C:\GameShardsSoftware\Resources\Sprites\Breeze\MainLayout.png"))
            .SpriteToggled = New Sprite(New Texture("C:\GameShardsSoftware\Resources\Sprites\Breeze\MainLayoutToggled.png"))

            MainGameGUI.Controls.Add(CloseBTN)
        End With
        Console.WriteLine("Successfully Loaded ""LevelEditorButton""")
        Console.WriteLine("Main GameGUI loaded successfully!")
    End Sub
#End Region



    Public Sub GameLoop()
        'Do While IsExit = False

        Application.DoEvents()

        If IsPaused Then

        Else
            'Start calculating framerate
            TimeSpan = Now

            'Do physics
            YSpeed += Gravity
            YSpeed += YAcceleration
            XSpeed = CSng(XSpeed + (Wind / 100))
            XSpeed += XAcceleration

            XSpeed *= Friction
            YSpeed *= Friction

            'Blocks.left -= xspeed
            'Pict.Left = CInt(Pict.Left + XSpeed)
            BackScroll += XSpeed / 25
            Pict.Top = CInt(Pict.Top + YSpeed)

            If Pict.Top > window.Size.Y - Pict.Height Then
                Pict.Top = CInt(window.Size.Y - Pict.Height)
                YSpeed *= -1
            ElseIf Pict.Top < 0 Then
                Pict.Top = 0
                YSpeed *= -1
            End If

            If Pict.Left > window.Size.X - Pict.Width Then
                Pict.Left = CInt(window.Size.X - Pict.Width)
                XSpeed *= -1
            ElseIf Pict.Left < 0 Then
                Pict.Left = 0
                XSpeed *= -1
            End If

            'Do light calculations
            'If Items.Count > 0 Then
            '    For x = 0 To Items.Count - 1
            '        If Items(x).Item = Item.ItemType.Light Then
            '            Dim r As New IntRect(CInt(DirectCast(Items(x), Light).Location.X), CInt(DirectCast(Items(x), Light).Location.Y), CInt(DirectCast(Items(x), Light).AoE), CInt(DirectCast(Items(x), Light).AoE))
            '            If Player.TextureRect.Intersects(r) Then
            '                Player.Color = (DirectCast(Items(x), Light).Color)
            '            End If
            '        End If
            '    Next
            'End If
        End If
    End Sub

#Region "Handles"

    Sub MainGameKeyDown(sender As Object, e As SFML.Window.KeyEventArgs)

        Select Case True
            Case e.Code = Keyboard.Key.W Or e.Code = Keyboard.Key.Up
                Gravity = 0
                'Select Case Env
                '    Case FlightEnvironment.Normal
                '        YAcceleration = -0.2
                '    Case FlightEnvironment.Underwater
                '        YAcceleration = -0.1
                '    Case FlightEnvironment.Moon
                '        YAcceleration = -0.05
                'End Select
                YAcceleration = -XSpeed / (2 * Env)
            Case e.Code = Keyboard.Key.S Or e.Code = Keyboard.Key.Down
                Select Case Env
                    Case FlightEnvironment.Normal
                        YAcceleration = 0.2
                    Case FlightEnvironment.Underwater
                        YAcceleration = 0.1
                    Case FlightEnvironment.Moon
                        YAcceleration = 0.05
                End Select
            Case e.Code = Keyboard.Key.D Or e.Code = Keyboard.Key.Right

                'Select Case Env
                '    Case FlightEnvironment.Normal
                '        XAcceleration = CSng(BreezeSpeed + 0.1)
                '    Case FlightEnvironment.Underwater
                '        XAcceleration = CSng(BreezeSpeed + 0.05)
                '    Case FlightEnvironment.Moon
                '        XAcceleration = CSng(BreezeSpeed + 0.025)
                'End Select
                XAcceleration = CSng(Wind / 100 + (Wind / 200))
                'Wind = 0
            Case e.Code = Keyboard.Key.A Or e.Code = Keyboard.Key.Left

                'Select Case Env
                '    Case FlightEnvironment.Normal
                '        XAcceleration = CSng(BreezeSpeed - 0.1)
                '    Case FlightEnvironment.Underwater
                '        XAcceleration = CSng(BreezeSpeed - 0.05)
                '    Case FlightEnvironment.Moon
                '        XAcceleration = CSng(BreezeSpeed - 0.025)
                'End Select
                XAcceleration = CSng(-Wind / 100) 'CSng(Wind / 100 - (Wind / 200))
        End Select
    End Sub

    Sub MainGameKeyUp(ByVal sender As Object, e As SFML.Window.KeyEventArgs)

        Select Case Env
            Case FlightEnvironment.Normal
                Gravity = 0.2
            Case FlightEnvironment.Underwater
                Gravity = 0.1
            Case FlightEnvironment.Moon
                Gravity = 0.05
        End Select

        Select Case e.Code
            Case Keyboard.Key.Right Or Keyboard.Key.Left
                XAcceleration = BreezeSpeed
        End Select


        XAcceleration = 0
        YAcceleration = 0
    End Sub

    Sub MainGameMouseMoved(sender As Object, e As MouseMoveEventArgs)
        For x = 0 To MainGameGUI.Controls.Count - 1
            If TypeOf MainGameGUI.Controls(x) Is SFMLButton Then
                If GGeom.CheckIfRectangleIntersectsPoint(DirectCast(MainGameGUI.Controls(x), SFMLButton).Bounds, New Point(e.X, e.Y)) Then
                    DirectCast(MainGameGUI.Controls(x), SFMLButton).IsToggled = True
                Else
                    DirectCast(MainGameGUI.Controls(x), SFMLButton).IsToggled = False
                End If
            End If
        Next
    End Sub


    Sub MainGameWindowClick(sender As Object, e As MouseButtonEventArgs)
        For x = 0 To MainGameGUI.Controls.Count - 1
            If TypeOf MainGameGUI.Controls(x) Is SFMLButton Then
                If GGeom.CheckIfRectangleIntersectsPoint(DirectCast(MainGameGUI.Controls(x), SFMLButton).Bounds, New Point(e.X, e.Y)) Then
                    CareerSelected = False
                    Select Case DirectCast(MainGameGUI.Controls(x), SFMLButton).Text.ToUpper
                        Case "LEVELEDITOR"
                            CurrentState = GameStates.LevelEditor
                        Case "X"
                            CurrentState = GameStates.MainMenu

                        Case ""
                    End Select

                End If
            End If
        Next
    End Sub

#End Region
End Module