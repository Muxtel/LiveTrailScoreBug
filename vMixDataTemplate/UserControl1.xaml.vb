Imports System.Xml
Imports System.Windows.Threading
Imports System.Windows.Media.Animation




Public Class UserControl1


    Public Lignes As XmlNodeList
    Public TimeStamps As New List(Of String)
    Public rippleStoryboard As Storyboard
    Private Headings
    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().




        Dim Timer As New DispatcherTimer()
        AddHandler Timer.Tick, AddressOf Refresh
        Timer.Interval = TimeSpan.FromMilliseconds(100)
        Timer.Start()



        ' démarre anim
        rippleStoryboard = DirectCast(FindResource("Storyboard1"), Storyboard)
        rippleStoryboard.Begin()


    End Sub

    Public Sub Refresh(ByVal sender As Object, ByVal e As EventArgs)

        Dim doc As New XmlDocument()
        doc.Load("c:\Users\dugla\OneDrive\Bureau\Projets en cours\Euskal trail 2022\vMix\Data\courses.xml")
        Dim course As XmlNode = doc.DocumentElement.SelectSingleNode("//course")
        Dim Timestamp As XmlNode = doc.DocumentElement.SelectSingleNode("//timestamp")
        Lignes = course.Item("lignes").ChildNodes

        TimeStamps.Add(Timestamp.InnerText)

        If TimeStamps.Count() = 1 Then




            MakeList()

        ElseIf TimeStamps.Count() > 1 Then

            If TimeStamps(TimeStamps.Count() - 1) <> TimeStamps(TimeStamps.Count() - 2) Then

                MakeList()
                ' redémarre l'anim
                rippleStoryboard.Begin()

            End If



        End If


    End Sub

    Public Sub MakeList()
        'Binding et datacontext
        Headings = New Headings(Lignes.Item(0).SelectNodes("Point"))
        Point1.DataContext = Headings
        Point2.DataContext = Headings
        Point3.DataContext = Headings
        Point4.DataContext = Headings
        List.Items.Clear()

        For i As Integer = 1 To (Lignes.Count() - 1)
            List.Items.Add(New Ligne(Lignes.Item(i).Item("Nom").InnerText,
                            Lignes.Item(i).SelectNodes("Point"),
                            "#ffffff",
                           Lignes.Item(i).Item("Dossard").InnerText,
                           Lignes.Item(i).Item("Classement").InnerText
                                                        )
                            )
        Next
    End Sub





    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class




Public Class Ligne
    Public Property Points As New ArrayList()
    Public Property Nom As String
    Public Property Dossard As String
    Public Property Classement As String

    Public Property Color As String
    Sub New(nom As String, PointNodes As XmlNodeList, color As String, dossard As String, classement As String)

        For Each PointNode In PointNodes
            Console.WriteLine(PointNode.innerText)
            Points.Add(PointNode.innerText)
        Next
        Me.Nom = nom.ToUpper()
        Me.Dossard = dossard
        Me.Classement = classement
        Me.Color = color

    End Sub

End Class

Public Class Headings
    Public Property NPoints As New List(Of String)
    Sub New(Heads As XmlNodeList)
        For Each Head In Heads
            NPoints.Add(Head.innerText.ToUpper())
        Next
    End Sub

End Class



