using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IViewFactory
{
    T CreateView<T>() where T : IMenu;
}

