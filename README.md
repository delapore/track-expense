# track-expense

Welcome to Track Expense !

Here you can retrieve, add, modify and delete expenses via a simple and beautiful website.

### Prerequisite

 - Docker
 - a command line
 - a web browser
 - sqlcmd to connect to sqlserver (optional)

Setup :
- Open a command line at the root of this repo
- SQL Server DB Initialization
  - launch the following command : `docker-compose up -d sqldata`
  - wait a moment so that SQL Server database is initialized (you can see the progression in the logs of track-expense_sqldata_1 image)
  - you can test the SQL Server database connection with the following cmd (sqlcmd must be installed) : `sqlcmd -S localhost,1433 -U sa -P Pass@word`
- Expense API Initialization
  - back to the command line at the root of this repo, launch the following command : `docker-compose up -d --build expenseapi`
  - wait a moment so that Expense API is initialized (you can see the progression in the logs of track-expense_expenseapi_1 image)
  - you can test the API by opening the swagger in a web browser : http://localhost:5000/swagger
- Web UI Initialisation
  - back to the command line at the root of this repo, launch the following command : `docker-compose up -d --build web`
  - wait a moment so that Web service is initialized (you can see the progression in the logs of track-expense_web_1 image)
  - you can open a browser and go to the http://localhost url

### User guide

- open http://localhost
- you are in the main page of Track Expense, you should see a table with some expenses
![track-expense web page](https://raw.githubusercontent.com/delapore/track-expense/master/track-expense.png?token=AAESIZ2GOSSJWPWKG6TORUK7OCBQO)
- add a new expense
  - fill the form on the left part of the page (recipient can be left blank, other fields are mandatory)
  - click `Add` you should see your expense in the expense table
- edit an existing expense
  - click `Edit` on one line of the expense table -> it should fill the form on the left with expense info
  - modify information and click on `Modify`, you should see your expense updated in the expense table
- delete an existing expense
  - click on `Delete` on one line, the line should disappear
