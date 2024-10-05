namespace KiiLang;


public class RawToken(string content, string @type)
{
    public string Content = content;
    public string @Type = @type;
}

public class HalfToken(object content, string @type) {
    public object Content = content;
    public string @Type = @type;
}