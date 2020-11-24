using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

"----------------".Dump("使用直接调用方式");
//使用直接调用方式
{
    //test1
    {
        var input = new CreateClaptrapInput();
        var (isOk, errorMessage) = PropertyValidationClass.ValidateTest1(input);
        isOk.Dump(nameof(isOk) + "=false");
        errorMessage.Dump(nameof(errorMessage) + "Missing Name");
    }
    //test2
    {
        var input = new CreateClaptrapInput
        {

            Name = "1"
        };
        var (isOk, errorMessage) = PropertyValidationClass.ValidateTest1(input);
        isOk.Dump(nameof(isOk) + "=false");
        errorMessage.Dump(nameof(errorMessage) + " Length of Name should be great than 3");
    }
    //test3
    {
        var input = new CreateClaptrapInput { Name = "wwmin" };
        var (isOk, errorMessage) = PropertyValidationClass.ValidateTest1(input);
        isOk.Dump(nameof(isOk) + "=true");
        errorMessage.Dump(nameof(errorMessage));
    }
}

static Func<CreateClaptrapInput, int, ValidateResult> _func;
#region

"----------------".Dump("使用表达式树 1");
{
    static void Init()
    {
        var method = typeof(PropertyValidationClass).GetMethod(nameof(PropertyValidationClass.ValidateCore));
        var pExp = Expression.Parameter(typeof(CreateClaptrapInput));
        var minLengthPExp = Expression.Parameter(typeof(int));
        var body = Expression.Call(method, pExp, minLengthPExp);
        var expression = Expression.Lambda<Func<CreateClaptrapInput, int, ValidateResult>>(body, pExp, minLengthPExp);
        _func = expression.Compile();
    }
    Init();
    {
        var input = new CreateClaptrapInput { Name = "wwmin" };
        var (isOk, errorMessage) = PropertyValidationClass.ValidateTest2(input);
        isOk.Dump(nameof(isOk) + "=true");
        errorMessage.Dump(nameof(errorMessage));
    }
}
#endregion
#region
"----------------".Dump("使用表达式树 2");
{
    static void Init(){
        var finalExpression = CreateCore();
        _func = finalExpression.Compile();
        Expression<Func<CreateClaptrapInput,int,ValidateResult>> CreateCore(){
            
        
        };
    }
}

#endregion


public static class PropertyValidationClass
{
    public static ValidateResult ValidateTest1(CreateClaptrapInput input)
    {

        return ValidateCore(input, 3);
    }

    public static ValidateResult ValidateTest2(CreateClaptrapInput input)
    {
        return _func.Invoke(input, 3);
    }

    public static ValidateResult ValidateCore(CreateClaptrapInput input, int minLength)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            return ValidateResult.Error("Missing Name");
        }
        if (input.Name.Length < minLength)
        {
            return ValidateResult.Error($"Length of Name should be great than {minLength}");
        }
        return ValidateResult.Ok();
    }

}


public class CreateClaptrapInput
{
    [Required] [MinLength(3)] public string? Name { get; set; }
}

public struct ValidateResult
{
    public bool IsOk { get; set; }
    public string ErrorMessage { get; set; }

    public void Deconstruct(out bool isOk, out string errorMessage)
    {
        isOk = IsOk;
        errorMessage = ErrorMessage;
    }

    public static ValidateResult Ok()
    {
        return new ValidateResult
        {
            IsOk = true
        };
    }

    public static ValidateResult Error(string errorMessage)
    {
        return new ValidateResult
        {
            IsOk = false,
            ErrorMessage = errorMessage
        };
    }
}