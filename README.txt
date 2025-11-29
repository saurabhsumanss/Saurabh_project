
================================================================================
TECHNOLOGY STACK
================================================================================

- .NET 9 - Latest .NET framework
- ASP.NET Core Web API - RESTful API framework
- Entity Framework Core 9 - ORM for database operations
- SQL Server LocalDB - Development database
- JWT Bearer Authentication - Secure token-based auth
- Swashbuckle (Swagger) 
- System.IdentityModel.Tokens.Jwt - JWT token generation


================================================================================
INSTALLATION & SETUP
================================================================================

PREREQUISITES
-------------
1. .NET 9 SDK - Download from: https://dotnet.microsoft.com/download/dotnet/9.0
2. SQL Server or LocalDB (included with Visual Studio)
3. Code Editor - Visual Studio 2022, VS Code, or Rider (optional)
4. Git (optional)

STEP 1: Verify .NET Installation
---------------------------------
Open command prompt and run:
    dotnet --version

Expected output: 9.0.200 or higher

STEP 2: Navigate to Project Directory
--------------------------------------
    cd C:/Saurabh/LibraryAPI

STEP 3: Restore NuGet Packages
-------------------------------
    dotnet restore

STEP 4: Install Entity Framework Tools (if not installed)
----------------------------------------------------------
    dotnet tool install --global dotnet-ef --version 9.0.0

Or update existing:
    dotnet tool update --global dotnet-ef

STEP 5: Verify Database Connection
-----------------------------------
The default connection string in appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}

For custom SQL Server, update the connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=LibraryDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}

STEP 6: Create Database (Skip if Exists)
-------------------------------------------------------
Create migration:
    dotnet ef migrations add InitialCreate

Apply migration to database:
    dotnet ef database update

STEP 7: Build the Project
--------------------------
    dotnet build

================================================================================
5. RUNNING THE APPLICATION
================================================================================

OPTION 1: Default Configuration
--------------------------------
 1. Build the project in Visual Studio 
  2. Run it from Visual Studio - Press F5 or click the green play button
  3. Access Swagger at http://localhost:5173/swagger

OR 
    cd C:/Saurabh/LibraryAPI
    dotnet run

The application will start on:
- HTTP:  http://localhost:5173
- HTTPS: https://localhost:7248

OPTION 2: Custom Ports
-----------------------
    dotnet run --urls "http://localhost:5000;https://localhost:5001"

OPTION 3: HTTP Only (No Certificate Needed)
--------------------------------------------
    dotnet run --urls "http://localhost:5000"

VERIFY APPLICATION IS RUNNING
------------------------------
You should see output like:

info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5173
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.

ACCESS SWAGGER UI
-----------------
Open your browser and navigate to:
    http://localhost:5173/swagger
    or
    https://localhost:7248/swagger


================================================================================
PROJECT STRUCTURE
================================================================================

LibraryAPI/
├── Controllers/              # API Controllers
│   ├── AuthController.cs          # Authentication endpoints
│   ├── BooksController.cs         # Book management
│   ├── ClientsController.cs       # Client management
│   └── BookLoansController.cs     # Loan operations
│
├── Models/                   # Domain Models (Entities)
│   ├── User.cs                    # User entity
│   ├── Book.cs                    # Book entity
│   ├── Client.cs                  # Client entity
│   └── BookLoan.cs                # BookLoan entity
│
├── DTOs/                     # Data Transfer Objects
│   ├── AuthDtos.cs                # Auth request/response DTOs
│   ├── BookDtos.cs                # Book DTOs
│   ├── ClientDtos.cs              # Client DTOs
│   └── BookLoanDtos.cs            # BookLoan DTOs
│
├── Data/                     # Database Context
│   └── LibraryDbContext.cs        # EF Core DbContext
│
├── Services/                 # Business Logic Services
│   ├── IAuthService.cs            # Auth service interface
│   ├── AuthService.cs             # Auth implementation
│   ├── ITokenService.cs           # Token service interface
│   └── TokenService.cs            # JWT token generation
│
├── Migrations/               # EF Core Migrations
├── Properties/
│   └── launchSettings.json        # Launch configuration
├── Program.cs                # Application entry point
├── appsettings.json          # Application configuration
└── LibraryAPI.csproj         # Project file


================================================================================
STEP-BY-STEP USAGE GUIDE
================================================================================

--------------------------------------------------------------------------------
STEP 1: REGISTER A NEW USER (LIBRARIAN)
--------------------------------------------------------------------------------

Endpoint: POST /api/Auth/register

Steps in Swagger:
1. Expand "POST /api/Auth/register"
2. Click "Try it out"
3. Enter the following JSON in the Request body:

{
  "username": "admin",
  "password": "admin123",
  "email": "admin@library.com",
  "role": "Librarian"
}

4. Click "Execute"

Expected Response (Status 200):

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@library.com",
  "role": "Librarian"
}

Copy the token value - you'll need it for authentication!



--------------------------------------------------------------------------------
STEP 2: LOGIN (GET JWT TOKEN)
--------------------------------------------------------------------------------

Endpoint: POST /api/Auth/login

Steps in Swagger:
1. Expand "POST /api/Auth/login"
2. Click "Try it out"
3. Enter credentials:

{
  "username": "admin",
  "password": "admin123"
}

4. Click "Execute"

Expected Response (Status 200):

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@library.com",
  "role": "Librarian"
}

NOTE: Registration also returns a token, so you can skip login if you just
registered!

--------------------------------------------------------------------------------
STEP 3: AUTHORIZE IN SWAGGER
--------------------------------------------------------------------------------

Steps:
1. Look for the green "Authorize" button at the top-right of Swagger UI
2. Click "Authorize"
3. In the popup, enter:

   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

   (Replace with your actual token)
4. Click "Authorize"
5. Click "Close"

✓ You're now authenticated! A lock icon will appear on protected endpoints.

--------------------------------------------------------------------------------
STEP 4: ADD BOOKS TO LIBRARY (LIBRARIAN ONLY)
--------------------------------------------------------------------------------

Endpoint: POST /api/Books

Steps in Swagger:
1. Expand "POST /api/Books"
2. Click "Try it out"
3. Enter book details:

{
  "title": "The Great Gatsby",
  "author": "F. Scott Fitzgerald",
  "isbn": "978-0743273565",
  "publisher": "Scribner",
  "publicationYear": 1925,
  "genre": "Fiction",
  "totalCopies": 5
}

4. Click "Execute"

Expected Response (Status 201):

{
  "id": 1,
  "title": "The Great Gatsby",
  "author": "F. Scott Fitzgerald",
  "isbn": "978-0743273565",
  "publisher": "Scribner",
  "publicationYear": 1925,
  "genre": "Fiction",
  "totalCopies": 5,
  "availableCopies": 5
}

Add more books:

Book 2:
{
  "title": "To Kill a Mockingbird",
  "author": "Harper Lee",
  "isbn": "978-0061120084",
  "publisher": "Harper Perennial",
  "publicationYear": 1960,
  "genre": "Fiction",
  "totalCopies": 3
}

Book 3:
{
  "title": "1984",
  "author": "George Orwell",
  "isbn": "978-0451524935",
  "publisher": "Signet Classic",
  "publicationYear": 1949,
  "genre": "Dystopian",
  "totalCopies": 4
}

--------------------------------------------------------------------------------
STEP 5: VIEW ALL BOOKS
--------------------------------------------------------------------------------

Endpoint: GET /api/Books

Steps in Swagger:
1. Expand "GET /api/Books"
2. Click "Try it out"
3. Click "Execute"

Expected Response (Status 200):

[
  {
    "id": 1,
    "title": "The Great Gatsby",
    "author": "F. Scott Fitzgerald",
    "isbn": "978-0743273565",
    "publisher": "Scribner",
    "publicationYear": 1925,
    "genre": "Fiction",
    "totalCopies": 5,
    "availableCopies": 5
  },
  {
    "id": 2,
    "title": "To Kill a Mockingbird",
    "author": "Harper Lee",
    "isbn": "978-0061120084",
    "publisher": "Harper Perennial",
    "publicationYear": 1960,
    "genre": "Fiction",
    "totalCopies": 3,
    "availableCopies": 3
  }
]

--------------------------------------------------------------------------------
STEP 6: CREATE LIBRARY CLIENTS (LIBRARIAN ONLY)
--------------------------------------------------------------------------------

Endpoint: POST /api/Clients

Steps in Swagger:
1. Expand "POST /api/Clients"
2. Click "Try it out"
3. Enter client details:

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@email.com",
  "phoneNumber": "+1-555-1234",
  "address": "123 Main St, New York, NY 10001"
}

4. Click "Execute"

Expected Response (Status 201):

{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@email.com",
  "phoneNumber": "+1-555-1234",
  "address": "123 Main St, New York, NY 10001",
  "membershipDate": "2025-11-29T19:30:00",
  "isActive": true
}

Add more clients:

Client 2:
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane.smith@email.com",
  "phoneNumber": "+1-555-5678",
  "address": "456 Oak Ave, Boston, MA 02101"
}

--------------------------------------------------------------------------------
STEP 7: BORROW A BOOK (CREATE LOAN)
--------------------------------------------------------------------------------

Endpoint: POST /api/BookLoans

Steps in Swagger:
1. Expand "POST /api/BookLoans"
2. Click "Try it out"
3. Enter loan details:

{
  "bookId": 1,
  "clientId": 1,
  "loanDurationDays": 14,
  "notes": "First time borrower"
}

4. Click "Execute"

Expected Response (Status 201):

{
  "id": 1,
  "bookId": 1,
  "bookTitle": "The Great Gatsby",
  "clientId": 1,
  "clientName": "John Doe",
  "loanDate": "2025-11-29T19:30:00",
  "dueDate": "2025-12-13T19:30:00",
  "returnDate": null,
  "status": "Active",
  "lateFee": null,
  "notes": "First time borrower"
}

NOTE: The book's availableCopies will decrease by 1.

--------------------------------------------------------------------------------
STEP 8: VIEW ALL ACTIVE LOANS (LIBRARIAN ONLY)
--------------------------------------------------------------------------------

Endpoint: GET /api/BookLoans

Steps in Swagger:
1. Expand "GET /api/BookLoans"
2. Click "Try it out"
3. Click "Execute"

Expected Response (Status 200):

[
  {
    "id": 1,
    "bookId": 1,
    "bookTitle": "The Great Gatsby",
    "clientId": 1,
    "clientName": "John Doe",
    "loanDate": "2025-11-29T19:30:00",
    "dueDate": "2025-12-13T19:30:00",
    "returnDate": null,
    "status": "Active",
    "lateFee": null,
    "notes": "First time borrower"
  }
]

--------------------------------------------------------------------------------
STEP 9: RETURN A BOOK
--------------------------------------------------------------------------------

Endpoint: POST /api/BookLoans/return

Steps in Swagger:
1. Expand "POST /api/BookLoans/return"
2. Click "Try it out"
3. Enter:

{
  "bookLoanId": 1,
  "notes": "Book returned in good condition"
}

4. Click "Execute"

Expected Response (Status 200):

{
  "message": "Book returned successfully",
  "lateFee": 0
}

If returned late (after due date):

{
  "message": "Book returned successfully",
  "lateFee": 5.0
}

(Late fee is $1 per day overdue)

NOTE: The book's availableCopies will increase by 1.

--------------------------------------------------------------------------------
STEP 10: VIEW CLIENT'S LOAN HISTORY
--------------------------------------------------------------------------------

Endpoint: GET /api/BookLoans/client/{clientId}

Steps in Swagger:
1. Expand "GET /api/BookLoans/client/{clientId}"
2. Click "Try it out"
3. Enter clientId: 1
4. Click "Execute"

Expected Response (Status 200):

[
  {
    "id": 1,
    "bookId": 1,
    "bookTitle": "The Great Gatsby",
    "clientId": 1,
    "clientName": "John Doe",
    "loanDate": "2025-11-29T19:30:00",
    "dueDate": "2025-12-13T19:30:00",
    "returnDate": "2025-12-10T10:30:00",
    "status": "Returned",
    "lateFee": 0,
    "notes": "First time borrower\nReturn notes: Book returned in good condition"
  }
]

================================================================================
COMPLETE API REFERENCE
================================================================================

--------------------------------------------------------------------------------
AUTHENTICATION ENDPOINTS
--------------------------------------------------------------------------------

1. REGISTER USER
   POST /api/Auth/register

   Request Body:
   {
     "username": "string",
     "password": "string",
     "email": "string",
     "role": "Client" or "Librarian"
   }

   Responses:
   - 200 OK: Returns JWT token and user info
   - 400 Bad Request: Username or email already exists

2. LOGIN
   POST /api/Auth/login

   Request Body:
   {
     "username": "string",
     "password": "string"
   }

   Responses:
   - 200 OK: Returns JWT token and user info
   - 401 Unauthorized: Invalid credentials

--------------------------------------------------------------------------------
BOOKS ENDPOINTS
--------------------------------------------------------------------------------

1. GET ALL BOOKS
   GET /api/Books
   Authorization: Required (any authenticated user)

   Responses:
   - 200 OK: Returns array of books

2. GET BOOK BY ID
   GET /api/Books/{id}
   Authorization: Required

   Responses:
   - 200 OK: Returns book details
   - 404 Not Found: Book doesn't exist

3. CREATE BOOK
   POST /api/Books
   Authorization: Required (Librarian only)

   Request Body:
   {
     "title": "string",
     "author": "string",
     "isbn": "string",
     "publisher": "string",
     "publicationYear": 0,
     "genre": "string",
     "totalCopies": 0
   }

   Responses:
   - 201 Created: Book created successfully
   - 403 Forbidden: User is not a Librarian

4. UPDATE BOOK
   PUT /api/Books/{id}
   Authorization: Required (Librarian only)

   Request Body (all fields optional):
   {
     "title": "string",
     "author": "string",
     "publisher": "string",
     "publicationYear": 0,
     "genre": "string",
     "totalCopies": 0
   }

   Responses:
   - 204 No Content: Updated successfully
   - 404 Not Found: Book doesn't exist
   - 403 Forbidden: User is not a Librarian

5. DELETE BOOK
   DELETE /api/Books/{id}
   Authorization: Required (Librarian only)

   Responses:
   - 204 No Content: Deleted successfully
   - 400 Bad Request: Book has active loans
   - 404 Not Found: Book doesn't exist
   - 403 Forbidden: User is not a Librarian

--------------------------------------------------------------------------------
CLIENTS ENDPOINTS
--------------------------------------------------------------------------------

1. GET ALL CLIENTS
   GET /api/Clients
   Authorization: Required (Librarian only)

   Responses:
   - 200 OK: Returns array of clients

2. GET CLIENT BY ID
   GET /api/Clients/{id}
   Authorization: Required

   Responses:
   - 200 OK: Returns client details
   - 404 Not Found: Client doesn't exist

3. CREATE CLIENT
   POST /api/Clients
   Authorization: Required (Librarian only)

   Request Body:
   {
     "firstName": "string",
     "lastName": "string",
     "email": "string",
     "phoneNumber": "string",
     "address": "string"
   }

   Responses:
   - 201 Created: Client created successfully
   - 400 Bad Request: Email already exists

4. UPDATE CLIENT
   PUT /api/Clients/{id}
   Authorization: Required (Librarian only)

   Request Body (all fields optional):
   {
     "firstName": "string",
     "lastName": "string",
     "email": "string",
     "phoneNumber": "string",
     "address": "string",
     "isActive": true
   }

   Responses:
   - 204 No Content: Updated successfully
   - 400 Bad Request: Email already exists
   - 404 Not Found: Client doesn't exist

5. DELETE CLIENT
   DELETE /api/Clients/{id}
   Authorization: Required (Librarian only)

   Responses:
   - 204 No Content: Deleted successfully
   - 400 Bad Request: Client has active loans
   - 404 Not Found: Client doesn't exist

--------------------------------------------------------------------------------
BOOK LOANS ENDPOINTS
--------------------------------------------------------------------------------

1. GET ALL LOANS
   GET /api/BookLoans
   Authorization: Required (Librarian only)

   Responses:
   - 200 OK: Returns array of all loans

2. GET LOAN BY ID
   GET /api/BookLoans/{id}
   Authorization: Required

   Responses:
   - 200 OK: Returns loan details
   - 404 Not Found: Loan doesn't exist

3. GET CLIENT LOANS
   GET /api/BookLoans/client/{clientId}
   Authorization: Required

   Responses:
   - 200 OK: Returns array of client's loans

4. CREATE BOOK LOAN
   POST /api/BookLoans
   Authorization: Required (Librarian only)

   Request Body:
   {
     "bookId": 0,
     "clientId": 0,
     "loanDurationDays": 14,
     "notes": "string" (optional)
   }

   Responses:
   - 201 Created: Loan created successfully
   - 400 Bad Request: No available copies / Client not active /
                      Client already has this book
   - 404 Not Found: Book or client doesn't exist

5. RETURN BOOK
   POST /api/BookLoans/return
   Authorization: Required (Librarian only)

   Request Body:
   {
     "bookLoanId": 0,
     "notes": "string" (optional)
   }

   Responses:
   - 200 OK: Returns success message and late fee
   - 400 Bad Request: Book already returned
   - 404 Not Found: Loan doesn't exist

6. DELETE LOAN
   DELETE /api/BookLoans/{id}
   Authorization: Required (Librarian only)

   Responses:
   - 204 No Content: Deleted successfully
   - 404 Not Found: Loan doesn't exist

================================================================================
DATABASE SCHEMA
================================================================================

USERS TABLE
-----------
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
);

BOOKS TABLE
-----------
CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) NOT NULL UNIQUE,
    Publisher NVARCHAR(100) NOT NULL,
    PublicationYear INT NOT NULL,
    Genre NVARCHAR(50) NOT NULL,
    TotalCopies INT NOT NULL,
    AvailableCopies INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);

CLIENTS TABLE
-------------
CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    MembershipDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NOT NULL
);

BOOKLOANS TABLE
---------------
CREATE TABLE BookLoans (
    Id INT PRIMARY KEY IDENTITY,
    BookId INT NOT NULL,
    ClientId INT NOT NULL,
    LoanDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NOT NULL,
    ReturnDate DATETIME2 NULL,
    Status NVARCHAR(20) NOT NULL,
    LateFee DECIMAL(18,2) NULL,
    Notes NVARCHAR(MAX) NULL,
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    FOREIGN KEY (ClientId) REFERENCES Clients(Id)
);

================================================================================
BUSINESS RULES
================================================================================

BOOK MANAGEMENT
---------------
ISBN must be unique
AvailableCopies is automatically set equal to TotalCopies on creation
Books with active loans cannot be deleted
Updating TotalCopies adjusts AvailableCopies accordingly

CLIENT MANAGEMENT
-----------------
Email must be unique
Clients are active by default (IsActive = true)
Clients with active loans cannot be deleted
MembershipDate is automatically set to current date

BOOK LOANS
----------
Only active clients can borrow books
Books must have AvailableCopies > 0
A client cannot borrow the same book twice simultaneously
AvailableCopies decreases when a book is loaned
AvailableCopies increases when a book is returned
Late fees are calculated at $1.00 per day overdue
Default loan duration is 14 days
Loan status: "Active" → "Returned"


================================================================================
TESTING SCENARIOS
================================================================================

SCENARIO 1: COMPLETE LIBRARY WORKFLOW
--------------------------------------

1. Register Librarian
   POST /api/Auth/register
   {
     "username": "librarian1",
     "password": "pass123",
     "email": "lib1@library.com",
     "role": "Librarian"
   }

2. Add Books
   POST /api/Books
   {
     "title": "Pride and Prejudice",
     "author": "Jane Austen",
     "isbn": "978-0141439518",
     "publisher": "Penguin Classics",
     "publicationYear": 1813,
     "genre": "Romance",
     "totalCopies": 3
   }

3. Register Clients
   POST /api/Clients
   {
     "firstName": "Alice",
     "lastName": "Johnson",
     "email": "alice@email.com",
     "phoneNumber": "+1-555-9999",
     "address": "789 Elm St, Chicago, IL"
   }

4. Loan a Book
   POST /api/BookLoans
   {
     "bookId": 1,
     "clientId": 1,
     "loanDurationDays": 14
   }

5. Return Book
   POST /api/BookLoans/return
   {
     "bookLoanId": 1,
     "notes": "Excellent condition"
   }

SCENARIO 2: TESTING BUSINESS RULES
-----------------------------------

Test 1: Borrow Unavailable Book
  1. Create a book with totalCopies: 1
  2. Loan it to Client A
  3. Try to loan the same book to Client B
     Expected: 400 Bad Request - "No available copies"

Test 2: Duplicate Loan
  1. Client A borrows Book X
  2. Client A tries to borrow Book X again
     Expected: 400 Bad Request - "Client already has this book on loan"

Test 3: Inactive Client
  1. Create a client
  2. Update client: isActive = false
  3. Try to create a loan for that client
     Expected: 400 Bad Request - "Client account is not active"

Test 4: Late Fee Calculation
  1. Create a loan with dueDate in the past
  2. Return the book
     Expected: lateFee calculated based on days overdue

================================================================================
TROUBLESHOOTING
================================================================================

DATABASE ISSUES
---------------

Error: "Cannot open database LibraryDB"

Solution:
    cd C:/Saurabh/LibraryAPI
    dotnet ef database update

---

Error: "Login failed for user"

Solution: Update connection string in appsettings.json with correct credentials

AUTHENTICATION ISSUES
---------------------

Error: "Invalid username or password"

Solutions:
1. Verify you registered first
2. Check username/password spelling (case-sensitive)
3. Try registering a new user with different credentials

---

Error: "401 Unauthorized"

Solutions:
1. Ensure you got a token from login/register
2. Click "Authorize" in Swagger and enter: Bearer <token>
3. Token might be expired (7-day lifetime) - login again

---

Error: "403 Forbidden"

Solutions:
1. Your role doesn't have permission
2. Librarian-only endpoints require role: "Librarian"
3. Register a new user with Librarian role

PORT ALREADY IN USE
-------------------

Error: "Address already in use"

Solution:
    # Find process using port 5173
    netstat -ano | findstr :5173

    # Kill the process (replace PID)
    taskkill /F /PID <PID>

    # Or use different port
    dotnet run --urls "http://localhost:8080"

JSON VALIDATION ERRORS
----------------------

Error: "One or more validation errors occurred"

Solutions:
1. Ensure JSON is properly formatted
2. Remove trailing commas
3. All string values in double quotes
4. Copy JSON examples exactly as shown
5. Don't modify default Swagger request body unnecessarily

SWAGGER NOT LOADING
-------------------

Solutions:
1. Ensure app is running: check console for "Application started"
2. Navigate to correct URL: http://localhost:5173/swagger
3. Clear browser cache
4. Try different browser


================================================================================
CONFIGURATION FILES
================================================================================

APPSETTINGS.JSON
----------------

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "LibraryAPI",
    "Audience": "LibraryAPIClients"
  }
}
