RealEstateApi - ASP.NET Core Web API
RealEstateApi is a backend API built using ASP.NET Core and Entity Framework Core. It provides functionalities for managing real estate listings, including property search, CRUD operations, and authentication.

Features
✅ Property Management: Create, update, delete, and fetch real estate listings.
✅ Search Functionality: Search properties by name or address.
✅ User Authentication: Secured API with authentication via JWT tokens.
✅ AutoMapper Integration: Efficient mapping between entities and DTOs.
✅ Database Support: Uses Entity Framework Core for database operations.

Tech Stack
Backend: ASP.NET Core Web API
Database: SQL Server (via Entity Framework Core)
Authentication: JWT (JSON Web Tokens)
Mapping: AutoMapper
Setup & Installation
1️⃣ Clone the Repository
sh
Copy
Edit
git clone https://github.com/yourusername/RealEstateApi.git
cd RealEstateApi
2️⃣ Configure Database
Update appsettings.json with your SQL Server connection string:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=RealEstateDB;Trusted_Connection=True;"
}
3️⃣ Apply Migrations & Seed Data
Run the following commands in the Package Manager Console:

sh
Copy
Edit
dotnet ef database update
4️⃣ Run the API
Start the server with:

sh
Copy
Edit
dotnet run
The API will be available at http://localhost:5000 (or another assigned port).

API Endpoints
🔍 Property Search
GET /api/Properties/SearchProperties?query=City

🏠 Properties CRUD Operations
Method	Endpoint	Description
GET	/api/Properties	Get all properties
GET	/api/Properties/{id}	Get property by ID
POST	/api/Properties	Add new property
PUT	/api/Properties/{id}	Update property
DELETE	/api/Properties/{id}	Delete property
Common Issues & Debugging
1️⃣ Empty Search Results? Ensure your query does not contain trailing spaces or newlines (\n). Use .Trim() in the controller:

csharp
Copy
Edit
query = query.Trim();
2️⃣ Database Connection Issues? Check if your SQL Server is running and the connection string is correct.
3️⃣ Authentication Failure? Ensure you provide a valid Bearer token in API requests.

Contributing
🙌 Contributions are welcome! If you want to improve this API:

Fork the repository
Create a new branch (feature-xyz)
Commit your changes
Create a Pull Request
License
📜 This project is licensed under the MIT License.

