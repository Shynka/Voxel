Public Class Form1
    Dim game As New Game
    Private Shared rnd As New Random
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Voxel.Load()

        Timer1.Start()
    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        e.Graphics.DrawImage(Voxel.Render, 0, 0, Me.Width, Me.Height)
    End Sub
    Dim z As Integer = 20
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For y = 0 To 9
            For x = 0 To 9
                Voxel.Draw(x, y, z, Color.Blue)
                Voxel.Draw(z, y, x, Color.Lime)
                Voxel.Draw(x, z, y, Color.Red)
            Next
        Next
        If z = 40 Then
            z = 9
        Else
            z += 1

        End If
        Me.Invalidate()
    End Sub
End Class
Public Class Voxel
    Public Shared PolyFace(2, 3) As Point
    Public Shared hexSize As Integer = 8
    Public Shared CubedSize As Integer = 60
    Public Shared Map(CubedSize, CubedSize, CubedSize) As Point
    Private Shared OriginPoint As New Point(400, 400)

    Private Shared hexWidth As Integer
    Private Shared hexWhalf As Integer
    Private Shared hsh As Integer
    Public Class Cube
        Public x, y, z As Integer
        Public color As Color

    End Class
    Public Shared Sub Load()
        hexWidth = hexSize * 2 - (hexSize / 3)
        hexWhalf = hexWidth / 2
        hsh = hexSize / 2
        BuildPolyFaces()
        ConstructMap()
    End Sub
    Private Shared bg As New Bitmap(Form1.Width, Form1.Height)
    Private Shared gra As Graphics = Graphics.FromImage(bg)
    Public Shared Cubes As New ArrayList
    Public Shared Function Render() As Bitmap
        gra.Clear(Color.Black)
        For Each c As Cube In Cubes
            DrawPoly(Map(c.x, c.y, c.z), c.color, gra)
        Next
        Cubes.Clear()
        Return bg
    End Function
    Public Shared Sub Draw(x As Integer, y As Integer, z As Integer, c As Color)
        Cubes.Add(New Cube With {.color = c, .x = x, .y = y, .z = z})
    End Sub
    Private Shared Sub DrawPoly(p As Point, c As Color, g As Graphics)
        For i = 0 To 2
            g.FillPolygon(New SolidBrush(c), {New Point(PolyFace(i, 0).X + p.X, PolyFace(i, 0).Y + p.Y),
                                              New Point(PolyFace(i, 1).X + p.X, PolyFace(i, 1).Y + p.Y),
                                              New Point(PolyFace(i, 2).X + p.X, PolyFace(i, 2).Y + p.Y),
                                              New Point(PolyFace(i, 3).X + p.X, PolyFace(i, 3).Y + p.Y)})
        Next
    End Sub
    Public Shared Sub ConstructMap()
        For y = 0 To CubedSize
            Dim OldOP As Point = OriginPoint
            RD(cd.x, 0)
            RD(cd.y, -4)

            For row = 0 To CubedSize
                For col = 0 To CubedSize
                    Map(row, col, y) = RD(cd.x, 1)
                Next
                OriginPoint = OldOP
                RD(cd.x, 0)
                RD(cd.y, -4)
                For i = 0 To row
                    RD(cd.z, 1)
                Next
            Next
            OriginPoint = OldOP
            RD(cd.y, -1)
        Next
    End Sub
    Public Shared Function RD(ccd As cd, v As Integer) As Point
        Select Case ccd
            Case cd.x
                OriginPoint.X += v * hexWhalf
                OriginPoint.Y += v * hsh
            Case cd.y
                OriginPoint.Y += v * hexSize
            Case cd.z
                OriginPoint.X -= v * hexWhalf
                OriginPoint.Y += v * hsh
        End Select
        Return OriginPoint
    End Function
    Public Enum cd
        x
        y
        z
    End Enum
    Public Shared Function BuildPolyFaces()
        PolyFace(0, 0) = New Point(0, 0)
        PolyFace(0, 1) = phc(0, 0, hexSize, 4)
        PolyFace(0, 2) = phc(0, 0, hexSize, 5)
        PolyFace(0, 3) = phc(0, 0, hexSize, 0)

        PolyFace(1, 0) = New Point(0, 0)
        PolyFace(1, 1) = phc(0, 0, hexSize, 0)
        PolyFace(1, 2) = phc(0, 0, hexSize, 1)
        PolyFace(1, 3) = phc(0, 0, hexSize, 2)

        PolyFace(2, 0) = New Point(0, 0)
        PolyFace(2, 1) = phc(0, 0, hexSize, 2)
        PolyFace(2, 2) = phc(0, 0, hexSize, 3)
        PolyFace(2, 3) = phc(0, 0, hexSize, 4)

        Return PolyFace
    End Function

    Private Shared Function phc(x As Integer, y As Integer, size As Integer, i As Integer) As Point
        Dim ar = Math.PI / 180 * (60 * i - 30)
        Return New Point(x + size * Math.Cos(ar), y + size * Math.Sin(ar))
    End Function
End Class