using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoduckStudio.Utils
{
    public class Resource<T> where T: ScriptableObject
    {
        public List<T> resources { get; protected set; }

        public Resource()
        {
            Reload();
        }

        public void Reload()
        {
            resources = new List<T>(Resources.LoadAll<T>(""));
        }
    }
}
