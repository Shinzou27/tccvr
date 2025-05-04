using System;

[Serializable]
public class Student
{
  public string id { get; set; }
  public string name { get; set; }
  public string classId { get; set; }
  public DateTime? createdAt { get; set; }
}