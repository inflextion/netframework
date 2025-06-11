# Java to .NET Transition Cheat Sheet

*For Java developers entering the .NET ecosystem*

---

## 🏗️ **Fundamental Differences**

### **Runtime & Platform**
| Aspect | Java | .NET |
|--------|------|------|
| **Runtime** | JVM (Java Virtual Machine) | CLR (Common Language Runtime) |
| **Platform** | "Write once, run anywhere" | Cross-platform (.NET Core/5+) |
| **Languages** | Primarily Java | C#, F#, VB.NET, C++/CLI |
| **Package Manager** | Maven, Gradle | NuGet |

### **Project Structure**
```
Java                           .NET
src/main/java/                 src/
├── com/company/app/           ├── Controllers/
│   ├── controllers/           ├── Models/
│   ├── models/                ├── Services/
│   └── services/              └── Program.cs
└── resources/                 appsettings.json
```

---

## 📝 **Language Syntax Comparison**

### **Basic Syntax**
| Feature | Java | C# |
|---------|------|-----|
| **Main Method** | `public static void main(String[] args)` | `static void Main(string[] args)` |
| **String Type** | `String` | `string` |
| **Console Output** | `System.out.println("Hello")` | `Console.WriteLine("Hello")` |
| **Variable Declaration** | `String name = "John";` | `string name = "John";` or `var name = "John";` |
| **Constants** | `final String NAME = "John"` | `const string NAME = "John"` |

### **Object-Oriented Features**
| Feature | Java | C# |
|---------|------|-----|
| **Inheritance** | `extends` | `:` |
| **Interface Implementation** | `implements` | `:` |
| **Package/Namespace** | `package com.company.app;` | `namespace Company.App` |
| **Import/Using** | `import java.util.*;` | `using System.Collections.Generic;` |
| **Abstract Classes** | `abstract class Animal` | `abstract class Animal` |
| **Method Override** | `@Override` | `override` (requires `virtual` in base) |

---

## 🔧 **Framework & Tooling**

### **Build & Dependency Management**
| Java | .NET |
|------|------|
| **Maven** `pom.xml` | **NuGet** `.csproj` |
| **Gradle** `build.gradle` | **MSBuild** `.csproj` |
| `mvn clean install` | `dotnet build` |
| `mvn test` | `dotnet test` |
| `mvn package` | `dotnet publish` |

### **Testing Frameworks**
| Java | .NET | Usage |
|------|------|-------|
| **JUnit** | **xUnit** | Most popular, parallel-friendly |
| **TestNG** | **NUnit** | Feature-rich, traditional |
| **Mockito** | **Moq** | Mocking framework |
| **RestAssured** | **RestSharp** | API testing |
| **Selenium** | **Playwright** | UI automation (modern choice) |

---

## 🧪 **Test Automation Specifics**

### **Test Structure Comparison**
```java
// Java (JUnit)
@Test
public void shouldCreateUser() {
    // Arrange
    User user = new User("John", "john@example.com");
    
    // Act
    User created = userService.create(user);
    
    // Assert
    assertEquals("John", created.getName());
}
```

```csharp
// C# (xUnit)
[Fact]
public void Should_Create_User() 
{
    // Arrange
    var user = new User("John", "john@example.com");
    
    // Act
    var created = userService.Create(user);
    
    // Assert
    Assert.Equal("John", created.Name);
}
```

### **Test Data & Setup**
| Java | .NET |
|------|------|
| `@BeforeEach` | `IAsyncLifetime.InitializeAsync()` |
| `@AfterEach` | `IAsyncLifetime.DisposeAsync()` |
| `@BeforeAll` | `IClassFixture<T>` |
| `@AfterAll` | `IClassFixture<T>` |
| `@ParameterizedTest` | `[Theory]` with `[InlineData]` |

---

## 🌐 **Web Automation**

### **Selenium vs Playwright**
```java
// Java Selenium
WebDriver driver = new ChromeDriver();
driver.get("https://example.com");
WebElement element = driver.findElement(By.id("username"));
element.sendKeys("testuser");
```

```csharp
// C# Playwright
var page = await browser.NewPageAsync();
await page.GotoAsync("https://example.com");
await page.FillAsync("#username", "testuser");
```

### **Page Object Model**
```java
// Java
public class LoginPage {
    @FindBy(id = "username")
    private WebElement usernameField;
    
    public void login(String username, String password) {
        usernameField.sendKeys(username);
    }
}
```

```csharp
// C#
public class LoginPage : BasePage 
{
    public LoginPage(IPage page) : base(page) { }
    
    public async Task LoginAsync(string username, string password) 
    {
        await Page.FillAsync("#username", username);
    }
}
```

---

## 🔌 **API Testing**

### **HTTP Clients**
```java
// Java (RestAssured)
given()
    .contentType(ContentType.JSON)
    .body(user)
.when()
    .post("/api/users")
.then()
    .statusCode(201)
    .body("name", equalTo("John"));
```

```csharp
// C# (RestSharp)
var request = new RestRequest("/api/users", Method.Post);
request.AddJsonBody(user);

var response = await client.ExecuteAsync<User>(request);

Assert.Equal(HttpStatusCode.Created, response.StatusCode);
Assert.Equal("John", response.Data.Name);
```

---

## 🗄️ **Database Testing**

### **ORM Comparison**
| Java | .NET |
|------|------|
| **Hibernate** | **Entity Framework Core** |
| **MyBatis** | **Dapper** (micro-ORM) |
| **JPA** | **EF Core** (similar concept) |

```java
// Java JPA
@Entity
public class User {
    @Id
    @GeneratedValue
    private Long id;
    
    @Column
    private String name;
}
```

```csharp
// C# EF Core
public class User 
{
    public long Id { get; set; }
    public string Name { get; set; }
}
```

---

## 📦 **Dependency Injection**

### **Framework Comparison**
| Java | .NET |
|------|------|
| **Spring** | **Built-in DI Container** |
| `@Autowired` | Constructor Injection (preferred) |
| `@Component` | `services.AddScoped<T>()` |
| `@Configuration` | `Startup.cs` or `Program.cs` |

```java
// Java Spring
@Service
public class UserService {
    @Autowired
    private UserRepository repository;
}
```

```csharp
// C# (Manual DI - Your Framework Style)
public class UserService 
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository) 
    {
        _repository = repository;
    }
}
```

---

## 🎯 **Best Practices for Transition**

### **Naming Conventions**
| Java | C# |
|------|-----|
| **Classes** | `PascalCase` | `PascalCase` ✅ |
| **Methods** | `camelCase` | `PascalCase` |
| **Variables** | `camelCase` | `camelCase` ✅ |
| **Constants** | `UPPER_SNAKE_CASE` | `PascalCase` |
| **Packages** | `lowercase.dot.separated` | `PascalCase.Dot.Separated` |

### **Memory Management**
- **Java**: Garbage Collection (automatic)
- **C#**: Garbage Collection + `IDisposable` pattern for resources
- **Key Point**: Use `using` statements for resource cleanup

### **Exception Handling**
```java
// Java
try {
    // risky code
} catch (IOException e) {
    // handle
} finally {
    // cleanup
}
```

```csharp
// C#
try 
{
    // risky code
} 
catch (IOException ex) 
{
    // handle
} 
finally 
{
    // cleanup
}
```

---

## 🚀 **Getting Started Checklist**

### **Development Environment**
- [ ] Install **.NET 9 SDK**
- [ ] Choose IDE: **Visual Studio** or **JetBrains Rider**
- [ ] Install **Playwright** browsers: `playwright install`
- [ ] Set up **Git** hooks for code formatting

### **Learning Path**
1. **Language Fundamentals**: C# syntax, LINQ, async/await
2. **Testing Framework**: xUnit patterns and fixtures
3. **Web Automation**: Playwright API and Page Object Model
4. **API Testing**: RestSharp and HTTP client patterns
5. **Database**: Entity Framework Core or Dapper

### **Key Mental Shifts**
- **Properties over getters/setters**: `public string Name { get; set; }`
- **LINQ over streams**: `users.Where(u => u.IsActive).ToList()`
- **async/await over CompletableFuture**: `await SomeAsyncMethod()`
- **using over try-finally**: `using var connection = new SqlConnection()`

---

## 💡 **Pro Tips**

1. **Use `var`** for type inference (like Java's `var`)
2. **Embrace async/await** - it's more mature than Java's async
3. **NuGet packages** are your friend - rich ecosystem
4. **Extension methods** add functionality to existing types
5. **Pattern matching** is powerful for conditionals
6. **Record types** for immutable data (Java records equivalent)

---

*Welcome to the .NET ecosystem! The transition from Java is smoother than you think.* 🎉