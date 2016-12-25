using System;
using Pet;
namespace Pet{
    public class Dog : IPet
    {
        public string Name
        {
            get; 
            set;
        } 
        public string Talk()=>Name+":wang wang";
    }
}