namespace KiiLang;

public class KiiBlockFinder {
    public static Dictionary<string, List<string>> FindIn(string code) {
        var result = new Dictionary<string, List<string>>();

        string name = "main";
        result[name] = new();
        foreach(var line in code.Split("\n")) {
            if(!line.StartsWith("//"))
            {
                if(line.EndsWith(':')) {
                    name = line.Replace(":", "");
		    if(!result.ContainsKey(name))
		    {
                    	result[name] = new();
		    }
                }
                else {
                    
                    result[name].Add(line);
                }
            }
        }

        return result;
    }
}
