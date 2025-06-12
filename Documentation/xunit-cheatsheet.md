# xUnit Cheat Sheet

## Test Attributes

* `[Fact]`: Single test method.
* `[Theory]`: Parameterized tests.

## Data Attributes (for `[Theory]`)

* `[InlineData(1, 2, 3)]`: Inline parameters.
* `[MemberData(nameof(DataMethod))]`: Data from a method or property.
* `[ClassData(typeof(DataClass))]`: Data from a separate class implementing `IEnumerable<object[]>`.

## Setup & Teardown

* **Constructor**: Runs before each test method.
* **Dispose (implement IDisposable)**: Runs after each test method.

## Skipping Tests

* `[Skip("Reason")]`

## Categorization

* `[Trait("Category", "Unit")]`

## Collections (Parallelism Control)

* `[Collection("CollectionName")]`: Tests in the same collection run sequentially.
* `[CollectionDefinition("CollectionName")]`: Defines a collection; usually combined with `ICollectionFixture<T>`.

## Fixtures

* `IClassFixture<T>`: Shared fixture for all tests in one class.
* `ICollectionFixture<T>`: Shared fixture across multiple test classes (collection).

## Example: `[Theory]` with `[MemberData]`

```csharp
public static IEnumerable<object[]> AdditionData =>
    new List<object[]>
    {
        new object[] { 1, 2, 3 },
        new object[] { 2, 3, 5 }
    };

[Theory]
[MemberData(nameof(AdditionData))]
public void AddTests(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

## Example: Collection and Fixtures

```csharp
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

public class DatabaseFixture : IDisposable
{
    public void Dispose() { /* Cleanup */ }
}

[Collection("Database")]
public class MyDbTests
{
    private readonly DatabaseFixture _fixture;

    public MyDbTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
}
```

Use this cheat sheet to quickly reference xUnit testing concepts and practices.
