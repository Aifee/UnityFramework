using System.Collections;
using System.Collections.Generic;
public class LanguageConfig: Config {
    public string id;
    public string cn;
    public string en;
    public string tw;
    public LanguageConfig()
    {
        tableName = "LanguageConfig";
    }
}
