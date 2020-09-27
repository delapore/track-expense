# track-expense

Welcome to Track Expense !

Here you can retrieve, add, modify and delete expenses via a simple and beautiful website.

### Prerequisite

 - Docker
 - a command line
 - a web browser
 - sqlcmd CLI to connect to sqlserver (optional)

### Setup
- Open a command line at the root of this repo
- SQL Server DB Initialization
  - Launch the following command : `docker-compose up -d sqldata`
  - Wait a moment so that SQL Server database is initialized (you can see the progression in the logs of track-expense_sqldata_1 image)
  - You can test the SQL Server database connection with the following cmd (sqlcmd must be installed) : `sqlcmd -S localhost,1433 -U sa -P Pass@word`
- Expense API Initialization
  - Back to the command line at the root of this repo, launch the following command : `docker-compose up -d --build expenseapi`
  - Wait a moment so that Expense API is initialized (you can see the progression in the logs of track-expense_expenseapi_1 image)
  - You can test the API by opening the swagger in a web browser : http://localhost:5000/swagger
- Web UI Initialisation
  - Back to the command line at the root of this repo, launch the following command : `docker-compose up -d --build web`
  - Wait a moment so that Web service is initialized (you can see the progression in the logs of track-expense_web_1 image)
  - You can open a browser and go to the http://localhost url

### User guide

- Open http://localhost
- You are in the main page of Track Expense, you should see a table with some expenses
![track-expense web page](https://raw.githubusercontent.com/delapore/track-expense/master/track-expense.png?token=AAESIZ2GOSSJWPWKG6TORUK7OCBQO)
- Add a new expense
  - Fill the form on the left part of the page (recipient can be left blank, other fields are mandatory)
  - Click `Add` you should see your expense in the expense table
- Edit an existing expense
  - Click `Edit` on one line of the expense table -> it should fill the form on the left with expense info
  - Modify information and click on `Modify`, you should see your expense updated in the expense table
- Delete an existing expense
  - Click on `Delete` on one line, the line should disappear

### Teardown

Open a command line at the root of this repo and launch : `docker-compose down`
