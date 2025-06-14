# Programming Task: Generic AllureHelper Method

## Task: Implement Generic AllureHelper Method
**Objective:** Refactor AllureHelper to support any API model type  
**Time:** 30 minutes  
**Level:** Intermediate

### Current Problem
The `AllureHelper.AttachString(string name, ProductRequest request)` method in `/Tests/Helpers/AllureHelper.cs:33` is hardcoded to only accept ProductRequest objects, limiting its reusability with other API models like LoginRequest.

### Solution Requirements
Replace the ProductRequest-specific method with a generic version that can work with any API request model.

### Implementation Steps

1. **Open the AllureHelper class**
   - File: `Tests/Helpers/AllureHelper.cs`
   - Locate the ProductRequest-specific AttachString method around line 33

2. **Replace with Generic Method**
   ```csharp
   /// <summary>
   /// Serializes any object and attaches it as a JSON body to the Allure report.
   /// </summary>
   /// <param name="name">The name of the attachment.</param>
   /// <param name="requestModel">The object to serialize and attach.</param>
   public static void AttachString<T>(string name, T requestModel) where T : class
   {
       AllureApi.Step($"Attach: {name}", () =>
       {
           AllureApi.AddAttachment("Request Body", "application/json", 
               Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestModel)));
       });
   }
   ```

3. **Optional Enhancement - Use JsonHelper for Consistency**
   ```csharp
   public static void AttachString<T>(string name, T requestModel) where T : class
   {
       AllureApi.Step($"Attach: {name}", () =>
       {
           var jsonContent = JsonHelper.Serialize(requestModel);
           AllureApi.AddAttachment("Request Body", "application/json", 
               Encoding.UTF8.GetBytes(jsonContent));
       });
   }
   ```

4. **Verify Compatibility**
   - Check existing usage in `Tests/Tests.API/ProductApiTests.cs`
   - Ensure the method still works with ProductRequest objects
   - Test with other API models (LoginRequest, etc.)

### What You'll Learn
- **Generic methods** with type constraints (`where T : class`)
- **Code reusability** principles
- **Method overloading** vs. generics
- **Consistent framework usage** (JsonHelper vs JsonConvert)
- **Refactoring for maintainability**

### Expected Results
✅ Method accepts any class object type  
✅ Existing ProductRequest usage continues to work  
✅ New API models can use the same attachment method  
✅ Consistent JSON serialization across the framework  

### Testing
Test the generic method with:
- ProductRequest objects
- LoginRequest objects  
- Any other API model classes

### Benefits
- **Improved reusability** - works with all API models
- **Consistent serialization** - uses framework utilities
- **Type safety** - generic constraints prevent misuse
- **Backward compatibility** - existing code continues to work