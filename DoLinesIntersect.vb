    Public Shared Function DoLinesIntersect(ByVal L1 As Line, ByVal L2 As Line, ByRef ptIntersection As PointF) As Boolean
        ' Denominator for ua and ub are the same, so store this calculation
        Dim d As Double = (L2.Y2 - L2.Y1) * (L1.X2 - L1.X1) - (L2.X2 - L2.X1) * (L1.Y2 - L1.Y1)

        'n_a and n_b are calculated as seperate values for readability
        Dim n_a As Double = (L2.X2 - L2.X1) * (L1.Y1 - L2.Y1) - (L2.Y2 - L2.Y1) * (L1.X1 - L2.X1)

        Dim n_b As Double = (L1.X2 - L1.X1) * (L1.Y1 - L2.Y1) - (L1.Y2 - L1.Y1) * (L1.X1 - L2.X1)

        ' Make sure there is not a division by zero - this also indicates that
        ' the lines are parallel.  
        ' If n_a and n_b were both equal to zero the lines would be on top of each 
        ' other (coincidental).  This check is not done because it is not 
        ' necessary for this implementation (the parallel check accounts for this).
        If d = 0 Then Return False

        ' Calculate the intermediate fractional point that the lines potentially intersect.
        Dim ua As Double = n_a / d
        Dim ub As Double = n_b / d

        ' The fractional point will be between 0 and 1 inclusive if the lines
        ' intersect.  If the fractional calculation is larger than 1 or smaller
        ' than 0 the lines would need to be longer to intersect.
        If ua >= 0D AndAlso ua <= 1D AndAlso ub >= 0D AndAlso ub <= 1D Then
            ptIntersection.X = L1.X1 + (ua * (L1.X2 - L1.X1))
            ptIntersection.Y = L1.Y1 + (ua * (L1.Y2 - L1.Y1))
            Return True
        End If

        Return False
    End Function
