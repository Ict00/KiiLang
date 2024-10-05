using KiiLang.Execution;

namespace KiiLang;

public class Parser {

    public static Dictionary<string, List<IExpression>> Prioritize(Dictionary<string, List<IExpression>> box) {
        var result = new Dictionary<string, List<IExpression>>(); 
        foreach(var key in box.Keys) 
	    {
            result[key] = new();
            foreach(var item in box[key]) 
	        {
                if((item as Expression).One.GetType().Equals(typeof(Expression))) {
                    if((item as Expression).Operator.priority == 0) 
		            {
                        IExpression one = Pri((item as Expression).One as Expression);
                        result[key].Add(new Expression(one, (item as Expression).Two, (item as Expression).Operator));
                    }
                    else
		            {
                        IExpression @new = Pri(item as Expression);
                        
                        while((@new as Expression).Operator.priority != 0) {
                            @new = Pri(@new as Expression);
                        }
                        result[key].Add(@new);
                    }
                }
                else if((item as Expression).Two.GetType().Equals(typeof(Expression))) 
                {
                    if((item as Expression).Operator.priority == 0) 
                    {
                        IExpression two = Pri((item as Expression).Two as Expression);
                        result[key].Add(new Expression((item as Expression).One, two, (item as Expression).Operator));
                    }
                    else 
                    {
                        IExpression @new = Pri(item as Expression);
                        while((@new as Expression).Operator.priority != 0) 
                        {
                            @new = Pri(@new as Expression);
                        }
                        result[key].Add(@new);
                    }
                }
                else
		        {
                    result[key].Add(item);
                }

            }
        }


        return result;
    }


    private static IExpression Pri(Expression @base) {
        Expression newExpression = @base;
        try
        {
            
            if(@base.One.GetType().Equals(typeof(Expression))) {
                try
                {
                    if(@base.Operator.priority > (@base.One as Expression).Operator.priority) {
                        var smallNode = (@base.Two as Expression).One;
                        var bigNode = @base.Two as Expression;

                        Expression e = new Expression(@base.One, smallNode, @base.Operator);

                        newExpression.Operator = bigNode.Operator;
                        newExpression.One = e;
                        newExpression.Two = bigNode.Two;


                    }
                    else {
                        if(newExpression.Two.GetType().Equals(typeof(Expression))) {
                            newExpression.Two = Pri(newExpression.Two as Expression);
                        }
                    }
                }
                catch(Exception ex) {
                    var ctx = new KiiLang.Execution.Context() {CurrentBlock="main"};

                    if(@base.Operator.priority > (@base.One as Expression).Operator.priority) {
                            
                        var smallNode = (@base.One as Expression).One;
                        var bigNode = @base.One as Expression;

                        Expression e = new Expression() {
                            Operator = @base.Operator,
                            One = smallNode
                        };

                        newExpression.Operator = bigNode.Operator;
                        newExpression.One = e;
                        newExpression.Two = bigNode.Two;

                    }
                }
            }
            if(@base.Two.GetType().Equals(typeof(Expression))) {                    
                    if(@base.Operator.priority > (@base.Two as Expression).Operator.priority) {
                        
                        var smallNode = (@base.Two as Expression).One;
                        var bigNode = @base.Two as Expression;

                        Expression e = new Expression(@base.One, smallNode, @base.Operator);

                        newExpression.Operator = bigNode.Operator;
                        newExpression.One = e;
                        newExpression.Two = bigNode.Two;


                    }
                    else {
                        if(newExpression.Two.GetType().Equals(typeof(Expression))) {
                            newExpression.Two = Pri(newExpression.Two as Expression);
                        }
                    }
                }
            

            
        } catch (Exception ex) {Console.WriteLine(ex);}
        return newExpression;
    }

    public static Dictionary<string, List<IExpression>> Parse(Dictionary<string, List<RawToken>> box) {
        var result = new Dictionary<string, List<IExpression>>();
        foreach(var key in box.Keys) {
            result[key] = new();

            for(int i = 0; i < box[key].Count; i++) {
                if(!box[key][i].Type.Equals("EndLine"))
                {
                    if(i+1 <box[key].Count) {
                        if(!box[key][i+1].Type.Equals("EndLine")) {
                            result[key].Add(Expr(ref i, box[key]));
                        }
                    }
                }
            }
        }

        return result;
    }

    private static IExpression Expr(ref int i, List<RawToken> box) {
        
        Expression newExpr = new Expression();
        bool isBinary;
        if (ParserHelper.ops.ContainsKey(box[i].Content)) {
            isBinary = ParserHelper.ops[box[i].Content];
        }
        else {
            isBinary = !box[i].Type.Equals("Operator");
        }
        var ti = i;
        if(isBinary) {
            i+=1;
            newExpr.One = SingleExpression.Of(box[i-1].Content);
            try {
                if(box[i+2].Type.Equals("Operator") || box[i+1].Type.Equals("Operator")) {
                    ti = i;
                    i+=1;
                    newExpr.Two = Expr(ref i, box);
                }
                else if(box[i+2].Type.Equals("EndLine")) {
                    newExpr.Two = SingleExpression.Of(box[i+1].Content);
                    ti = i;
                }   
            }
            catch { }
        }
        else {
            try {
                if(box[i+2].Type.Equals("Operator")) {
                    ti = i;
                    i+=1;
                    newExpr.One = Expr(ref i, box);
                }
                else if(box[i+1].Type.Equals("Operator")) {
                    ti = i;
                    i+=1;
                    newExpr.One = Expr(ref i, box);
                }
                else {
                    newExpr.One = SingleExpression.Of(box[i+1].Content);
                }
            }
            catch { }
        }
        
        newExpr.Operator = Operators.ops[box[ti].Content];
        

        return newExpr;
    }
}

public class ParserHelper() {
    public static Dictionary<string, bool> ops = new();
}
