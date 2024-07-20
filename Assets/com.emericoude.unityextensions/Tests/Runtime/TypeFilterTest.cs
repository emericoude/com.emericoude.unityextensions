using System;
using System.Collections.Generic;
using Emericoude;
using UnityEngine;

public class TypeFilterTest : MonoBehaviour
{
    public BaseClass BaseThing;

    [SerializeReference, TypeFilter(typeof(BaseClass))]
    public BaseClass TypeFilteredBaseThing;
    
    [SerializeReference, TypeFilter(typeof(BaseClass))]
    public List<BaseClass> TypeFilteredBaseThingList = new List<BaseClass>();
}


