Imports System.Data

Public Class Form1
    Private db As New DatabaseHelper("Data Source=customers.db")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db.Initialize()
        LoadCustomerList()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim c As New Customer With {
            .Name = txtName.Text,
            .Email = txtEmail.Text,
            .Phone = txtPhone.Text
        }
        db.InsertCustomer(c)
        LoadCustomerList()
    End Sub

    Private Sub LoadCustomerList()
        dgvCustomers.DataSource = db.GetAllCustomers()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim keyword = txtSearch.Text.Trim()
        If keyword = "" Then
            LoadCustomerList()
        Else
            dgvCustomers.DataSource = db.SearchCustomers(keyword)
        End If
    End Sub

End Class
