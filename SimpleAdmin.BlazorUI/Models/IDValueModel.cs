namespace SimpleAdmin.BlazorUI.Models;
public class MultiDDLModel {
    public string Id { set; get; }
    public string Value { set; get; }
    public bool Selected { set; get; }
    public MultiDDLModel(string id, string value, bool selected)
    {
        Id = id;
        Value = value;
        Selected = selected;
    }
 }