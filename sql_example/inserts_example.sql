-- those are just some examplery values for the database for easier testing and development

INSERT INTO Role (RoleName)
VALUES
    ('Admin'),
    ('Moderator'),
    ('User'),
    ('Guest'),
    ('Support'),
    ('Developer');

INSERT INTO Permission (PermissionName)
VALUES
    ('ViewUsers'),
    ('EditUsers'),
    ('DeleteUsers'),
    ('ViewReports'),
    ('GenerateReports'),
    ('ManageSettings'),
    ('AccessDashboard'),
    ('ResetPasswords'),
    ('AssignRoles');