using System;
using UnityEngine;
using System.Text.RegularExpressions;

public class SimpleRegex : MonoBehaviour{
    private string realNumberRegex = "([-+]?[0-9]*\\.?[0-9]+)"; // need double \ as \ is an escape character in javascript strings
    private string integerRegex = "([-+]?[0-9]+)";
    private string nameRegex = "([A-Za-z0-9_]+)";
    private string unsignedIntegerRegex = "([0-9]+)";
    private string startOfLineRegex = "^\\s*";
    private string endOfLineRegex = "\\s*$";
    private string spaceRegex = "\\s*";
    private string restOfLineRegex = "(.*$)";
    private string anythingRegex = ".*";
    private string gameObjectNameRegex = "(.+) \\|";  // need a double \ to imply an escape in the regular expression syntax... wheee
    private string propertyStringRegex = "\"(.+)\"";
    private string vector3StringRegex = "";//string.Format("{0},{1},{2}",realNumberRegex,realNumberRegex,realNumberRegex);
    string inputString = "rusty turret base | spawn=\"turret\" scale=2,2,2 raise=1 rotate=0,45,10";

    void Start(){
        vector3StringRegex = string.Format("{0},{1},{2}",realNumberRegex,realNumberRegex,realNumberRegex);
        // extract name
        Match match = Regex.Match(inputString, startOfLineRegex + gameObjectNameRegex);
        if (match.Success)
        {
            gameObject.name = match.Groups[1].Value; // the groups are things matched inside the parentheses.  It starts at group 1, which is our gameObjectName
            Debug.Log("Name:" + gameObject.name);
        }
        else
        {
            Debug.Log("Name is required in :\"" + inputString + "\"");
        }
        
        // extract spawn
        match = Regex.Match(inputString, startOfLineRegex + gameObjectNameRegex + spaceRegex + anythingRegex + "spawn=" + propertyStringRegex);
        if (match.Success)
        {
            //spawn = match.Groups[2].Value;  // we want the second group matched because the first one is the gameObjectName
            //Debug.Log("Spawn:" + spawn);
        }
        
        // extract scale
        match = Regex.Match(inputString, startOfLineRegex + gameObjectNameRegex + spaceRegex + anythingRegex + "scale=" + vector3StringRegex);
        if (match.Success)
        {
            //scale = Vector3(Convert.ToSingle(match.Groups[2].Value), Convert.ToSingle(match.Groups[3].Value), Convert.ToSingle(match.Groups[4].Value));
            //Debug.Log("Scale:" + scale);
        }
        
        // extract raise
        match = Regex.Match(inputString, startOfLineRegex + gameObjectNameRegex + spaceRegex + anythingRegex + "raise=" + realNumberRegex);
        if (match.Success)
        {
            //raise = Convert.ToInt32(match.Groups[2].Value);  // we want the second group matched because the first one is the gameObjectName
            //Debug.Log("Raise:" + raise);
        }
        
        // extract rotate
        match = Regex.Match(inputString, startOfLineRegex + gameObjectNameRegex + spaceRegex + anythingRegex + "rotate=" + vector3StringRegex);
        if (match.Success)
        {
            //rotate = Vector3(Convert.ToSingle(match.Groups[2].Value), Convert.ToSingle(match.Groups[3].Value), Convert.ToSingle(match.Groups[4].Value));
            //Debug.Log("Rotate:" + rotate);
        }
    }
}
