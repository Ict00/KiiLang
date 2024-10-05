namespace KiiLang.Execution;


public class Memory {
    public Dictionary<string, Variable> variables = new();

    public void Clean() {
        List<string> toRemove = new();

        foreach(var item in variables.Keys) {
            if(!variables[item].isStatic) {
                toRemove.Add(item);
            }
        }
        foreach(var item in toRemove) {
            variables.Remove(item);
        }
    }
}