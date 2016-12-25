
using System;

namespace Pet{

    public class Cat : IPet
    {
        public string Name
        {
            get;
            set ;
        } 
        public string Talk()=>Name+"miao miao";
    }
}