using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ModelStateExtensions
{
    public static string GetErrorMessages(this ModelStateDictionary dictionary)
    {
        return string.Join(Environment.NewLine, dictionary.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage));
    }
    public static void RemoveFor<TModel>(this ModelStateDictionary modelState,
                                       System.Linq.Expressions.Expression<Func<TModel, object>> expression)
    {
        string expressionText = System.Web.Mvc.ExpressionHelper.GetExpressionText(expression);

        foreach (var ms in modelState.ToArray())
        {
            if (ms.Key.StartsWith(expressionText + "."))
            {
                modelState.Remove(ms.Key);
            }
        }
    }
}