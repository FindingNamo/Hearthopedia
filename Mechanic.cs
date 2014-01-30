using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public class Mechanic
    {
        public static readonly Mechanic None = new Mechanic() { Id = 0, description= "", name = "None" };

        /// <summary>
        /// The mechanic id
        /// </summary>
        public int Id;
        public string name {get;set;}
        public string description {get;set;}
    }
}
