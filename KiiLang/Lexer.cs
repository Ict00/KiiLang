

namespace KiiLang;

public class Lexer {

    private static string transform(string other) {
        return other.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\x1b", "\x1b").Replace("\\\"", "\"");
    }

    public static Dictionary<string, List<RawToken>> RawTokenize(Dictionary<string, List<string>> blocks) {
        var result = new Dictionary<string, List<RawToken>>();
        
        foreach(string key in blocks.Keys) {
            result[key] = new();
            foreach(var line in blocks[key]) {
                string contents = "";
                bool writingString = false;
                for(int i = 0; i < line.Length; i++) {
                    var symbol = line[i];
                    switch(symbol) {
                        case '"':
                            if(writingString) {
                                if(line[i-1]!='\\') 
                                {
                                    writingString = !writingString;
                                    result[key].Add(new(transform(contents), "String"));
                                    contents = "";
                                }
                                else {
                                    contents += "\"";
                                }
                            }
                            else {
                                writingString = true;
                            }
                            break;
                        case ' ':
                            if(!writingString) {
                                if (!string.IsNullOrWhiteSpace(contents))
                                {
                                    result[key].Add(new(contents, "Identifier"));
                                    contents = "";
                                }
                            }
                            else {
                                contents += " ";
                            }
                            break;
                        case '.':
                            if(!writingString) {
                                if (!string.IsNullOrWhiteSpace(contents))
                                {
                                    result[key].Add(new(contents, "Identifier"));
                                    contents = "";
                                    result[key].Add(new(".", "Operator"));
                                }
                            }
                            else {
                                contents += ".";
                            }
                            break;
                        default:
                            contents += symbol;
                            break;
                    }
                }
                if(contents != "" && contents != "\0" && contents != "\r") {
                    if(!writingString) {
                        result[key].Add(new(contents, "Identifier"));
                    }
                    else {
                        result[key].Add(new(transform(contents), "String"));
                    }
                }
                result[key].Add(new(";", "EndLine"));
            }
        }

        return result;
    }

    public static Dictionary<string, List<RawToken>> Tokenize2(Dictionary<string, List<RawToken>> raw) {
        var result = new Dictionary<string, List<RawToken>>();

        foreach(var key in raw.Keys) {
            result[key] = new();
            foreach(var token in raw[key]) {
                var newToken = token;

                if(Operators.ops.ContainsKey(newToken.Content) && newToken.Type != "String") {
                    newToken.Type = "Operator";
                }
                else {
                    bool isBlockReference = false;
                    raw.Keys.ToList().ForEach(x => {
                        if(isBlockReference == false) {
                            isBlockReference = newToken.Content.Contains(x);
                        }
                    });
                    if(isBlockReference) {
                        newToken.Type = "BlockReference";
                    }
                }
                int t1;

                if(int.TryParse(newToken.Content, out t1)) {
                    newToken.Type = "Integer";
                }
                
                
                if(newToken.Content.Contains(',') && !newToken.Type.Equals("String")) {
                    newToken.Content = newToken.Content.Replace(",", "");
                    result[key].Add(newToken);
                    result[key].Add(new RawToken(",", "Operator"));
                }
                else if(newToken.Content.Contains("..") && !newToken.Type.Equals("String") && !newToken.Type.Equals("Operator")) {
                    newToken.Content = newToken.Content.Replace("..", " ");
                    if(newToken.Content.Split(" ").ToList().Count == 2) {
                        result[key].Add(new RawToken(newToken.Content.Split()[0], "Identifier"));
                        result[key].Add(new RawToken("..", "Operator"));
                        result[key].Add(new RawToken(newToken.Content.Split()[1], "Identifier"));
                    }
                    else {
                        result[key].Add(newToken);
                        result[key].Add(new RawToken("..", "Operator"));
                    }
                    
                }
                else {
                    if(newToken.Content != "") 
                    {
                        result[key].Add(newToken);
                    }
                }
                
            }
        }

        return result;
    }
}
