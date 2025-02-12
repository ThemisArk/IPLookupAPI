# **IPLookupAPI**  

## **Overview**  
This project is a .NET Core REST API that provides information about IP addresses using the **IP2C** service. It implements a caching mechanism and periodic updates to ensure efficiency and accuracy.  

## **Features**  
- **Retrieve IP Information**: Fetch country details (Country Name, Two-Letter Code, Three-Letter Code) for a given IP address.  
- **Caching Mechanism**: First checks the cache, then the database, and finally calls IP2C if necessary.  
- **Database Storage**: Uses **Entity Framework** for database operations.  
- **Periodic Data Update**: A background service updates IP information every hour in batches of 100.  

## **Technology Stack**  
- **.NET Core** (latest version)  
- **Entity Framework Core**  
- **SQL Database**  
- **IP2C API** (External Web Service)  
- **Caching (In-Memory Cache)**  

## **Installation & Setup**  
### **Prerequisites**  
- .NET Core SDK  
- SQL Server / PostgreSQL (or any supported database) 

### **Setup Instructions**  
1. **Clone the repository**  
   ```sh
   git clone https://github.com/ThemisArk/IPLookupAPI.git
   cd IPLookupAPI
   ```

2. **Run database migrations**  
   ```sh
   dotnet ef database update
   ```

4. **Start the API**  
   ```sh
   dotnet run
   ```

## **Endpoints**  
### **1. Get IP Information**  
**GET** `/api/ipinfo/{ip}`  

### **2. Update IP Information** (Automated job runs every hour)  
The service updates the database and cache in the background.  


## **License**  
This project is licensed under the MIT License.  
