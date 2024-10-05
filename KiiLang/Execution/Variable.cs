namespace KiiLang.Execution;

public class Variable {
    dynamic content;
    public Types type;
    public bool isStatic = false;

    public void Set(dynamic val) {
        content = val;
    }


    public Variable(string content) {
        this.content = content;
        type = Types.String;
    }

    public Variable(bool content) {
        this.content = content;
        type = Types.Boolean;
    }

    public Variable(int content) {
        this.content = content;
        type = Types.Int;
    }

    public Variable(object content) {
        this.content = content;
        type = Types.Null;
    }

    public Variable(List<dynamic> content) {
        this.content = content;
        type = Types.List;
    }

    public Variable(object content, bool isStatic) {
        this.content = content;
        this.isStatic = isStatic;
        type = Types.Null;
    }

    public dynamic Get() {
        switch(type) {
            case Types.String:
                return content.ToString();
            case Types.Boolean:
                if(content.ToString() == "true") {return true;}
                return false;
            case Types.Int:
                return Convert.ToInt32(content);
            case Types.List:
                return content as List<dynamic>;
            case Types.Null:
                return content;
            default: goto case Types.Null;
        }
    }
}

public enum Types {
    Int,
    String,
    Boolean,
    List,
    Null
}