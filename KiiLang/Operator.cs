using KiiLang.Execution;

namespace KiiLang;

public static class Operators {
    public static Dictionary<string, IOperator> ops = new();
}

public interface IOperator{
    public int priority {get;set;}
    public dynamic Do(Context context, dynamic[] args);
}

public class MultiplicationOperator : IOperator
{
    public int priority {get;set;} = 5;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            int x = 1;
            args.ToList().ForEach(y => x*=Convert.ToInt32(Ch.VarExist(context.CurrentBlock, y)));
            return x;
        } catch {
            throw new Exception("K0002");
        }
    }
}
public class DivisionOperator : IOperator
{
    public int priority {get;set;} = 5;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) / Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class Division2Operator : IOperator
{
    public int priority {get;set;} = 5;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToDecimal(Convert.ToDecimal(Ch.VarExist(context.CurrentBlock, args[0]))) / Convert.ToDecimal(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class AddOperator : IOperator
{
    public int priority {get;set;} = 4;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            int x = 0;
            args.ToList().ForEach(y => x+=Convert.ToInt32(Ch.VarExist(context.CurrentBlock, y)));
            return x;
        }
        catch {
            string x = "";
            args.ToList().ForEach(y => x+=Ch.VarExist(context.CurrentBlock, y));
            return x;
        }
    }
}

public class SubtractOperator : IOperator
{
    public int priority {get;set;} = 4;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) - Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        }
        catch {
            throw new Exception("K0002");
        }
    }
}

public class AssignOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        string name = Convert.ToString(args[0]);
        dynamic value = Ch.VarExist(context.CurrentBlock, args[1]);
        Executor.MemoryOfBlocks[context.CurrentBlock].variables[name] = new Variable(value);
        return value;
    }
}

public class AssignStaticOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        string name = args[0];
      	dynamic value = Ch.VarExist(context.CurrentBlock, args[1]);
        if(Executor.MemoryOfBlocks[context.CurrentBlock].variables.ContainsKey(name)) {
            if(!Executor.MemoryOfBlocks[context.CurrentBlock].variables[name].isStatic) {
                Executor.MemoryOfBlocks[context.CurrentBlock].variables[name] = new Variable(value, true);
            }
        }
        else { 
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[name] = new Variable(value, true);
        }
        
        return value;
    }
}

public class AssignAddOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Set(Convert.ToInt32(Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get())+Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1])));
        }
        catch {
            try {
                Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Set(Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get()+Ch.VarExist(context.CurrentBlock, args[1]));
            } catch {
                throw new Exception("K0002");
            }
        }
        return Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get();
    }
}

public class AssignSubtractionOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Set(Convert.ToInt32(Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get())-Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1])));
        }
        catch {
            throw new Exception("K0002");
        }
        return Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get();
    }
}

public class AssignMultiplyOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Set(Convert.ToInt32(Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get())*Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1])));
        }
        catch {
            throw new Exception("K0002");
        }
        return Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get();
    }
}

public class AssignDivisionOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Set(Convert.ToInt32(Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get())/Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1])));
        }
        catch {
            throw new Exception("K0002");
        }
        return Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]].Get();
    }
}

public class ReverseOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return !Convert.ToBoolean(Ch.VarExist(context.CurrentBlock, args[0]));
        }
        catch {
            throw new Exception("K0002");
        }
    }
}

public class PrintOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        Console.Write(Convert.ToString(Ch.VarExist(context.CurrentBlock, args[0])));
        
        return Ch.VarExist(context.CurrentBlock, args[0]);
    }
}

public class DotOperator : IOperator
{
    public int priority {get;set;} = 15;
    public dynamic Do(Context context, dynamic[] args)
    {
        return $"{Ch.VarExist(context.CurrentBlock, args[0].ToString())} {Ch.VarExist(context.CurrentBlock, args[1].ToString())}";
    }
}

public class PrintLnOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        Console.WriteLine(Convert.ToString(Ch.VarExist(context.CurrentBlock, args[0])));
        
        return Ch.VarExist(context.CurrentBlock, args[0]);
    }
}

public class AndListOperator : IOperator
{
    public int priority {get;set;} = 1;
    public dynamic Do(Context context, dynamic[] args)
    {
        var lst = new List<dynamic>();
        lst.Add(args[0]);

        if(args[1].GetType().Equals(typeof(List<dynamic>))) {
            foreach(dynamic item in (args[1] as List<dynamic>)) {
                lst.Add(item);
            }
        }
        else {
            lst.Add(args[1]);
        }

        

        return lst;
    }
}

public class CarryOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            if(args[1].GetType().Equals(typeof(string))) {
                if(Executor.Blocks.Contains(args[1])) {
                    if(args[0].GetType().Equals(typeof(List<dynamic>))) {
                        foreach(var item in (args[0] as List<dynamic>)) {
                            Executor.MemoryOfBlocks[args[1]].variables[item] = new Variable(Ch.VarExist(context.CurrentBlock, item));
                        }
                    }
                    else {  
                        Executor.MemoryOfBlocks[args[1]].variables[args[0]] = new Variable(Ch.VarExist(context.CurrentBlock, args[0]));
                    }
                }
                
            }
            else {
                if(args[1].GetType().Equals(typeof(List<dynamic>))) {
                    if(args[0].GetType().Equals(typeof(List<dynamic>))) 
                    {
                        foreach(var item in (args[0] as List<dynamic>)) {
                            foreach (var jtem in (args[1] as List<dynamic>)) {
                                if(Executor.Blocks.Contains(jtem)) {
                                    Executor.MemoryOfBlocks[jtem.ToString()].variables[item.ToString()] = new Variable(Ch.VarExist(context.CurrentBlock, item));
                                }
                                else {
                                    throw new Exception("K0004");
                                }
                            }
                        }
                    }
                    else {
                        foreach (var jtem in (args[1] as List<dynamic>)) {
                            if(Executor.Blocks.Contains(jtem)) {
                                Executor.MemoryOfBlocks[jtem].variables[args[0]] = new Variable(Ch.VarExist(context.CurrentBlock, args[0]));
                            }
                            else {
                                throw new Exception("K0004");
                            }
                        }
                    }
                }
                else {
                    throw new Exception("K0002");
                }
            }
            
        }
        catch(Exception ex) {
            Console.WriteLine(ex);
            throw new Exception("K0002");
        }
        return "^";
    }
}

public class CarryRunOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            new CarryOperator().Do(context, args);
            Executor.Exec(args[1]);
        }
        catch {
            throw new Exception("K0002");
        }
        return "^";
    }
}
public class RunOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            if(Executor.Blocks.Contains(args[0]))
            {
                Executor.Exec(args[0]);
            }
        }
        catch {
            throw new Exception("K0002");
        }
        return "^";
    }
}

public class DelOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            if(args[0].GetType().Equals(typeof(List<dynamic>))) {
                foreach(var item in args[0] as List<dynamic>) {
                    if(Executor.MemoryOfBlocks[context.CurrentBlock].variables.ContainsKey(item)) {
                        Executor.MemoryOfBlocks[context.CurrentBlock].variables.Remove(item);
                    }
                }
            }
            else {
                if(Executor.MemoryOfBlocks[context.CurrentBlock].variables.ContainsKey(args[0])) {
                    Executor.MemoryOfBlocks[context.CurrentBlock].variables.Remove(args[0]);
                }
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex);
            throw new Exception("K0002");
        }
        return "^";
    }
}

public class RunIfOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {

            if (args[1].GetType().Equals(typeof(List<dynamic>)))
            {

                if (Ch.VarExist(context.CurrentBlock, args[0]))
                {
                    if (Executor.Blocks.Contains((args[1] as List<dynamic>)[1]))
                    {
                        Executor.Exec((args[1] as List<dynamic>)[1]);
                    }
                    else
                    {
                        throw new Exception("K0004");
                    }
                }
                else
                {
                    if (Executor.Blocks.Contains((args[1] as List<dynamic>)[0]))
                    {
                        Executor.Exec((args[1] as List<dynamic>)[0]);
                    }
                    else
                    {
                        throw new Exception("K0004");
                    }
                }
            }
            else
            {
                if (!Ch.VarExist(context.CurrentBlock, args[0]))
                {
                    if (Executor.Blocks.Contains(args[1]))
                    {
                        Executor.Exec(args[1]);
                    }
                    else
                    {
                        throw new Exception("K0004");
                    }
                }
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex);
            throw new Exception("K0002");
        }
        return "^";
    }
}

public class MoreOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) > Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch(Exception ex) {
            Console.WriteLine(ex);
            throw new Exception("K0002");
        }
    }
}

public class LessOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) < Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class EqualOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Ch.VarExist(context.CurrentBlock, args[0]).Equals(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class NotEqualOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return !Ch.VarExist(context.CurrentBlock, args[0]).Equals(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class EqualMoreOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) >= Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}
public class EqualLessOperator : IOperator
{
    public int priority {get;set;} = 3;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0])) <= Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class BoolAndOperator : IOperator
{
    public int priority {get;set;} = 2;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Ch.VarExist(context.CurrentBlock, args[0]) && Ch.VarExist(context.CurrentBlock, args[1]);
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class BoolOrOperator : IOperator
{
    public int priority {get;set;} = 2;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            return Ch.VarExist(context.CurrentBlock, args[0]) || Ch.VarExist(context.CurrentBlock, args[1]);
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class InOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[0]] = new Variable(Console.ReadLine());
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class StopOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            Environment.Exit(Convert.ToInt32(args[0]));
            return args[0];
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class StringLengthOperator : IOperator
{
    public int priority {get;set;} = 6;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic operand = Ch.VarExist(context.CurrentBlock, args[0]);
            if(operand.GetType().Equals(typeof(List<dynamic>))) {
                return (operand as List<dynamic>).Count;
            }
            else 
            {
                return (operand as string).Length;
            }
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class IntervalOperator : IOperator
{
    public int priority {get;set;} = 15;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            List<dynamic> result = new();
            var start = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0]));
            var end = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[1]));
	    for(int i = start; i <= end; i++)
	    {
		result.Add(i);
	    }
            return result;
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class BelongOperator : IOperator
{
    public int priority {get;set;} = 4;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic val = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0]));
            List<dynamic> list = (Ch.VarExist(context.CurrentBlock, args[1]) as List<dynamic>);
            return list.Contains(val);
        } catch {
            throw new Exception("K0002");
        }
    }
}



public class NotBelongOperator : IOperator
{
    public int priority {get;set;} = 4;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic val = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0]));
            List<dynamic> list = (Ch.VarExist(context.CurrentBlock, args[1]) as List<dynamic>);
            return !list.Contains(val);
        } catch {
            throw new Exception("K0002");
        }
    }
}



public class ImportOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            string val = Convert.ToString(Ch.VarExist(context.CurrentBlock, args[0]));
            if(File.Exists(val)) {
                string code = File.ReadAllText(val);
                Executor.Init(Parser.Prioritize(Parser.Parse(Lexer.Tokenize2(Lexer.RawTokenize(KiiBlockFinder.FindIn(code))))));
                Executor.ImportedFiles.Add(val);
            }
            else {
                throw new Exception("K0005");
            }
            return "^";
        } catch(Exception ex) {
            Console.WriteLine(ex);
            throw new Exception("K0002");
        }
    }
}

public class UnImportOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            string val = Convert.ToString(Ch.VarExist(context.CurrentBlock, args[0]));
            if(Executor.ImportedFiles.Contains(val)) {
                Executor.ImportedFiles.Remove(val);
            }
            else {
                throw new Exception("K0006");
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class ReloadOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            foreach(var block in Executor.Blocks) {
                if(!block.Equals(context.CurrentBlock)) {
                    Executor.AllTheBlocks.Remove(block);
                    Executor.MemoryOfBlocks.Remove(block);
                }
            }
            foreach(var item in Executor.ImportedFiles) {
                string code = File.ReadAllText(item);
                var parsed = Parser.Prioritize(Parser.Parse(Lexer.Tokenize2(Lexer.RawTokenize(KiiBlockFinder.FindIn(code)))));
                Executor.OverrideInit(parsed);
            }
            Console.WriteLine("Reload completed!");
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class LoopOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            string val = Convert.ToString(Ch.VarExist(context.CurrentBlock, args[0]));
            if(Executor.Blocks.Contains(val)) {
                while(true) {
                    Executor.Exec(val);
                }
            }
            else {
                throw new Exception("K0004");
            }
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class ForeachOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            List<dynamic> something = args[0] as List<dynamic>;
            string val = Convert.ToString(Ch.VarExist(context.CurrentBlock, args[1]));
            if(Executor.Blocks.Contains(val)) {
                while(true) {
                    Executor.Exec(val);
                }
            }
            else {
                throw new Exception("K0004");
            }
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class GetByIndexOperator : IOperator
{
    public int priority {get;set;} = 5;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic list = Ch.VarExist(context.CurrentBlock, args[1]);
            int index = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0]));
            if(list.GetType().Equals(typeof(List<dynamic>))) {
                return (list as List<dynamic>)[index];
            }
            else {
                throw new Exception("K0002");
            }
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class GetLastOperator : IOperator
{
    public int priority {get;set;} = 5;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic list = Ch.VarExist(context.CurrentBlock, args[0]);
            if(list.GetType().Equals(typeof(List<dynamic>))) {
                return (list as List<dynamic>).Last();
            }
            else {
                throw new Exception("K0002");
            }
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class AddInOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic list = Ch.VarExist(context.CurrentBlock, args[1]);
            dynamic value = Ch.VarExist(context.CurrentBlock, args[0]);
            if(list.GetType().Equals(typeof(List<dynamic>))) {
                (list as List<dynamic>).Add(value);
                Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[1]] = new Variable(list);
            }
            else {
                throw new Exception("K0002");
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class RemoveAtOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic list = Ch.VarExist(context.CurrentBlock, args[1]);
            int value = Convert.ToInt32(Ch.VarExist(context.CurrentBlock, args[0]));
            if(list.GetType().Equals(typeof(List<dynamic>))) {
                (list as List<dynamic>).RemoveAt(value);
                Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[1]] = new Variable(list);
            }
            else {
                throw new Exception("K0002");
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class RemoveSpecificOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic list = Ch.VarExist(context.CurrentBlock, args[1]);
            dynamic value = Ch.VarExist(context.CurrentBlock, args[0]);
            if(list.GetType().Equals(typeof(List<dynamic>))) {
                (list as List<dynamic>).Remove(value);
                Executor.MemoryOfBlocks[context.CurrentBlock].variables[args[1]] = new Variable(list);
            }
            else {
                throw new Exception("K0002");
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class GetFromCrateOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic block = Ch.VarExist(context.CurrentBlock, args[1]);
            dynamic vars = args[0];
            if(Executor.Blocks.Contains(block)) {
                var ctx = new Context() {
                    CurrentBlock = block
                };
                new CarryOperator().Do(ctx, [vars, context.CurrentBlock]);
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}

public class UnloadBlocksOperator : IOperator
{
    public int priority {get;set;} = 0;
    public dynamic Do(Context context, dynamic[] args)
    {
        try {
            dynamic block = Ch.VarExist(context.CurrentBlock, args[0]);
            if(block.GetType().Equals(typeof(List<dynamic>))) {
                foreach(var item in (block as List<dynamic>)) {
                    if(Executor.Blocks.Contains(item) && !item.Equals(context.CurrentBlock)) {
                        Executor.UnInit(item);
                    }
                }
            }
            else {
                if(Executor.Blocks.Contains(block) && !block.Equals(context.CurrentBlock)) {
                    Executor.UnInit(block);
                }
            }
            return "^";
        } catch {
            throw new Exception("K0002");
        }
    }
}
