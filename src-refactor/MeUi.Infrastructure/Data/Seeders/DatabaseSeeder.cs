using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MeUi.Application.Common.Behaviors;
using MeUi.Application.Common.Interfaces;
using MeUi.Domain.Common.Constants;
using MeUi.Domain.Entities;
using System.Reflection;

namespace MeUi.Infrastructure.Data.Seeders;

public class DatabaseSeeder
{
    private readonly IRepository<LoginMethod> _loginMethodRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Resource> _resourceRepository;
    private readonly IRepository<MeUi.Domain.Entities.Action> _actionRepository;
    private readonly IRepository<PageGroup> _pageGroupRepository;
    private readonly IRepository<Page> _pageRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<PageResourceAction> _pageResourceActionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        IRepository<LoginMethod> loginMethodRepository,
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IRepository<Resource> resourceRepository,
        IRepository<MeUi.Domain.Entities.Action> actionRepository,
        IRepository<PageGroup> pageGroupRepository,
        IRepository<Page> pageRepository,
        IRepository<Permission> permissionRepository,
        IRepository<PageResourceAction> pageResourceActionRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<DatabaseSeeder> logger)
    {
        _loginMethodRepository = loginMethodRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _resourceRepository = resourceRepository;
        _actionRepository = actionRepository;
        _pageGroupRepository = pageGroupRepository;
        _pageRepository = pageRepository;
        _permissionRepository = permissionRepository;
        _pageResourceActionRepository = pageResourceActionRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            await SeedLoginMethodsAsync(ct);
            await SeedRolesAsync(ct);
            await SeedResourcesAsync(ct);
            await SeedActionsAsync(ct);
            await SeedPageGroupsAsync(ct);
            await SeedPagesAsync(ct);
            await SeedPagePermissionsAsync(ct);
            await SeedSuperUserAsync(ct);

            await _unitOfWork.SaveChangesAsync(ct);
            await _unitOfWork.CommitTransactionAsync(ct);

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during database seeding");
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }

    private async Task SeedLoginMethodsAsync(CancellationToken ct)
    {
        var loginMethods = new[]
        {
            new { Code = AuthenticationConstants.LoginMethods.Password, Name = "Password", Description = "Username/Email and Password authentication" },
            new { Code = AuthenticationConstants.LoginMethods.OAuth, Name = "OAuth", Description = "OAuth 2.0 authentication" },
            new { Code = AuthenticationConstants.LoginMethods.TwoFactor, Name = "Two Factor", Description = "Two-factor authentication" }
        };

        foreach (var method in loginMethods)
        {
            var existing = await _loginMethodRepository.FirstOrDefaultAsync(x => x.Code == method.Code, ct);
            if (existing == null)
            {
                await _loginMethodRepository.AddAsync(new LoginMethod
                {
                    Code = method.Code,
                    Name = method.Name,
                    Description = method.Description,
                    IsActive = true
                }, ct);

                _logger.LogInformation("Seeded login method: {Code}", method.Code);
            }
        }
    }

    private async Task SeedRolesAsync(CancellationToken ct)
    {
        var roles = new[]
        {
            new { Code = AuthorizationConstants.Roles.SuperAdmin, Name = "Super Administrator", Description = "Full system access" },
            new { Code = AuthorizationConstants.Roles.Admin, Name = "Administrator", Description = "Administrative access" },
            new { Code = AuthorizationConstants.Roles.User, Name = "User", Description = "Standard user access" },
            new { Code = AuthorizationConstants.Roles.Guest, Name = "Guest", Description = "Limited guest access" }
        };

        foreach (var role in roles)
        {
            var existing = await _roleRepository.FirstOrDefaultAsync(x => x.Code == role.Code, ct);
            if (existing == null)
            {
                await _roleRepository.AddAsync(new Role
                {
                    Code = role.Code,
                    Name = role.Name,
                    Description = role.Description
                }, ct);

                _logger.LogInformation("Seeded role: {Code}", role.Code);
            }
        }
    }

    private async Task SeedResourcesAsync(CancellationToken ct)
    {
        var permissions = ScanPermissionsFromAssemblies();
        var resources = permissions.Select(p => p.Resource).Distinct().ToList();

        foreach (var resourceCode in resources)
        {
            var existing = await _resourceRepository.FirstOrDefaultAsync(x => x.Code == resourceCode, ct);
            if (existing == null)
            {
                await _resourceRepository.AddAsync(new Resource
                {
                    Code = resourceCode,
                    Name = FormatDisplayName(resourceCode),
                    Description = $"Operations related to {FormatDisplayName(resourceCode).ToLower()}"
                }, ct);

                _logger.LogInformation("Seeded resource: {Code}", resourceCode);
            }
        }
    }

    private async Task SeedActionsAsync(CancellationToken ct)
    {
        var permissions = ScanPermissionsFromAssemblies();
        var actions = permissions.Select(p => p.Action).Distinct().ToList();

        foreach (var actionCode in actions)
        {
            var existing = await _actionRepository.FirstOrDefaultAsync(x => x.Code == actionCode, ct);
            if (existing == null)
            {
                await _actionRepository.AddAsync(new MeUi.Domain.Entities.Action
                {
                    Code = actionCode,
                    Name = FormatDisplayName(actionCode),
                    Description = $"{FormatDisplayName(actionCode)} operation"
                }, ct);

                _logger.LogInformation("Seeded action: {Code}", actionCode);
            }
        }
    }

    private async Task SeedSuperUserAsync(CancellationToken ct)
    {
        var superUserEmail = _configuration["SuperUser:Email"];
        var superUserPassword = _configuration["SuperUser:Password"];

        if (string.IsNullOrEmpty(superUserEmail) || string.IsNullOrEmpty(superUserPassword))
        {
            _logger.LogWarning("SuperUser configuration is missing, skipping super user creation");
            return;
        }

        var existingUser = await _userRepository.FirstOrDefaultAsync(x => x.Email == superUserEmail, ct);
        if (existingUser == null)
        {
            var superUser = new User
            {
                Email = superUserEmail,
                Username = _configuration["SuperUser:Username"] ?? "superadmin",
                Name = $"{_configuration["SuperUser:FirstName"]} {_configuration["SuperUser:LastName"]}".Trim(),
                IsSuspended = false
            };

            await _userRepository.AddAsync(superUser, ct);
            _logger.LogInformation("Seeded super user: {Email}", superUserEmail);
        }
    }

    private List<(string Action, string Resource)> ScanPermissionsFromAssemblies()
    {
        var permissions = new List<(string Action, string Resource)>();

        try
        {
            // Get all loaded assemblies that might contain commands/queries with RequirePermissionAttribute
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic &&
                           (a.FullName?.Contains("MeUi") == true ||
                            a.FullName?.Contains("Application") == true))
                .ToList();

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract)
                        .ToList();

                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes<RequirePermissionAttribute>().ToList();

                        foreach (var attribute in attributes)
                        {
                            var permissionStrings = new List<string>();

                            // Single permission
                            if (!string.IsNullOrEmpty(attribute.Permission))
                            {
                                permissionStrings.Add(attribute.Permission);
                            }

                            // Multiple permissions (ANY)
                            if (attribute.AnyPermissions?.Any() == true)
                            {
                                permissionStrings.AddRange(attribute.AnyPermissions);
                            }

                            // Multiple permissions (ALL)
                            if (attribute.AllPermissions?.Any() == true)
                            {
                                permissionStrings.AddRange(attribute.AllPermissions);
                            }

                            // Parse permissions in format "action:resource"
                            foreach (var permissionString in permissionStrings)
                            {
                                if (TryParsePermission(permissionString, out var action, out var resource))
                                {
                                    permissions.Add((action.ToUpper(), resource.ToUpper()));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to scan assembly {AssemblyName} for permissions", assembly.FullName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while scanning assemblies for permissions");
        }

        return permissions.Distinct().ToList();
    }

    private static bool TryParsePermission(string permission, out string action, out string resource)
    {
        action = string.Empty;
        resource = string.Empty;

        if (string.IsNullOrWhiteSpace(permission))
            return false;

        var parts = permission.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
            return false;

        action = parts[0].Trim();
        resource = parts[1].Trim();

        return !string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(resource);
    }

    private async Task SeedPageGroupsAsync(CancellationToken ct)
    {
        var pageGroups = new[]
        {
            new { Code = "DASHBOARD", Name = "Dashboard", Icon = "dashboard" },
            new { Code = "USER_MANAGEMENT", Name = "User Management", Icon = "people" },
            new { Code = "AUTHORIZATION", Name = "Authorization", Icon = "security" },
            new { Code = "SYSTEM", Name = "System", Icon = "settings" },
            new { Code = "REPORTS", Name = "Reports", Icon = "analytics" }
        };

        foreach (var group in pageGroups)
        {
            var existing = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == group.Code, ct);
            if (existing == null)
            {
                await _pageGroupRepository.AddAsync(new PageGroup
                {
                    Code = group.Code,
                    Name = group.Name,
                    Icon = group.Icon
                }, ct);

                _logger.LogInformation("Seeded page group: {Code}", group.Code);
            }
        }
    }

    private async Task SeedPagesAsync(CancellationToken ct)
    {
        // Get page groups for foreign key references
        var dashboardGroup = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == "DASHBOARD", ct);
        var userMgmtGroup = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == "USER_MANAGEMENT", ct);
        var authGroup = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == "AUTHORIZATION", ct);
        var systemGroup = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == "SYSTEM", ct);
        var reportsGroup = await _pageGroupRepository.FirstOrDefaultAsync(x => x.Code == "REPORTS", ct);

        var pages = new[]
        {
            // Dashboard pages
            new { Code = "DASHBOARD_HOME", Name = "Dashboard", Path = "/dashboard", PageGroupId = dashboardGroup?.Id },
            new { Code = "DASHBOARD_ANALYTICS", Name = "Analytics", Path = "/dashboard/analytics", PageGroupId = dashboardGroup?.Id },

            // User Management pages
            new { Code = "USERS_LIST", Name = "Users List", Path = "/users", PageGroupId = userMgmtGroup?.Id },
            new { Code = "USERS_CREATE", Name = "Create User", Path = "/users/create", PageGroupId = userMgmtGroup?.Id },
            new { Code = "USERS_EDIT", Name = "Edit User", Path = "/users/edit", PageGroupId = userMgmtGroup?.Id },
            new { Code = "USERS_VIEW", Name = "View User", Path = "/users/view", PageGroupId = userMgmtGroup?.Id },

            // Authorization pages
            new { Code = "ROLES_LIST", Name = "Roles List", Path = "/roles", PageGroupId = authGroup?.Id },
            new { Code = "ROLES_CREATE", Name = "Create Role", Path = "/roles/create", PageGroupId = authGroup?.Id },
            new { Code = "ROLES_EDIT", Name = "Edit Role", Path = "/roles/edit", PageGroupId = authGroup?.Id },
            new { Code = "PERMISSIONS_LIST", Name = "Permissions List", Path = "/permissions", PageGroupId = authGroup?.Id },
            new { Code = "PAGES_LIST", Name = "Pages List", Path = "/pages", PageGroupId = authGroup?.Id },

            // System pages
            new { Code = "SYSTEM_SETTINGS", Name = "System Settings", Path = "/system/settings", PageGroupId = systemGroup?.Id },
            new { Code = "SYSTEM_LOGS", Name = "System Logs", Path = "/system/logs", PageGroupId = systemGroup?.Id },

            // Reports pages
            new { Code = "REPORTS_USER", Name = "User Reports", Path = "/reports/users", PageGroupId = reportsGroup?.Id },
            new { Code = "REPORTS_SYSTEM", Name = "System Reports", Path = "/reports/system", PageGroupId = reportsGroup?.Id }
        };

        foreach (var page in pages)
        {
            var existing = await _pageRepository.FirstOrDefaultAsync(x => x.Code == page.Code, ct);
            if (existing == null)
            {
                await _pageRepository.AddAsync(new Page
                {
                    Code = page.Code,
                    Name = page.Name,
                    Path = page.Path,
                    PageGroupId = page.PageGroupId
                }, ct);

                _logger.LogInformation("Seeded page: {Code}", page.Code);
            }
        }
    }

    private async Task SeedPagePermissionsAsync(CancellationToken ct)
    {
        // Get some basic permissions that should exist
        var viewPermission = await _permissionRepository.FirstOrDefaultAsync(x => x.ActionCode == "READ" && x.ResourceCode == "USER", ct);
        var createPermission = await _permissionRepository.FirstOrDefaultAsync(x => x.ActionCode == "CREATE" && x.ResourceCode == "USER", ct);
        var editPermission = await _permissionRepository.FirstOrDefaultAsync(x => x.ActionCode == "UPDATE" && x.ResourceCode == "USER", ct);
        var deletePermission = await _permissionRepository.FirstOrDefaultAsync(x => x.ActionCode == "DELETE" && x.ResourceCode == "USER", ct);

        // Get some pages
        var usersListPage = await _pageRepository.FirstOrDefaultAsync(x => x.Code == "USERS_LIST", ct);
        var usersCreatePage = await _pageRepository.FirstOrDefaultAsync(x => x.Code == "USERS_CREATE", ct);
        var usersEditPage = await _pageRepository.FirstOrDefaultAsync(x => x.Code == "USERS_EDIT", ct);
        var dashboardPage = await _pageRepository.FirstOrDefaultAsync(x => x.Code == "DASHBOARD_HOME", ct);

        var pagePermissions = new List<(Guid? PageId, Guid? PermissionId)>();

        // Dashboard - accessible with any user permission
        if (dashboardPage != null && viewPermission != null)
            pagePermissions.Add((dashboardPage.Id, viewPermission.Id));

        // Users list - requires view permission
        if (usersListPage != null && viewPermission != null)
            pagePermissions.Add((usersListPage.Id, viewPermission.Id));

        // Users create - requires create permission
        if (usersCreatePage != null && createPermission != null)
            pagePermissions.Add((usersCreatePage.Id, createPermission.Id));

        // Users edit - requires edit permission
        if (usersEditPage != null && editPermission != null)
            pagePermissions.Add((usersEditPage.Id, editPermission.Id));

        foreach (var (pageId, permissionId) in pagePermissions)
        {
            if (pageId.HasValue && permissionId.HasValue)
            {
                var existing = await _pageResourceActionRepository.FirstOrDefaultAsync(
                    x => x.PageId == pageId && x.PermissionId == permissionId, ct);

                if (existing == null)
                {
                    await _pageResourceActionRepository.AddAsync(new PageResourceAction
                    {
                        PageId = pageId,
                        PermissionId = permissionId
                    }, ct);

                    _logger.LogInformation("Seeded page permission: PageId={PageId}, PermissionId={PermissionId}", pageId, permissionId);
                }
            }
        }
    }

    private static string FormatDisplayName(string code)
    {
        if (string.IsNullOrEmpty(code))
            return code;

        // Convert UPPER_CASE_WITH_UNDERSCORES to Title Case
        return string.Join(" ", code.Split('_'))
            .Split(' ')
            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower())
            .Aggregate((current, next) => current + " " + next);
    }
}