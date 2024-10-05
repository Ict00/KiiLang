namespace KiiLang.Execution;

public static class Executor
{
    public static List<string> Blocks = new();
    public static Dictionary<string, Memory> MemoryOfBlocks = new();

    public static Dictionary<string, List<IExpression>> AllTheBlocks = new();

    public static List<string> ImportedFiles = new();

    public static List<string> BlocksInUse = new();

    public static void ReInit(Dictionary<string, List<IExpression>> expressions) {
        AllTheBlocks = expressions;
        Blocks = expressions.Keys.ToList();
        foreach(var item in Blocks) {
            if(!MemoryOfBlocks.ContainsKey(item)) {
                MemoryOfBlocks.Add(item, new());
            }
        }
    }

    public static void UnInit(string block) {
        Blocks.Remove(block);
        MemoryOfBlocks.Remove(block);
        AllTheBlocks.Remove(block);
    }

    public static void OverrideInit(Dictionary<string, List<IExpression>> expressions) {
        foreach(var item in expressions) {
            if(!BlocksInUse.Contains(item.Key)) {
                AllTheBlocks[item.Key] = item.Value;
            }
        }
        foreach(var item in expressions.Keys.ToList()) {
            if(!Blocks.Contains(item)) {
                Blocks.Add(item);
            }
        }
        foreach(var item in Blocks) {
            if(!BlocksInUse.Contains(item)) {
                MemoryOfBlocks[item] = new();
            }
        }
    }

    public static void Init(Dictionary<string, List<IExpression>> expressions) {
        foreach(var item in expressions) {
            if(!AllTheBlocks.ContainsKey(item.Key)) {
                AllTheBlocks.Add(item.Key, item.Value);
            }
        }
        foreach(var item in expressions.Keys.ToList()) {
            if(!Blocks.Contains(item)) {
                Blocks.Add(item);
            }
        }
        foreach(var item in Blocks) {
            if(!MemoryOfBlocks.ContainsKey(item)) {
                MemoryOfBlocks.Add(item, new());
            }
        }
    }

    public static void Exec(string block) {
        if(Blocks.Contains($"pre-{block}")) {
            Exec($"pre-{block}");
        }
        BlocksInUse.Add(block);
        foreach(var item in AllTheBlocks[block]) {
            var ctx = new Context() {
                CurrentBlock = block
            };
            try {
                item.Do(ref ctx);
            }
            catch(Exception ex) {
                if(ex.Message.StartsWith("K")) {
                    int line = AllTheBlocks[block].IndexOf(item) + 1;
                    Console.WriteLine($"ERROR {ex.Message} at {line} of {block}");
                }
                else {
                    Console.WriteLine(ex);
                }
                Environment.Exit(-1);
            }
            
        }
        if(!block.Contains("crate-")) {
            MemoryOfBlocks[block].Clean();
        }
        BlocksInUse.Remove(block);
    }

    public static void Launch() {
        if(Blocks.Contains("main")) {
            Exec("main");
        }
        else {
            Console.WriteLine("K0003");
        }
    }
}