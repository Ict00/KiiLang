using KiiLang;
using KiiLang.Execution;
// Set operators up
ParserHelper.ops.Add("=", true);
ParserHelper.ops.Add("-", true);
ParserHelper.ops.Add("len", false);
ParserHelper.ops.Add("==", true);
ParserHelper.ops.Add("runif", true);

// Add operators
Operators.ops.Add("+", new AddOperator());
Operators.ops.Add("-", new SubtractOperator());
Operators.ops.Add("=", new AssignOperator());
Operators.ops.Add("#=", new AssignStaticOperator());
Operators.ops.Add("+=", new AssignAddOperator());
Operators.ops.Add("-=", new AssignSubtractionOperator());
Operators.ops.Add("*=", new AssignMultiplyOperator());
Operators.ops.Add("/=", new AssignDivisionOperator());
Operators.ops.Add("*", new MultiplicationOperator());
Operators.ops.Add("/", new DivisionOperator());
Operators.ops.Add("!", new ReverseOperator());
Operators.ops.Add("out", new PrintOperator());
Operators.ops.Add("outln", new PrintLnOperator());
Operators.ops.Add("print", new PrintOperator());
Operators.ops.Add("println", new PrintLnOperator());
Operators.ops.Add(",", new AndListOperator());
Operators.ops.Add("->", new CarryOperator());
Operators.ops.Add("=>", new CarryRunOperator());
Operators.ops.Add("<-", new GetFromCrateOperator());
Operators.ops.Add("run", new RunOperator());
Operators.ops.Add("del", new DelOperator());
Operators.ops.Add("runif", new RunIfOperator());
Operators.ops.Add(">=", new EqualMoreOperator());
Operators.ops.Add("<=", new EqualLessOperator());
Operators.ops.Add("==", new EqualOperator());
Operators.ops.Add("!=", new NotEqualOperator());
Operators.ops.Add("&&", new BoolAndOperator());
Operators.ops.Add("||", new BoolOrOperator());
Operators.ops.Add(">", new MoreOperator());
Operators.ops.Add("<", new LessOperator());
Operators.ops.Add("in", new InOperator());
Operators.ops.Add("equit", new StopOperator());
Operators.ops.Add("len", new StringLengthOperator());
Operators.ops.Add("..", new IntervalOperator());
Operators.ops.Add("=)", new BelongOperator());
Operators.ops.Add("!=)", new NotBelongOperator());
Operators.ops.Add("import", new ImportOperator());
Operators.ops.Add("unimport", new UnImportOperator());
Operators.ops.Add("loop", new LoopOperator());
Operators.ops.Add("at", new GetByIndexOperator());
Operators.ops.Add(".", new DotOperator());
Operators.ops.Add("|", new Division2Operator());
Operators.ops.Add("lastof", new GetLastOperator());
Operators.ops.Add("[+]", new AddInOperator());
Operators.ops.Add("[-]", new RemoveSpecificOperator());
Operators.ops.Add("-=]", new RemoveAtOperator());
Operators.ops.Add("unload", new UnloadBlocksOperator());
Operators.ops.Add("reload", new ReloadOperator());

string code = "main:\noutln \"Wrong arguments\"";
string file = "main.kii";
if(args.Length == 0)
{
    if(File.Exists("main.kii")) 
    {
        code = File.ReadAllText("main.kii").Replace("\r", "");
    }
}
else {
    if(File.Exists(args[0])) 
    {
        file = args[0];
        code = File.ReadAllText(args[0]).Replace("\r", "");
    }
}

Executor.ImportedFiles.Add(file);


var b = Lexer.Tokenize2(Lexer.RawTokenize(KiiBlockFinder.FindIn(code)));
// foreach(var key in b.Keys) {
//       Console.WriteLine($"\x1b[38;5;121m=    {key} \x1b[38;5;156mBlock\x1b[0m");
//     foreach(var item in b[key]) {
//         Console.WriteLine($"\x1b[38;5;159m-  {item.Content} \x1b[38;5;156m{item.Type}\x1b[0m");
//     }
// }
var e = Parser.Prioritize(Parser.Parse(b));

Executor.ReInit(e);
Executor.Launch();