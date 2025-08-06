# MeUi Application Layer

This is the refactored Application layer implementing Clean Architecture principles with MediatR, pipeline behaviors, and permission-based authorization.

## Features

- **MediatR** for CQRS pattern implementation
- **Pipeline Behaviors** for cross-cutting concerns
- **Permission-based Authorization** with dynamic permissions
- **FluentValidation** for request validation
- **Mapster** for object mapping
- **Structured Logging** with request timing

## Pipeline Behaviors

The following behaviors are executed in order for each request:

1. **LoggingBehavior** - Logs request handling with execution time
2. **ValidationBehavior** - Validates requests using FluentValidation
3. **AuthorizationBehavior** - Checks permissions using dynamic permission system
4. **TransactionBehavior** - Manages database transactions for commands

## Permission-Based Authorization

The system uses dynamic permissions in the format `action:resource`. Examples:

### Single Permission

```csharp
[RequirePermission("read:user")]
public class GetUserQuery : IRequest<UserDto>
```

### Multiple Permissions (OR Logic)

User needs ANY of the specified permissions:

```csharp
[RequirePermission(AnyPermissions = new[] { "read:user", "read:admin" })]
public class GetUserDetailsQuery : IRequest<UserDetailsDto>
```

### Multiple Permissions (AND Logic)

User needs ALL of the specified permissions:

```csharp
[RequirePermission(AllPermissions = new[] { "read:user", "write:user" })]
public class UpdateUserCommand : IRequest<bool>
```

## Permission Examples

### User Management

- `read:user` - View user information
- `write:user` - Create/update user data
- `delete:user` - Delete users
- `read:user-profile` - View user profiles
- `write:user-profile` - Edit user profiles

### Order Management

- `read:order` - View orders
- `write:order` - Create/update orders
- `delete:order` - Cancel/delete orders
- `read:order-history` - View order history
- `write:order-status` - Update order status

### Admin Operations

- `read:admin-dashboard` - Access admin dashboard
- `write:system-config` - Modify system configuration
- `read:audit-logs` - View audit logs
- `write:user-permissions` - Manage user permissions

## Usage

### 1. Register Services

```csharp
services.AddApplication();
```

### 2. Create Commands/Queries

```csharp
[RequirePermission("write:order")]
public class CreateOrderCommand : IRequest<Guid>
{
    public string ProductName { get; set; }
    public decimal Amount { get; set; }
}
```

### 3. Implement Handlers

```csharp
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Implementation
    }
}
```

### 4. Use in Controllers

```csharp
[HttpPost]
public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
{
    var orderId = await _mediator.Send(command);
    return Ok(orderId);
}
```

## Authorization Flow

1. Request comes in with `[RequirePermission]` attribute
2. AuthorizationBehavior checks if user is authenticated
3. Behavior validates user has required permission(s)
4. If authorized, request continues to handler
5. If not authorized, `UnauthorizedAccessException` is thrown

## Transaction Management

Commands (requests that modify data) automatically get wrapped in database transactions:

- Transaction begins before command execution
- Transaction commits on success
- Transaction rolls back on exception
- Queries bypass transaction logic for performance

## Validation

All requests can have FluentValidation validators:

```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
```

## Logging

All requests are automatically logged with:

- Request name and execution time
- Success/failure status
- User context for authorization
- Performance metrics
