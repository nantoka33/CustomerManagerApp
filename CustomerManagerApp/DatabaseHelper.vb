Imports System.Data.SQLite
Imports System.Data

Public Class DatabaseHelper
    Private connStr As String

    Public Sub New(connectionString As String)
        Me.connStr = connectionString
    End Sub

    Public Sub Initialize()
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Dim sql As String =
                "CREATE TABLE IF NOT EXISTS Customers (" &
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," &
                "Name TEXT," &
                "Email TEXT," &
                "Phone TEXT," &
                "CreatedAt TEXT)"

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.ExecuteNonQuery()
            End Using

            Dim sql2 As String =
                "CREATE TABLE IF NOT EXISTS Orders (" &
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," &
                "CustomerId INTEGER," &
                "OrderDate TEXT," &
                "Product TEXT," &
                "Amount REAL," &
                "FOREIGN KEY(CustomerId) REFERENCES Customers(Id))"

            Using cmd2 As New SQLiteCommand(sql2, conn)
                cmd2.ExecuteNonQuery()
            End Using

        End Using
    End Sub

    Public Sub InsertCustomer(c As Customer)
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Dim sql As String =
                "INSERT INTO Customers (Name, Email, Phone, CreatedAt) " &
                "VALUES (@Name, @Email, @Phone, @CreatedAt)"

            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@Name", c.Name)
                cmd.Parameters.AddWithValue("@Email", c.Email)
                cmd.Parameters.AddWithValue("@Phone", c.Phone)
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.ExecuteNonQuery()
            End Using

        End Using
    End Sub

    Public Sub InsertOrder(o As Order)
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Dim sql As String = "INSERT INTO Orders (CustomerId, OrderDate, Product, Amount) VALUES (@CustomerId, @OrderDate, @Product, @Amount)"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@CustomerId", o.CustomerId)
                cmd.Parameters.AddWithValue("@OrderDate", o.OrderDate)
                cmd.Parameters.AddWithValue("@Product", o.Product)
                cmd.Parameters.AddWithValue("@Amount", o.Amount)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub


    Public Function GetAllCustomers() As DataTable
        Dim dt As New DataTable()
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Using da As New SQLiteDataAdapter("SELECT * FROM Customers ORDER BY CreatedAt DESC", conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Public Function GetAllOrders() As DataTable
        Dim dt As New DataTable()
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Using da As New SQLiteDataAdapter("SELECT * FROM Orders", conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function


    Public Function SearchCustomers(keyword As String) As DataTable
        Dim dt As New DataTable()
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Dim sql = "SELECT * FROM Customers WHERE Name LIKE @kw OR Email LIKE @kw OR Phone LIKE @kw"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@kw", "%" & keyword & "%")
                Using da As New SQLiteDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Function SearchOrders(keyword As String) As DataTable
        Dim dt As New DataTable()
        Using conn As New SQLiteConnection(connStr)
            conn.Open()
            Dim sql = "SELECT * FROM Orders WHERE Product LIKE @kw"
            Using cmd As New SQLiteCommand(sql, conn)
                cmd.Parameters.AddWithValue("@kw", "%" & keyword & "%")
                Using da As New SQLiteDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function
End Class
