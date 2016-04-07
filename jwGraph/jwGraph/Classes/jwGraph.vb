Option Explicit On
Option Strict On
Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports jwGraph.GeneralTools

Namespace jwGraph
    Public Class jwGraph
        Inherits Control
        Implements IDisposable

#Region "Constructor"
        Public Sub New()
            MyBase.New()

            'Find DPI
            Using g = Me.CreateGraphics
                Me.DPI = g.DpiX
                Me.DPIScaling = Me.DPI / 96.0!
            End Using

            'Initialize collections
            Series = New SeriesCollection(Me)

            _FreeMarkers = New FreeMarkerCollection
            _HorizontalMarkers = New HorizontalMarkerCollection
            _VerticalMarkers = New VerticalMarkerCollection

            GraphObjects = New BindingList(Of clsGraphObject)
            SeriesExcludedFromScaling = New System.Collections.Specialized.StringCollection ' List(Of String)

            'Initialize axes
            XAxis = New HorizontalAxis(Me) With {.AxisLocation = Axis.enumAxisLocation.Primary, .LabelFormat = "shortprec2"}
            Y1Axis = New VerticalAxis(Me) With {.AxisLocation = Axis.enumAxisLocation.Primary, .LabelFormat = "shortprec2"}
            Y2Axis = New VerticalAxis(Me) With {.AxisLocation = Axis.enumAxisLocation.Secondary, .LabelFormat = "shortprec2", .TitleDistance = 75, .OriginColor = Color.Red}


            'Set control styes
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

            Me.Size = New Size(400, 400)
            Me.MinimumSize = New Size(160, 105)

            'Initialize misc properties
            _HighQuality = True
            _EnableLegend = True
            _EnableGraphObjects = True
            _EnableMarkers = True
            _LegendPosition = enumLegendPosition.TopRight
            _EnableAutoscaling = True
            _IncludeMarkersInScaling = True
            _TopLeftColor = Color.White
            _BottomRightColor = Color.LightSteelBlue
            _GraphBackColor = Color.GhostWhite
            _AutoScaleBorder = 0.1
            _GraphBorder = New Padding(CInt(100 * DPIScaling), CInt(20 * DPIScaling), CInt(100 * DPIScaling), CInt(70 * DPIScaling))
            _ScaleProportional = False
            _MouseWheelZoomEnabled = True
            _LeftMouseFunctionalityEnabled = True
            _MiddleMouseFunctionalityEnabled = True
            _RightMouseFunctionalityEnabled = True

            _Message = ""
            _MessageColor = Color.Black
            'Internal fields
            isLegendExpanded = False
            ZoomSteps = New Stack(Of AxisBoundSet)
            lastMouseMove = Date.Now
            IsUserScaled = False
            isUpdating = False

            'Series defaults
            _MarkerLineColor = Color.Green
            _MarkerLineWidth = 1

            'Set up mouse functionality
            LeftMouseAction = enLeftMouseAction.ZoomIn
            MouseWheelRedirector.Attach(Me)
            AddHandler Me.Resize, AddressOf jwGraph_Resize 'Enables rescale on resize
            LineCursor = LoadCursorFromResource(My.Resources.LineCursor)
            CircleCursor = LoadCursorFromResource(My.Resources.CircleCursor)
            EraseCursor = LoadCursorFromResource(My.Resources.EraserCursor)

            InitializeContextMenu()
        End Sub
#End Region

#Region "Declares"
        <DllImport("User32.dll", CharSet:=CharSet.Ansi, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Private Shared Function LoadCursorFromFile(str As [String]) As IntPtr
        End Function
#End Region

#Region "Delegates"
        ''' <summary>
        ''' Allows the creation of custom labels when a datapoint is clicked
        ''' </summary>
        Public Delegate Function DatapointToString(Graph As jwGraph, Datapoint As Datapoint, Series As Series) As String
#End Region

#Region "Misc"


#End Region

#Region "Enums"
        Public Enum enumLegendPosition
            TopLeft = 0
            Top = 1
            TopRight = 2
            Left = 3
            Right = 4
            BottomLeft = 5
            Bottom = 6
            BottomRight = 7
        End Enum

        Private Enum MDownMode
            NotDown = 0
            Zoom = 2
            Marker = 3
            Move = 4
            ObjectMove = 5
            CreateObject = 6
        End Enum
        Public Enum WhichAxis
            X = 0
            PrimaryY = 1
            SecondaryY = 2
        End Enum
        Public Enum enLeftMouseAction
            ZoomIn = 0
            CreateCircle = 1
            CreateLine = 2
            MoveGraph = 3
            ErasePoint = 4
        End Enum
        Public Enum enGraphListChangedType
            ItemAdded = 0
            ItemRemoved = 1
        End Enum
        Public Enum enumMarkerType
            Free = 0
            Vertical = 1
            Horizontal = 2
        End Enum
#End Region

#Region "Structures"
        Structure AxisBoundSet
            Dim XMin As Double
            Dim XMax As Double
            Dim Y1Min As Double
            Dim Y1Max As Double
            Dim Y2Min As Double
            Dim Y2Max As Double
            Public Sub New(XMin As Double, XMax As Double, Y1Min As Double, Y1Max As Double, Y2Min As Double, Y2Max As Double)
                Me.XMin = XMin
                Me.XMax = XMax
                Me.Y1Min = Y1Min
                Me.Y1Max = Y1Max
                Me.Y2Min = Y2Min
                Me.Y2Max = Y2Max
            End Sub
        End Structure
#End Region

#Region "Properties"
        Public Property EraserVisible As Boolean
            Get
                Return ErasePointToolStripMenuItem.Visible
            End Get
            Set(value As Boolean)
                ErasePointToolStripMenuItem.Visible = value
            End Set
        End Property

        Private _ExportDialog As frmGraphExportSetup
        Private Property ExportDialog As frmGraphExportSetup
            Get
                If _ExportDialog Is Nothing OrElse _ExportDialog.IsDisposed Then _ExportDialog = New frmGraphExportSetup
                Return _ExportDialog
            End Get
            Set(value As frmGraphExportSetup)
                _ExportDialog = value
            End Set
        End Property

        Private _ExportDataDialog As frmDataExportSetup
        Private Property ExportDataDialog As frmDataExportSetup
            Get
                If _ExportDataDialog Is Nothing OrElse _ExportDataDialog.IsDisposed Then _ExportDataDialog = New frmDataExportSetup
                Return _ExportDataDialog
            End Get
            Set(value As frmDataExportSetup)
                _ExportDataDialog = value
            End Set
        End Property

        Private _LegendAlwaysVisible As Boolean
        Public Property LegendAlwaysVisible As Boolean
            Get
                Return _LegendAlwaysVisible
            End Get
            Set(value As Boolean)
                _LegendAlwaysVisible = value
                Me.Invalidate()
            End Set
        End Property

        Private _LegendPosition As enumLegendPosition
        Public Property LegendPosition As enumLegendPosition
            Get
                Return _LegendPosition
            End Get
            Set(value As enumLegendPosition)
                _LegendPosition = value
                Me.Invalidate()
            End Set
        End Property

        Private _HighQuality As Boolean
        Public Property HighQuality As Boolean
            Get
                Return _HighQuality
            End Get
            Set(value As Boolean)
                _HighQuality = value
                Me.Invalidate()
            End Set
        End Property

        Private _RightMouseFunctionalityEnabled As Boolean
        Public Property RightMouseFunctionalityEnabled As Boolean
            Get
                Return _RightMouseFunctionalityEnabled
            End Get
            Set(value As Boolean)
                _RightMouseFunctionalityEnabled = value
            End Set
        End Property
        Private _LeftMouseFunctionalityEnabled As Boolean
        Public Property LeftMouseFunctionalityEnabled As Boolean
            Get
                Return _LeftMouseFunctionalityEnabled
            End Get
            Set(value As Boolean)
                _LeftMouseFunctionalityEnabled = value
            End Set
        End Property
        Private _MiddleMouseFunctionalityEnabled As Boolean
        Public Property MiddleMouseFunctionalityEnabled As Boolean
            Get
                Return _MiddleMouseFunctionalityEnabled
            End Get
            Set(value As Boolean)
                _MiddleMouseFunctionalityEnabled = value
            End Set
        End Property


        Private _MouseWheelZoomEnabled As Boolean
        Public Property MouseWheelZoomEnabled As Boolean
            Get
                Return _MouseWheelZoomEnabled
            End Get
            Set(value As Boolean)
                _MouseWheelZoomEnabled = value
            End Set
        End Property

        Private _LeftMouseAction As enLeftMouseAction
        Public Property LeftMouseAction As enLeftMouseAction
            Get
                Return _LeftMouseAction
            End Get
            Set(value As enLeftMouseAction)
                _LeftMouseAction = value
                UpdateCursor()
                RaiseEvent LeftMouseActionChanged(Me, New MouseActionChangedEventArgs(value))
            End Set
        End Property
        Private Sub UpdateCursor()
            Select Case Me.LeftMouseAction
                Case enLeftMouseAction.ZoomIn : Me.Cursor = Cursors.Cross
                Case enLeftMouseAction.MoveGraph : Me.Cursor = Cursors.SizeAll
                Case enLeftMouseAction.CreateCircle : Me.Cursor = CircleCursor
                Case enLeftMouseAction.CreateLine : Me.Cursor = LineCursor
                Case enLeftMouseAction.ErasePoint : Me.Cursor = EraseCursor
            End Select
        End Sub

        Private _IncludeMarkersInScaling As Boolean
        Public Property IncludeMarkersInScaling As Boolean
            Get
                Return _IncludeMarkersInScaling
            End Get
            Set(value As Boolean)
                _IncludeMarkersInScaling = value
                DoAutoScale()
            End Set
        End Property

        Private _ScaleProportional As Boolean
        Public Property ScaleProportional As Boolean
            Get
                Return _ScaleProportional
            End Get
            Set(value As Boolean)
                _ScaleProportional = value
                DoAutoScale()
            End Set
        End Property

        Private _AutoScaleBorder As Double
        Public Property AutoScaleBorder As Double
            Get
                Return _AutoScaleBorder
            End Get
            Set(value As Double)
                _AutoScaleBorder = value
                DoAutoScale()
            End Set
        End Property
        Private _TopLeftColor As Color
        Public Property TopLeftColor As Color
            Get
                Return _TopLeftColor
            End Get
            Set(value As Color)
                _TopLeftColor = value
                Me.Invalidate()
            End Set
        End Property
        Private _BottomRightColor As Color
        Public Property BottomRightColor As Color
            Get
                Return _BottomRightColor
            End Get
            Set(value As Color)
                _BottomRightColor = value
                Me.Invalidate()
            End Set
        End Property
        Private _GraphBackColor As Color
        Public Property GraphBackColor As Color
            Get
                Return _GraphBackColor
            End Get
            Set(value As Color)
                _GraphBackColor = value
                Me.Invalidate()
            End Set
        End Property

        Private _GraphBorder As Padding
        Public Property GraphBorder As Padding
            Get
                Return _GraphBorder
            End Get
            Set(value As Padding)
                _GraphBorder = value
                Me.Invalidate()
            End Set
        End Property


        Private _MessageColor As Color
        Public Property MessageColor() As Color
            Get
                Return _MessageColor
            End Get
            Set(ByVal value As Color)
                _MessageColor = value
            End Set
        End Property

        Private _Message As String
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
                Me.Invalidate()
            End Set
        End Property

        Private _MarkerLineWidth As Integer
        Private _MarkerLineColor As Color

        Private DoOverrideChartArea As Boolean
        Private OverrideChartAreaRectangle As Rectangle

        Public ReadOnly Property InnerChartArea As Rectangle
            Get
                If DoOverrideChartArea Then
                    Return GetInnerChartArea(Me.OverrideChartAreaRectangle) 'New Rectangle(Me.GraphBorder.Left, Me.GraphBorder.Top, Me.ClientSize.Width - Me.GraphBorder.Left - Me.GraphBorder.Right, Me.ClientSize.Height - Me.GraphBorder.Top - Me.GraphBorder.Bottom)
                Else
                    Return GetInnerChartArea(Me.ClientRectangle) 'New Rectangle(Me.GraphBorder.Left, Me.GraphBorder.Top, Me.ClientSize.Width - Me.GraphBorder.Left - Me.GraphBorder.Right, Me.ClientSize.Height - Me.GraphBorder.Top - Me.GraphBorder.Bottom)
                End If
            End Get
        End Property

        Private Function GetInnerChartArea(OuterRectangle As Rectangle) As Rectangle
            Return New Rectangle(Me.GraphBorder.Left, Me.GraphBorder.Top, OuterRectangle.Width - Me.GraphBorder.Left - Me.GraphBorder.Right, OuterRectangle.Height - Me.GraphBorder.Top - Me.GraphBorder.Bottom)
        End Function

        Private _CenterImage As Bitmap
        Public Property CenterImage As Bitmap
            Get
                Return _CenterImage
            End Get
            Set(value As Bitmap)
                _CenterImage = value
                Me.Invalidate()
            End Set
        End Property
        Private _CenterImageMinSize As Size
        Public Property CenterImageMinSize As Size
            Get
                Return _CenterImageMinSize
            End Get
            Set(value As Size)
                _CenterImageMinSize = value
                Me.Invalidate()
            End Set
        End Property
        Private _CenterImageMaxSize As Size
        Public Property CenterImageMaxSize As Size
            Get
                Return _CenterImageMaxSize
            End Get
            Set(value As Size)
                _CenterImageMaxSize = value
                Me.Invalidate()
            End Set
        End Property

        Private _LegendTitle As String
        Public Property LegendTitle() As String
            Get
                Return _LegendTitle
            End Get
            Set(ByVal value As String)
                _LegendTitle = value
                Me.Invalidate()
            End Set
        End Property
        'Public Function ShouldSerializeSeriesExcludedFromScaling() As Boolean
        '    Return False
        'End Function
        'Public Sub ResetSeriesExcludedFromScaling()
        '    SeriesExcludedFromScaling = New List(Of String)
        'End Sub

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        <Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.Drawing.Design.UITypeEditor))>
        Private WithEvents _SeriesExcludedFromScaling As System.Collections.Specialized.StringCollection

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        <Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(System.Drawing.Design.UITypeEditor))>
        Public Property SeriesExcludedFromScaling As System.Collections.Specialized.StringCollection
            Get
                Return _SeriesExcludedFromScaling
            End Get
            Set(value As System.Collections.Specialized.StringCollection)
                _SeriesExcludedFromScaling = value
                DoAutoScale()
            End Set
        End Property

        Private _EnableGraphObjects As Boolean
        Public Property EnableGraphObjects As Boolean
            Get
                Return _EnableGraphObjects
            End Get
            Set(value As Boolean)
                _EnableGraphObjects = value
            End Set
        End Property
        Private _EnableMarkers As Boolean
        Public Property EnableMarkers As Boolean
            Get
                Return _EnableMarkers
            End Get
            Set(value As Boolean)
                _EnableMarkers = value
            End Set
        End Property
        Private _EnableLegend As Boolean
        Public Property EnableLegend As Boolean
            Get
                Return _EnableLegend
            End Get
            Set(value As Boolean)
                _EnableLegend = value
                Me.Invalidate()
            End Set
        End Property

        Private _EnableAutoscaling As Boolean
        Public Property EnableAutoscaling As Boolean
            Get
                Return _EnableAutoscaling
            End Get
            Set(value As Boolean)
                _EnableAutoscaling = value
            End Set
        End Property

        Public Property FreeMarkerCount As Integer
            Get
                Return _FreeMarkers.Count
            End Get
            Set(value As Integer)
                Dim minX As Double = XAxis.Minimum
                Dim maxx As Double = XAxis.Maximum
                Dim minY As Double = Y1Axis.Minimum
                Dim maxY As Double = Y1Axis.Maximum
                Dim deltaX As Double = (maxx - minX) / (value + 1)
                Dim deltaY As Double = (maxY - minY) / (value + 1)
                _FreeMarkers.Clear()
                For i = 1 To value
                    _FreeMarkers.Add(New FreeMarker(minX + deltaX * i, minY + deltaY * i, Axis.enumAxisLocation.Primary))
                Next
            End Set
        End Property
        Public Property VerticalMarkerCount As Integer
            Get
                Return _VerticalMarkers.Count
            End Get
            Set(value As Integer)
                Dim minX As Double = XAxis.Minimum
                Dim maxx As Double = XAxis.Maximum
                Dim deltaX As Double = (maxx - minX) / (value + 1)
                VerticalMarkers.Clear()
                For i = 1 To value
                    VerticalMarkers.Add(New VerticalMarker(minX + deltaX * i))
                Next
            End Set
        End Property
        Public Property HorizontalMarkerCount As Integer
            Get
                Return _HorizontalMarkers.Count
            End Get
            Set(value As Integer)
                Dim minY As Double = Y1Axis.Minimum
                Dim maxY As Double = Y1Axis.Maximum
                Dim deltaY As Double = (maxY - minY) / (value + 1)
                HorizontalMarkers.Clear()
                For i = 1 To value
                    HorizontalMarkers.Add(New HorizontalMarker(minY + deltaY * i, Axis.enumAxisLocation.Primary))
                Next
            End Set
        End Property
#End Region

#Region "EventArgs"
        Public Class MouseActionChangedEventArgs
            Inherits EventArgs
            Public Property NewAction As enLeftMouseAction
            Public Sub New(NewAction As enLeftMouseAction)
                MyBase.New()
                Me.NewAction = NewAction
            End Sub
        End Class
        Public Class PointToEraseEventArgs
            Inherits EventArgs
            Public Property DataPoint As Datapoint
            Public Property Series As Series
            Public Sub New(Datapoint As Datapoint, Series As Series)
                MyBase.New()
                Me.DataPoint = Datapoint
                Me.Series = Series
            End Sub
        End Class

        Public Class GraphObjectListChangedEventArgs
            Public Property Type As enGraphListChangedType
            Public Property Index As Integer
            Public Sub New(index As Integer, type As enGraphListChangedType)
                Me.Type = type
                Me.Index = index
            End Sub
        End Class
        Public Class MarkerDraggedEventArgs
            Inherits EventArgs
            Public Property MarkerType As enumMarkerType
            Public Property Marker As Marker
        End Class
#End Region

#Region "Events"
        Public Shadows Event MouseWheel(sender As Object, e As MouseEventArgs)
        Public Shadows Event MouseDown(sender As Object, e As MouseEventArgs)
        Public Shadows Event MouseMove(sender As Object, e As MouseEventArgs)
        Public Shadows Event MouseUp(sender As Object, e As MouseEventArgs)

        Public Event LeftMouseActionChanged(sender As Object, e As MouseActionChangedEventArgs)
        Public Event GraphObjectListChanged(sender As Object, e As ListChangedEventArgs)
        Public Event MarkerDragged(sender As Object, e As MarkerDraggedEventArgs)

        Public Event PointToBeErased(sender As Object, e As PointToEraseEventArgs)
#End Region

#Region "Context menu definitions and designer"
        Friend WithEvents cmsGraphOptions As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents ZoomToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DragToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DrawCircleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DrawLineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ZoomOutOnceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ResetZoomToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents LeftMouseButtonActionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents MiscellaneousToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RemoveAllObjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ErasePointToolStripMenuItem As HQMenuItem

        Friend WithEvents tsiLegend As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiShowLegend As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendSep As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents tsiLegendTopLeft As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendTop As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendTopRight As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendLeft As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendRight As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendBottomLeft As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendBottom As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiLegendBottomRight As System.Windows.Forms.ToolStripMenuItem

        Friend WithEvents tsiExportGraphImage As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsiExportData As System.Windows.Forms.ToolStripMenuItem

        Private Sub InitializeContextMenu()
            Me.cmsGraphOptions = New System.Windows.Forms.ContextMenuStrip()
            Me.ZoomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DragToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DrawCircleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DrawLineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.ZoomOutOnceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ResetZoomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.LeftMouseButtonActionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RemoveAllObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.MiscellaneousToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.tsiExportGraphImage = New System.Windows.Forms.ToolStripMenuItem()
            Me.tsiExportData = New System.Windows.Forms.ToolStripMenuItem()
            Me.ErasePointToolStripMenuItem = New HQMenuItem()

            tsiLegend = New ToolStripMenuItem With {.Text = "Legend"}
            tsiShowLegend = New ToolStripMenuItem With {.Text = "Toggle legend"}
            AddHandler tsiShowLegend.Click, (Sub(sender, e) EnableLegend = Not EnableLegend)
            tsiLegendTopLeft = New ToolStripMenuItem With {.Text = "Top-Left"}
            AddHandler tsiLegendTopLeft.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.TopLeft)
            tsiLegendTop = New ToolStripMenuItem With {.Text = "Top"}
            AddHandler tsiLegendTop.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.Top)
            tsiLegendTopRight = New ToolStripMenuItem With {.Text = "Top-Right"}
            AddHandler tsiLegendTopRight.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.TopRight)
            tsiLegendLeft = New ToolStripMenuItem With {.Text = "Left"}
            AddHandler tsiLegendLeft.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.Left)
            tsiLegendRight = New ToolStripMenuItem With {.Text = "Right"}
            AddHandler tsiLegendRight.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.Right)
            tsiLegendBottomLeft = New ToolStripMenuItem With {.Text = "Bottom-Left"}
            AddHandler tsiLegendBottomLeft.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.BottomLeft)
            tsiLegendBottom = New ToolStripMenuItem With {.Text = "Bottom"}
            AddHandler tsiLegendBottom.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.Bottom)
            tsiLegendBottomRight = New ToolStripMenuItem With {.Text = "Bottom-Right"}
            AddHandler tsiLegendBottomRight.Click, (Sub(sender, e) LegendPosition = enumLegendPosition.BottomRight)
            tsiLegendSep = New ToolStripSeparator

            tsiLegend.DropDownItems.AddRange({tsiShowLegend, tsiLegendSep, tsiLegendTopLeft, tsiLegendTop, tsiLegendTopRight, tsiLegendLeft, tsiLegendRight,
                 tsiLegendBottomLeft, tsiLegendBottom, tsiLegendBottomRight})

            '
            'cmsGraphOptions
            '
            Me.cmsGraphOptions.ImageScalingSize = New System.Drawing.Size(24, 24)
            Me.cmsGraphOptions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LeftMouseButtonActionToolStripMenuItem,
                                                                                        Me.ZoomToolStripMenuItem,
                                                                                        Me.DragToolStripMenuItem,
                                                                                        Me.DrawCircleToolStripMenuItem,
                                                                                        Me.DrawLineToolStripMenuItem,
                                                                                        Me.ToolStripSeparator2,
                                                                                        Me.MiscellaneousToolStripMenuItem,
                                                                                        Me.ErasePointToolStripMenuItem,
                                                                                        Me.tsiLegend,
                                                                                        Me.ZoomOutOnceToolStripMenuItem,
                                                                                        Me.ResetZoomToolStripMenuItem,
                                                                                        Me.RemoveAllObjectsToolStripMenuItem,
                                                                                        Me.tsiExportGraphImage,
                                                                                        Me.tsiExportData})
            Me.cmsGraphOptions.Name = "cmsGraphOptions"
            Me.cmsGraphOptions.Size = New System.Drawing.Size(217, 280)
            AddHandler Me.cmsGraphOptions.Opening, AddressOf ContextOpening

            '
            'ZoomToolStripMenuItem
            '
            Me.ZoomToolStripMenuItem.Image = My.Resources.Resources.ZoomDrag
            Me.ZoomToolStripMenuItem.Name = "ZoomToolStripMenuItem"
            Me.ZoomToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.ZoomToolStripMenuItem.Text = "Zoom"
            AddHandler Me.ZoomToolStripMenuItem.Click, AddressOf ZoomInButtonClicked
            '
            'DragToolStripMenuItem
            '
            Me.DragToolStripMenuItem.Image = My.Resources.Resources.MoveGraph
            Me.DragToolStripMenuItem.Name = "DragToolStripMenuItem"
            Me.DragToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.DragToolStripMenuItem.Text = "Drag"
            AddHandler Me.DragToolStripMenuItem.Click, AddressOf MoveButtonClicked
            '
            'DrawCircleToolStripMenuItem
            '
            Me.DrawCircleToolStripMenuItem.Image = My.Resources.Resources.CreateCirc
            Me.DrawCircleToolStripMenuItem.Name = "DrawCircleToolStripMenuItem"
            Me.DrawCircleToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.DrawCircleToolStripMenuItem.Text = "Draw Circle"
            AddHandler Me.DrawCircleToolStripMenuItem.Click, AddressOf CreateCircleButtonClicked

            '
            'DrawLineToolStripMenuItem
            '
            Me.DrawLineToolStripMenuItem.Image = My.Resources.Resources.AddLine
            Me.DrawLineToolStripMenuItem.Name = "DrawLineToolStripMenuItem"
            Me.DrawLineToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.DrawLineToolStripMenuItem.Text = "Draw Line"
            AddHandler Me.DrawLineToolStripMenuItem.Click, AddressOf CreateLineButtonClicked
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(213, 6)
            '
            'ZoomOutOnceToolStripMenuItem
            '
            Me.ZoomOutOnceToolStripMenuItem.Image = My.Resources.Resources.ZoomOut
            Me.ZoomOutOnceToolStripMenuItem.Name = "ZoomOutOnceToolStripMenuItem"
            Me.ZoomOutOnceToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.ZoomOutOnceToolStripMenuItem.Text = "Zoom out once"
            AddHandler Me.ZoomOutOnceToolStripMenuItem.Click, AddressOf ZoomOutOnceButtonClicked
            '
            'ResetZoomToolStripMenuItem
            '
            Me.ResetZoomToolStripMenuItem.Image = My.Resources.Resources.ZoomOutFull
            Me.ResetZoomToolStripMenuItem.Name = "ResetZoomToolStripMenuItem"
            Me.ResetZoomToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.ResetZoomToolStripMenuItem.Text = "Reset zoom"
            AddHandler Me.ResetZoomToolStripMenuItem.Click, AddressOf ZoomOutAllButtonClicked
            '
            'LeftMouseButtonActionToolStripMenuItem
            '
            Me.LeftMouseButtonActionToolStripMenuItem.Enabled = False
            Me.LeftMouseButtonActionToolStripMenuItem.Name = "LeftMouseButtonActionToolStripMenuItem"
            Me.LeftMouseButtonActionToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.LeftMouseButtonActionToolStripMenuItem.Text = "Left mouse button action"
            '
            'RemoveAllObjectsToolStripMenuItem
            '
            Me.RemoveAllObjectsToolStripMenuItem.Image = My.Resources.Resources.RemoveCirc
            Me.RemoveAllObjectsToolStripMenuItem.Name = "RemoveAllObjectsToolStripMenuItem"
            Me.RemoveAllObjectsToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.RemoveAllObjectsToolStripMenuItem.Text = "Remove all objects"
            AddHandler Me.RemoveAllObjectsToolStripMenuItem.Click, AddressOf RemoveAllObjectsButtonClicked
            '
            'MiscellaneousToolStripMenuItem
            '
            Me.MiscellaneousToolStripMenuItem.Enabled = False
            Me.MiscellaneousToolStripMenuItem.Name = "MiscellaneousToolStripMenuItem"
            Me.MiscellaneousToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.MiscellaneousToolStripMenuItem.Text = "Miscellaneous"
            '
            'tsiExportGraphImage
            '
            Me.tsiExportGraphImage.Name = "tsiExportGraphImage"
            Me.tsiExportGraphImage.Size = New System.Drawing.Size(216, 30)
            Me.tsiExportGraphImage.Text = "Export graph image"
            Me.tsiExportGraphImage.Image = My.Resources.ExportGraphImage
            AddHandler Me.tsiExportGraphImage.Click, AddressOf ExportGraphImageClicked
            '
            'tsiExportData
            '
            Me.tsiExportData.Name = "tsiExportData"
            Me.tsiExportData.Size = New System.Drawing.Size(216, 30)
            Me.tsiExportData.Text = "Export raw data"
            Me.tsiExportData.Image = My.Resources.ExportRawData
            AddHandler Me.tsiExportData.Click, AddressOf ExportDataClicked
            '
            'ErasePointToolStripItem
            '
            Me.ErasePointToolStripMenuItem.Name = "ErasePointToolStripMenuItem"
            Me.ErasePointToolStripMenuItem.Size = New System.Drawing.Size(216, 30)
            Me.ErasePointToolStripMenuItem.Text = "Erase data points"
            Me.ErasePointToolStripMenuItem.Visible = False
            Me.ErasePointToolStripMenuItem.Image = My.Resources.Eraser
            AddHandler Me.ErasePointToolStripMenuItem.Click, AddressOf ErasePointClicked


            If AdditionalCMSItems IsNot Nothing AndAlso AdditionalCMSItems.Count > 0 Then
                cmsGraphOptions.Items.Add(New ToolStripSeparator)
                For Each m As ToolStripItem In AdditionalCMSItems
                    cmsGraphOptions.Items.Add(m)
                Next
            End If
            Me.ContextMenuStrip = cmsGraphOptions
        End Sub

        Private AdditionalCMSItems As List(Of ToolStripItem)
        Public Sub AddAdditionalCMSItem(Item As ToolStripItem)
            If AdditionalCMSItems Is Nothing Then AdditionalCMSItems = New List(Of ToolStripItem)
            AdditionalCMSItems.Add(Item)
            InitializeContextMenu()
        End Sub
        Public Sub ClearAdditionalCMSItems()
            If AdditionalCMSItems Is Nothing Then AdditionalCMSItems = New List(Of ToolStripItem)
            For i = AdditionalCMSItems.Count - 1 To 0 Step -1
                AdditionalCMSItems(i).Dispose()
            Next
            AdditionalCMSItems.Clear()
            InitializeContextMenu()
        End Sub

        Private Sub ZoomInButtonClicked(sender As Object, e As EventArgs)
            LeftMouseAction = enLeftMouseAction.ZoomIn
        End Sub
        Private Sub MoveButtonClicked(sender As Object, e As EventArgs)
            LeftMouseAction = enLeftMouseAction.MoveGraph
        End Sub
        Private Sub CreateCircleButtonClicked(sender As Object, e As EventArgs)
            LeftMouseAction = enLeftMouseAction.CreateCircle
        End Sub
        Private Sub CreateLineButtonClicked(sender As Object, e As EventArgs)
            LeftMouseAction = enLeftMouseAction.CreateLine
        End Sub
        Private Sub ErasePointClicked(sender As Object, e As EventArgs)
            LeftMouseAction = enLeftMouseAction.ErasePoint
        End Sub
        Private Sub ZoomOutOnceButtonClicked(sender As Object, e As EventArgs)
            ZoomOutOnce()
        End Sub
        Private Sub ZoomOutAllButtonClicked(sender As Object, e As EventArgs)
            ZoomOutAll()
        End Sub
        Private Sub RemoveAllObjectsButtonClicked(sender As Object, e As EventArgs)
            ResetGraphObjects()
        End Sub
        Private Sub ExportGraphImageClicked(sender As Object, e As EventArgs)
            ExportDialog.ShowDialog(Me, Me)
        End Sub
        Private Function GetParentForm(c As Control) As Form
            If c.Parent Is Nothing Then Return Nothing
            If TypeOf c.Parent Is Form Then
                Return CType(c.Parent, Form)
            Else
                Return GetParentForm(c.Parent)
            End If
        End Function
        Private Sub ExportDataClicked(sender As Object, e As EventArgs)
            If (From s As Series In Series Select s.Datapoints.Count).Sum = 0 Then
                Dim pForm = GetParentForm(Me)
                If pForm IsNot Nothing Then
                    PopupTooltip.ShowPopup(pForm, 10, "No Data", "The chart contains no data to export.")
                End If
                Exit Sub
            End If
            ExportDataDialog.ShowDialog(Me, Me)
        End Sub

        Private Sub ContextOpening(sender As Object, e As CancelEventArgs)
            DrawCircleToolStripMenuItem.Enabled = EnableGraphObjects
            DrawLineToolStripMenuItem.Enabled = EnableGraphObjects
            RemoveAllObjectsToolStripMenuItem.Enabled = EnableGraphObjects

            'Try to find a graph object and kill it
            If EnableGraphObjects AndAlso Me.GraphObjects.Count > 0 Then
                Dim foundone As Boolean = False
                Dim loc As Point = LastContextOpenPosition - New Size(Me.InnerChartArea.Left, Me.InnerChartArea.Top)
                For i = 0 To Me.GraphObjects.Count - 1
                    Dim mhandles() As ObjectMouseHandle = Me.GraphObjects(i).GetMouseHandles(XAxis, If(Me.GraphObjects(i).YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis))
                    Dim selected As Integer = -1
                    For a = 0 To mhandles.Count - 1
                        If New RectangleF(CSng(mhandles(a).X - HandlesSize), CSng(mhandles(a).Y - HandlesSize), 2 * HandlesSize, 2 * HandlesSize).Contains(loc) Then
                            selected = a
                            Exit For
                        End If
                    Next
                    If selected <> -1 Then
                        'Found one, DIE
                        Me.GraphObjects.RemoveAt(i)
                        e.Cancel = True
                        Exit Sub
                    End If
                Next
            End If

            If (System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) And System.Windows.Input.KeyStates.Down) > 0 Then
                ZoomOutOnce()

                e.Cancel = True
                Exit Sub
            End If


        End Sub

#End Region

#Region "Fields"
        Private DPI As Single
        Private DPIScaling As Single

        Public DataToString As DatapointToString = Nothing

        Private CircleCursor As Cursor
        Private LineCursor As Cursor
        Private EraseCursor As Cursor

        Private HandlesSize As Integer = 13

        Private MovingObject As clsGraphObject
        Private MovingObjectHandleIndex As Integer

        Private isLegendExpanded As Boolean

        Private isUpdating As Boolean

        Public CancleMouseEvent As Boolean

        Public Function ShouldSerializeGraphObjects() As Boolean
            Return False
        End Function
        Public Sub ResetGraphObjects()
            If GraphObjects Is Nothing Then GraphObjects = New BindingList(Of clsGraphObject)
            GraphObjects.Clear()
            Me.Invalidate()
            RaiseEvent GraphObjectListChanged(Me, New ListChangedEventArgs(ListChangedType.Reset, -1))
        End Sub

        '   <System.ComponentModel.Browsable(False)>
        '<EditorBrowsable(EditorBrowsableState.Never)>
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        <DefaultValue(GetType(BindingList(Of Series)), Nothing)>
        Public WithEvents GraphObjects As BindingList(Of clsGraphObject)

        'Public Function ShouldSerializeSeries() As Boolean
        '    Return False
        'End Function
        'Public Sub ResetSeries()
        '    Series = New SeriesCollection(Me) 'BindingList(Of Series)
        'End Sub

        '    <System.ComponentModel.Browsable(False)>
        '    <EditorBrowsable(EditorBrowsableState.Never)>
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Private WithEvents _Series As SeriesCollection 'BindingList(Of Series)

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Public Property Series As SeriesCollection
            Get
                Return _Series
            End Get
            Set(value As SeriesCollection)
                _Series = value
            End Set
        End Property



        Public WithEvents Y1Axis As VerticalAxis
        Public WithEvents Y2Axis As VerticalAxis
        Public WithEvents XAxis As HorizontalAxis

        Private ZoomSteps As Stack(Of AxisBoundSet)

#Region "Marker Collections"
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Private WithEvents _HorizontalMarkers As HorizontalMarkerCollection

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Public Property HorizontalMarkers As HorizontalMarkerCollection
            Get
                Return _HorizontalMarkers
            End Get
            Set(value As HorizontalMarkerCollection)
                _HorizontalMarkers = value
            End Set
        End Property

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Private WithEvents _VerticalMarkers As VerticalMarkerCollection

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Public Property VerticalMarkers As VerticalMarkerCollection
            Get
                Return _VerticalMarkers
            End Get
            Set(value As VerticalMarkerCollection)
                _VerticalMarkers = value
            End Set
        End Property

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Private WithEvents _FreeMarkers As FreeMarkerCollection

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)>
        Public Property FreeMarkers As FreeMarkerCollection
            Get
                Return _FreeMarkers
            End Get
            Set(value As FreeMarkerCollection)
                _FreeMarkers = value
            End Set
        End Property
#End Region

        'These are used to handle the dragging of elements and zoom
        'Private MDragging As Boolean = False

        Private MDownPos As Point 'May change during mouse move
        Private MDownPosStatic As Point 'Will not be changed from MouseDown to MouseUp
        Private CurrentMDownMode As MDownMode
        Private DraggingMarker As Marker

        Private SecondMousePointForRect As Point
        Private isResizing As Boolean = False
        Private lastMouseMove As Date
        Private LastMousePosition As Point
        Private LastContextOpenPosition As Point

        'Verschoben oder gezoomt
        Private IsUserScaled As Boolean
        Private UserBounds As AxisBoundSet


#End Region

#Region "Autoscaling"
        Public Sub AutoScale()
            ZoomSteps.Clear()
            IsUserScaled = False
            DoAutoScale()
        End Sub
        Private Function GetMinX(data As BindingList(Of Datapoint)) As Double
            Dim dataVals() As Double = (From d As Datapoint In data Where d.IsOKVal Select d.X).ToArray
            If dataVals.Count > 0 Then Return dataVals.Min Else Return 0
        End Function
        Private Function GetMaxX(data As BindingList(Of Datapoint)) As Double
            Dim dataVals() As Double = (From d As Datapoint In data Where d.IsOKVal Select d.X).ToArray
            If dataVals.Count > 0 Then Return dataVals.Max Else Return 0
        End Function
        Private Function GetMinY(data As BindingList(Of Datapoint)) As Double
            Dim dataVals() As Double = (From d As Datapoint In data Where d.IsOKVal Select d.Y).ToArray
            If dataVals.Count > 0 Then Return dataVals.Min Else Return 0
        End Function
        Private Function GetMaxY(data As BindingList(Of Datapoint)) As Double
            Dim dataVals() As Double = (From d As Datapoint In data Where d.IsOKVal Select d.Y).ToArray
            If dataVals.Count > 0 Then Return dataVals.Max Else Return 0
        End Function

        Public Sub DoAutoScale()
            If isUpdating = True Or EnableAutoscaling = False Then Exit Sub
            If IsUserScaled = False Then
                Dim Xmin As Double = Double.PositiveInfinity
                Dim XMax As Double = Double.NegativeInfinity
                Dim Y1Min As Double = Double.PositiveInfinity
                Dim Y1Max As Double = Double.NegativeInfinity
                Dim Y2Min As Double = Double.PositiveInfinity
                Dim Y2Max As Double = Double.NegativeInfinity

                For Each s As Series In Me.Series
                    If s.Data.Count = 0 Then Continue For
                    If s.UseInAutoScale = False Then Continue For
                    If _SeriesExcludedFromScaling IsNot Nothing AndAlso Me._SeriesExcludedFromScaling.Contains(s.Name) Then Continue For
                    Dim thisMinX As Double = GetMinX(s.Data) 'CDbl(Aggregate d As Datapoint In s.Data Where d.IsOKVal Select d.X Into Min())
                    Dim thisMaxX As Double = GetMaxX(s.Data) 'CDbl(Aggregate d As Datapoint In s.Data Where d.IsOKVal Select d.X Into Max())
                    If thisMinX < Xmin Then Xmin = thisMinX
                    If thisMaxX > XMax Then XMax = thisMaxX
                    Dim thisMinY As Double = GetMinY(s.Data) 'CDbl(Aggregate d As Datapoint In s.Data Where d.IsOKVal Select d.Y Into Min())
                    Dim thisMaxY As Double = GetMaxY(s.Data) 'CDbl(Aggregate d As Datapoint In s.Data Where d.IsOKVal Select d.Y Into Max())
                    If s.YAxisType = Axis.enumAxisLocation.Primary Then
                        If thisMinY < Y1Min Then Y1Min = thisMinY
                        If thisMaxY > Y1Max Then Y1Max = thisMaxY
                    Else
                        If thisMinY < Y2Min Then Y2Min = thisMinY
                        If thisMaxY > Y2Max Then Y2Max = thisMaxY
                    End If
                Next
                If IncludeMarkersInScaling Then
                    For Each f As FreeMarker In _FreeMarkers
                        If f.YAxisType = Axis.enumAxisLocation.Primary Then
                            Y1Min = Math.Min(Y1Min, f.Y)
                            Y1Max = Math.Max(Y1Max, f.Y)
                        Else
                            Y2Min = Math.Min(Y2Min, f.Y)
                            Y2Max = Math.Max(Y2Max, f.Y)
                        End If
                        Xmin = Math.Min(Xmin, f.X)
                        XMax = Math.Max(XMax, f.X)
                    Next
                    For Each v As VerticalMarker In VerticalMarkers
                        Xmin = Math.Min(Xmin, v.X)
                        XMax = Math.Max(XMax, v.X)
                    Next
                    For Each h As HorizontalMarker In HorizontalMarkers
                        If h.YAxisType = Axis.enumAxisLocation.Primary Then
                            Y1Min = Math.Min(Y1Min, h.Y)
                            Y1Max = Math.Max(Y1Max, h.Y)
                        Else
                            Y2Min = Math.Min(Y2Min, h.Y)
                            Y2Max = Math.Max(Y2Max, h.Y)
                        End If
                    Next
                End If

                Dim deltaX As Double = XMax - Xmin
                Dim deltay As Double = Y1Max - Y1Min
                Dim deltay2 As Double = Y2Max - Y2Min
                If deltaX = 0 Then deltaX = Math.Abs(XMax)
                If deltay = 0 Then deltay = Math.Abs(Y1Max)
                If deltay2 = 0 Then deltay2 = Math.Abs(Y2Max)
                If Xmin = 0 AndAlso XMax = 0 Then Xmin = -1 : XMax = 1
                If Y1Min = 0 AndAlso Y1Max = 0 Then Y1Min = -1 : Y1Max = 1
                If Y2Min = 0 AndAlso Y2Max = 0 Then Y2Min = -1 : Y2Max = 1
                XAxis.Minimum = Xmin - AutoScaleBorder * deltaX
                XAxis.Maximum = XMax + AutoScaleBorder * deltaX
                Y1Axis.Minimum = Y1Min - AutoScaleBorder * 1 * deltay
                Y1Axis.Maximum = Y1Max + AutoScaleBorder * 1 * deltay
                Y2Axis.Minimum = Y2Min - AutoScaleBorder * 1.15 * deltay2
                Y2Axis.Maximum = Y2Max + AutoScaleBorder * 1.15 * deltay2
            Else
                XAxis.Minimum = UserBounds.XMin
                XAxis.Maximum = UserBounds.XMax
                Y1Axis.Minimum = UserBounds.Y1Min
                Y1Axis.Maximum = UserBounds.Y1Max
                Y2Axis.Minimum = UserBounds.Y2Min
                Y2Axis.Maximum = UserBounds.Y2Max
            End If

            If ScaleProportional Then DoScaleProportional()
            Me.Invalidate()
        End Sub
        Private Sub DoScaleProportional()
            ScaleAxesProportional(XAxis.Minimum, XAxis.Maximum, Y1Axis.Minimum, Y1Axis.Maximum, InnerChartArea.Width / InnerChartArea.Height)
        End Sub
#End Region

#Region "Painting"
        Private Sub DrawLegend(ByRef g As Graphics, Bounds As Rectangle, Series As IEnumerable(Of Series))
            Dim font As Font = New Font("Arial", 10)
            Dim fontTitle As Font = New Font("Arial", 10, FontStyle.Underline)
            Dim isExpanded As Boolean = isLegendExpanded Or LegendAlwaysVisible
            Using bmp As New Bitmap(Bounds.Width, Bounds.Height)
                Using tempg As Graphics = Graphics.FromImage(bmp)
                    'First draw the whole legend on a canvas (bmp)
                    'Measure it in the process
                    tempg.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
                    Dim typicalSize As SizeF = g.MeasureString("Asdf", font)
                    Dim y As Single = 5
                    Dim delta As Single = 3
                    Dim border As Single = 3
                    Dim title = If(Me.LegendTitle = "", "Legend", Me.LegendTitle)
                    Dim titlesize As SizeF = g.MeasureString(title, fontTitle)
                    tempg.DrawString(title, fontTitle, Brushes.Black, border, y)
                    If isExpanded = False Then ControlPaint.DrawSizeGrip(tempg, Color.White, New Rectangle(CInt(titlesize.Width + 10), CInt(border + 5), CInt(20 * DPIScaling), CInt(20 * DPIScaling)))
                    y += titlesize.Height + delta
                    Dim maxR As Single = titlesize.Width

                    'Don't draw and measure the rest of the legend if it's not expanded
                    If isExpanded Then
                        SetHQ(tempg)
                        For Each s As Series In Series
                            If s.IsVisibleInLegend = False Then Continue For
                            Dim thish As SizeF = tempg.MeasureString(s.LegendText, font)
                            Dim TheseBounds As New RectangleF(5, y, 20, thish.Height)
                            s.PaintLegend(tempg, TheseBounds)
                            tempg.DrawString(s.LegendText, font, Brushes.Black, 25, y)
                            maxR = Math.Max(thish.Width, maxR)
                            y += typicalSize.Height + delta
                        Next
                    End If

                    Using legendbmp As New Bitmap(CInt(maxR + 5 + 20 + 2 * border), CInt(y - 5 + 2 * border))
                        Using legendg As Graphics = Graphics.FromImage(legendbmp)
                            'Create a new temporary canvas and paint the legend on it
                            legendg.FillRectangle(Brushes.White, 0, 0, legendbmp.Width, legendbmp.Height)
                            legendg.DrawRectangle(Pens.Black, 0, 0, legendbmp.Width - 1, legendbmp.Height - 1)
                            legendg.DrawImageUnscaled(bmp, 0, 0)
                        End Using
                        'Paint this canvas at the wanted position
                        Select Case Me.LegendPosition
                            Case enumLegendPosition.TopLeft
                                g.DrawImageUnscaled(legendbmp, 2, 2)
                            Case enumLegendPosition.Top
                                g.DrawImageUnscaled(legendbmp, Math.Max(2, CInt(Bounds.Width / 2 - legendbmp.Width / 2)), 2)
                            Case enumLegendPosition.TopRight
                                g.DrawImageUnscaled(legendbmp, Math.Max(2, Bounds.Width - legendbmp.Width - 2), 2)
                            Case enumLegendPosition.Left
                                g.DrawImageUnscaled(legendbmp, 2, Math.Max(2, CInt(Bounds.Height / 2 - legendbmp.Height / 2)))
                            Case enumLegendPosition.Right
                                g.DrawImageUnscaled(legendbmp, Math.Max(2, Bounds.Width - legendbmp.Width - 2), Math.Max(2, CInt(Bounds.Height / 2 - legendbmp.Height / 2)))
                            Case enumLegendPosition.BottomLeft
                                g.DrawImageUnscaled(legendbmp, 2, Math.Max(2, Bounds.Height - legendbmp.Height - 2))
                            Case enumLegendPosition.Bottom
                                g.DrawImageUnscaled(legendbmp, Math.Max(2, CInt(Bounds.Width / 2 - legendbmp.Width / 2)), Math.Max(2, Bounds.Height - legendbmp.Height - 2))
                            Case enumLegendPosition.BottomRight
                                g.DrawImageUnscaled(legendbmp, Math.Max(2, Bounds.Width - legendbmp.Width - 2), Math.Max(2, Bounds.Height - legendbmp.Height - 2))
                        End Select
                        'g.DrawImageUnscaled(legendbmp, Bounds.Width - legendbmp.Width - 2, 2)

                    End Using

                End Using
            End Using
        End Sub
        Private Sub DrawZoomRectangle(ByRef g As Graphics, Bounds As Rectangle)
            'Draw the zoom rectangle
            If CurrentMDownMode = MDownMode.Zoom Then
                Dim p1 As Point = MDownPos - New Size(Bounds.X, Bounds.Y)
                Dim p2 As Point = SecondMousePointForRect - New Size(Bounds.X, Bounds.Y)
                Dim r As Rectangle = New Rectangle(Min(p1.X, p2.X), Min(p1.Y, p2.Y), Abs(p1.X - p2.X), Abs(p1.Y - p2.Y))

                Dim c As Color = Color.DodgerBlue
                Dim b As New SolidBrush(Color.FromArgb(75, c.R, c.G, c.B))
                g.FillRectangle(b, r)
                g.DrawRectangle(Pens.RoyalBlue, r)
                b.Dispose()
            End If
        End Sub
        Private Sub DrawMouseEvents(ByRef g As Graphics, Bounds As Rectangle)
            DrawZoomRectangle(g, Bounds)
        End Sub
        Private Sub DrawMarkers(ByRef g As Graphics, Bounds As Rectangle)
            Dim xAxisOK, PrimaryOK, SecondaryOK As Boolean
            CheckAxes(xAxisOK, PrimaryOK, SecondaryOK)
            If VerticalMarkers IsNot Nothing Then
                For Each v As VerticalMarker In Me.VerticalMarkers
                    If v Is Nothing Then Continue For
                    If v.YAxisType = Axis.enumAxisLocation.Primary AndAlso Not PrimaryOK Then Continue For
                    If v.YAxisType = Axis.enumAxisLocation.Secondary AndAlso Not SecondaryOK Then Continue For

                    Try
                        Using MarkerPen As New Pen(v.Color, _MarkerLineWidth)
                            v.Paint(g, XAxis, Y1Axis, MarkerPen, Bounds)
                        End Using
                    Catch ex As Exception
                        Debug.WriteLine("Error in drawing a vertical marker: " & GetCompleteExceptionMessage(ex))
                        hasDrawingError = True
                        DrawingErrors.Add("Vertical marker " & Me.VerticalMarkers.IndexOf(v))
                    End Try
                Next
            End If
            If HorizontalMarkers IsNot Nothing Then
                For Each h As HorizontalMarker In Me.HorizontalMarkers
                    If h Is Nothing Then Continue For
                    If Not xAxisOK Then Continue For

                    Try
                        Using MarkerPen As New Pen(h.Color, _MarkerLineWidth)
                            h.Paint(g, XAxis, If(h.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), MarkerPen, Bounds)
                        End Using
                    Catch ex As Exception
                        Debug.WriteLine("Error in drawing a horizontal marker: " & GetCompleteExceptionMessage(ex))
                        hasDrawingError = True
                        DrawingErrors.Add("Horizontal marker " & Me.HorizontalMarkers.IndexOf(h))
                    End Try

                Next
            End If

            If _FreeMarkers IsNot Nothing Then
                For Each f As FreeMarker In Me._FreeMarkers
                    If f Is Nothing Then Continue For
                    If Not xAxisOK Then Continue For
                    If f.YAxisType = Axis.enumAxisLocation.Primary AndAlso Not PrimaryOK Then Continue For
                    If f.YAxisType = Axis.enumAxisLocation.Secondary AndAlso Not SecondaryOK Then Continue For
                    Try
                        Using MarkerPen As New Pen(f.Color, _MarkerLineWidth)
                            f.Paint(g, XAxis, If(f.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), MarkerPen, Bounds)
                        End Using
                    Catch ex As Exception
                        Debug.WriteLine("Error in drawing a free marker: " & GetCompleteExceptionMessage(ex))
                        hasDrawingError = True
                        DrawingErrors.Add("Free marker " & Me._FreeMarkers.IndexOf(f))
                    End Try

                Next
            End If

        End Sub

        Private Sub CheckAxes(ByRef XAxisOK As Boolean, ByRef PrimaryOK As Boolean, ByRef SecondaryOK As Boolean)
            With Me.XAxis
                XAxisOK = Not (Double.IsInfinity(.Minimum) OrElse Double.IsNaN(.Minimum) OrElse
                Double.IsInfinity(.Maximum) OrElse Double.IsNaN(.Maximum))
            End With
            With Me.Y1Axis
                PrimaryOK = Not (Double.IsInfinity(.Minimum) OrElse Double.IsNaN(.Minimum) OrElse
                Double.IsInfinity(.Maximum) OrElse Double.IsNaN(.Maximum))
            End With
            With Me.Y2Axis
                SecondaryOK = Not (Double.IsInfinity(.Minimum) OrElse Double.IsNaN(.Minimum) OrElse
                Double.IsInfinity(.Maximum) OrElse Double.IsNaN(.Maximum))
            End With

        End Sub

        Private Sub DrawGraphObjects(ByRef g As Graphics, Bounds As Rectangle)
            Dim xAxisOK, PrimaryOK, SecondaryOK As Boolean
            CheckAxes(xAxisOK, PrimaryOK, SecondaryOK)

            If Not xAxisOK Then Exit Sub
            For Each o As clsGraphObject In Me.GraphObjects
                Try
                    If o.YAxisType = Axis.enumAxisLocation.Primary AndAlso Not PrimaryOK Then Continue For
                    If o.YAxisType = Axis.enumAxisLocation.Secondary AndAlso Not SecondaryOK Then Continue For

                    o.Paint(g, XAxis, If(o.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), Bounds)
                    Dim mhandles() As ObjectMouseHandle = o.GetMouseHandles(XAxis, If(o.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis))

                    For i = 0 To mhandles.Count - 1
                        If o.SelectedIndizes IsNot Nothing AndAlso o.SelectedIndizes.Contains(i) Then
                            mhandles(i).IsSelected = True
                        Else
                            mhandles(i).IsSelected = False
                        End If
                        mhandles(i).Paint(g, HandlesSize)
                    Next
                Catch ex As Exception
                    Debug.WriteLine("Error in drawing a graph object: " & GetCompleteExceptionMessage(ex))
                    hasDrawingError = True
                    DrawingErrors.Add("Graph object " & Me.GraphObjects.IndexOf(o))
                End Try

            Next
        End Sub

        Private Function GetNewBounds(image As Bitmap, oldsize As Size, maxWidth As Integer, maxHeight As Integer) As Size
            maxWidth = Math.Min(oldsize.Width, maxWidth)
            maxHeight = Math.Min(oldsize.Height, maxHeight)
            Dim ratioX As Double = CDbl(maxWidth) / image.Width
            Dim ratioY As Double = CDbl(maxHeight) / image.Height
            Dim ratio As Double = Math.Min(ratioX, ratioY)

            Dim newWidth As Integer = CInt(image.Width * ratio)
            Dim newHeight As Integer = CInt(image.Height * ratio)
            Return New Size(newWidth, newHeight)
        End Function

        Public Event Y1AxisDoubleClicked(sender As Object, e As EventArgs)
        Public Event Y2AxisDoubleClicked(sender As Object, e As EventArgs)
        Public Event XAxisDoubleClicked(sender As Object, e As EventArgs)

        Private Sub jwGraph_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If _ExportDialog IsNot Nothing Then _ExportDialog.Dispose()
            If _ExportDataDialog IsNot Nothing Then _ExportDataDialog.Dispose()
        End Sub

        Private Sub jwGraph_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
            Dim e1 As MouseEventArgs = CType(e, MouseEventArgs)
            Dim y1rect As New Rectangle(InnerChartArea.Left - Me.GraphBorder.Left, InnerChartArea.Top, Me.GraphBorder.Left, InnerChartArea.Height)
            If y1rect.Contains(e1.Location) Then
                RaiseEvent Y1AxisDoubleClicked(Me, New EventArgs)
                Exit Sub
            End If
            Dim y2rect As New Rectangle(InnerChartArea.Right, InnerChartArea.Top, Me.GraphBorder.Right, InnerChartArea.Height)
            If y2rect.Contains(e1.Location) Then
                RaiseEvent Y2AxisDoubleClicked(Me, New EventArgs)
                Exit Sub
            End If

            Dim xrect As New Rectangle(InnerChartArea.Left, InnerChartArea.Bottom, InnerChartArea.Width, Me.GraphBorder.Bottom)
            If xrect.Contains(e1.Location) Then
                RaiseEvent XAxisDoubleClicked(Me, New EventArgs)
                Exit Sub
            End If
        End Sub

        Private hasDrawingError As Boolean
        Private DrawingErrors As List(Of String)

        Private Sub SetHQ(g As Graphics)
            If HighQuality Then
                g.SmoothingMode = SmoothingMode.HighQuality
            Else
                g.SmoothingMode = SmoothingMode.HighSpeed
            End If
        End Sub


        Private Sub PaintGraph(ByRef eGraphics As Graphics, DrawBounds As Rectangle)
            Try
                Dim bounds = Rectangle.Inflate(DrawBounds, 1, 1)
                hasDrawingError = False
                DrawingErrors = New List(Of String)
                SetHQ(eGraphics)

                'Draw Background
                Try
                    bounds.Inflate(1, 1)
                    Using s As New LinearGradientBrush(New Point(0, 0), New Point(bounds.Width, bounds.Height), TopLeftColor, BottomRightColor)
                        eGraphics.FillRectangle(s, bounds)
                    End Using
                Catch ex As Exception
                    Debug.WriteLine("Error in background drawing: " & GetCompleteExceptionMessage(ex))
                    hasDrawingError = True
                    DrawingErrors.Add("Background")
                End Try

                'Draw Inner Chart
                Dim InnerChartArea As Rectangle = GetInnerChartArea(bounds)
                Try
                    Using s As New SolidBrush(Me.GraphBackColor)
                        eGraphics.FillRectangle(s, InnerChartArea)
                    End Using
                Catch ex As Exception
                    Debug.WriteLine("Error in inner chart drawing: " & GetCompleteExceptionMessage(ex))
                    hasDrawingError = True
                    DrawingErrors.Add("Inner chart")
                End Try

                'Axes look crisper without antialiasing
                eGraphics.SmoothingMode = SmoothingMode.HighSpeed
                'Draw axes
                Using AxisPen As Pen = New Pen(Brushes.Black, 2) With {.Alignment = PenAlignment.Center}
                    Dim HasPrimarySeries As Boolean = False
                    Dim HasSecondarySeries As Boolean = False
                    Dim hasData As Boolean = False
                    Using gridPen As New Pen(Color.Silver)
                        gridPen.DashStyle = DashStyle.Custom
                        gridPen.DashPattern = {3.0F, 3.0F}
                        'Find out if there is anything to draw
                        If Series IsNot Nothing Then
                            For Each s As Series In Series
                                If s Is Nothing Then Continue For
                                If s.YAxisType = Axis.enumAxisLocation.Primary Then HasPrimarySeries = True
                                If s.YAxisType = Axis.enumAxisLocation.Secondary Then HasSecondarySeries = True
                                If s.Data.Count > 0 Then hasData = True
                            Next
                            If (HorizontalMarkers.Count > 0 OrElse _FreeMarkers.Count > 0) AndAlso IncludeMarkersInScaling Then hasData = True
                        End If

                        Try
                            If Y1Axis IsNot Nothing AndAlso HasPrimarySeries Then
                                Y1Axis.Paint(eGraphics, AxisPen, gridPen, InnerChartArea.Top, InnerChartArea.Bottom, InnerChartArea.Left, InnerChartArea.Right)
                            Else
                                eGraphics.DrawLine(AxisPen, InnerChartArea.Left, InnerChartArea.Top, InnerChartArea.Left, InnerChartArea.Bottom)
                            End If
                        Catch ex As Exception
                            Debug.WriteLine("Error in Y1 axis drawing: " & GetCompleteExceptionMessage(ex))
                            hasDrawingError = True
                            DrawingErrors.Add("Y1 axis")
                        End Try

                        Try
                            If Y2Axis IsNot Nothing AndAlso HasSecondarySeries Then
                                Y2Axis.Paint(eGraphics, AxisPen, gridPen, InnerChartArea.Top, InnerChartArea.Bottom, InnerChartArea.Right, InnerChartArea.Left)
                            Else
                                eGraphics.DrawLine(AxisPen, InnerChartArea.Right, InnerChartArea.Top, InnerChartArea.Right, InnerChartArea.Bottom)
                            End If
                        Catch ex As Exception
                            Debug.WriteLine("Error in Y2 axis drawing: " & GetCompleteExceptionMessage(ex))
                            hasDrawingError = True
                            DrawingErrors.Add("Y2 axis")
                        End Try

                        Try
                            If XAxis IsNot Nothing AndAlso hasData Then
                                XAxis.Paint(eGraphics, AxisPen, gridPen, InnerChartArea.Left, InnerChartArea.Right, InnerChartArea.Bottom, InnerChartArea.Top)
                            Else
                                eGraphics.DrawLine(AxisPen, InnerChartArea.Left, InnerChartArea.Bottom, InnerChartArea.Right, InnerChartArea.Bottom)
                            End If
                        Catch ex As Exception
                            Debug.WriteLine("Error in X axis drawing: " & GetCompleteExceptionMessage(ex))
                            hasDrawingError = True
                            DrawingErrors.Add("X axis")
                        End Try
                    End Using
                    'Close axes rectangle
                    eGraphics.DrawLine(AxisPen, InnerChartArea.Left, InnerChartArea.Top, InnerChartArea.Right, InnerChartArea.Top)
                    SetHQ(eGraphics)


                    If isUpdating Then Exit Sub

                    Dim XOK As Boolean, Y1OK As Boolean, Y2OK As Boolean
                    CheckAxes(XOK, Y1OK, Y2OK)

                    If InnerChartArea.Width < 1 OrElse InnerChartArea.Height < 1 Then Exit Sub
                    'Create a new canvas in the bounds of the inner area, to avoid drawing around it
                    Using bmp As New Bitmap(InnerChartArea.Width, InnerChartArea.Height)
                        Using g As Graphics = Graphics.FromImage(bmp)

                            SetHQ(g)

                            If CenterImage IsNot Nothing AndAlso Not hasData Then
                                'No data-->Draw center image
                                Try
                                    Dim maxWidth As Integer = InnerChartArea.Width
                                    Dim maxHeight As Integer = InnerChartArea.Height

                                    Dim newsize As Size = GetNewBounds(CenterImage, CenterImageMaxSize, maxWidth, maxHeight)
                                    g.DrawImage(CenterImage, CInt(InnerChartArea.Width / 2 - newsize.Width / 2) + 1, CInt(InnerChartArea.Height / 2 - newsize.Height / 2) + 1, newsize.Width - 2, newsize.Height - 2)
                                Catch ex As Exception
                                    Debug.WriteLine("Error in center image drawing: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Center image")
                                End Try

                            ElseIf hasData Then

                                'Draw origin lines if selected
                                'These look crisper without AA as well
                                g.SmoothingMode = SmoothingMode.HighSpeed
                                Try
                                    If Y1Axis IsNot Nothing AndAlso Y1OK AndAlso HasPrimarySeries AndAlso Y1Axis.ShowOrigin Then
                                        Dim y As Single = CSng(Y1Axis.ValueToPixelPosition(0))
                                        If IsOK(CDbl(y)) AndAlso y >= 0 AndAlso y <= bmp.Height Then
                                            Using p As New Pen(Y1Axis.OriginColor) With {.DashStyle = DashStyle.Dot}
                                                g.DrawLine(p, 0, y, InnerChartArea.Width, y)
                                            End Using
                                        End If

                                    End If
                                Catch ex As Exception
                                    Debug.WriteLine("Error in Y1 origin drawing: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Y1 axis origin")
                                End Try

                                Try
                                    If Y2Axis IsNot Nothing AndAlso Y2OK AndAlso HasSecondarySeries AndAlso Y2Axis.ShowOrigin Then
                                        Dim y As Single = CSng(Y2Axis.ValueToPixelPosition(0))
                                        If IsOK(CDbl(y)) AndAlso y >= 0 AndAlso y <= bmp.Height Then
                                            Using p As New Pen(Y2Axis.OriginColor) With {.DashStyle = DashStyle.Dot}
                                                g.DrawLine(p, 0, y, InnerChartArea.Width, y)
                                            End Using
                                        End If
                                    End If
                                Catch ex As Exception
                                    Debug.WriteLine("Error in Y2 origin drawing: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Y2 axis origin")
                                End Try

                                Try
                                    If XAxis IsNot Nothing AndAlso XOK AndAlso XAxis.ShowOrigin Then
                                        Dim x As Single = CSng(XAxis.ValueToPixelPosition(0))
                                        If IsOK(CDbl(x)) AndAlso x >= 0 AndAlso x <= bmp.Width Then
                                            Using p As New Pen(XAxis.OriginColor) With {.DashStyle = DashStyle.Dot}
                                                g.DrawLine(p, x, 0, x, InnerChartArea.Height)
                                            End Using
                                        End If
                                    End If
                                Catch ex As Exception
                                    Debug.WriteLine("Error in X origin drawing: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("X axis origin")
                                End Try
                                SetHQ(g)

                                'Draw message
                                If Message <> "" Then
                                    Try
                                        Dim MessagePosition As RectangleF
                                        Using sf As New Drawing.StringFormat
                                            Dim MsgSize = g.MeasureString(Me.Message, Me.Font, CInt(Me.Width / 3), sf)
                                            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
                                            If LegendPosition = enumLegendPosition.TopLeft Then
                                                MessagePosition = New RectangleF(Me.InnerChartArea.Width - 5 - MsgSize.Width, 5, MsgSize.Width, MsgSize.Height)
                                                Dim BorderPosition = New RectangleF(MessagePosition.X, MessagePosition.Y, MessagePosition.Width, MessagePosition.Height)
                                                BorderPosition.Inflate(3, 3)
                                                Using b As New SolidBrush(Me.GraphBackColor)
                                                    g.FillRectangle(b, BorderPosition)
                                                    g.DrawRectangles(Pens.Black, {BorderPosition})
                                                End Using
                                                Using b As New SolidBrush(MessageColor)
                                                    g.DrawString(Message, Me.Font, b, MessagePosition, sf)
                                                End Using
                                            Else
                                                MessagePosition = New RectangleF(5, 5, MsgSize.Width, MsgSize.Height)
                                                Dim BorderPosition = New RectangleF(MessagePosition.X, MessagePosition.Y, MessagePosition.Width, MessagePosition.Height)
                                                BorderPosition.Inflate(3, 3)
                                                Using b As New SolidBrush(Me.GraphBackColor)
                                                    g.FillRectangle(b, BorderPosition)
                                                    g.DrawRectangles(Pens.Black, {BorderPosition})
                                                End Using
                                                Using b As New SolidBrush(MessageColor)
                                                    g.DrawString(Message, Me.Font, b, MessagePosition, sf)
                                                End Using
                                            End If
                                        End Using
                                    Catch ex As Exception
                                        Debug.WriteLine("Error in drawing the message: " & GetCompleteExceptionMessage(ex))
                                        hasDrawingError = True
                                        DrawingErrors.Add("Message")
                                    End Try
                                End If

                                'Draw Data
                                Dim dataCount As Integer = 0
                                If Series IsNot Nothing AndAlso XOK Then
                                    For Each s As Series In Series
                                        Try
                                            If s Is Nothing Then Continue For
                                            If s.YAxisType = Axis.enumAxisLocation.Primary AndAlso Not Y1OK Then Continue For
                                            If s.YAxisType = Axis.enumAxisLocation.Secondary AndAlso Not Y2OK Then Continue For
                                            dataCount += s.Data.Count
                                            s.Paint(g, Me.XAxis, If(s.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), InnerChartArea)
                                        Catch ex As Exception
                                            Dim faultyseries As Integer = Series.IndexOf(s)
                                            Debug.WriteLine("Error in drawing data for series " & faultyseries.ToString & ": " & GetCompleteExceptionMessage(ex))
                                            hasDrawingError = True
                                            DrawingErrors.Add("Series " & faultyseries.ToString)
                                        End Try
                                    Next
                                End If


                                'Draw markers, checks if axes are ok
                                If EnableMarkers Then DrawMarkers(g, InnerChartArea)

                                'Draw graph objects
                                If EnableGraphObjects AndAlso XAxis.IsAxisOK Then DrawGraphObjects(g, InnerChartArea)

                                'Draw Mouse stuff
                                Try
                                    DrawMouseEvents(g, InnerChartArea)
                                Catch ex As Exception
                                    Debug.WriteLine("Error in drawing mouse events: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Mouse events")
                                End Try

                                'Draw legend
                                Try
                                    If EnableLegend Then DrawLegend(g, InnerChartArea, Series)
                                Catch ex As Exception
                                    Debug.WriteLine("Error in drawing the legend: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Legend")
                                End Try

                                'Draw the datapoint marker
                                Try
                                    If DoShowDatapoint Then

                                        Dim size As SizeF = g.MeasureString(DataPointText, New Font("Lucida Console", 10))
                                        Dim border As Integer = 5
                                        size.Width += border * 2
                                        size.Height += border * 2

                                        Dim x As Integer = Math.Max(CInt(DataPointPosition.X - size.Width / 2), 3)
                                        Dim y As Integer = CInt(DataPointPosition.Y - size.Height - 25)
                                        If y < 0 Then
                                            y = CInt(DataPointPosition.Y + 25)
                                            g.DrawLine(Pens.MediumBlue, DataPointPosition.X, DataPointPosition.Y, CInt(x + size.Width / 2), y)
                                        Else
                                            g.DrawLine(Pens.MediumBlue, DataPointPosition.X, DataPointPosition.Y, CInt(x + size.Width / 2), y + size.Height)
                                        End If
                                        g.FillRectangle(Brushes.White, x, y, size.Width, size.Height)
                                        ControlPaint.DrawBorder3D(g, New Rectangle(x, y, CInt(size.Width), CInt(size.Height)))
                                        'GeneralTools.DrawGradientRectangle(g, New Rectangle(x, y, CInt(size.Width), CInt(size.Height)), 5, Color.DarkBlue, Color.White)
                                        g.DrawString(DataPointText, New Font("Lucida Console", 10), Brushes.Black, x + 5, y + 5)
                                    End If
                                Catch ex As Exception
                                    Debug.WriteLine("Error in drawing a datapoint marker: " & GetCompleteExceptionMessage(ex))
                                    hasDrawingError = True
                                    DrawingErrors.Add("Datapoint marker")
                                End Try
                            End If
                            If hasDrawingError Then
                                g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
                                g.DrawString("Drawing error(s):" & vbCrLf & Strings.Join(DrawingErrors.ToArray, vbCrLf), New Font("Arial", 8), Brushes.Black, 5, 5)
                            End If
                        End Using
                        eGraphics.DrawImageUnscaled(bmp, InnerChartArea.Location)
                        '  eGraphics.DrawString(timers.ToString, New Font("Arial", 8), Brushes.Black, New Point(120, 30))
                    End Using
                End Using

                'et.StopTimer()
                'e.Graphics.DrawString(PaintCount.ToString & "; " & et.ToString, New Font("Arial", 10), Brushes.Black, New PointF(3, 3))
            Catch ex As Exception
                Debug.WriteLine("General unhandled error in drawing: " & GeneralTools.GetCompleteExceptionMessage(ex))
            End Try
        End Sub

        Private Sub jwGraph_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
            PaintGraph(e.Graphics, Me.ClientRectangle)
        End Sub

        Public Function GetGraphImage(Size As Size, FixHeightInsteadOfWidth As Boolean) As Bitmap

            If Me.ScaleProportional Then
                If Not FixHeightInsteadOfWidth Then
                    Size.Height = CInt(Size.Width / Me.Width * Me.Height)
                Else
                    Size.Width = CInt(Me.Width * Size.Height / Me.Height)
                End If
            End If

            Dim res As New Bitmap(Size.Width, Size.Height)
            Using g As Graphics = Graphics.FromImage(res)
                Dim rect As Rectangle = New Rectangle(0, 0, res.Width, res.Height)
                OverrideChartAreaRectangle = rect
                DoOverrideChartArea = True
                PaintGraph(g, rect)
                DoOverrideChartArea = False
            End Using
            Return res
        End Function

#End Region

#Region "Event Handlers"
        Private Sub SeriesChanged(sender As Object, e As PropertyChangedEventArgs)
            DoAutoScale()
        End Sub

        Private Sub Series_ListChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles _Series.CollectionChanged
            If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
                For Each i As Series In e.NewItems
                    AddHandler i.PropertyChanged, AddressOf SeriesChanged
                Next
            End If
            DoAutoScale()
        End Sub

        Private Sub Marker_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles _FreeMarkers.CollectionChanged, _HorizontalMarkers.CollectionChanged, _VerticalMarkers.CollectionChanged
            If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
                If TypeOf sender Is FreeMarkerCollection Then
                    For Each i In e.NewItems
                        AddHandler CType(i, FreeMarker).PropertyChanged, AddressOf MarkerChanged
                    Next
                ElseIf TypeOf sender Is VerticalMarkerCollection Then
                    For Each i In e.NewItems
                        AddHandler CType(i, VerticalMarker).PropertyChanged, AddressOf MarkerChanged
                    Next
                ElseIf TypeOf sender Is HorizontalMarkerCollection Then
                    For Each i In e.NewItems
                        AddHandler CType(i, HorizontalMarker).PropertyChanged, AddressOf MarkerChanged
                    Next
                End If
            End If
            DoAutoScale()
        End Sub

        Private Sub GraphObjects_ListChanged(sender As Object, e As ListChangedEventArgs) Handles GraphObjects.ListChanged
            Me.Invalidate()
            RaiseEvent GraphObjectListChanged(Me, e)

        End Sub

        Private Sub MarkerChanged(sender As Object, e As PropertyChangedEventArgs)
            DoAutoScale()
        End Sub

        Private Sub XAxis_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles XAxis.PropertyChanged
            Me.Invalidate()
        End Sub

        Private Sub PrimaryYAxis_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Y1Axis.PropertyChanged
            Me.Invalidate()
        End Sub

        Private Sub SecondaryYAxis_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Y2Axis.PropertyChanged
            Me.Invalidate()
        End Sub

        Private Sub jwGraph_Resize(sender As Object, e As EventArgs)
            DoAutoScale()
        End Sub
#End Region

#Region "Mouse events, Zooming, Moving"
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            'Bubble the event
            CancleMouseEvent = False
            RaiseEvent MouseDown(Me, e)
            MyBase.OnMouseDown(e)
            If CancleMouseEvent = True Then Exit Sub

            'Mögliche Linke Maus Aktionen:
            '1. Zoom Rechteck
            '2. Kreis erstellen
            '3. Linie erstellen
            '4. Bewegen

            MDownPos = e.Location
            MDownPosStatic = e.Location
            If e.Button = System.Windows.Forms.MouseButtons.Left AndAlso LeftMouseFunctionalityEnabled Then

                'First check if the user clicked on a marker or a graph object's handle. This has top priority
                Dim checkLocation As Point = e.Location
                'Test if a marker is hit
                If EnableMarkers Then
                    For Each m As Marker In _FreeMarkers
                        If m.HitTest(checkLocation, XAxis, If(m.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), InnerChartArea) = True Then
                            DraggingMarker = m
                            CurrentMDownMode = MDownMode.Marker
                            Exit Sub
                        End If
                    Next
                    For Each m As Marker In VerticalMarkers
                        If m.HitTest(checkLocation, XAxis, If(m.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), InnerChartArea) = True Then
                            DraggingMarker = m
                            CurrentMDownMode = MDownMode.Marker
                            Exit Sub
                        End If
                    Next
                    For Each m As Marker In HorizontalMarkers
                        If m.HitTest(checkLocation, XAxis, If(m.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), InnerChartArea) = True Then
                            DraggingMarker = m
                            CurrentMDownMode = MDownMode.Marker
                            Exit Sub
                        End If
                    Next
                End If

                'Check if objects are to be moved
                If EnableGraphObjects AndAlso Me.GraphObjects.Count > 0 Then
                    Dim foundone As Boolean = False
                    Dim loc As Point = e.Location - New Size(Me.InnerChartArea.Left, Me.InnerChartArea.Top)
                    For i = Me.GraphObjects.Count - 1 To 0 Step -1
                        Dim mhandles() As ObjectMouseHandle = Me.GraphObjects(i).GetMouseHandles(XAxis, If(Me.GraphObjects(i).YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis))
                        Dim selected As Integer = -1
                        For a = 0 To mhandles.Count - 1
                            If New RectangleF(CSng(mhandles(a).X - HandlesSize), CSng(mhandles(a).Y - HandlesSize), 2 * HandlesSize, 2 * HandlesSize).Contains(loc) Then
                                selected = a
                                Exit For
                            End If
                        Next
                        If selected <> -1 Then
                            Me.MovingObject = Me.GraphObjects(i)
                            MovingObjectHandleIndex = selected
                            foundone = True
                            Exit For
                        End If
                    Next
                    If foundone Then
                        CurrentMDownMode = MDownMode.ObjectMove
                        Exit Sub
                    End If
                End If

                'Check if datapoint is hit
                Dim d As Datapoint = Nothing
                Dim ser As Series = Nothing
                Dim HitTestOK = PointHitTest(e.Location, ser, 0, d)
                If Not (EnableGraphObjects AndAlso (LeftMouseAction = enLeftMouseAction.CreateLine Or LeftMouseAction = enLeftMouseAction.CreateCircle)) AndAlso
                    HitTestOK AndAlso LeftMouseAction <> enLeftMouseAction.ErasePoint Then
                    ShowDatapoint(d, ser, e.Location)
                    Exit Sub
                ElseIf HitTestOK AndAlso LeftMouseAction = enLeftMouseAction.ErasePoint
                    RaiseEvent PointToBeErased(Me, New PointToEraseEventArgs(d, ser))
                End If


                If LeftMouseAction = enLeftMouseAction.ZoomIn Then
                    'No marker is hit, initiate Zoom
                    SecondMousePointForRect = e.Location
                    CurrentMDownMode = MDownMode.Zoom
                ElseIf EnableGraphObjects AndAlso LeftMouseAction = enLeftMouseAction.CreateLine Then
                    Dim ypos As Double = Y1Axis.PixelPositionToValue(e.Y)

                    Dim l As LineObject
                    If ypos >= 0 Then
                        l = New LineObject(0, 0, 1, 1, clsGraphObject.enClipArea.Above0)
                    Else
                        l = New LineObject(0, 0, 1, 1, clsGraphObject.enClipArea.Under0)
                    End If
                    l.Name = "Line (#" & Me.GraphObjects.Count + 1 & ")"
                    GraphObjects.Add(l)
                    MovingObject = l
                    CurrentMDownMode = MDownMode.CreateObject

                ElseIf EnableGraphObjects AndAlso LeftMouseAction = enLeftMouseAction.CreateCircle Then
                    Dim xpos As Double = XAxis.PixelPositionToValue(e.X)
                    Dim ypos As Double = Y1Axis.PixelPositionToValue(e.Y)

                    Dim l As CircleObject
                    If ypos >= 0 Then
                        l = New CircleObject(xpos, 0, Math.Abs(ypos), clsGraphObject.enClipArea.Above0)
                    Else
                        l = New CircleObject(xpos, 0, Math.Abs(ypos), clsGraphObject.enClipArea.Under0)
                    End If
                    l.Name = "Circle (#" & Me.GraphObjects.Count + 1 & ")"
                    GraphObjects.Add(l)
                    MovingObject = l
                    CurrentMDownMode = MDownMode.CreateObject
                ElseIf LeftMouseAction = enLeftMouseAction.MoveGraph Then
                    'Initiate moving of graph
                    CurrentMDownMode = MDownMode.Move
                End If

            End If

            If e.Button = System.Windows.Forms.MouseButtons.Middle AndAlso MiddleMouseFunctionalityEnabled Then
                'Initiate moving of graph
                CurrentMDownMode = MDownMode.Move
            End If

            If e.Button = System.Windows.Forms.MouseButtons.Right AndAlso RightMouseFunctionalityEnabled Then
                LastContextOpenPosition = e.Location
            End If
        End Sub


        Private Sub ZoomOutOnce()
            If ZoomSteps.Count > 1 Then
                Dim b As AxisBoundSet = ZoomSteps.Pop
                XAxis.Minimum = b.XMin
                XAxis.Maximum = b.XMax
                Y1Axis.Minimum = b.Y1Min
                Y1Axis.Maximum = b.Y1Max
                Y2Axis.Minimum = b.Y2Min
                Y2Axis.Maximum = b.Y2Max
            Else
                ZoomOutAll()
            End If
        End Sub
        Private Sub ZoomOutAll()
            ZoomSteps.Clear()
            IsUserScaled = False
            DoAutoScale()
        End Sub
        Private Function GetLegendBounds() As Rectangle
            Dim LegendSize As Size = New Size(81, 26)
            Dim res As Rectangle
            Select Case LegendPosition
                Case enumLegendPosition.TopLeft
                    res = (New Rectangle(2, 2, LegendSize.Width, LegendSize.Height))
                Case enumLegendPosition.Top
                    res = New Rectangle(Math.Max(2, CInt(InnerChartArea.Width / 2 - LegendSize.Width / 2)), 2, LegendSize.Width, LegendSize.Height)
                Case enumLegendPosition.TopRight
                    res = New Rectangle(Math.Max(2, CInt(InnerChartArea.Width - 2 - LegendSize.Width)), 2, LegendSize.Width, LegendSize.Height)
                Case enumLegendPosition.Left
                    res = New Rectangle(2, Math.Max(2, CInt(InnerChartArea.Height / 2 - LegendSize.Height / 2)), LegendSize.Width, LegendSize.Height)
                Case enumLegendPosition.Right
                    res = New Rectangle(Math.Max(2, CInt(InnerChartArea.Width - 2 - LegendSize.Width)), Math.Max(2, CInt(InnerChartArea.Height / 2 - LegendSize.Height / 2)), LegendSize.Width, LegendSize.Height)

                Case enumLegendPosition.BottomLeft
                    res = New Rectangle(2, Math.Max(2, CInt(InnerChartArea.Height - 2 - LegendSize.Height)), LegendSize.Width, LegendSize.Height)
                Case enumLegendPosition.Bottom
                    res = New Rectangle(Math.Max(2, CInt(InnerChartArea.Width / 2 - LegendSize.Width / 2)), Math.Max(2, CInt(InnerChartArea.Height - 2 - LegendSize.Height)), LegendSize.Width, LegendSize.Height)
                Case enumLegendPosition.BottomRight
                    res = New Rectangle(Math.Max(2, CInt(InnerChartArea.Width - 2 - LegendSize.Width)), Math.Max(2, CInt(InnerChartArea.Height - 2 - LegendSize.Height)), LegendSize.Width, LegendSize.Height)
            End Select
            res.Offset(InnerChartArea.Left, InnerChartArea.Top)
            Return res
        End Function
        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            CancleMouseEvent = False
            RaiseEvent MouseMove(Me, e)
            MyBase.OnMouseMove(e)
            If CancleMouseEvent Then Exit Sub
            If (Date.Now - lastMouseMove).TotalMilliseconds < 10 Then Exit Sub
            Select Case CurrentMDownMode
                Case MDownMode.Marker
                    Dim dX As Double = XAxis.PixelPositionToValue(e.Location.X) - XAxis.PixelPositionToValue(MDownPos.X)
                    Dim yAxis As VerticalAxis = If(DraggingMarker.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis)
                    Dim dY As Double = yAxis.PixelPositionToValue(e.Location.Y) - yAxis.PixelPositionToValue(MDownPos.Y)
                    DraggingMarker.X += dX
                    DraggingMarker.Y += dY
                    MDownPos = e.Location
                    RaiseEvent MarkerDragged(Me, New MarkerDraggedEventArgs With {.Marker = DraggingMarker})
                    Me.Invalidate()
                Case MDownMode.Move
                    Dim dX As Double = XAxis.PixelPositionToValue(e.Location.X) - XAxis.PixelPositionToValue(MDownPos.X)
                    Dim dY1 As Double = Y1Axis.PixelPositionToValue(e.Location.Y) - Y1Axis.PixelPositionToValue(MDownPos.Y)
                    Dim dY2 As Double = Y2Axis.PixelPositionToValue(e.Location.Y) - Y2Axis.PixelPositionToValue(MDownPos.Y)
                    XAxis.Minimum -= dX
                    XAxis.Maximum -= dX
                    Y1Axis.Minimum -= dY1
                    Y1Axis.Maximum -= dY1
                    Y2Axis.Minimum -= dY2
                    Y2Axis.Maximum -= dY2
                    MDownPos = e.Location
                    UserBounds = AxisBoundsFromCurrent()
                    IsUserScaled = True
                    Me.Invalidate()
                Case MDownMode.Zoom
                    SecondMousePointForRect = e.Location
                    Me.Invalidate()
                Case MDownMode.CreateObject
                    MovingObject.UpdateOnCreate(MDownPos, e.Location, XAxis, Y1Axis)
                    Me.Invalidate()
                Case MDownMode.NotDown
                    'Check the legend
                    Dim prev As Boolean = isLegendExpanded
                    If EnableLegend Then

                        If GetLegendBounds.Contains(e.Location) Then
                            isLegendExpanded = True
                        Else
                            isLegendExpanded = False
                        End If

                    End If

                    'Check the object handles (select)

                    Dim newselected As Boolean = False
                    If EnableGraphObjects Then
                        Dim loc As Point = e.Location - New Size(Me.InnerChartArea.Left, Me.InnerChartArea.Top)
                        For Each o As clsGraphObject In Me.GraphObjects
                            Dim oldselect As New List(Of Integer)
                            oldselect.AddRange(o.SelectedIndizes)
                            Dim mhandles() As ObjectMouseHandle = o.GetMouseHandles(XAxis, If(o.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis))
                            If o.SelectedIndizes Is Nothing Then o.SelectedIndizes = New List(Of Integer)
                            o.SelectedIndizes.Clear()
                            For i = 0 To mhandles.Count - 1
                                Dim selected As Integer = -1
                                For a = 0 To mhandles.Count - 1
                                    If New RectangleF(CSng(mhandles(a).X - HandlesSize), CSng(mhandles(a).Y - HandlesSize), 2 * HandlesSize, 2 * HandlesSize).Contains(loc) Then
                                        o.SelectedIndizes.Add(a)
                                    End If
                                Next
                            Next
                            If oldselect.Count <> o.SelectedIndizes.Count Then
                                newselected = True
                            Else
                                For i = 0 To oldselect.Count - 1
                                    If oldselect(i) <> o.SelectedIndizes(i) Then newselected = True : Exit For
                                Next
                            End If
                        Next
                    End If

                    If isLegendExpanded <> prev OrElse newselected Then Me.Invalidate()

                Case MDownMode.ObjectMove
                    MovingObject.UpDateByMouse(MovingObjectHandleIndex, e.Location.X - MDownPos.X, e.Location.Y - MDownPos.Y, XAxis, If(MovingObject.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), clsGraphObject.enMouseAction.Move)
                    MDownPos = e.Location
                    Me.Invalidate()
            End Select
            lastMouseMove = Date.Now
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            Dim lastmDownMode As MDownMode = CurrentMDownMode
            CurrentMDownMode = MDownMode.NotDown
            CancleMouseEvent = False
            RaiseEvent MouseUp(Me, e)
            MyBase.OnMouseUp(e)
            If CancleMouseEvent Then Exit Sub

            If lastmDownMode = MDownMode.Zoom Then
                If Math.Abs(MDownPos.X - e.X) < 5 OrElse Math.Abs(MDownPos.Y - e.Y) < 5 Then Me.Invalidate() : Exit Sub
                Dim AxeBounds As New AxisBoundSet(XAxis.Minimum, XAxis.Maximum, Y1Axis.Minimum, Y1Axis.Maximum, Y2Axis.Minimum, Y2Axis.Maximum)
                ZoomSteps.Push(AxeBounds)
                Dim p1 As Point = MDownPos
                Dim p2 As Point = e.Location
                Dim r As Rectangle = New Rectangle(Min(p1.X, p2.X), Min(p1.Y, p2.Y), Abs(p1.X - p2.X), Abs(p1.Y - p2.Y))
                If r.Width <= 0 OrElse r.Height <= 0 Then Me.Invalidate() : Exit Sub
                Dim newXMin As Double = XAxis.PixelPositionToValue(r.Left)
                Dim newXMAx As Double = XAxis.PixelPositionToValue(r.Right)

                Dim newYM1in As Double = Y1Axis.PixelPositionToValue(r.Bottom)
                Dim newY1MAx As Double = Y1Axis.PixelPositionToValue(r.Top)
                Dim newY2Min As Double = Y2Axis.PixelPositionToValue(r.Bottom)
                Dim newY2MAx As Double = Y2Axis.PixelPositionToValue(r.Top)

                XAxis.Minimum = newXMin
                XAxis.Maximum = newXMAx
                Y1Axis.Minimum = newYM1in
                Y1Axis.Maximum = newY1MAx
                Y2Axis.Minimum = newY2Min
                Y2Axis.Maximum = newY2MAx
                UserBounds = AxisBoundsFromCurrent()
                IsUserScaled = True
                DoAutoScale()
            ElseIf lastmDownMode = MDownMode.ObjectMove Then
                If MDownPosStatic.X - e.Location.X = 0 AndAlso MDownPosStatic.Y - e.Location.Y = 0 Then
                    'Different approach to detect clicked mouse
                    MovingObject.UpDateByMouse(MovingObjectHandleIndex, 0, 0, XAxis, If(MovingObject.YAxisType = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis), clsGraphObject.enMouseAction.Up)
                End If
                MDownPos = e.Location
                Me.Invalidate()
                'ElseIf lastmDownMode = MDownMode.CreateObject Then
                '    LeftMouseAction = enLeftMouseAction.ZoomIn
            End If
            Me.Invalidate()
        End Sub
        Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
            CancleMouseEvent = False
            RaiseEvent MouseWheel(Me, e)
            MyBase.OnMouseWheel(e)
            If CancleMouseEvent Then Exit Sub
            If MouseWheelZoomEnabled = False Then Exit Sub

            Dim OverX As Boolean = (New Rectangle(0, Me.Height - GraphBorder.Bottom, Me.Width, GraphBorder.Bottom)).Contains(e.Location)
            Dim OverY1 As Boolean = (New Rectangle(0, 0, Me.GraphBorder.Left, Me.Height)).Contains(e.Location)
            Dim OverY2 As Boolean = (New Rectangle(Me.Width - GraphBorder.Right, 0, Me.GraphBorder.Right, Me.Height)).Contains(e.Location)

            Const ZoomDist As Double = 0.05
            Dim xValue As Double = XAxis.PixelPositionToValue(e.X)
            Dim y1Value As Double = Y1Axis.PixelPositionToValue(e.Y)
            Dim y2Value As Double = Y2Axis.PixelPositionToValue(e.Y)
            Dim xdelta As Double = (XAxis.Maximum - XAxis.Minimum)
            Dim y1delta As Double = (Y1Axis.Maximum - Y1Axis.Minimum)
            Dim y2delta As Double = (Y2Axis.Maximum - Y2Axis.Minimum)

            Dim xPercent As Double = (xValue - XAxis.Minimum) / xdelta
            Dim y1Percent As Double = (y1Value - Y1Axis.Minimum) / y1delta
            Dim y2Percent As Double = (y2Value - Y2Axis.Minimum) / y2delta

            Dim xadd As Double = (xdelta * ZoomDist)
            Dim y1add As Double = (y1delta * ZoomDist)
            Dim y2add As Double = (y2delta * ZoomDist)

            Dim addix As Double = (xPercent - 0.5) * (xdelta * 0.2)
            Dim addiy1 As Double = (y1Percent - 0.5) * (y1delta * 0.2)
            Dim addiy2 As Double = (y2Percent - 0.5) * (y2delta * 0.2)

            Dim sign = Math.Sign(e.Delta)

            If (Not (OverY1 Or OverY2)) Or (OverY1 And ScaleProportional) Then
                XAxis.Minimum += sign * xadd
                XAxis.Maximum -= sign * xadd
                XAxis.Minimum += addix
                XAxis.Maximum += addix
            End If

            If (Not (OverX Or OverY2)) Or (OverX And ScaleProportional) Then
                Y1Axis.Minimum += sign * y1add
                Y1Axis.Maximum -= sign * y1add
                Y1Axis.Minimum += addiy1
                Y1Axis.Maximum += addiy1
            End If

            If Not (OverX Or OverY1) Then
                Y2Axis.Minimum += sign * y2add
                Y2Axis.Maximum -= sign * y2add
                Y2Axis.Minimum += addiy2
                Y2Axis.Maximum += addiy2
            End If

            UserBounds = AxisBoundsFromCurrent()
            IsUserScaled = True
        End Sub
#End Region

#Region "Hit test"
        Public Function PointHitTest(Location As Point, ByRef ResultSeries As Series, ByRef ResultIndex As Integer, ByRef result As Datapoint) As Boolean
            Try
                If InnerChartArea.Contains(Location) = False Then Return False
                Dim LocX As Single = CSng(XAxis.PixelPositionToValue(Location.X))
                Dim LocY1 As Single = CSng(Y1Axis.PixelPositionToValue(Location.Y))
                Dim LocY2 As Single = CSng(Y2Axis.PixelPositionToValue(Location.Y))

                For Each s As Series In Me.Series
                    Dim found As Boolean = False
                    Dim radiusX As Double = Math.Abs(XAxis.PixelPositionToValue(100.0F) - XAxis.PixelPositionToValue(100.0F - s.MarkerSize))
                    Dim radiusY As Double
                    Dim yaxis As VerticalAxis
                    If s.YAxisType = Axis.enumAxisLocation.Primary Then
                        yaxis = Y1Axis
                    Else
                        yaxis = Y2Axis
                    End If
                    radiusY = Math.Abs(yaxis.PixelPositionToValue(100.0F) - yaxis.PixelPositionToValue(100.0F - s.MarkerSize))
                    Dim count As Integer = 0
                    For Each d As Datapoint In s.Data
                        Dim dX As Single = XAxis.ValueToPixelPosition(d.X) + Me.GraphBorder.Left
                        Dim dY As Single = yaxis.ValueToPixelPosition(d.Y) + Me.GraphBorder.Top

                        Dim rect As New RectangleF(CSng(d.X - radiusX / 2), CSng(d.Y - radiusY / 2), CSng(radiusX), CSng(radiusY))
                        Dim realrect As New RectangleF(dX - s.MarkerSize / 2, dY - s.MarkerSize / 2, s.MarkerSize, s.MarkerSize)
                        If realrect.Contains(Location) Then
                            ResultSeries = s
                            ResultIndex = count
                            result = d
                            Return True
                        End If
                        count += 1
                    Next
                Next
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

#Region "Datapoint to String"
        Private WithEvents AnimateTimer As New Timer With {.Interval = 2500}
        Private DoShowDatapoint As Boolean
        Private DataPointPosition As Point
        Private DataPointText As String
        Private Sub ShowDatapoint(d As Datapoint, Series As Series, Location As Point)
            If DataToString IsNot Nothing Then
                DataPointText = DataToString(Me, d, Series)
            Else
                Dim xVal As Double = If(Me.XAxis.ShowLogarithmic, 10 ^ d.X, d.X)
                Dim yVal As Double = If(If(Series.YAxisType = Axis.enumAxisLocation.Primary, Me.Y1Axis, Me.Y2Axis).ShowLogarithmic, 10 ^ d.Y, d.Y)
                DataPointText = If(Series.LegendText <> "", Series.LegendText, Series.Name) & vbCrLf & "X=" & xVal.TryFormat("short") & vbCrLf & "Y=" & yVal.TryFormat("short")
            End If
            DataPointPosition = Location
            DataPointPosition.X -= GraphBorder.Left
            DataPointPosition.Y -= GraphBorder.Top

            DoShowDatapoint = True
            Me.Invalidate()
            AnimateTimer.Stop()
            AnimateTimer.Start()
        End Sub
        Private Sub AnimateTimerTick(sender As Object, e As EventArgs) Handles AnimateTimer.Tick
            DoShowDatapoint = False
            AnimateTimer.Stop()
            Me.Invalidate()
        End Sub
#End Region

#Region "Helper methods"
        Private Sub ScaleAxesProportional(ByRef minx As Double, ByRef maxx As Double, ByRef miny As Double, ByRef maxy As Double,
                                          aspectratio As Double)
            Dim calcMaxX, calcMinX As Double
            Dim calcMaxY, calcMinY As Double
            calcMinX = minx : calcMaxX = maxx
            calcMinY = miny : calcMaxY = maxy
            If calcMaxX < (calcMaxY - calcMinY) * aspectratio + calcMinX Then calcMaxX = (calcMaxY - calcMinY) * aspectratio + calcMinX
            calcMaxY = (calcMaxX - calcMinX) / (aspectratio) + calcMinY
            minx = calcMinX : maxx = calcMaxX
            miny = calcMinY : maxy = calcMaxY
        End Sub
        Private Function AxisBoundsFromCurrent() As AxisBoundSet
            Dim res As New AxisBoundSet(XAxis.Minimum, XAxis.Maximum, Y1Axis.Minimum, Y1Axis.Maximum, Y2Axis.Minimum, Y2Axis.Maximum)
            Return res
        End Function
        Friend Shared Function IsOK(d As Double) As Boolean
            Return Not Double.IsNaN(d) AndAlso Not Double.IsInfinity(d)
        End Function
        Friend Shared Function IsOK(d As Single) As Boolean
            Return Not Single.IsNaN(d) AndAlso Not Single.IsInfinity(d)
        End Function

        ''' <summary>
        ''' When called the graph will not repaint or rescale itself until EndUpdate() is called.
        ''' </summary>
        Public Sub BeginUpdate()
            isUpdating = True
        End Sub

        ''' <summary>
        ''' Will end the Updating status and allow the graph to paint/update again.
        ''' </summary>
        Public Sub EndUpdate()
            isUpdating = False
            DoAutoScale()
        End Sub
        Public Sub ResetUserZoom(DoScale As Boolean)
            ZoomSteps.Clear()
            IsUserScaled = False
            If DoScale Then DoAutoScale()
        End Sub

        ''' <summary>
        ''' Creates a mouse cursor from a ressource
        ''' </summary>
        ''' <remarks>Converted from (Anders Forsgren): http://stackoverflow.com/questions/6897274/c-how-to-load-cursor-from-resource-file</remarks>
        Public Shared Function LoadCursorFromResource(CursorRessource As Byte()) As Cursor
            Try
                ' Assuming that the resource is an Icon, but also could be a Image or a Bitmap
                ' Saving cursor icon in temp file, necessary for loading through Win API
                Dim fileName As String = System.IO.Path.GetTempPath() & Guid.NewGuid().ToString() & ".cur"
                Using fileStream = File.Open(fileName, FileMode.Create)
                    fileStream.Write(CursorRessource, 0, CursorRessource.Length)
                End Using

                ' Loading cursor from temp file, using Win API
                Dim result As New Cursor(LoadCursorFromFile(fileName))

                ' Deleting temp file
                File.Delete(fileName)

                Return result
            Catch
                Return Cursors.Default
            End Try
        End Function

        ''' <summary>
        ''' Calculates the values at a given pixelposition in relation to the top left of the graph
        ''' </summary>
        ''' <param name="Point">The point in relation to the graphs origin (top left)</param>
        ''' <param name="YAxis">The Y-Axis to use for the determination of the Y value</param>
        ''' <returns>A tuple of X/Y value at the given position</returns>
        Public Function PixelPositionToValue(Point As PointF, YAxis As Axis.enumAxisLocation) As Tuple(Of Double, Double)
            Dim dx = XAxis.PixelPositionToValue(Point.X - GraphBorder.Left)
            Dim yaxe = If(YAxis = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis)
            Dim dy = yaxe.PixelPositionToValue(Point.Y - GraphBorder.Top)
            Return Tuple.Create(dx, dy)
        End Function

        ''' <summary>
        ''' Calculates the pixelposition for a given value in relation to the top left of the graph
        ''' </summary>
        ''' <param name="Values">The values to use in the form of (X,Y) tuple</param>
        ''' <param name="YAxis">The Y-Axis to use for the determination of the Y value</param>
        ''' <returns>The pixelposition, where the valuepair can be found in the graph</returns>
        Public Function ValuesToPixelposition(Values As Tuple(Of Double, Double), YAxis As Axis.enumAxisLocation) As PointF
            Dim dx = XAxis.ValueToPixelPosition(Values.Item1) + GraphBorder.Left
            Dim yaxe = If(YAxis = Axis.enumAxisLocation.Primary, Y1Axis, Y2Axis)
            Dim dy = yaxe.ValueToPixelPosition(Values.Item2) + GraphBorder.Top
            Return New PointF(dx, dy)
        End Function

#End Region

#Region "Copy format"
        Private Sub CopyFormat_Axis(SourceAxis As Axis, DestinationAxis As Axis)
            DestinationAxis.Title = SourceAxis.Title
            DestinationAxis.ShowGridlines = SourceAxis.ShowGridlines
            DestinationAxis.ShowLogarithmic = SourceAxis.ShowLogarithmic
            DestinationAxis.ShowOrigin = SourceAxis.ShowOrigin
            DestinationAxis.ShowTickText = SourceAxis.ShowTickText
            DestinationAxis.TickDirection = SourceAxis.TickDirection
            DestinationAxis.TickWidth = SourceAxis.TickWidth
            DestinationAxis.TitleDistance = SourceAxis.TitleDistance
            DestinationAxis.TitleFont = SourceAxis.TitleFont
        End Sub

        Public Sub CopyFormat(Source As jwGraph)
            CopyFormat_Axis(Source.XAxis, Me.XAxis)
            CopyFormat_Axis(Source.Y1Axis, Me.Y1Axis)
            CopyFormat_Axis(Source.Y2Axis, Me.Y2Axis)

            Me.GraphBorder = Source.GraphBorder
            Me.TopLeftColor = Source.TopLeftColor
            Me.BottomRightColor = Source.BottomRightColor

            Me.HighQuality = Source.HighQuality
            Me.ScaleProportional = Source.ScaleProportional

        End Sub
#End Region
    End Class

End Namespace

