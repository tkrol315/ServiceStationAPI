# ServiceStationAPI

ServiceStationAPI is an application designed to store information about repairs in service station.
It allows both clients and mechanics to access a centralized platform to track the history of repairs performed on vehicles
The application has three roles: Client, Mechanic, and Manager. Below, you can find details about what each role can do.

## Client

### Vehicles

- **Create Vehicle**: Allows clients to create a vehicle that needs repair.
  - **Method**: POST
  - **Endpoint**: `/api/vehicle`

- **Read All Vehicles**: Enables clients to view all of their own vehicles.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle`

- **Read Specific Vehicle by ID**: Allows clients to retrieve details of a specific vehicle by its ID.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{id}`

- **Update Own Vehicle**: Allows clients to update information about their own vehicle.
  - **Method**: PUT
  - **Endpoint**: `/api/vehicle/{id}`

- **Remove Own Vehicle by ID**: Allows clients to remove their own vehicle by its ID.
  - **Method**: DELETE
  - **Endpoint**: `/api/vehicle/{id}`

### Order Notes

- **Read Vehicle Order Notes**: Enables clients to view order notes related to their vehicles.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote`

- **Read Specific Vehicle Order Note by ID**: Allows clients to view a specific order note for their vehicle by its ID.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote/{orderNoteId}`

## Mechanic

### Vehicles

- **Read All Vehicles**: Enables mechanics to view all vehicles (including those owned by clients).
  - **Method**: GET
  - **Endpoint**: `/api/vehicle`

- **Read Specific Vehicle by ID**: Allows mechanics to retrieve details of a specific vehicle by its ID.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{id}`

### Order Notes

- **Read All Car Order Notes**: Enables mechanics to view order notes for all vehicles.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote`

- **Read Specific Order Note by ID**: Allows mechanics to view a specific order note by its ID.
  - **Method**: GET
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote/{orderNoteId}`

- **Create Order Note for Specific Car**: Allows mechanics to create order notes for a specific vehicle.
  - **Method**: POST
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote`

## Manager

- **All Endpoints**: Managers have access to all the endpoints described above.

- **Remove Order Notes by ID**: Allows managers to remove order notes for a specific vehicle by its ID.
  - **Method**: DELETE
  - **Endpoint**: `/api/vehicle/{vehicleId}/ordernote/{orderNoteId}`

- **Change User Roles**: Enables managers to change the roles of users.
  - **Method**: PUT
  - **Endpoint**: `/api/account/{accountEmailAddress}`

- **Read All Accounts**: Allows managers to view information about all accounts.
  - **Method**: GET
  - **Endpoint**: `/api/account`

- **Read Account by Email Address**: Enables managers to retrieve information about a specific account by its email address.
  - **Method**: GET
  - **Endpoint**: `/api/account/{emailAddress}`

- **Delete Account**: Allows managers to delete an account by its email address.
  - **Method**: DELETE
  - **Endpoint**: `/api/account/{emailAddress}`
