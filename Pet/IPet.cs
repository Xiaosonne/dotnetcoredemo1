using System;
namespace Pet{
    public interface IPet{
        string Name{get;set;}
        string Talk();
    }
}