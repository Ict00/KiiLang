namespace KiiLang;
public static class Debug {
    public static string spaces(int times) {
        string e = "";
        for(int i = 0; i < times; i++) {
            e += " ";
        }
        return e;
    }

    public static void Out(IExpression y, int spaceLevel) {
        var ctx = new KiiLang.Execution.Context() {CurrentBlock="main"};
        if(y.GetType().Equals(typeof(Expression))) {
            try {
                Console.WriteLine($"{spaces(spaceLevel)}{(y as Expression).Operator} {(y as Expression).Operator.priority}");

                if((y as Expression).One.GetType().Equals(typeof(Expression))) {
                    Out((y as Expression).One, spaceLevel+1);
                }
                else {
                    Console.WriteLine($"{spaces(spaceLevel)}{(y as Expression).One.Do(ref ctx)}");
                }

                if((y as Expression).Two.GetType().Equals(typeof(Expression))) {
                    Out((y as Expression).Two, spaceLevel+1);
                }
                else {
                    Console.WriteLine($"{spaces(spaceLevel)}{(y as Expression).Two.Do(ref ctx)}");
                }
            }
            catch {

            }
        }
        else {
            Console.WriteLine($"{spaces(spaceLevel)}{y.Do(ref ctx)}");
        }
    }
}
