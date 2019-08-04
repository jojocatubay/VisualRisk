# Net Presentation Value - A Simple MVVM based WPF application with SQL Database

## **Prerequisites**

You need the following tools in order to run/edit the solution.

- Microsoft Visual Studio 2017 / 2019 (Latest recommended)

- Microsoft SQL Server 2014 (Latest recommended)

## **Getting Started**

The application requires a database to store the data. Follow the below
steps to setup database. 

1. SQL Server     
Run the script 'VisualRisk_NPV.sql' to create and
populate database (MS SQL SERVER is required)

2. Code Behind [MainViewModel.cs] >> [public void SaveNPV(...)]    
  
>>Set the connection string

Change server connection. look for this connection string and change base on your setting

string connectionString = @"Data Source=[Data Source];Initial Catalog=VisualRisk;Integrated Security=SSPI;";




