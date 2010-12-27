using System;
using System.Collections.Generic;

namespace SubtitleChecker.Domain
{
    public class Author
    {
        #region Properties 

        public string Name { get; set; }

        #endregion

        #region Constructors 

        public Author()
            : this(string.Empty)
        {
        }

        public Author(string name)
        {
            Name = name;
        }

        #endregion
    }
}