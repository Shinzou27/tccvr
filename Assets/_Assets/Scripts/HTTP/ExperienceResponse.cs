using System;
using System.Collections.Generic;

[Serializable]
public class ExperienceResponse
{
  public string id { get; set; }
  public string userId { get; set; }
  public string pin { get; set; }
  public string joinCode { get; set; }
  public DateTime? enterDate { get; set; }
  public Student[] students { get; set; }

}

