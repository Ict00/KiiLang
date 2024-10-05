namespace KiiLang.Execution;

public class Ch {
    public static dynamic VarExist(string block, dynamic name) {
        if(name.GetType().Equals(typeof(string))) {

            if(Executor.MemoryOfBlocks[block].variables.ContainsKey(name)) {
                return Executor.MemoryOfBlocks[block].variables[name].Get();
            }
            else {
                if(name == "true") {return true;}
                if(name == "false") {return false;}
                return name;
            }
        }
        else {
            return name;
        }
    }
}