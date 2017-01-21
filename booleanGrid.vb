Module Module1

    Sub Main()

        Dim operand As Integer = 0  ' this is for a select case statement - possibly being replaced.
        Dim LENGTH As Integer = 22500  ' Set this to the number of grid points you want. Must be a SQUARE.
        Dim FILL As Boolean = False ' This flag will enable filling the grid with 1s, rather than leaving it intact
        Dim randy As New Random(System.DateTime.Now.Millisecond) ' This is a random seed taken from the time.
        Dim t As New System.TimeSpan(Date.Now.Millisecond())

        'this next bit sets up a test grid, displays the contents (to test FILL for now)
        Dim g As New Grid(LENGTH, FILL)

        'displayGrid(g, True)

        ' this should be explained, because VB is retarded. VB takes a non-zero positive integer as the size of an array.
        ' this means that Dim a as new array(1) will set up an array with 2 indexes, 0, and 1. So for a 25 cell array, 
        ' the highest index is 24. upon consideration i will leave the default logic intact incase i need to interface with
        ' another class with default array logic.
        For index As Integer = 0 To LENGTH - 1
            g.setPOS(index, genRndBit(randy))   ' fetch a random bit (1 or 0) and put it at position "index" on the grid.
        Next                                    ' this fills the whole grid (to point out that grid.setpos() works)

        Console.WriteLine()

        Console.WriteLine("********************************************")

        Console.WriteLine()

        'displayGrid(g, True)

        Console.WriteLine()


        '' from here on out it's all debug output for now.
        Dim timer1 As New System.TimeSpan(Date.Now.Ticks())
        Console.Write(timer1.Subtract(t).Milliseconds()) ' computational time.
        Console.WriteLine(" millseconds to build object")
        While (True)
            'test getCoord and getPOS
            Console.WriteLine(" x , y ")

            Dim tmp1 = 0
            Dim tmp2 As Boolean = False
            Dim tmp3(1) As Integer
            Dim tmp4 As Integer = 0
            For Each bittle As Integer In g.getCoord(randy.Next(0, g.length())) ' this generates a random position and gets coords for it
                Console.Write(bittle)
                tmp3(tmp4) = bittle
                If tmp4 = 0 Then
                    Console.Write(", ") ' hackass (read lazy) tabulation
                End If
                tmp4 += 1
            Next

            Console.WriteLine()
            
            Console.WriteLine(" Position ")
            Console.WriteLine(g.getPos(tmp3))

            Console.WriteLine()

            Console.WriteLine(" state ")
            Console.WriteLine(g.getBool(tmp3)) 'getBool is overloaded to support the array(1) OR index format, here it is doing array.
            Console.WriteLine(g.getBool(g.getPos(tmp3))) ' and here it's doing an index.


            Dim tmp5 As String
            tmp5 = Console.ReadKey().Key.ToString.ToLower '' this quick section lets you relist the grid to check accuracy.
            If tmp5 = "r" Then
                displayGrid(g, True)
            ElseIf tmp5 = "q" Or tmp5 = "x" Or tmp5 = "c" Then '' this one allows you to quit.
                End
            End If




        End While



    End Sub

    Public Function genRndBit(ByVal rnd As Random) As Boolean
        '' this function generates a random bit (true/false, 1/0). Because the seed can be input on the command line, you can do fake randomness;
        '' such a thing could be used for rolling back mutations, as well as assuring that certain ones always ouccur 
        '' after so many milliseconds.
        Dim temp As Integer             ' in a real language, this stuff would be irrelevant.
        'Dim parsed As Boolean          ' in a real language, oh, i'm not using this anymore, but it'll be a bug someday.
        temp = rnd.Next(2)              ' Next(2) returns the set {0,1} with psuedo random distribution
        'Console.Write(temp.ToString)   'debug
        If temp = 0 Then                'HURR I AM VISTAUL BASIC I CAN'T FUCKING USE 0 and NONZERO TO REPRESENT TRUE AND FALSE
            Return False                'DURRRRRRRRRRRRRRRRRRRRRRRRRRRR
        ElseIf temp = 1 Then            ' DERPDERPDERPDERP
            Return True                 'I hate you, visualbasicdotnet.
        Else
            Console.WriteLine("error")  'I'm guessing that this is inevitable, considering the overall shenanigan-ness of VB.
            Console.ReadKey()           'Pauses program if the random number generator spits up "4" or any other stupid number.

        End If

        'Console.Write(parsed.ToString) 'debug
        'Return parsed                  'removing a potentially nasty bug
    End Function

    Class clock
        '' This will be the class that represents the base of all clocks, the ability to measure time, and pulse ones and zeros.
        '' it will be extended to clocks that have settable parameters, such as cycles per second or mhz or special fuzzy logic 
        '' output to mirror real-life chip timing. in a language that is not VB, 0 and non-0 are valid pulses as far as logic
        '' goes (and that should be able to ignore time or not, in the gates.) so i might have to write a "realBoolean" class to
        '' handle true boolean (or at least the way that C, and every piece of electronics on earth does it-boolean)

        ' may as well get the largest "clock tick units" out of the way. at least in a 64bit memory address. [bigint if not]
        Private clock As Int64
        Private bClk As Boolean ' This will probably change to CLK_C because i think that's what it's called in fpgas.
        'should make any eventual ability to read from VHDL/verilog way easier that way.
    End Class
    Class Wire
        '' here is how a wire works, you pass it a start and end grid position and it flips 
        '' the bit on endPOS to match the bit on startPOS.
        '' Everything else should extend this class. Also, this is an exercise to show how grid works.

        '' for the Genetic algorithm, you'd want to ensure that at least 1 input (of which wire would be 1) has a 1 to start, from an
        '' heretofore unprogrammed serial interface pin.

        '' this also needs the ability to check the output pin for other output pins eventually as well as 
        '' know which direction it is going NS or EW (diode extends this class to add voltage drop (analogue) or N, E, S, W cardinality.

        '' note, for graph paper interface you can use getPOS on the x,y pairs to get the startxy and endxy variables.

        Private startPOS As Integer
        Private endPOS As Integer
        Public Sub New(ByVal s As Integer, ByVal e As Integer, ByRef t As Grid)
            startPOS = s
            endPOS = e
            t.setPOS(e, t.getBool(s))
        End Sub
    End Class

    '

    Class Grid
        '' Basic grid class. i can probably build this as an extention to array called gridarray the same way bitarray exists.

        Private size As Integer
        Private len As Integer
        Private XY As BitArray 'the physical layer

        ' output is going to be moved to the outputMask class which extends Grid class to check for outputs. 
        Private output As BitArray 'output pin flag (outputs can't be connected together, only to other inputs (including the output header pin_)


        ' Constructor
        Public Sub New(ByVal k As Integer, ByVal fill As Boolean)   ' k is LENGTH fill is FILL
            If (Int(Math.Sqrt(k)) Mod Math.Sqrt(k)) = 0 Then        ' verifies that it's actually a square number
                size = Math.Sqrt(k)                                 ' caching the value
                len = k                                             ' storing it in the object
                Dim coord As New BitArray(len)                      ' construct the new Temporary grid of the right size.
                XY = New BitArray(coord.Length())                   ' Copy tmp grid to object's grid.
                If fill Then                                        ' fill it if requested
                    For index As Integer = 0 To (len - 1)
                        'coord(index) = fill                        'CRAPPY WAY
                        Me.setPOS(index, fill)                      'AWESOmEr WAY!
                    Next
                End If
            Else
                Console.WriteLine("error: square assertion failed")
                Console.Error.WriteLine("error: Constructor assertion failure")
                End                                                 '' accidentally forgot this part. if it's not a square it fails.
            End If

        End Sub

        '' destructor: garbage collection is sane... we hope.
        Overloads Sub finalize()

        End Sub

        ' getCoord takes a "POS" index and outputs an array(1) containing the x,y coordinate pair. 
        Public Function getCoord(ByVal index) As Array
            Dim a(1) As Integer
            a(0) = Math.Floor(index / (size))   '' derived from (y * SCREEN_WIDTH) + x for images, or something. it works great.
            a(1) = index Mod size               '' if you have the index, the int result of index/size is the X, and the remainder of that
            Return a                            '' is Y. 5x5 grid: 4,0 coords; 20 is the index. 20 / 5 = 4 MOD 0  ... see? 
        End Function

        Public Function getBool(ByVal index As Integer) As Boolean
            Return XY(index)                    '' simple getPrivate
        End Function

        Public Function getBool(ByVal pos As Array) As Boolean
            '' this overrides the previous, allowing for both input types to work.
            '' as you see, both of them provide the EXACT same functionality under the hood...
            '' but the interface LOOKS different to the caller.
            Return XY(getPos(pos))
        End Function

        Public Function getPos(ByVal a As Array) As Integer
            '' remember that (y * SCREEN_WIDTH) + x thing? here it is getting a position from an array(1) of an X,Y coordinate pair.
            Dim result As Integer
            result = (a(0) * size) + a(1)
            Return result
        End Function

        Public Sub setPOS(ByVal index As Integer, ByVal b As Boolean)
            '' Set a bit at position(index) to b.
            XY(index) = b
        End Sub

        Public Sub setCoord(ByVal a As Array, ByVal b As Boolean)
            '' Same, except it does it by coordinates. converted to position index, first.
            XY(getPos(a)) = b
        End Sub

        Public Function length() As Integer
            '' standard, since it is basically an extended array. which reminds me i should get around to actually just extending array.
            Return len
        End Function

    End Class

    '
    '' cFlag does nothing yet, i just wanted to be able to have two displaygrids in a hurry if i needed to.
    '' input is a Grid, outputs to console a really shitty looking table.
    Private Sub displayGrid(ByVal grid As Grid, ByVal cFlag As Boolean)
        For index As Integer = 0 To grid.length() - 1
            Console.Write(grid.getBool(index).ToString)
            Console.Write(ControlChars.Tab)
            Console.Write(index.ToString)
            Console.Write(",    ")
        Next
    End Sub

    '
End Module
