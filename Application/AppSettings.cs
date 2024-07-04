namespace Application;

public class AppSettings
{
    public string DatabaseName { get; set;} = default!;
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    public FirebaseSettings FirebaseSettings { get; set; } = default!;
    public JWTOptions JWTOptions { get; set; } = default!;
}

public class JWTOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}

public class ConnectionStrings
{
    public string MongoDbConnection { get; set; } = string.Empty;
}

public class EmailConfig
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class FirebaseSettings
{
    public string SenderId { get; set; } = default!;
    public string ServerKey { get; set; } = default!;
    public string ApiKeY { get; set; } = default!;
    public string Bucket { get; set; } = default!;
    public string AuthEmail { get; set; } = default!;
    public string AuthPassword { get; set; } = default!;
}