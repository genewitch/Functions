Public Class Line
    Private xfirst As Double
    Private yfirst As Double
    Private xsecond As Double
    Private ysecond As Double
    Private s As Double ' slope
    Private bYIntercept As Double

    Public Sub New(ByVal P As PointF, ByVal slope As Double)
        xfirst = P.X
        yfirst = P.Y
        s = slope
        xsecond = 1.0
        ysecond = (s * (xsecond - xfirst) + yfirst)
        bYIntercept = (yfirst - (s * xfirst))
    End Sub

    Public Sub New(ByVal Px, ByVal Py, ByVal slope) 'point/slope
        ' y = m(x-Px) + Py 
        ' x,y points on the line
        ' Px,Py point on line given
        ' m is slope
        s = slope
        xfirst = Px
        yfirst = Py
        xsecond = 1.0
        ysecond = (s * (xsecond - xfirst) + yfirst)
        bYIntercept = (yfirst - (s * xfirst))
    End Sub

    Public Sub New(ByVal Px1, ByVal Py1, ByVal Px2, ByVal Py2)
        xfirst = Px1
        xsecond = Px2
        yfirst = Py1
        ysecond = Py2
        s = ((ysecond - yfirst) / (xsecond - xfirst))
        bYIntercept = yfirst - s * xfirst
    End Sub

    Public Function X1() As Double
        Return xfirst
    End Function
    Public Function X2() As Double
        Return xsecond
    End Function
    Public Function Y1() As Double
        Return yfirst
    End Function
    Public Function Y2() As Double
        Return ysecond
    End Function

End Class
