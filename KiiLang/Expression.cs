using KiiLang.Execution;

namespace KiiLang;

public interface IExpression
{
    public dynamic Do(ref Context context);
}

public class Expression : IExpression 
{
    public IExpression One;
    public IExpression Two;
    public IOperator Operator;

    public Expression() 
    {
        One = SingleExpression.Of(0);
        Two = SingleExpression.Of(0);
        Operator = new AddOperator();
    }

    public Expression(IExpression one, IExpression two, IOperator @operator)
    {
        One = one;
        Two = two;
        Operator = @operator;
    }

    public dynamic Do(ref Context context)
    {
        try 
	{
            return Operator.Do(context, [One.Do(ref context), Two.Do(ref context)]);
        }
        catch(Exception ex)
	{
            return Operator.Do(context, [One.Do(ref context)]);

        }
    }
}

public class SingleExpression : IExpression 
{
    dynamic content;

    private SingleExpression(dynamic content) 
    {
        this.content = content;
    }

    public static SingleExpression Of(dynamic something) 
    {
        return new SingleExpression (something);
    }

    public dynamic Do(ref Context context) 
    {
        return content;
    }
}
