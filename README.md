# Customer Service API Specification

## All endpoints return a response object.

The response object has the following structure:

```json
{
  "data": <response_data>,
  "message": "<response_message>",
  "success": <true_or_false>
}
```

Where:

- `data`: Contains the response data (can be `null` if no data is returned).
- `message`: A descriptive message, often indicating success or error.
- `success`: A boolean indicating whether the request was successful.

## Endpoints

### 1. Get All Customers

**GET** `/api/customers`

#### Description

Retrieves all customers in the system.

#### Response

- **Status 200 OK**
  - **Body:**
    ```json
    {
      "data": [
        /* array of customer objects */
      ],
      "message": "",
      "success": true
    }
    ```

### 2. Get Customer by ID

**GET** `/api/customers/{id}`

#### Description

Retrieves a customer by their unique identifier.

#### Parameters

- **id** (Guid): The unique identifier of the customer.

#### Response

- **Status 200 OK** (if customer found)
  - **Body:**
    ```json
    {
      "data": {
        /* customer object */
      },
      "message": "",
      "success": true
    }
    ```
- **Status 404 Not Found** (if customer not found)
  - **Body:**
    ```json
    {
      "data": null,
      "message": "Customer not found",
      "success": false
    }
    ```

### 3. Create a Customer

**POST** `/api/customers`

#### Description

Creates a new customer.

#### Request Body

```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "address": "ExampleAddress"
}
```

- **Status 201 Created (if creation successful)**

  - **Body:**
    ```json
    {
      "data": {
        /* created customer object */
      },
      "message": "Customer created successfully",
      "success": true
    }
    ```

- **Status 400 Bad Request (if creation fails)**
  - **Body:**
    ```json
    {
      "data": null,
      "message": "Customer created successfully",
      "success": true
    }
    ```

### 4. Update a Customer

**PUT** `/api/customers/{id}`

#### Description

Creates a new customer.

#### Parameters

- **id** (Guid): The unique identifier of the customer.

#### Request Body

```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "address": "ExampleAddress"
}
```

- **Status 200 OK (if update successful)**

  - **Body:**

    ```json
    {
      "data": {
        /* updated customer object */
      },
      "message": "Customer updated successfully",
      "success": true
    }
    ```

    - **Status 400 Bad Request (if ID mismatch)**

  - **Body:**

    ```json
    {
      "data": null,
      "message": "Customer ID mismatch.",
      "success": false
    }
    ```

    - **Status 404 Not Found (if customer not found)**

  - **Body:**
    ```json
    {
      "data": null,
      "message": "Customer not found",
      "success": false
    }
    ```

### DELETE /api/customers/{id}

Deletes a customer by their unique identifier.

#### Parameters

- `id` (Guid): The unique identifier of the customer to delete.

#### Responses

- **200 OK**

  - Description: The customer was deleted successfully.
  - Body:
    ```json
    {
      "data": null,
      "message": "Customer deleted successfully",
      "success": true
    }
    ```

- **404 Not Found**
  - Description: The customer with the specified ID was not found.
  - Body:
    ```json
    {
      "data": null,
      "message": "Customer not found",
      "success": false
    }
    ```
