# C# Cheat Sheet

---

## Inheritance
- **Definition:** When one class (derived/child) acquires the properties and behaviors of another (base/parent).
- **Benefits:**  
  - Manages information hierarchically  
  - Promotes code reuse  

- **Key Concepts:**  
  - **Base (Super) Class**  
  - **Derived (Sub) Class**  

- **Types of Inheritance:**  
  1. **Single-Level:**  
     ```
     BaseClass → DerivedClass
     ```  
  2. **Hierarchical:**  
     ```
     BaseClass
       ├─ DerivedA
       └─ DerivedB
     ```  
  3. **Multilevel:**  
     ```
     BaseClass → Derived1 → Derived2
     ```  
  4. **Multiple (via interfaces only)**  

---

## Constructors
- Special methods named after the class, with **no return type**.  
- Automatically invoked when a new object is created.  
- **Purpose:** Initialize object state.

- **Types:**  
  1. **Default constructor** (auto-added by C# if none defined)  
  2. **No-args constructor** (explicit, takes no parameters)  
  3. **Parameterized constructor** (takes one or more parameters)

```csharp
public class Person
{
    public string Name { get; set; }

    // No-args constructor
    public Person()
    {
        Name = "Unknown";
    }

    // Parameterized constructor
    public Person(string name)
    {
        Name = name;
    }
}
```

---

## Polymorphism
> “Many forms” — same method name, different behaviors.

### 1. Compile-Time (Static) / Overloading
- Multiple methods **same name**, **different signatures**.  
- Varies by number/type/order of parameters.  
- **Not** achievable by return type alone (causes ambiguity).

```csharp
public void Print() { … }
public void Print(string s) { … }
public void Print(int x, int y) { … }
```

### 2. Run-Time (Dynamic) / Overriding
- Derived class **overrides** a base class method.  
- Requires `virtual` in base and `override` in derived.  
- Enables polymorphic references:
  ```csharp
  IWebDriver driver = new ChromeDriver();
  ```

#### Quick Checklist
- Add `virtual` in base methods.  
- Use `override` in derived classes.  
- Seal overrides with `sealed override` to prevent further customization.  
- Don’t confuse `new` (hiding) with `override`.

---

## Abstraction & Interfaces

### Abstract Classes
- **Cannot** be instantiated.  
- Can contain both abstract (no body) and concrete methods.  
- May define constructors.

```csharp
public abstract class Animal
{
    public abstract void Speak();
    public void Eat() { … }
}
```

### Interfaces
- 100% abstract: only method/property signatures.  
- No constructors, no method bodies (until C# 8 default interface methods).  
- Support multiple inheritance (a class can implement multiple interfaces).

```csharp
public interface IFlyable
{
    void Fly();
}
```

---

## Collections

### Non-Generic vs Generic
- **Non-Generic** (e.g., `ArrayList`, `Hashtable`): for legacy code.  
- **Generic** (`List<T>`, `Dictionary<TKey,TValue>`): type-safe, preferred.

### HashSet<T>
- Stores **unique** values.  
- Unordered.

### Dictionary<TKey,TValue>
- Key‐value pairs.  
- Fast lookups by key.

---

## Exceptions
- **try**: Wrap code that may throw.  
- **catch**: Handle specific exceptions.  
- **finally**: Executes always, for cleanup.  
- **throw**: Explicitly raise an exception.

```csharp
try
{
    // risky code
}
catch (IOException ex)
{
    // handle I/O errors
}
finally
{
    // always runs
}
```

- **Custom Exceptions:** Derive from `Exception` for domain-specific errors.

---

## Multithreading

### ConcurrentDictionary<TKey,TValue>
- Thread-safe, high-performance dictionary.  
- Allows multiple threads to read/write without explicit locks.

```csharp
var dict = new ConcurrentDictionary<int, string>();
dict.TryAdd(1, "One");
```

---

## using Keyword
- Ensures disposal of `IDisposable` objects.  
- Syntax:
  ```csharp
  using (var stream = new FileStream(path, FileMode.Open))
  {
      // use stream
  } // automatically calls stream.Dispose()
  ```
- C# 8.0+ **using declaration**:
  ```csharp
  using var conn = new SqlConnection(connString);
  // conn is disposed at end of scope
  ```
### XUnit VS NUnit
NUnit is more natural for using static or class-level shared test context and gives you more flexibility with shared setup and teardown.

xUnit is intentionally designed to avoid shared state (favoring fixtures for shared resources) to maximize test isolation and safety in parallel execution.

If your testing style requires static/shared context, NUnit may be a better fit than xUnit.